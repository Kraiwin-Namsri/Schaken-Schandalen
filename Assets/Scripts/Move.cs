using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class MoveManager
{
    public class Move
    {
        public Vector2Int startPosition;
        public Vector2Int endPosition;
        public Move appendedMove;

        public bool isCapture;

        public Move(Vector2Int startPosition, Vector2Int endPosition, Board.Intern board, Move appendMove = null)
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
                if (move == moveToCheck) { return true; }
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
                Piece mainPiece = internBoard.board[currentMove.startPosition.x, currentMove.startPosition.y];
                do
                {
                    if (Legal.IsLegal(currentMove, internBoard) | !legalOnly)
                    {
                        Piece buffer1 = internBoard.board[currentMove.startPosition.x, currentMove.startPosition.y];
                        Piece buffer2 = internBoard.board[currentMove.endPosition.x, currentMove.endPosition.y];

                        UpdateIsFirstMove(buffer1);

<<<<<<< Updated upstream
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
                        legalMoves.Add(new Move(startPosition, endPosition, internBoard));
=======
                        buffer1.internPiece.position = currentMove.endPosition;
                        if (currentMove.isCapture)
                        {
                            Piece buffer3 = new Piece.None(internBoard.board[currentMove.startPosition.x, currentMove.startPosition.y].board);
                            buffer3.CreateExtern(buffer1.board.externBoard.board);

                            buffer3.internPiece.position = currentMove.startPosition;

                            internBoard.board[buffer1.internPiece.position.x, buffer1.internPiece.position.y] = buffer1;
                            internBoard.board[buffer3.internPiece.position.x, buffer3.internPiece.position.y] = buffer3;
                            internBoard.captured.Add(buffer2);
                        }
                        else
                        {
                            buffer2.internPiece.position = currentMove.startPosition;

                            internBoard.board[buffer1.internPiece.position.x, buffer1.internPiece.position.y] = buffer1;
                            internBoard.board[buffer2.internPiece.position.x, buffer2.internPiece.position.y] = buffer2;
                        }
>>>>>>> Stashed changes
                    }

                    currentMove = currentMove.appendedMove;
                } while (currentMove is not null);
                UpdateCastleAbility(mainPiece, internBoard);
                internBoard.fenManager.BoardToFen(internBoard);
            }
            moves.Clear();
        }
        private static void UpdateIsFirstMove(Piece piece)
        {
            if ((piece.GetType() == typeof(Piece.White_Pawn) | piece.GetType() == typeof(Piece.Black_Pawn)))
            {
                if (piece.internPiece.isFirstMove)
                {
                    piece.internPiece.moveOffsets.RemoveAt(1);
                }
            }
            piece.internPiece.isFirstMove = false;
        }
        private static void UpdateCastleAbility(Piece piece, Board.Intern internboard)
        {
            if (piece.GetType() == typeof(Piece.White_King))
            {
                internboard.castleAbility.whiteKingSide = false;
                internboard.castleAbility.whiteQueenSide = false;
            }
            else if (piece.GetType() == typeof(Piece.Black_King))
            {
                internboard.castleAbility.blackKingSide = false;
                internboard.castleAbility.blackQueenSide = false;
            }
            else if (piece.GetType() == typeof(Piece.White_Rook))
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
            else if (piece.GetType() == typeof(Piece.Black_Rook))
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
    }

    
}
}