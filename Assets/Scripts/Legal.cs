using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using Unity.Mathematics;
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
    public List<string> gamePositions;
    public Legal(Board.Intern board)
    {
        this.board = board;
        castleAbility = new CastleAbility(this);
        remise = new Remise(this);
        enPassant = new EnPassant(this);
        check = new Check(this);
    }
    public static bool IsCapture(Move move, Piece[,] array)
    {
        Piece piece1 = array[move.startPosition.x, move.startPosition.y];
        Piece piece2 = array[move.endPosition.x, move.endPosition.y];
        bool differentColor = piece1.GetColor() != piece2.GetColor();
        bool endPositionEmpty = piece2.GetType() == typeof(Piece.None);
        return (!endPositionEmpty) && differentColor;
    }
    public bool IsCapture(Move move)
    {
        Piece piece1 = board.array[move.startPosition.x, move.startPosition.y];
        Piece piece2 = board.array[move.endPosition.x, move.endPosition.y];
        bool differentColor = piece1.GetColor() != piece2.GetColor();
        bool endPositionEmpty = piece2.GetType() == typeof(Piece.None);
        return (!endPositionEmpty) && differentColor;
    }
    public bool IsLegal(Move move)
    {
        bool endPositionFree = IsPositionFree(move.endPosition);

        Piece movedPiece = board.array[move.startPosition.x, move.startPosition.y];
        Update(movedPiece, move.startPosition);
        bool legalPos = Move.Contains(movedPiece.intern.legalMoves, move);
        return (endPositionFree & legalPos);
    }
    public bool IsPositionFree(Vector2Int position)
    {
        Piece piece = board.array[position.x, position.y];
        bool positionEmpty = piece.GetColor() == typeof(Piece.None);
        return positionEmpty;
    }
    public void Update(Piece piece, Vector2Int startPosition)
    {
        piece.intern.legalMoves = new List<Move>();
        if (piece.intern.IsSliding())
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
        }

        if (piece.GetType() == typeof(Piece.White.Pawn) | piece.GetType() == typeof(Piece.Black.Pawn))
        {
            GeneratePawnCapture(piece, startPosition);
            enPassant.Generate(piece, startPosition);
        }

        check.Update(piece);
    }
    void GenerateSliding(Piece piece, Vector2Int startPosition)
    {
        foreach(Vector2Int moveOffset in piece.intern.moveOffsets)
        {
            Vector2Int endPosition = startPosition + moveOffset;
            while (board.IsInsideBounds(endPosition))
            {
                if (board.array[endPosition.x, endPosition.y].GetColor() != piece.GetColor())
                    piece.intern.legalMoves.Add(new Move(startPosition, endPosition));
                if (board.array[endPosition.x, endPosition.y].GetColor() == piece.GetColor())
                    break;
                if (board.array[endPosition.x, endPosition.y].GetType() != typeof(Piece.None))
                    break;
                endPosition = endPosition + moveOffset;
            }
        }
    }
    void GenerateNonSliding(Piece piece, Vector2Int startPosition)
    {
        foreach (Vector2Int moveOffset in piece.intern.moveOffsets)
        {
            Vector2Int endPosition = startPosition + moveOffset;
            if (board.IsInsideBounds(endPosition))
            {
                if (board.array[endPosition.x, endPosition.y].GetColor() != piece.GetColor())
                    piece.intern.legalMoves.Add(new Move(startPosition, endPosition));
            }
        }
    }

    void GeneratePawnCapture(Piece piece, Vector2Int startPosition)
    {
        if (piece.GetType() == typeof(Piece.Black.Pawn))
        {
            Vector2Int blackEndPosition = startPosition + new Vector2Int(1, 1);
            if (board.IsInsideBounds(blackEndPosition))
            {
                if (board.array[blackEndPosition.x, blackEndPosition.y].GetColor() != piece.GetColor())
                    if (board.array[blackEndPosition.x, blackEndPosition.y].GetType() != typeof(Piece.None))
                        piece.intern.legalMoves.Add(new Move(startPosition, blackEndPosition));
            }
            
            blackEndPosition = startPosition + new Vector2Int(-1, 1);
            if (board.IsInsideBounds(blackEndPosition))
            {
                if (board.array[blackEndPosition.x, blackEndPosition.y].GetColor() != piece.GetColor())
                    if (board.array[blackEndPosition.x, blackEndPosition.y].GetType() != typeof(Piece.None))
                        piece.intern.legalMoves.Add(new Move(startPosition, blackEndPosition));
            }
        }

        if (piece.GetType() == typeof(Piece.White.Pawn))
        {
            Vector2Int whiteEndPosition = startPosition + new Vector2Int(-1, -1);
            if (board.IsInsideBounds(whiteEndPosition))
            {
                if (board.array[whiteEndPosition.x, whiteEndPosition.y].GetColor() != piece.GetColor())
                    if (board.array[whiteEndPosition.x, whiteEndPosition.y].GetType() != typeof(Piece.None))
                        piece.intern.legalMoves.Add(new Move(startPosition, whiteEndPosition));
            }

            whiteEndPosition = startPosition + new Vector2Int(1, -1);
            if (board.IsInsideBounds(whiteEndPosition))
            {   
                if (board.array[whiteEndPosition.x, whiteEndPosition.y].GetColor() != piece.GetColor())
                    if (board.array[whiteEndPosition.x, whiteEndPosition.y].GetType() != typeof(Piece.None))
                        piece.intern.legalMoves.Add(new Move(startPosition, whiteEndPosition));
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
        private bool isEmptyBetween(Vector2Int position1, Vector2Int position2)
        {
            for (int i = position1.x+1; i < position2.x-1; i++)
            {
                if (legal.board.array[i, position1.y].GetType() != typeof(Piece.None))
                    return false;
            }
            return true;
        }
        public void Generate(Piece piece)
        {
            if (whiteKingSide)
            {
                if (piece.GetType() == typeof(Piece.White.King))
                {
                    Move rookMove = new Move(new Vector2Int(7, 7), new Vector2Int(5, 7));
                    Move kingMove = new Move(new Vector2Int(4, 7), new Vector2Int(6, 7), rookMove);
                    if (isEmptyBetween(new Vector2Int(4, 7), new Vector2Int(7, 7)))
                        piece.intern.legalMoves.Add(kingMove);
                }
            }
            if (whiteQueenSide)
            {
                if (piece.GetType() == typeof(Piece.White.King))
                {
                    Move rookMove = new Move(new Vector2Int(0, 7), new Vector2Int(3, 7));
                    Move kingMove = new Move(new Vector2Int(4, 7), new Vector2Int(2, 7), rookMove);
                    if (isEmptyBetween(new Vector2Int(0, 7), new Vector2Int(4, 7)))
                        piece.intern.legalMoves.Add(kingMove);
                }
            }
            if (blackKingSide)
            {
                if (piece.GetType() == typeof(Piece.Black.King))
                {
                    Move rookMove = new Move(new Vector2Int(7, 0), new Vector2Int(5, 0));
                    Move kingMove = new Move(new Vector2Int(4, 0), new Vector2Int(6, 0), rookMove);
                    if (isEmptyBetween(new Vector2Int(4, 0), new Vector2Int(7, 0)))
                        piece.intern.legalMoves.Add(kingMove);
                }
            }
            if (blackQueenSide)
            {
                if (piece.GetType() == typeof(Piece.Black.King))
                {
                    Move rookMove = new Move(new Vector2Int(0, 0), new Vector2Int(3, 0));
                    Move kingMove = new Move(new Vector2Int(4, 0), new Vector2Int(2, 0), rookMove);
                    if (isEmptyBetween(new Vector2Int(0, 0), new Vector2Int(4, 0)))
                        piece.intern.legalMoves.Add(kingMove);
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
    }
    public class EnPassant
    {
        public Legal legal;
        public Vector2Int coordinate;
        public EnPassant(Legal legal)
        {
            this.legal = legal;
        }
        public void Generate(Piece piece, Vector2Int startPosition)
        {
            if (piece.GetType() == typeof(Piece.White.Pawn))
            {
                bool isPawnOnSide = coordinate.y + 1 == startPosition.y && (coordinate.x + 1 == startPosition.x | coordinate.x - 1 == startPosition.x);
                if (isPawnOnSide)
                {
                    Move capture = new Move(startPosition, new Vector2Int(coordinate.x, coordinate.y + 1));
                    piece.intern.legalMoves.Add(new Move(startPosition, coordinate, capture));
                }
            }
            else if (piece.GetType() == typeof(Piece.Black.Pawn))
            {
                bool isPawnOnSide = coordinate.y - 1 == startPosition.y && (coordinate.x + 1 == startPosition.x | coordinate.x - 1 == startPosition.x);
                if (isPawnOnSide)
                {
                    Move capture = new Move(startPosition, new Vector2Int(coordinate.x, coordinate.y - 1));
                    piece.intern.legalMoves.Add(new Move(startPosition, coordinate, capture));
                }
            }
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
            legal.gamePositions.Add(fenstring);

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
        public void UpdateHalfMove()
        {
            legal.halfMoveCounter++;
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
        public void UpdateFullMove()
        {
            legal.halfMoveCounter = (int)math.floor(legal.halfMoveCounter/2);
        }
    }
    public class Check
    {
        Legal legal;
        public int inCheck; //-1 No-one, 0 white, 1 black
        public Check(Legal legal)
        {
            this.legal = legal;
        }
        public void Update(Piece piece)
        {
            if (piece.GetColor() == typeof(Piece.White)) {
                Move kingCapture = new Move(piece.intern.position, legal.board.blackKing.intern.position);
                if (piece.intern.legalMoves.Contains(kingCapture))
                {
                    inCheck = 1;
                } else
                {
                    inCheck = -1;
                }
            }
            else
            {
                Move kingCapture = new Move(piece.intern.position, legal.board.whiteKing.intern.position);
                if (piece.intern.legalMoves.Contains(kingCapture))
                {
                    inCheck = 0;
                } else
                {
                    inCheck = -1;
                }
            }
        }
    }
}