using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Move
{
    public Vector2Int startPosition;
    public Vector2Int endPosition;

    public bool isCapture;
    public bool isLegal;
    public Move(Vector2Int startPosition, Vector2Int endPosition, Board.Intern board)
    {
        this.startPosition = startPosition;
        this.endPosition = endPosition;
        this.isLegal = Legal.IsLegal(this, board);
        this.isCapture = Legal.isCapture(this, board);
    }
    public Move(Vector2Int startPosition, Vector2Int endPosition)
    {
        this.startPosition = startPosition;
        this.endPosition = endPosition;
        this.isLegal = false;
    }
    public static bool Contains(List<Move> moves, Move moveToCheck)
    {
        foreach (Move move in moves)
        {
            if (move == moveToCheck) { return true;}
        }
        return false;
    }
    public static bool operator ==(Move move1, Move move2)
    {
        return (move1.startPosition == move2.startPosition && move1.endPosition == move2.endPosition);
    }
    public static bool operator !=(Move move1, Move move2)
    {
        return (move1.startPosition != move2.startPosition | move1.endPosition != move2.endPosition);
    }

    public static class Legal
    {   
        public static bool isCapture(Move move, Board.Intern internBoard)
        {
            bool endPositionEmpty = internBoard.board[move.endPosition.x, move.endPosition.y].GetType() == typeof(Pieces.None);
            bool differentColor = internBoard.board[move.startPosition.x, move.startPosition.y].internPiece.isWhite != internBoard.board[move.endPosition.x, move.endPosition.y].internPiece.isWhite;
            return !endPositionEmpty && differentColor;
        }
        public static bool IsLegal(Move move, Board.Intern internBoard)
        {
            bool endPositionFree = IsEndPositionFree(move, internBoard);

            Pieces movedPiece = internBoard.board[move.startPosition.x, move.startPosition.y];
            List<Move> legalMoves;
            if (movedPiece.internPiece.isSlidingPiece)
            {
                legalMoves = GenerateSliding(move.startPosition, movedPiece, internBoard);
            }
            else
            {
                legalMoves = GenerateNonSliding(move.startPosition, movedPiece, internBoard);
            }
            //To Do: find better name
            bool legalPos = Move.Contains(legalMoves, move);

            return (endPositionFree & legalPos);
        }
        public static bool IsEndPositionFree(Move move, Board.Intern internBoard)
        {
            bool endPositionEmpty = internBoard.board[move.endPosition.x, move.endPosition.y].GetType() == typeof(Pieces.None);
            bool differentColor = internBoard.board[move.startPosition.x, move.startPosition.y].internPiece.isWhite != internBoard.board[move.endPosition.x, move.endPosition.y].internPiece.isWhite;
            return endPositionEmpty | differentColor;
        }
        public static void Generate(Board.Intern internBoard)
        {
            for (int y = 0; y < internBoard.board.GetLength(1); y++) 
            {
                for (int x = 0; x < internBoard.board.GetLength(0); x++)
                {
                    Pieces piece = internBoard.board[x, y];
                    if (piece.GetType() != typeof(Pieces.None)) 
                    {
                        Vector2Int startPosition = new Vector2Int(x, y);
                        if (piece.internPiece.isSlidingPiece)
                        {
                            piece.internPiece.legalMoves = GenerateSliding(startPosition, piece, internBoard);
                        } else
                        {
                            piece.internPiece.legalMoves = GenerateNonSliding(startPosition, piece, internBoard);
                        }
                    }
                }
            }
        }
        private static List<Move> GenerateSliding(Vector2Int startPosition, Pieces piece, Board.Intern internBoard)
        {
            List<Move> legalMoves = new List<Move>();
            for (int i = 0; i < piece.internPiece.moveOffsets.Count; i++)
            {
                Vector2Int moveOffset = piece.internPiece.moveOffsets[i];
                Vector2Int delta = new Vector2Int(moveOffset.x, moveOffset.y);


                while (delta.x + startPosition.x < internBoard.board.GetLength(0) && delta.y + startPosition.y < internBoard.board.GetLength(1) && delta.x + startPosition.x >= 0 && delta.y + startPosition.y >= 0)
                {
                    Vector2Int endPosition = new Vector2Int(delta.x + startPosition.x, delta.y + startPosition.y);
                    if (internBoard.board[endPosition.x, endPosition.y].GetType() != typeof(Pieces.None))
                        break;
                    legalMoves.Add(new Move(startPosition, endPosition));
                    delta += moveOffset;
                }
            }
            return legalMoves;
        }
        private static List<Move> GenerateNonSliding(Vector2Int startPosition, Pieces piece, Board.Intern internBoard)
        {
            List<Move> legalMoves = new List<Move>();
            for (int i = 0; i < piece.internPiece.moveOffsets.Count; i++)
            {
                Vector2Int moveOffset = piece.internPiece.moveOffsets[i];
                if (startPosition.x + moveOffset.x < internBoard.board.GetLength(0) && startPosition.y + moveOffset.y < internBoard.board.GetLength(1) && startPosition.x + moveOffset.x >= 0 && startPosition.y + moveOffset.y >= 0)
                {
                    if (internBoard.board[startPosition.x, startPosition.y].internPiece.isWhite != internBoard.board[startPosition.x + moveOffset.x, startPosition.y + moveOffset.y].internPiece.isWhite | internBoard.board[startPosition.x + moveOffset.x, startPosition.y + moveOffset.y].GetType() == typeof(Pieces.None))
                    {
                        Vector2Int endPosition = new Vector2Int(startPosition.x + moveOffset.x, startPosition.y + moveOffset.y);
                        legalMoves.Add(new Move(startPosition, endPosition));
                    }
                }
            }
            return legalMoves;
        }
    }
    
}
