using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.OpenXR.Input;

public class GameManager : MonoBehaviour
{
    private Board board;
    private Pedestal pedestal;
    private MarkerManager markerManager;
    private MoveManager moveManager;
    private MenuManager menuManager;
    private Player player1;
    private Player player2;
    private Bot supervisor;

    // Start is called before the first frame update
    void Start()
    {
        board = new Board();
        ApplyFenstring("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        pedestal = new Pedestal();
        markerManager = new MarkerManager();
        moveManager = new MoveManager();

        Action<Player, Move> callback = new Action<Player, Move>((Player player, Move move) => SubmitMoveCallback(player, move));
        player1 = new Player(callback, true);
        player2 = new Player(callback, false);
        player2.bot = new Bot.StockFishOnline(player2, board, "http://127.0.0.1:5000");
        supervisor = new Bot.StockFishOnline(null, board, "http://127.0.0.1:5000");

        menuManager = new MenuManager();
        StartCoroutine(LateStart(0.1f));
    }
    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime); 
        board.@extern.Update(this);
    }

    // This function is assigned in the unity editor, and called when any object is grabbed.
    public void OnGrab(GameObject gameObject)
    {
        Piece grabedPiece = Piece.Lookup(gameObject);
        if (gameObject != null & grabedPiece != null)
        {
            board.intern.GenerateLegalMoves();
            markerManager.hints.Visualize(board, grabedPiece);
        }
    }
    public void OnRelease(GameObject gameObject)
    {
        Piece releasedPiece = Piece.Lookup(gameObject);
        
        markerManager.hints.Delete();

        if (gameObject != null & releasedPiece != null)
        {
            Vector2Int releasePosition = board.@extern.ToIntern(gameObject.transform.localPosition);
            
            bool isInsideBounds = board.intern.IsInsideBounds(releasePosition);
            if (isInsideBounds)
            {
                foreach (Move legalMove in releasedPiece.intern.legalMoves)
                {
                    if (legalMove == new Move(releasedPiece.intern.position, releasePosition))
                    {
                        player1.SubmitMove(legalMove);

                        string fen = board.intern.fenManager.BoardToFen();
                        StartCoroutine(((Bot.StockFishOnline)player2.bot).GetBestMove(fen));
                        break;
                    }
                }
            }

        }
        board.@extern.Update(this);;
    }
    public void SubmitMoveCallback(Player player, Move move)
    {
        if (player.isPlayingWhite == board.intern.legal.whiteToMove)
        {
            moveManager.AddMove(move);
            List<Piece> capturedPieces = moveManager.ExecuteMoveQueue(board);
            pedestal.AddPieces(capturedPieces);
            board.intern.legal.whiteToMove = !board.intern.legal.whiteToMove;
        }
        board.@extern.Update(this);
    }

    public void LoadMainScene()
    {
        menuManager.LoadMainScene();
    }
    public void LoadStockfishScene()
    {
        menuManager.LoadStockFishScene();
    }
    public void Backwards()
    {
        Debug.Log("backwards");
        menuManager.Backwards(board.intern);
        board.@extern.Update(this);
    }

    public void Forwards()
    {
        Debug.Log("forewards");
        menuManager.Forwards(board.intern);
        board.@extern.Update(this);
    }
    public void SwitchColour()
    {
        menuManager.SwitchColour(player1, player2);
    }

    public void ApplyFenstring(string fenString)
    {
        board.intern.fenManager.Apply(fenString);
    }
}

