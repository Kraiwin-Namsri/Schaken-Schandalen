using JetBrains.Annotations;
using Microsoft.MixedReality.Toolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UIElements;


public class Board
{
    public Intern intern;
    public Extern @extern;
    public Board()
    {
        intern = new Intern(this);
        @extern = new Extern(this);
    }
    public class Intern
    {
        public Board board;
        public Piece[,] array;
        public bool whiteToMove;
        public int gameState; // -1 Nothing 0 remise, 1 white won, 2 black won
        public CastleAbility castleAbility;
        public EnPassant enPassant;
        public int halfMoveCounter = 0;
        public int halfMoveClockCounter = 0;
        public int fullMoveCounter = 1;
        public Fen fenManager;

        public List<string> gamePositions;

        public Intern(Board board)
        {
            this.board = board;
            array = new Piece[8, 8];    
            fenManager = new Fen(board);
        }
        public void UpdateHalfMoveClock(bool isCapture)
        {
            if (isCapture)
            {
                halfMoveClockCounter = 0;
            }
            else 
            {
                halfMoveClockCounter++;
            }
        }
        public void UpdateHalfMove()
        {
            halfMoveCounter++;
        }
        public void UpdateCastleAbility(Piece piece, Move currentMove)
        {
            if (piece.GetType() == typeof(Piece.White.King))
            {
                castleAbility.whiteKingSide = false;
                castleAbility.whiteQueenSide = false;
            }
            else if (piece.GetType() == typeof(Piece.Black.King))
            {
                castleAbility.blackKingSide = false;
                castleAbility.blackQueenSide = false;
            }
            else if (piece.GetType() == typeof(Piece.White.Rook))
            {
                if (new Vector2(currentMove.startPosition.x, currentMove.startPosition.y) == new Vector2(7, 7))
                {
                    castleAbility.whiteKingSide = false;
                }
                else if (new Vector2(currentMove.startPosition.x, currentMove.startPosition.y) == new Vector2(0, 7))
                {
                    castleAbility.whiteQueenSide = false;
                }
            }
            else if (piece.GetType() == typeof(Piece.Black.Rook))
            {
                if (new Vector2(currentMove.startPosition.x, currentMove.startPosition.y) == new Vector2(0, 0))
                {
                    castleAbility.blackQueenSide = false;
                }
                else if (new Vector2(currentMove.startPosition.x, currentMove.startPosition.y) == new Vector2(7, 0))
                {
                    castleAbility.blackKingSide = false;
                }
            }

        }
        public void UpdateEnPassant(Piece piece, Move move)
        {
            if (piece.GetType() == typeof(Piece.White.Pawn) | piece.GetType() == typeof(Piece.Black.Pawn))
            {
                enPassant.coordinate.y = (move.startPosition.y + move.endPosition.y) / 2;
                enPassant.coordinate.x = move.startPosition.x;
            }
            enPassant.coordinate = new Vector2Int(-1,-1);
        }
        public bool IsInsideBounds(Vector2Int position)
        {
            return position.x >= 0 && position.y >= 0 && position.x < array.GetLength(0) && position.y < array.GetLength(1);
        }
        public struct CastleAbility
        {
            public bool whiteKingSide;
            public bool whiteQueenSide;
            public bool blackKingSide;
            public bool blackQueenSide;
            public CastleAbility(bool whiteKingSide, bool whiteQueenSide, bool blackKingSide, bool blackQueenSide)
            {
                this.whiteKingSide = whiteKingSide;
                this.whiteQueenSide = whiteQueenSide;
                this.blackKingSide = blackKingSide;
                this.blackQueenSide = blackQueenSide;
            }
        }
        public struct EnPassant
        {
            public Vector2Int coordinate;   
            public EnPassant(Vector2Int coordinate) {
                this.coordinate = coordinate;
            }
        }
        public Vector3 ToExtern(Vector2Int internDestination)
        {
            Vector3 parentSize = board.@extern.boardPlaySurfaceGameObject.GetComponent<MeshFilter>().mesh.bounds.size;
            Vector3 externDestination = (((Vector2)internDestination * (Vector2)parentSize / 8) + ((Vector2)parentSize / 16) - ((Vector2)parentSize / 2));
            externDestination.z = 0;
            return externDestination;
        }
    }
    public class Extern
    {
        public Board board;
        public GameObject boardGameObject;
        public GameObject boardPlaySurfaceGameObject;
        public GameObject pieces;


        public Vector3 boardOrigin;
        public Vector3 boardScale;
        
        public Extern(Board board)
        {
            this.board = board;
            boardGameObject = Prefab.Instantiate(Prefab.ChessBoard, null);
            boardPlaySurfaceGameObject = boardGameObject.GetNamedChild("PlaySurface");
            pieces = boardGameObject.GetNamedChild("Pieces");
        }

        public void Update(GameManager gameManager)
        {
            for (int y = 0; y < board.intern.array.GetLength(1); y++)
            {
                for (int x = 0; x < board.intern.array.GetLength(0); x++)
                {
                    Piece piece = board.intern.array[x, y];
                    gameManager.StartCoroutine(piece.@extern.Update(0.1f));
                }
            }
        }
        public Vector2Int ToIntern(Vector3 externPosition)
        {
            Vector3 parentSize = board.@extern.boardPlaySurfaceGameObject.GetComponent<MeshFilter>().mesh.bounds.size;
            Vector2 internPosition = (((Vector2)externPosition) - ((Vector2)parentSize / 16) + ((Vector2)parentSize / 2)) / ((Vector2)parentSize / 8);
            return Vector2Int.RoundToInt(internPosition);
        }
    }
}