using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
    public Move(Vector2Int startPosition, Vector2Int endPosition, bool isLegal=false)
    {
        this.startPosition = startPosition;
        this.endPosition = endPosition;
        this.isLegal = isLegal;
    }
    
    //For comparing Moves
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

    public static void ExecuteMoves(List<Move> moves, Board.Intern internBoard, bool legalOnly)
    {
        foreach (Move move in moves)
        {
            if (move.isLegal | !legalOnly)
            {
                Pieces buffer1 = internBoard.board[move.startPosition.x, move.startPosition.y];
                Pieces buffer2 = internBoard.board[move.endPosition.x, move.endPosition.y];
                
                UpdateIsFirstMove(buffer1);
                UpdateCastleAbility(buffer1, internBoard);

                buffer1.internPiece.position = move.endPosition;
                if (move.isCapture)
                {
                    Pieces buffer3 = new Pieces.None(internBoard.board[move.startPosition.x, move.startPosition.y].board);
                    buffer3.CreateExtern(buffer1.board.externBoard.board);

                    buffer3.internPiece.position = move.startPosition;

                    internBoard.board[buffer1.internPiece.position.x, buffer1.internPiece.position.y] = buffer1;
                    internBoard.board[buffer3.internPiece.position.x, buffer3.internPiece.position.y] = buffer3;
                    internBoard.captured.Add(buffer2);
                    GameManager.UpdatePedestal();
                }
                else
                {
                    buffer2.internPiece.position = move.startPosition;

                    internBoard.board[buffer1.internPiece.position.x, buffer1.internPiece.position.y] = buffer1;
                    internBoard.board[buffer2.internPiece.position.x, buffer2.internPiece.position.y] = buffer2;
                }
            }
        }
        moves.Clear();
    }
    private static void UpdateIsFirstMove(Pieces piece)
    {
        if ((piece.GetType() == typeof(Pieces.White_Pawn) | piece.GetType() == typeof(Pieces.Black_Pawn)))
        {
            if (piece.internPiece.isFirstMove)
            {
                piece.internPiece.moveOffsets.RemoveAt(1);
            }
        }
        piece.internPiece.isFirstMove = false;
    }
    private static void UpdateCastleAbility(Pieces piece, Board.Intern internboard)
    {
        if (piece.GetType() == typeof(Pieces.White_King))
        {
            internboard.castleAbility.whiteKingSide = false;
            internboard.castleAbility.whiteQueenSide = false;
        }
        else if (piece.GetType() == typeof(Pieces.Black_King))
        {
            internboard.castleAbility.blackKingSide = false;
            internboard.castleAbility.blackQueenSide = false;
        }
        else if (piece.GetType() == typeof(Pieces.White_Rook))
        {
            if (piece.internPiece.position == new Vector2(7, 7))
            {
                internboard.castleAbility.whiteKingSide = false;
            }
            else if (piece.internPiece.position == new Vector2(0, 7))
            {
                internboard.castleAbility.whiteQueenSide = false;
            }
        }
        else if (piece.GetType() == typeof(Pieces.Black_Rook))
        {
            if (piece.internPiece.position == new Vector2(0, 0))
            {
                internboard.castleAbility.blackQueenSide = false;
            }
            else if (piece.internPiece.position == new Vector2(7, 0))
            {
                internboard.castleAbility.blackKingSide = false;
            }
        }

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
                        Vector2Int startPosition = new Vector2Int(x, y);
                        piece.internPiece.legalMoves = GeneratePieceMoves(piece, new Move(startPosition, startPosition), internBoard);
                    }
                }
            }
        }

        // All the boolean Functions
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
            
            bool legalPos = Move.Contains(GeneratePieceMoves(movedPiece, move, internBoard), move);

            return (endPositionFree & legalPos);
        }
        public static bool IsEndPositionFree(Move move, Board.Intern internBoard)
        {
            bool endPositionEmpty = internBoard.board[move.endPosition.x, move.endPosition.y].GetType() == typeof(Pieces.None);
            bool differentColor = internBoard.board[move.startPosition.x, move.startPosition.y].internPiece.isWhite != internBoard.board[move.endPosition.x, move.endPosition.y].internPiece.isWhite;
            return endPositionEmpty | differentColor;
        }

        // All the functions that return a list
        private static List<Move> GeneratePieceMoves(Pieces piece, Move move, Board.Intern internBoard)
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
            if ((piece.GetType() == typeof(Pieces.White_King) | piece.GetType() == typeof(Pieces.Black_King) | piece.GetType() == typeof(Pieces.White_Rook) | piece.GetType() == typeof(Pieces.Black_Rook)))
            {
                pieceMoves = pieceMoves.Concat(GenerateCastling(move.startPosition, piece, internBoard)).ToList();
            }
            return pieceMoves;

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

        private static List<Move> GenerateCastling(Vector2Int startPosition, Pieces piece, Board.Intern internBoard)
        {
            List<Move> legalMoves = new List<Move>();
            if (internBoard.castleAbility.whiteKingSide)
            {
                if (piece.GetType() == typeof(Pieces.White_King) | piece.GetType() == typeof(Pieces.White_Rook))
                {
                    legalMoves.Add(new Move(new Vector2Int(4, 7), new Vector2Int(6,7), true));
                    legalMoves.Add(new Move(new Vector2Int(7, 7), new Vector2Int(5, 7), true));
                }
            }
            if (internBoard.castleAbility.whiteQueenSide)
            {
                if (piece.GetType() == typeof(Pieces.White_King) | piece.GetType() == typeof(Pieces.White_Rook))
                {
                    legalMoves.Add(new Move(new Vector2Int(4, 7), new Vector2Int(3, 7), true));
                    legalMoves.Add(new Move(new Vector2Int(0, 7), new Vector2Int(2, 7), true));
                }
            }
            if (internBoard.castleAbility.blackKingSide)
            {
                if (piece.GetType() == typeof(Pieces.Black_King) | piece.GetType() == typeof(Pieces.Black_Rook))
                {
                    legalMoves.Add(new Move(new Vector2Int(4, 0), new Vector2Int(6, 0), true));
                    legalMoves.Add(new Move(new Vector2Int(7, 0), new Vector2Int(5, 0), true));
                }
            }
            if (internBoard.castleAbility.blackQueenSide)
            {
                if (piece.GetType() == typeof(Pieces.Black_King) | piece.GetType() == typeof(Pieces.White_Rook))
                {
                    legalMoves.Add(new Move(new Vector2Int(4, 0), new Vector2Int(2, 0), true));
                    legalMoves.Add(new Move(new Vector2Int(0, 0), new Vector2Int(3, 0), true));
                }
            }
            return legalMoves;
        }
    }
    
}
