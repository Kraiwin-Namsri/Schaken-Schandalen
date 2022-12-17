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
            for (int y = 0; y < internBoard.board.GetLength(0); y++) 
            {
                for (int x = 0; x < internBoard.board.GetLength(1); x++)
                {
                    Pieces piece = internBoard.board[x, y];
                    if (piece.GetType() != typeof(Pieces.None)) 
                    {
                        piece.internPiece.legalMoves = new List<Move>();
                        if (piece.internPiece.isSlidingPiece)
                        {
                            Vector2Int startPosition = new Vector2Int(x, y);
                            for (int i = 0; i < piece.internPiece.moveOffsets.Count; i++)
                            {
                                Vector2Int moveOffset = piece.internPiece.moveOffsets[i];
                                int xMoveIndex = x + moveOffset.x;
                                int yMoveIndex = y + moveOffset.y;
                                while (xMoveIndex < internBoard.board.GetLength(0) && yMoveIndex < internBoard.board.GetLength(1) && xMoveIndex >= 0 && yMoveIndex >= 0)
                                {
                                    if (internBoard.board[x, y].internPiece.isWhite != internBoard.board[x + moveOffset.x, y + moveOffset.y].internPiece.isWhite)
                                    {
                                        //Propably a bug here but will fix later
                                        if (internBoard.board[x, y].internPiece.isWhite != internBoard.board[x + moveOffset.x, y + moveOffset.y].internPiece.isWhite | internBoard.board[x + moveOffset.x, y + moveOffset.y].GetType() == typeof(Pieces.None))
                                        {
                                            Vector2Int endPosition = new Vector2Int(x + moveOffset.x, y + moveOffset.y);
                                            piece.internPiece.legalMoves.Add(new Move(startPosition, endPosition));
                                            xMoveIndex += moveOffset.x;
                                            yMoveIndex += moveOffset.y;
                                        }
                                    } else
                                    {
                                        break;
                                    }
                                }
                            }
                        } else
                        {
                            Vector2Int startPosition = new Vector2Int(x, y);
                            for (int i = 0; i < piece.internPiece.moveOffsets.Count; i++) 
                            {
                                Vector2Int moveOffset = piece.internPiece.moveOffsets[i];
                                if (x+moveOffset.x < internBoard.board.GetLength(0) && y+moveOffset.y < internBoard.board.GetLength(1) && x + moveOffset.x >= 0 && y + moveOffset.y >= 0)
                                {
                                    if (internBoard.board[x, y].internPiece.isWhite != internBoard.board[x + moveOffset.x, y + moveOffset.y].internPiece.isWhite | internBoard.board[x + moveOffset.x, y + moveOffset.y].GetType() == typeof(Pieces.None))
                                    {
                                        Vector2Int endPosition = new Vector2Int(x + moveOffset.x, y + moveOffset.y);
                                        piece.internPiece.legalMoves.Add(new Move(startPosition, endPosition));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
