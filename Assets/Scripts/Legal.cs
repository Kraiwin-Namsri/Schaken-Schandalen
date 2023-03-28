using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static MoveManager;

public class Legal
{
    public static void Generate(Board.Intern internBoard)
    {
        for (int y = 0; y < internBoard.board.GetLength(1); y++)
        {
            for (int x = 0; x < internBoard.board.GetLength(0); x++)
            {
                Piece piece = internBoard.board[x, y];
                if (piece.GetType() != typeof(Piece.None))
                {
                    Vector2Int startPosition = new Vector2Int(x, y);
                    piece.internPiece.legalMoves = GeneratePieceMoves(piece, new Move(startPosition, startPosition, internBoard), internBoard);
                }
            }
        }
    }

    // All the boolean Functions
    public static bool isCapture(Move move, Board.Intern internBoard)
    {
        bool endPositionEmpty = internBoard.board[move.endPosition.x, move.endPosition.y].GetType() == typeof(Piece.None);
        bool differentColor = internBoard.board[move.startPosition.x, move.startPosition.y].internPiece.isWhite != internBoard.board[move.endPosition.x, move.endPosition.y].internPiece.isWhite;
        return !endPositionEmpty && differentColor;
    }

    public static bool IsLegal(Move move, Board.Intern internBoard)
    {
        bool endPositionFree = IsEndPositionFree(move, internBoard);

        Piece movedPiece = internBoard.board[move.startPosition.x, move.startPosition.y];

        bool legalPos = Move.Contains(GeneratePieceMoves(movedPiece, move, internBoard), move);

        return (endPositionFree & legalPos);
    }
    public static bool IsEndPositionFree(Move move, Board.Intern internBoard)
    {
        bool endPositionEmpty = internBoard.board[move.endPosition.x, move.endPosition.y].GetType() == typeof(Piece.None);
        bool differentColor = internBoard.board[move.startPosition.x, move.startPosition.y].internPiece.isWhite != internBoard.board[move.endPosition.x, move.endPosition.y].internPiece.isWhite;
        return endPositionEmpty | differentColor;
    }

    // All the functions that return a list
    private static List<Move> GeneratePieceMoves(Piece piece, Move move, Board.Intern internBoard)
    {
        List<Move> pieceMoves = new List<Move>();
        if (piece.internPiece.isSlidingPiece)
        {
            pieceMoves = pieceMoves.Concat(GenerateSliding(move.startPosition, piece, internBoard)).ToList();
        }
        else
        {
            pieceMoves = pieceMoves.Concat(GenerateNonSliding(move.startPosition, piece, internBoard)).ToList();
        }

        if (piece.GetType() == typeof(Piece.White_King) | piece.GetType() == typeof(Piece.Black_King) | piece.GetType() == typeof(Piece.White_Rook) | piece.GetType() == typeof(Piece.Black_Rook))
        {
            pieceMoves = pieceMoves.Concat(GenerateCastling(move.startPosition, piece, internBoard)).ToList();
        }

        if (piece.GetType() == typeof(Piece.White_Pawn) | piece.GetType() == typeof(Piece.Black_Pawn))
        {
            pieceMoves = pieceMoves.Concat(GeneratePawnCapture(move.startPosition, piece, internBoard)).ToList();
        }
        return pieceMoves;

    }
    private static List<Move> GenerateSliding(Vector2Int startPosition, Piece piece, Board.Intern internBoard)
    {
        List<Move> legalMoves = new List<Move>();
        for (int i = 0; i < piece.internPiece.moveOffsets.Count; i++)
        {
            Vector2Int moveOffset = piece.internPiece.moveOffsets[i];
            Vector2Int delta = new Vector2Int(moveOffset.x, moveOffset.y);


            while (delta.x + startPosition.x < internBoard.board.GetLength(0) && delta.y + startPosition.y < internBoard.board.GetLength(1) && delta.x + startPosition.x >= 0 && delta.y + startPosition.y >= 0)
            {
                Vector2Int endPosition = new Vector2Int(delta.x + startPosition.x, delta.y + startPosition.y);
                if (internBoard.board[endPosition.x, endPosition.y].GetType() != typeof(Piece.None))
                    break;
                legalMoves.Add(new Move(startPosition, endPosition, internBoard));
                delta += moveOffset;
            }
        }
        return legalMoves;
    }
    private static List<Move> GenerateNonSliding(Vector2Int startPosition, Piece piece, Board.Intern internBoard)
    {
        List<Move> legalMoves = new List<Move>();
        for (int i = 0; i < piece.internPiece.moveOffsets.Count; i++)
        {
            Vector2Int moveOffset = piece.internPiece.moveOffsets[i];
            if (startPosition.x + moveOffset.x < internBoard.board.GetLength(0) && startPosition.y + moveOffset.y < internBoard.board.GetLength(1) && startPosition.x + moveOffset.x >= 0 && startPosition.y + moveOffset.y >= 0)
            {
                if (internBoard.board[startPosition.x, startPosition.y].internPiece.isWhite != internBoard.board[startPosition.x + moveOffset.x, startPosition.y + moveOffset.y].internPiece.isWhite | internBoard.board[startPosition.x + moveOffset.x, startPosition.y + moveOffset.y].GetType() == typeof(Piece.None))
                {
                    Vector2Int endPosition = new Vector2Int(startPosition.x + moveOffset.x, startPosition.y + moveOffset.y);

                    if (internBoard.board[startPosition.x, startPosition.y].GetType() == typeof(Piece.White_Pawn) || internBoard.board[startPosition.x, startPosition.y].GetType() == typeof(Piece.Black_Pawn))
                    {
                        if (internBoard.board[startPosition.x + moveOffset.x, startPosition.y + moveOffset.y].GetType() != typeof(Piece.None))
                        {

                        }
                        else
                        {
                            legalMoves.Add(new Move(startPosition, endPosition, internBoard));
                        }
                    }
                    else
                    {
                        legalMoves.Add(new Move(startPosition, endPosition, internBoard));
                        Debug.Log(internBoard.board[startPosition.x, startPosition.y]);
                    }
                }
            }
        }
        return legalMoves;
    }

    private static List<Move> GenerateCastling(Vector2Int startPosition, Piece piece, Board.Intern internBoard)
    {
        List<Move> legalMoves = new List<Move>();
        if (internBoard.castleAbility.whiteKingSide)
        {
            if (piece.GetType() == typeof(Piece.White_King))
            {
                legalMoves.Add(new Move(new Vector2Int(4, 7), new Vector2Int(6, 7), internBoard, new Move(new Vector2Int(7, 7), new Vector2Int(5, 7), internBoard)));
            }
            else if (piece.GetType() == typeof(Piece.White_Rook))
            {
                legalMoves.Add(new Move(new Vector2Int(7, 7), new Vector2Int(5, 7), internBoard));
            }
        }
        if (internBoard.castleAbility.whiteQueenSide)
        {
            if (piece.GetType() == typeof(Piece.White_King))
            {
                legalMoves.Add(new Move(new Vector2Int(4, 7), new Vector2Int(2, 7), internBoard, new Move(new Vector2Int(0, 7), new Vector2Int(3, 7), internBoard)));
            }
            else if (piece.GetType() == typeof(Piece.White_Rook))
            {
                legalMoves.Add(new Move(new Vector2Int(0, 7), new Vector2Int(3, 7), internBoard));
            }
        }
        if (internBoard.castleAbility.blackKingSide)
        {
            if (piece.GetType() == typeof(Piece.Black_King))
            {
                legalMoves.Add(new Move(new Vector2Int(4, 0), new Vector2Int(6, 0), internBoard, new Move(new Vector2Int(7, 0), new Vector2Int(5, 0), internBoard)));
            }
            else if (piece.GetType() == typeof(Piece.Black_Rook))
            {
                legalMoves.Add(new Move(new Vector2Int(7, 0), new Vector2Int(5, 0), internBoard));
            }
        }
        if (internBoard.castleAbility.blackQueenSide)
        {
            if (piece.GetType() == typeof(Piece.Black_King))
            {
                legalMoves.Add(new Move(new Vector2Int(4, 0), new Vector2Int(2, 0), internBoard, new Move(new Vector2Int(0, 0), new Vector2Int(3, 0), internBoard)));
            }
            else if (piece.GetType() == typeof(Piece.Black_Rook))
            {
                legalMoves.Add(new Move(new Vector2Int(0, 0), new Vector2Int(3, 0), internBoard));
            }
        }
        return legalMoves;
    }

    private static List<Move> GeneratePawnCapture(Vector2Int startPosition, Piece piece, Board.Intern internBoard)
    {
        List<Move> legalMoves = new List<Move>();
        if (piece.GetType() == typeof(Piece.Black_Pawn))
        {
            Vector2Int blackOffset1 = new Vector2Int(1, 1);
            Vector2Int blackOffset2 = new Vector2Int(-1, 1);

            Vector2Int blackEndPosition1 = startPosition + blackOffset1;
            Vector2Int blackEndPosition2 = startPosition + blackOffset2;

            if (blackEndPosition1.x >= 0 && blackEndPosition1.y >= 0 && blackEndPosition1.x < internBoard.board.GetLength(0) && blackEndPosition1.y < internBoard.board.GetLength(1))
            {
                if (internBoard.board[blackEndPosition1.x, blackEndPosition1.y].GetType() != typeof(Piece.None))
                {
                    legalMoves.Add(new Move(startPosition, blackEndPosition1, internBoard));
                }
            }
            if (blackEndPosition2.x >= 0 && blackEndPosition2.y >= 0 && blackEndPosition2.x < internBoard.board.GetLength(0) && blackEndPosition2.y < internBoard.board.GetLength(1))
            {
                if (internBoard.board[blackEndPosition2.x, blackEndPosition2.y].GetType() != typeof(Piece.None))
                {
                    legalMoves.Add(new Move(startPosition, blackEndPosition2, internBoard));
                }
            }
        }
        else if (piece.GetType() == typeof(Piece.White_Pawn))
        {
            Vector2Int whiteOffset1 = new Vector2Int(-1, -1);
            Vector2Int whiteOffset2 = new Vector2Int(1, -1);

            Vector2Int whiteEndPosition1 = startPosition + whiteOffset1;
            Vector2Int whiteEndPosition2 = startPosition + whiteOffset2;

            if (whiteEndPosition1.x >= 0 && whiteEndPosition1.y >= 0 && whiteEndPosition1.x < internBoard.board.GetLength(0) && whiteEndPosition1.y < internBoard.board.GetLength(1))
            {
                if (internBoard.board[whiteEndPosition1.x, whiteEndPosition1.y].GetType() != typeof(Piece.None))
                {
                    legalMoves.Add(new Move(startPosition, whiteEndPosition1, internBoard));
                }
            }
            if (whiteEndPosition2.x >= 0 && whiteEndPosition2.y >= 0 && whiteEndPosition2.x < internBoard.board.GetLength(0) && whiteEndPosition2.y < internBoard.board.GetLength(1))
            {
                if (internBoard.board[whiteEndPosition2.x, whiteEndPosition2.y].GetType() != typeof(Piece.None))
                {
                    legalMoves.Add(new Move(startPosition, whiteEndPosition2, internBoard));
                }
            }
        }
        return legalMoves;
    }
}
