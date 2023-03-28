using Microsoft.MixedReality.Toolkit;
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
    public Move appendedMove;

    public bool isCapture;

    public Move(Vector2Int startPosition, Vector2Int endPosition, Board.Intern board, Move appendMove=null)
    {
        this.startPosition = startPosition;
        this.endPosition = endPosition;
        this.isCapture = Legal.isCapture(this, board);
        this.appendedMove = appendMove;
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
            Move currentMove = move;
            //To Do: Find better name
            Pieces mainPiece = internBoard.board[currentMove.startPosition.x, currentMove.startPosition.y];
            do 
            { 
                if (Legal.IsLegal(currentMove, internBoard) | !legalOnly)
                {
                    Pieces buffer1 = internBoard.board[currentMove.startPosition.x, currentMove.startPosition.y];
                    Pieces buffer2 = internBoard.board[currentMove.endPosition.x, currentMove.endPosition.y];

                    UpdateIsFirstMove(buffer1);

                    buffer1.internPiece.position = currentMove.endPosition;
                    if (currentMove.isCapture)
                    {
                        Pieces buffer3 = new Pieces.None(internBoard.board[currentMove.startPosition.x, currentMove.startPosition.y].board);
                        buffer3.CreateExtern(buffer1.board.externBoard.board);

                        buffer3.internPiece.position = currentMove.startPosition;

                        internBoard.board[buffer1.internPiece.position.x, buffer1.internPiece.position.y] = buffer1;
                        internBoard.board[buffer3.internPiece.position.x, buffer3.internPiece.position.y] = buffer3;
                        internBoard.captured.Add(buffer2);
                        Board.Intern.UpdateFenstringRequirements(internBoard, true, "None", 0, 0, 0);
                        GameManager.UpdatePedestal();
                    }
                    else
                    {
                        buffer2.internPiece.position = currentMove.startPosition;

                        internBoard.board[buffer1.internPiece.position.x, buffer1.internPiece.position.y] = buffer1;
                        internBoard.board[buffer2.internPiece.position.x, buffer2.internPiece.position.y] = buffer2;
                        Board.Intern.UpdateFenstringRequirements(internBoard, false, buffer1.ToString(), buffer1.internPiece.position.y, buffer2.internPiece.position.y, buffer2.internPiece.position.x);
                    }
                }
                UpdateCastleAbility(mainPiece, internBoard, currentMove);
                currentMove = currentMove.appendedMove;
            } while (currentMove is not null);
            Board.Intern.Fen.BoardToFen(internBoard, internBoard.enPassantCordsString);
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
    private static void UpdateCastleAbility(Pieces piece, Board.Intern internboard, Move currentMove)
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
            if (new Vector2(currentMove.startPosition.x, currentMove.startPosition.y) == new Vector2(7, 7))
            {
                internboard.castleAbility.whiteKingSide = false;
            }
            else if (new Vector2(currentMove.startPosition.x, currentMove.startPosition.y) == new Vector2(0, 7))
            {
                internboard.castleAbility.whiteQueenSide = false;
            }
        }
        else if (piece.GetType() == typeof(Pieces.Black_Rook))
        {
            if (new Vector2(currentMove.startPosition.x, currentMove.startPosition.y) == new Vector2(0, 0))
            {
                internboard.castleAbility.blackQueenSide = false;
            }
            else if (new Vector2(currentMove.startPosition.x, currentMove.startPosition.y) == new Vector2(7, 0))
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
                        piece.internPiece.legalMoves = GeneratePieceMoves(piece, new Move(startPosition, startPosition, internBoard), internBoard);
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

            if (piece.GetType() == typeof(Pieces.White_King) | piece.GetType() == typeof(Pieces.Black_King) | piece.GetType() == typeof(Pieces.White_Rook) | piece.GetType() == typeof(Pieces.Black_Rook))
            {
                pieceMoves = pieceMoves.Concat(GenerateCastling(move.startPosition, piece, internBoard)).ToList();
            }

            if (piece.GetType() == typeof(Pieces.White_Pawn) | piece.GetType() == typeof(Pieces.Black_Pawn))
            {
                pieceMoves = pieceMoves.Concat(GeneratePawnCapture(move.startPosition, piece, internBoard)).ToList();
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
                    legalMoves.Add(new Move(startPosition, endPosition, internBoard));
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

                        if (internBoard.board[startPosition.x, startPosition.y].GetType() == typeof(Pieces.White_Pawn) || internBoard.board[startPosition.x, startPosition.y].GetType() == typeof(Pieces.Black_Pawn))
                        {
                            if (internBoard.board[startPosition.x + moveOffset.x, startPosition.y + moveOffset.y].GetType() != typeof(Pieces.None))
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

        private static List<Move> GenerateCastling(Vector2Int startPosition, Pieces piece, Board.Intern internBoard)
        {
            List<Move> legalMoves = new List<Move>();
            if (internBoard.castleAbility.whiteKingSide)
            {
                if (piece.GetType() == typeof(Pieces.White_King))
                {
                    legalMoves.Add(new Move(new Vector2Int(4, 7), new Vector2Int(6,7), internBoard, new Move(new Vector2Int(7, 7), new Vector2Int(5, 7), internBoard)));
                }
                else if (piece.GetType() == typeof(Pieces.White_Rook))
                {
                    legalMoves.Add(new Move(new Vector2Int(7, 7), new Vector2Int(5, 7), internBoard));
                }
            }
            if (internBoard.castleAbility.whiteQueenSide)
            {
                if (piece.GetType() == typeof(Pieces.White_King))
                {
                    legalMoves.Add(new Move(new Vector2Int(4, 7), new Vector2Int(2, 7), internBoard, new Move(new Vector2Int(0, 7), new Vector2Int(3, 7), internBoard)));
                }
                else if (piece.GetType() == typeof(Pieces.White_Rook))
                {
                    legalMoves.Add(new Move(new Vector2Int(0, 7), new Vector2Int(3, 7), internBoard));
                }
            }
            if (internBoard.castleAbility.blackKingSide)
            {
                if (piece.GetType() == typeof(Pieces.Black_King))
                {
                    legalMoves.Add(new Move(new Vector2Int(4, 0), new Vector2Int(6, 0), internBoard, new Move(new Vector2Int(7, 0), new Vector2Int(5, 0), internBoard)));
                }
                else if (piece.GetType() == typeof(Pieces.Black_Rook))
                {
                    legalMoves.Add(new Move(new Vector2Int(7, 0), new Vector2Int(5, 0), internBoard));
                }
            }
            if (internBoard.castleAbility.blackQueenSide)
            {
                if (piece.GetType() == typeof(Pieces.Black_King))
                {
                    legalMoves.Add(new Move(new Vector2Int(4, 0), new Vector2Int(2, 0), internBoard, new Move(new Vector2Int(0, 0), new Vector2Int(3, 0), internBoard)));
                }
                else if (piece.GetType() == typeof(Pieces.Black_Rook))
                {
                    legalMoves.Add(new Move(new Vector2Int(0, 0), new Vector2Int(3, 0), internBoard));
                }
            }
            return legalMoves;
        }

        private static List<Move> GeneratePawnCapture(Vector2Int startPosition, Pieces piece, Board.Intern internBoard)
        {
            List<Move> legalMoves = new List<Move>();
            if (piece.GetType() == typeof(Pieces.Black_Pawn))
            {
                Vector2Int blackOffset1 = new Vector2Int(1, 1);
                Vector2Int blackOffset2 = new Vector2Int(-1, 1);

                Vector2Int blackEndPosition1 = startPosition + blackOffset1;
                Vector2Int blackEndPosition2 = startPosition + blackOffset2;

                if (blackEndPosition1.x >= 0 && blackEndPosition1.y >= 0 && blackEndPosition1.x < internBoard.board.GetLength(0) && blackEndPosition1.y < internBoard.board.GetLength(1))
                {
                    if (internBoard.board[blackEndPosition1.x, blackEndPosition1.y].GetType() != typeof(Pieces.None))
                    {
                        legalMoves.Add(new Move(startPosition, blackEndPosition1, internBoard));
                    }
                }
                if (blackEndPosition2.x >= 0 && blackEndPosition2.y >= 0 && blackEndPosition2.x < internBoard.board.GetLength(0) && blackEndPosition2.y < internBoard.board.GetLength(1))
                {
                    if (internBoard.board[blackEndPosition2.x, blackEndPosition2.y].GetType() != typeof(Pieces.None))
                    {
                        legalMoves.Add(new Move(startPosition, blackEndPosition2, internBoard));
                    }
                }
            }
            else if (piece.GetType() == typeof(Pieces.White_Pawn))
            {
                Vector2Int whiteOffset1 = new Vector2Int(-1, -1);
                Vector2Int whiteOffset2 = new Vector2Int(1, -1);

                Vector2Int whiteEndPosition1 = startPosition + whiteOffset1;
                Vector2Int whiteEndPosition2 = startPosition + whiteOffset2;

                if (whiteEndPosition1.x >= 0 && whiteEndPosition1.y >= 0 && whiteEndPosition1.x < internBoard.board.GetLength(0) && whiteEndPosition1.y < internBoard.board.GetLength(1))
                {
                    if (internBoard.board[whiteEndPosition1.x, whiteEndPosition1.y].GetType() != typeof(Pieces.None))
                    {
                        legalMoves.Add(new Move(startPosition, whiteEndPosition1, internBoard));
                    }
                }
                if (whiteEndPosition2.x >= 0 && whiteEndPosition2.y >= 0 && whiteEndPosition2.x < internBoard.board.GetLength(0) && whiteEndPosition2.y < internBoard.board.GetLength(1))
                {
                    if (internBoard.board[whiteEndPosition2.x, whiteEndPosition2.y].GetType() != typeof(Pieces.None))
                    {
                        legalMoves.Add(new Move(startPosition, whiteEndPosition2, internBoard));
                    }
                }
            }
            return legalMoves;
        }
    }
}