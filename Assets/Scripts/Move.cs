using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public Tuple<int, int> startPosition;
    public Tuple<int, int> endPosition;
    public Move(Tuple<int, int> startPosition, Tuple<int, int> endPosition)
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
                            Tuple<int, int> startPosition = new Tuple<int, int>(x, y);
                            for (int i = 0; i < piece.internPiece.moveOffsets.Count; i++)
                            {
                                Tuple<int, int> moveOffset = piece.internPiece.moveOffsets[i];
                                int xMoveIndex = x + moveOffset.Item1;
                                int yMoveIndex = y + moveOffset.Item2;
                                while (xMoveIndex < internBoard.board.GetLength(0) && yMoveIndex < internBoard.board.GetLength(1) && xMoveIndex >= 0 && yMoveIndex >= 0)
                                {
                                    if (internBoard.board[x, y].internPiece.isWhite != internBoard.board[x + moveOffset.Item1, y + moveOffset.Item2].internPiece.isWhite)
                                    {
                                        //Propably a bug here but will fix later
                                        if (internBoard.board[x, y].internPiece.isWhite != internBoard.board[x + moveOffset.Item1, y + moveOffset.Item2].internPiece.isWhite | internBoard.board[x + moveOffset.Item1, y + moveOffset.Item2].GetType() == typeof(Pieces.None))
                                        {
                                            Tuple<int, int> endPosition = new Tuple<int, int>(x + moveOffset.Item1, y + moveOffset.Item2);
                                            piece.internPiece.legalMoves.Add(new Move(startPosition, endPosition));
                                            xMoveIndex += moveOffset.Item1;
                                            yMoveIndex += moveOffset.Item2;
                                        }
                                    } else
                                    {
                                        break;
                                    }
                                }
                            }
                        } else
                        {
                            Tuple<int, int> startPosition = new Tuple<int, int>(x, y);
                            for (int i = 0; i < piece.internPiece.moveOffsets.Count; i++) 
                            {
                                Tuple<int, int> moveOffset = piece.internPiece.moveOffsets[i];
                                if (x+moveOffset.Item1 < internBoard.board.GetLength(0) && y+moveOffset.Item2 < internBoard.board.GetLength(1) && x + moveOffset.Item1 >= 0 && y + moveOffset.Item2 >= 0)
                                {
                                    if (internBoard.board[x, y].internPiece.isWhite != internBoard.board[x + moveOffset.Item1, y + moveOffset.Item2].internPiece.isWhite | internBoard.board[x + moveOffset.Item1, y + moveOffset.Item2].GetType() == typeof(Pieces.None))
                                    {
                                        Tuple<int, int> endPosition = new Tuple<int, int>(x + moveOffset.Item1, y + moveOffset.Item2);
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
