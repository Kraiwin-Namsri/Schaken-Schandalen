using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public Vector2Int startPosition;
    public Vector2Int endPosition;

    // isCapture is assigned when calling Board.Addmove(move)
    public bool isCapture;
    public Move(Vector2Int startPosition, Vector2Int endPosition)
    {
        this.startPosition = startPosition;
        this.endPosition = endPosition;
    }
    public static class Legal
    {   
        public static void Generate(Board.Intern internBoard)
        {
            for (int y = 0; y < internBoard.board.GetLength(1); y++) 
            {
                for (int x = 0; x < internBoard.board.GetLength(0); x++)
                {
                    Pieces piece = internBoard.board[x, y];
                    if (piece.GetType() != typeof(Pieces.None)) 
                    {
                        piece.internPiece.legalMoves = new List<Move>();
                        Vector2Int startPosition = new Vector2Int(x, y);
                        if (piece.internPiece.isSlidingPiece)
                        {
                            GenerateSliding(startPosition, piece, internBoard);
                        } else
                        {
                            GenerateNonSliding(startPosition, piece, internBoard);
                        }
                    }
                }
            }
        }
        private static void GenerateSliding(Vector2Int startPosition, Pieces piece, Board.Intern internBoard)
        {
            for (int i = 0; i < piece.internPiece.moveOffsets.Count; i++)
            {
                Vector2Int moveOffset = piece.internPiece.moveOffsets[i];
                Vector2Int delta = new Vector2Int(moveOffset.x, moveOffset.y);

                while (delta.x + startPosition.x < internBoard.board.GetLength(0) && delta.y + startPosition.y < internBoard.board.GetLength(1) && delta.x + startPosition.x >= 0 && delta.y + startPosition.y >= 0)
                {
                    Vector2Int endPosition = new Vector2Int(delta.x + startPosition.x, delta.y + startPosition.y);
                    if (internBoard.board[endPosition.x, endPosition.y].GetType() != typeof(Pieces.None))
                        break;
                    piece.internPiece.legalMoves.Add(new Move(startPosition, endPosition));
                    delta += moveOffset;
                }
            }
        }
        private static void GenerateNonSliding(Vector2Int startPosition, Pieces piece, Board.Intern internBoard)
        {
            for (int i = 0; i < piece.internPiece.moveOffsets.Count; i++)
            {
                Vector2Int moveOffset = piece.internPiece.moveOffsets[i];
                if (startPosition.x + moveOffset.x < internBoard.board.GetLength(0) && startPosition.y + moveOffset.y < internBoard.board.GetLength(1) && startPosition.x + moveOffset.x >= 0 && startPosition.y + moveOffset.y >= 0)
                {
                    if (internBoard.board[startPosition.x, startPosition.y].internPiece.isWhite != internBoard.board[startPosition.x + moveOffset.x, startPosition.y + moveOffset.y].internPiece.isWhite | internBoard.board[startPosition.x + moveOffset.x, startPosition.y + moveOffset.y].GetType() == typeof(Pieces.None))
                    {
                        Vector2Int endPosition = new Vector2Int(startPosition.x + moveOffset.x, startPosition.y + moveOffset.y);
                        piece.internPiece.legalMoves.Add(new Move(startPosition, endPosition));
                    }
                }
            }
        }
    }
    
}
