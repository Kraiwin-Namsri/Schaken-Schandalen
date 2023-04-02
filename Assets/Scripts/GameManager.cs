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
        board.intern.fenManager.Apply("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
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
                        if(board.intern.legal.whiteToMove ==  player1.isPlayingWhite)
                        {
                            moveManager.AddMove(legalMove);
                            List<Piece> capturedPieces = moveManager.ExecuteMoveQueue(board);
                            pedestal.AddPieces(capturedPieces);
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
        bool isOpponentsMove = board.intern.legal.IsOpponentsMove(move, player1);
        if(isOpponentsMove == true)
        {
            moveManager.AddMove(move);
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
        menuManager.Backwards();
    }

    public void Forwards()
    {
        menuManager.Backwards();
    }
    public void SwitchColour()
    {
        menuManager.SwitchColour(player1, player2);
    }

}

