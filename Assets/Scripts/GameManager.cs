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
    private Marker markerManager;
    private Board board;
    private MoveManager moveManager;

    // Start is called before the first frame update
    void Start()
    {
        board = new Board();
        UpdateExternBoard();
        markerManager = new Marker();

    }
   
    // Update all pieces on the board
    public void UpdateExternBoard()
    {
        for (int y = 0; y < board.intern.board.GetLength(1); y++)
        {
            for (int x = 0; x < board.intern.board.GetLength(0); x++)
            {
                Piece piece = board.intern.board[x, y];
                UpdateExternPosition(piece, new Vector2Int(x, y));
            }
        }
        UpdatePedestal();
    }

    private void UpdateExternPosition(Piece piece, Vector2Int internDestination)
    {
        //Calculate the destination for the external piece.
        Vector3 externDestination = ConvertInternToExternPosition(internDestination, );
        //Smoothly move the piece to the destination over a period of 0.1 seconds.
        StartCoroutine(piece.@extern.SmoothMove(externDestination, 0.1f));
    }

    // Refactor
    public void UpdatePedestal()
    {
        int x = board.intern.captured.Count - 1;
        int y = 0;
        int totalAmountOfWhitePieces = 0;
        int amountOfWhitePieces;
        int amountOfWhitePawns = 0;
        int totalAmountOfBlackPieces;
        int amountOfBlackPieces;
        int amountOfBlackPawns = 0;

        Vector2 size = board.@extern.playSurface.GetComponent<MeshFilter>().mesh.bounds.size * (Vector2)board.@extern.pedestalPlaySurface.transform.localScale;

        Piece capturedPiece;
        capturedPiece = board.intern.captured[board.intern.captured.Count - 1];

        foreach (Piece piece in board.intern.captured)
        {
            if (piece.intern.isWhite == true)
            {
                totalAmountOfWhitePieces++;
            }
            if (piece.GetType() == typeof(Piece.White_Pawn))
            {
                amountOfWhitePawns++;
            }
            if (piece.GetType() == typeof(Piece.Pawn))
            {
                amountOfBlackPawns++;
            }
        }

        amountOfWhitePieces = totalAmountOfWhitePieces - amountOfWhitePawns;
        totalAmountOfBlackPieces = board.intern.captured.Count - totalAmountOfWhitePieces;
        amountOfBlackPieces = totalAmountOfBlackPieces - amountOfBlackPawns;
        if (capturedPiece.intern.isWhite == true && capturedPiece.GetType() == typeof(Piece.White_Pawn))
        {
            x = amountOfWhitePawns - 1;
            y = 1;
        }
        if (capturedPiece.intern.isWhite == true && capturedPiece.GetType() != typeof(Piece.White_Pawn))
        {
            x = amountOfWhitePieces - 1;
            y = 0;
        }
        if (capturedPiece.intern.isWhite == false && capturedPiece.GetType() == typeof(Piece.Pawn))
        {
            x = 8 + amountOfBlackPawns - 1;
            y = 1;
        }
        if (capturedPiece.intern.isWhite == false && capturedPiece.GetType() != typeof(Piece.Pawn))
        {
            x = 8 + amountOfBlackPieces - 1;
            y = 0;
        }

        capturedPiece = board.intern.captured[board.intern.captured.Count - 1];
        capturedPiece.@extern.pieceGameObject.transform.parent = board.@extern.pedestalPlaySurface.transform;

        Vector2 position = new Vector3((size.x / 32) - (8 * size.x / 16) + ((x * size.x / 16)), -size.y + (y * size.y * 2));

        capturedPiece.@extern.pieceGameObject.transform.parent = board.@extern.pedestalPlaySurface.transform;
        capturedPiece.@extern.pieceGameObject.transform.localPosition = position;
    }
    
    // Calculate an internal position from an external position
    public Vector2Int ConvertExternToInternPosition(Vector3 externPosition)
    {
        Vector3 parentSize = board.@extern.playSurface.GetComponent<MeshFilter>().mesh.bounds.size;
        Vector2 internPosition = (((Vector2)externPosition) - ((Vector2)parentSize / 16) + ((Vector2)parentSize / 2)) / ((Vector2)parentSize / 8);
        return Vector2Int.RoundToInt(internPosition);
    }

    // Calculate an external position from an internal position
    public Vector3 ConvertInternToExternPosition(Vector2Int internDestination)
    {
        Vector3 parentSize = board.@extern.playSurface.GetComponent<MeshFilter>().mesh.bounds.size;
        Vector3 externDestination = (((Vector2)internDestination * (Vector2)parentSize/8) + ((Vector2)parentSize / 16) - ((Vector2)parentSize / 2));
        externDestination.z = 0;
        return externDestination;
    }


    public Transform GetExternBoardPosition()
    {
        return board.@extern.playSurface.transform;
    }


    // This function is assigned in the unity editor, and called when any object is grabbed.
    public void OnGrab(GameObject gameObject)
    {
        Piece grabedPiece = Piece.Lookup(gameObject);
        if (gameObject != null & grabedPiece != null)
        {
            Move.Legal.Generate(board.intern);
            markerManager.hints.Visualize(this, grabedPiece);
        }
    }
    public void OnRelease(GameObject gameObject)
    {
        markerManager.hints.Delete();
        Piece releasedPiece = Piece.Lookup(gameObject);

        if (gameObject != null & releasedPiece != null)
        {
            Vector2Int releasePosition = ConvertExternToInternPosition(gameObject.transform.localPosition);
            bool isInsideBounds = board.intern.IsInsideBounds(releasePosition);
            if (isInsideBounds)
            {
                foreach (Move legalMove in releasedPiece.intern.legalMoves)
                {
                    if (legalMove == new Move(releasedPiece.intern.position, releasePosition, board.intern))
                    {
                        board.intern.AddMove(legalMove);
                        break;
                    }
                }
            }
            UpdateExternBoard();
        }
    }
}

