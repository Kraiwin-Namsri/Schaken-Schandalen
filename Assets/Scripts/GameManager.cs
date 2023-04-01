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

    // Start is called before the first frame update
    void Start()
    {
        board = new Board();
        pedestal = new Pedestal();
        board.intern.fenManager.Apply(board, "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        markerManager = new MarkerManager();
        moveManager = new MoveManager();
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
        markerManager.hints.Delete();
        Piece releasedPiece = Piece.Lookup(gameObject);

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
                        moveManager.AddMove(legalMove);
                        List<Piece> capturedPieces = moveManager.ExecuteMoveQueue(board);
                        pedestal.AddPieces(capturedPieces);
                        break;
                    }
                }
            }
            board.@extern.Update(this);
        }
    }
}

