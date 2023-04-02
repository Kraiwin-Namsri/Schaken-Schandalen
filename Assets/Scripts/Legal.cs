using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;
using static Board.Intern;
using static Legal;

public class Legal
{
    public Board.Intern board;
    public CastleAbility castleAbility;
    public Remise remise;
    public Check check;
    public EnPassant enPassant;
    public bool whiteToMove;
    public int gameState; // -1 Nothing 0 remise, 1 white won, 2 black won
    public int halfMoveCounter = 0;
    public int halfMoveClockCounter = 0;
    public int fullMoveCounter = 1;
    public int inCheck; //-1 No-one, 0 white, 1 black
    public List<string> gamePositions;
    public Legal(Board.Intern board)
    {
        this.board = board;
        castleAbility = new CastleAbility(this);
        remise = new Remise(this);
        enPassant = new EnPassant(this);
        check = new Check(this);
    }
    public bool IsOpponentsMove(Move bestMove, Player player)
    {
        if (board.array[bestMove.startPosition.x, bestMove.startPosition.y].GetColor() == typeof(Piece.White) && player.isPlayingWhite == false)
        {
            return false;
        }
        //if the piece is black and player1 is black 
        if(board.array[bestMove.startPosition.x, bestMove.startPosition.y].GetColor() != typeof(Piece.White) && player.isPlayingWhite == false)
        {
            return false;
        }
        //if the piece is white and player1 is black
        if (board.array[bestMove.startPosition.x, bestMove.startPosition.y].GetColor() == typeof(Piece.White) && player.isPlayingWhite != false)
        {
            return true;
        }
        //if the piece is black and player1 is white
        if (board.array[bestMove.startPosition.x, bestMove.startPosition.y].GetColor() != typeof(Piece.White) && player.isPlayingWhite == true)
        {
            return true;
        }
        return false;
    }
    public bool IsCapture(Move move)
    {
        Piece piece1 = board.array[move.startPosition.x, move.startPosition.y];
        Piece piece2 = board.array[move.endPosition.x, move.endPosition.y];
        bool endPositionEmpty = piece2.GetColor() == typeof(Piece.None);
        bool differentColor = piece1.GetColor() != piece2.GetColor();
        return (!endPositionEmpty) && differentColor;
    }
    public bool IsLegal(Move move)
    {
        bool endPositionFree = IsPositionFree(move.endPosition);

        Piece movedPiece = board.array[move.startPosition.x, move.startPosition.y];
        Update(movedPiece.intern, move.startPosition);
        bool legalPos = Move.Contains(movedPiece.intern.legalMoves, move);
        return (endPositionFree & legalPos);
    }
    public bool IsPositionFree(Vector2Int position)
    {
        Piece piece = board.array[position.x, position.y];
        bool positionEmpty = piece.GetColor() == typeof(Piece.None);
        return positionEmpty;
    }

    public void Update(Piece.Intern piece, Vector2Int startPosition)
    {
        piece.legalMoves = new List<Move>();
        if (piece.IsSliding())
        {
            GenerateSliding(piece, startPosition);
        }
        else
        {
            GenerateNonSliding(piece, startPosition);
        }


        if (piece.GetType() == typeof(Piece.White.King) | piece.GetType() == typeof(Piece.Black.King) | piece.GetType() == typeof(Piece.White.Rook) | piece.GetType() == typeof(Piece.Black.Rook))
        {
            castleAbility.Generate(piece);

            if (piece.GetType() == typeof(Piece.White.Pawn) | piece.GetType() == typeof(Piece.Black.Pawn))
            {
                GeneratePawnCapture(piece, startPosition);
                // Does pawn check work?
                enPassant.Generate(piece, startPosition);
            }

            inCheck = -1;
            Move kingCapture;
            kingCapture = new Move(startPosition, board.whiteKing.intern.position);
            if (Move.Contains(piece.legalMoves, kingCapture) && piece.GetColor() == typeof(Piece.Black))
            {
                inCheck = 0;
            }
            kingCapture = new Move(startPosition, board.blackKing.intern.position);
            if (Move.Contains(piece.legalMoves, kingCapture) && piece.GetColor() == typeof(Piece.White))
            {
                inCheck = 1;
            }
        }
    }
    void GenerateSliding(Piece.Intern piece, Vector2Int startPosition)
    {
        for (int i = 0; i < piece.moveOffsets.Count; i++)
        {
            Vector2Int moveOffset = piece.moveOffsets[i];
            Vector2Int delta = new Vector2Int(moveOffset.x, moveOffset.y);


            while (delta.x + startPosition.x < board.array.GetLength(0) && delta.y + startPosition.y < board.array.GetLength(1) && delta.x + startPosition.x >= 0 && delta.y + startPosition.y >= 0)
            {
                Vector2Int endPosition = new Vector2Int(delta.x + startPosition.x, delta.y + startPosition.y);
                if (board.array[endPosition.x, endPosition.y].GetType() != typeof(Piece.None))
                    break;
                piece.legalMoves.Add(new Move(startPosition, endPosition));
                delta += moveOffset;
            }
        }
    }
    void GenerateNonSliding(Piece.Intern piece, Vector2Int startPosition)
    {
        for (int i = 0; i < piece.moveOffsets.Count; i++)
        {
            Vector2Int moveOffset = piece.moveOffsets[i];
            if (startPosition.x + moveOffset.x < board.array.GetLength(0) && startPosition.y + moveOffset.y < board.array.GetLength(1) && startPosition.x + moveOffset.x >= 0 && startPosition.y + moveOffset.y >= 0)
            {
                if (board.array[startPosition.x, startPosition.y].GetColor() != board.array[startPosition.x + moveOffset.x, startPosition.y + moveOffset.y].GetColor() | board.array[startPosition.x + moveOffset.x, startPosition.y + moveOffset.y].GetType() == typeof(Piece.None))
                {
                    Vector2Int endPosition = new Vector2Int(startPosition.x + moveOffset.x, startPosition.y + moveOffset.y);

                    if (board.array[startPosition.x, startPosition.y].GetType() == typeof(Piece.White.Pawn) || board.array[startPosition.x, startPosition.y].GetType() == typeof(Piece.Black.Pawn))
                    {
                        if (board.array[startPosition.x + moveOffset.x, startPosition.y + moveOffset.y].GetType() != typeof(Piece.None))
                        {

                        }
                        else
                        {
                            piece.legalMoves.Add(new Move(startPosition, endPosition));
                        }
                    }
                    else
                    {
                        piece.legalMoves.Add(new Move(startPosition, endPosition));
                    }
                }
            }
        }
    }
    void GeneratePawnCapture(Piece.Intern piece, Vector2Int startPosition)
    {
        if (piece.GetType() == typeof(Piece.Black.Pawn))
        {
            Vector2Int blackOffset1 = new Vector2Int(1, 1);
            Vector2Int blackOffset2 = new Vector2Int(-1, 1);

            Vector2Int blackEndPosition1 = startPosition + blackOffset1;
            Vector2Int blackEndPosition2 = startPosition + blackOffset2;

            if (blackEndPosition1.x >= 0 && blackEndPosition1.y >= 0 && blackEndPosition1.x < board.array.GetLength(0) && blackEndPosition1.y < board.array.GetLength(1))
            {
                if (board.array[blackEndPosition1.x, blackEndPosition1.y].GetType() != typeof(Piece.None))
                {
                    piece.legalMoves.Add(new Move(startPosition, blackEndPosition1));
                }
            }
            if (blackEndPosition2.x >= 0 && blackEndPosition2.y >= 0 && blackEndPosition2.x < board.array.GetLength(0) && blackEndPosition2.y < board.array.GetLength(1))
            {
                if (board.array[blackEndPosition2.x, blackEndPosition2.y].GetType() != typeof(Piece.None))
                {
                    piece.legalMoves.Add(new Move(startPosition, blackEndPosition2));
                }
            }
        }
        else if (piece.GetType() == typeof(Piece.White.Pawn))
        {
            Vector2Int whiteOffset1 = new Vector2Int(-1, -1);
            Vector2Int whiteOffset2 = new Vector2Int(1, -1);

            Vector2Int whiteEndPosition1 = startPosition + whiteOffset1;
            Vector2Int whiteEndPosition2 = startPosition + whiteOffset2;

            if (whiteEndPosition1.x >= 0 && whiteEndPosition1.y >= 0 && whiteEndPosition1.x < board.array.GetLength(0) && whiteEndPosition1.y < board.array.GetLength(1))
            {
                if (board.array[whiteEndPosition1.x, whiteEndPosition1.y].GetType() != typeof(Piece.None))
                {
                    piece.legalMoves.Add(new Move(startPosition, whiteEndPosition1));
                }
            }
            if (whiteEndPosition2.x >= 0 && whiteEndPosition2.y >= 0 && whiteEndPosition2.x < board.array.GetLength(0) && whiteEndPosition2.y < board.array.GetLength(1))
            {
                if (board.array[whiteEndPosition2.x, whiteEndPosition2.y].GetType() != typeof(Piece.None))
                {
                    piece.legalMoves.Add(new Move(startPosition, whiteEndPosition2));
                }
            }
        }
    }
    
    public class CastleAbility
    {
        public Legal legal;
        public bool whiteKingSide;
        public bool whiteQueenSide;
        public bool blackKingSide;
        public bool blackQueenSide;
        public CastleAbility(Legal legal)
        {
            this.legal = legal;
        }
        public void Generate(Piece.Intern piece)
        {
            if (whiteKingSide)
            {
                if (piece.GetType() == typeof(Piece.White.King))
                {
                    piece.legalMoves.Add(new Move(new Vector2Int(4, 7), new Vector2Int(6, 7), new Move(new Vector2Int(7, 7), new Vector2Int(5, 7))));
                }
                else if (piece.GetType() == typeof(Piece.White.Rook))
                {
                    piece.legalMoves.Add(new Move(new Vector2Int(7, 7), new Vector2Int(5, 7)));
                }
            }
            if (whiteQueenSide)
            {
                if (piece.GetType() == typeof(Piece.White.King))
                {
                    piece.legalMoves.Add(new Move(new Vector2Int(4, 7), new Vector2Int(2, 7), new Move(new Vector2Int(0, 7), new Vector2Int(3, 7))));
                }
                else if (piece.GetType() == typeof(Piece.White.Rook))
                {
                    piece.legalMoves.Add(new Move(new Vector2Int(0, 7), new Vector2Int(3, 7)));
                }
            }
            if (blackKingSide)
            {
                if (piece.GetType() == typeof(Piece.Black.King))
                {
                    piece.legalMoves.Add(new Move(new Vector2Int(4, 0), new Vector2Int(6, 0), new Move(new Vector2Int(7, 0), new Vector2Int(5, 0))));
                }
                else if (piece.GetType() == typeof(Piece.Black.Rook))
                {
                    piece.legalMoves.Add(new Move(new Vector2Int(7, 0), new Vector2Int(5, 0)));
                }
            }
            if (blackQueenSide)
            {
                if (piece.GetType() == typeof(Piece.Black.King))
                {
                    piece.legalMoves.Add(new Move(new Vector2Int(4, 0), new Vector2Int(2, 0), new Move(new Vector2Int(0, 0), new Vector2Int(3, 0))));
                }
                else if (piece.GetType() == typeof(Piece.Black.Rook))
                {
                    piece.legalMoves.Add(new Move(new Vector2Int(0, 0), new Vector2Int(3, 0)));
                }
            }
        }
        public void Update(Piece piece, Move currentMove)
        {
            if (piece.GetType() == typeof(Piece.White.King))
            {
                whiteKingSide = false;
                whiteQueenSide = false;
            }
            else if (piece.GetType() == typeof(Piece.Black.King))
            {
                blackKingSide = false;
                blackQueenSide = false;
            }
            else if (piece.GetType() == typeof(Piece.White.Rook))
            {
                if (new Vector2(currentMove.startPosition.x, currentMove.startPosition.y) == new Vector2(7, 7))
                {
                    whiteKingSide = false;
                }
                else if (new Vector2(currentMove.startPosition.x, currentMove.startPosition.y) == new Vector2(0, 7))
                {
                    whiteQueenSide = false;
                }
            }
            else if (piece.GetType() == typeof(Piece.Black.Rook))
            {
                if (new Vector2(currentMove.startPosition.x, currentMove.startPosition.y) == new Vector2(0, 0))
                {
                    blackQueenSide = false;
                }
                else if (new Vector2(currentMove.startPosition.x, currentMove.startPosition.y) == new Vector2(7, 0))
                {
                    blackKingSide = false;
                }
            }

        }
        //What does this function do?
        void Update2(Piece piece, Board.Intern internboard)
        {
            if (piece.GetType() == typeof(Piece.White.King))
            {
                whiteKingSide = false;
                whiteQueenSide = false;
            }
            else if (piece.GetType() == typeof(Piece.Black.King))
            {
                blackKingSide = false;
                blackQueenSide = false;
            }
            else if (piece.GetType() == typeof(Piece.White.Rook))
            {
                if (piece.intern.position == new Vector2(7, 7))
                {
                    whiteKingSide = false;
                }
                else if (piece.intern.position == new Vector2(0, 7))
                {
                    whiteQueenSide = false;
                }
            }
            else if (piece.GetType() == typeof(Piece.Black.Rook))
            {
                if (piece.intern.position == new Vector2(0, 0))
                {
                    blackQueenSide = false;
                }
                else if (piece.intern.position == new Vector2(7, 0))
                {
                    blackKingSide = false;
                }
            }

        }
    }
    public class EnPassant
    {
        public Legal legal;
        public Vector2Int coordinate;
        public EnPassant(Legal legal)
        {
            this.legal = legal;
        }
        public List<Move> Generate(Piece.Intern piece, Vector2Int startPosition)
        {
            List<Move> legalMoves = new List<Move>();
            bool isWhitePawn = piece.GetColor() == typeof(Piece.White);
            if (isWhitePawn)
            {
                bool isPawnOnSide = coordinate.y + 1 == startPosition.y && (coordinate.x + 1 == startPosition.x | coordinate.x - 1 == startPosition.x);
                if (isPawnOnSide)
                {
                    Move capture = new Move(startPosition, new Vector2Int(coordinate.x, coordinate.y + 1));
                    legalMoves.Add(new Move(startPosition, coordinate, capture));
                }
            }
            else
            {
                bool isPawnOnSide = coordinate.y - 1 == startPosition.y && (coordinate.x + 1 == startPosition.x | coordinate.x - 1 == startPosition.x);
                if (isPawnOnSide)
                {
                    Move capture = new Move(startPosition, new Vector2Int(coordinate.x, coordinate.y - 1));
                    legalMoves.Add(new Move(startPosition, coordinate, capture));
                }
            }
            return legalMoves;
        }

        public void Update(Piece piece, Move move)
        {
            if (piece.GetType() == typeof(Piece.White.Pawn) || piece.GetType() == typeof(Piece.Black.Pawn))
            {
                coordinate.y = (move.startPosition.y + move.endPosition.y) / 2;
                coordinate.x = move.startPosition.x;
                return;
            }
            coordinate = new Vector2Int(-1, -1);
        }
    }

    public class Remise
    {
        Legal legal;
        public Remise(Legal legal) {
            this.legal = legal;
        }
        public void Is(Board.Intern internboard)
        {
            string fen = internboard.fenManager.BoardToFen();
            StalesMate(fen, internboard);
            FiftyMoveRule(internboard);
            InsuficientMaterial(fen, internboard);
            RepeatedPosition(fen, internboard);
        }
        public void StalesMate(string fenstring, Board.Intern internboard)
        {

        }

        public void FiftyMoveRule(Board.Intern internboard)
        {
            if (legal.halfMoveClockCounter >= 50)
            {
                legal.gameState = 0;
            }
        }
        public void InsuficientMaterial(string fenstring, Board.Intern internboard)
        {
            //Hoeveelheid stukken buiten de koningen
            int piecesCount = fenstring.Count(f => f == 'Q' || f == 'q' || f == 'R' || f == 'r' || f == 'B' || f == 'b' || f == 'N' || f == 'n' || f == 'P' || f == 'p');
            int bishopCount = fenstring.Count(f => f == 'B' || f == 'b');
            int knightCount = fenstring.Count(f => f == 'N' || f == 'n');

            if (piecesCount <= 2)
            {
                if (knightCount == 2)
                {
                    legal.gameState = 0;
                }
                else if (bishopCount == 2)
                {
                    legal.gameState = 0;
                }
                else if (bishopCount == 1 && knightCount == 1)
                {
                    legal.gameState = 0;
                }
            }
        }
        public void RepeatedPosition(string fenstring, Board.Intern internboard)
        {
            string newFenstring = Regex.Replace(fenstring.Split()[0], @" ", "");
            legal.gamePositions.Add(newFenstring);

            int occurences = 0;
            foreach (string gamePosition in legal.gamePositions)
            {
                string newGamePosition = Regex.Replace(gamePosition.Split()[0], @" ", "");
                if (newFenstring == newGamePosition)
                {
                    occurences++;
                }
            }
            if (occurences >= 3)
            {
                legal.gameState = 0;
            }
        }
        public void UpdateHalfMoveClock(Move move)
        {
            if (legal.IsCapture(move))
            {
                legal.halfMoveClockCounter = 0;
            }
            else
            {
                legal.halfMoveClockCounter++;
            }
        }
        public void UpdateHalfMove()
        {
            legal.halfMoveCounter++;
        }
    }
    public class Check
    {
        Legal legal;
        public Check(Legal legal)
        {
            this.legal = legal;
        }
        public bool Is(Move move, bool fromWhite)
        {
            if (fromWhite) {
                return legal.board.blackKing.intern.position == move.endPosition;
            }
            else
            {
                return legal.board.whiteKing.intern.position == move.endPosition;
            }
        }
    }
}