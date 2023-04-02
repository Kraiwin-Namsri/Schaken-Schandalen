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

    // Start is called before the first frame update
    void Start()
    {
        board = new Board();
        ApplyFenstring("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        pedestal = new Pedestal();
        markerManager = new MarkerManager();
        moveManager = new MoveManager();
        player1 = new Player(true);
        player2 = new Player(false);
        player2.bot = new Bot.StockFishOnline(player2, board, "http://127.0.0.1:5000");
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
            Legal.Generate(board.intern);
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
                    if (legalMove == new Move(releasedPiece.intern.position, releasePosition, board.intern))
                    {
                        if(board.intern.whiteToMove ==  player1.isPlayingWhite)
                        {
                            moveManager.AddMove(legalMove);
                            List<Piece> capturedPieces = moveManager.ExecuteMoveQueue(board);
                            pedestal.AddPieces(capturedPieces);
                            board.intern.whiteToMove = !board.intern.whiteToMove;
                        }
                        break;
                    }
                }
            }
            board.@extern.Update(this);

            string fen = board.intern.fenManager.BoardToFen();

            StartCoroutine(((Bot.StockFishOnline)player2.bot).GetBestMove(fen, new Action<Move>((Move move) => Callback_StockFish(move))));
        }
    }
    public void Callback_StockFish(Move move)
    {
        Debug.Log("has been returned");
        bool isOpponentsMove = Legal.IsMoveFromOpponent(move, board, player1);

        Debug.Log(isOpponentsMove);
        if(isOpponentsMove == true)
        {
            Debug.Log("added move");
            moveManager.AddMove(move);
            List<Piece> capturedPieces = moveManager.ExecuteMoveQueue(board);
            pedestal.AddPieces(capturedPieces);
            board.@extern.Update(this);
            board.intern.whiteToMove = !board.intern.whiteToMove;
        }
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
        menuManager.Backwards(board.intern);
    }

    public void Forwards()
    {
        menuManager.Forwards(board.intern);
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

