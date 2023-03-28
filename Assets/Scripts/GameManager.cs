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
        for (int y = 0; y < board.internBoard.board.GetLength(1); y++)
        {
            for (int x = 0; x < board.internBoard.board.GetLength(0); x++)
            {
                Piece piece = board.internBoard.board[x, y];
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
        StartCoroutine(piece.externPiece.SmoothMove(externDestination, 0.1f));
    }

    // Refactor
    public void UpdatePedestal()
    {
        int x = board.internBoard.captured.Count - 1;
        int y = 0;
        int totalAmountOfWhitePieces = 0;
        int amountOfWhitePieces;
        int amountOfWhitePawns = 0;
        int totalAmountOfBlackPieces;
        int amountOfBlackPieces;
        int amountOfBlackPawns = 0;

        Vector2 size = board.externBoard.playSurface.GetComponent<MeshFilter>().mesh.bounds.size * (Vector2)board.externBoard.pedestalPlaySurface.transform.localScale;

        Piece capturedPiece;
        capturedPiece = board.internBoard.captured[board.internBoard.captured.Count - 1];

        foreach (Piece piece in board.internBoard.captured)
        {
            if (piece.internPiece.isWhite == true)
            {
                totalAmountOfWhitePieces++;
            }
            if (piece.GetType() == typeof(Piece.White_Pawn))
            {
                amountOfWhitePawns++;
            }
            if (piece.GetType() == typeof(Piece.Black_Pawn))
            {
                amountOfBlackPawns++;
            }
        }

        amountOfWhitePieces = totalAmountOfWhitePieces - amountOfWhitePawns;
        totalAmountOfBlackPieces = board.internBoard.captured.Count - totalAmountOfWhitePieces;
        amountOfBlackPieces = totalAmountOfBlackPieces - amountOfBlackPawns;
        if (capturedPiece.internPiece.isWhite == true && capturedPiece.GetType() == typeof(Piece.White_Pawn))
        {
            x = amountOfWhitePawns - 1;
            y = 1;
        }
        if (capturedPiece.internPiece.isWhite == true && capturedPiece.GetType() != typeof(Piece.White_Pawn))
        {
            x = amountOfWhitePieces - 1;
            y = 0;
        }
        if (capturedPiece.internPiece.isWhite == false && capturedPiece.GetType() == typeof(Piece.Black_Pawn))
        {
            x = 8 + amountOfBlackPawns - 1;
            y = 1;
        }
        if (capturedPiece.internPiece.isWhite == false && capturedPiece.GetType() != typeof(Piece.Black_Pawn))
        {
            x = 8 + amountOfBlackPieces - 1;
            y = 0;
        }

        capturedPiece = board.internBoard.captured[board.internBoard.captured.Count - 1];
        capturedPiece.externPiece.pieceGameObject.transform.parent = board.externBoard.pedestalPlaySurface.transform;

        Vector2 position = new Vector3((size.x / 32) - (8 * size.x / 16) + ((x * size.x / 16)), -size.y + (y * size.y * 2));

        capturedPiece.externPiece.pieceGameObject.transform.parent = board.externBoard.pedestalPlaySurface.transform;
        capturedPiece.externPiece.pieceGameObject.transform.localPosition = position;
    }
    
    // Calculate an internal position from an external position
    public Vector2Int ConvertExternToInternPosition(Vector3 externPosition)
    {
        Vector3 parentSize = board.externBoard.playSurface.GetComponent<MeshFilter>().mesh.bounds.size;
        Vector2 internPosition = (((Vector2)externPosition) - ((Vector2)parentSize / 16) + ((Vector2)parentSize / 2)) / ((Vector2)parentSize / 8);
        return Vector2Int.RoundToInt(internPosition);
    }

    // Calculate an external position from an internal position
    public Vector3 ConvertInternToExternPosition(Vector2Int internDestination)
    {
        Vector3 parentSize = board.externBoard.playSurface.GetComponent<MeshFilter>().mesh.bounds.size;
        Vector3 externDestination = (((Vector2)internDestination * (Vector2)parentSize/8) + ((Vector2)parentSize / 16) - ((Vector2)parentSize / 2));
        externDestination.z = 0;
        return externDestination;
    }


    public Transform GetExternBoardPosition()
    {
        return board.externBoard.playSurface.transform;
    }


    // This function is assigned in the unity editor, and called when any object is grabbed.
    public void OnGrab(GameObject gameObject)
    {
        Piece grabedPiece = Piece.Lookup(gameObject);
        if (gameObject != null & grabedPiece != null)
        {
            Move.Legal.Generate(board.internBoard);
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
            bool isInsideBounds = board.internBoard.IsInsideBounds(releasePosition);
            if (isInsideBounds)
            {
                foreach (Move legalMove in releasedPiece.internPiece.legalMoves)
                {
                    if (legalMove == new Move(releasedPiece.internPiece.position, releasePosition, board.internBoard))
                    {
                        board.internBoard.AddMove(legalMove);
                        break;
                    }
                }
            }
            UpdateExternBoard();
        }
    }
}

