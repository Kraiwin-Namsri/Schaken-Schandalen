using JetBrains.Annotations;
using Microsoft.MixedReality.Toolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class Board : MonoBehaviour
{
    public Intern internBoard;
    public Extern externBoard;
    public Board()
    {
        internBoard = new Intern(this);
        externBoard = new Extern(this);
    }
    public class Intern
    {
        public Piece[,] board = new Piece[8, 8];
        public bool whiteToMove;
        public CastleAbility castleAbility;
        public EnPassant enPassant;
        public int halfMoveCounter;
        public int halfMoveClockCounter;
        public int fullMoveCounter;

        public List<Move> moves = new List<Move>();
        public List<Piece> captured = new List<Piece>();
        
        public Fen fenManager;

        public Intern(Board board)
        {
            fenManager = new Fen();
            fenManager.Apply(board, "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        }

        public void AddMove(Move move)
        {
            moves.Add(move);
            Move.ExecuteMoves(moves, this, true);

        }
        public bool IsInsideBounds(Vector2Int position)
        {
            return position.x >= 0 && position.y >= 0 && position.x < board.GetLength(0) && position.y < board.GetLength(1);
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
            public bool forWhite;
            public int column;
            public EnPassant(bool forWhite, int column) {
                this.forWhite = forWhite;
                this.column = column;
            }
        }
    }
    public class Extern
    {
        public static GameObject PREFAB_externBoard;

        public GameObject board;
        public GameObject playSurface;
        public GameObject pedestalPlaySurface;
        public GameObject piecesParent;

        public Vector3 boardOrigin;
        public Vector3 boardScale;
        
        public Extern(Board board) 
        {
            PREFAB_externBoard.SetActive(false);
            this.board = Instantiate(PREFAB_externBoard);
            this.board.SetActive(true);
            this.playSurface = this.board.transform.GetChild(0).gameObject;
            this.pedestalPlaySurface = this.board.transform.GetChild(3).GetChild(0).gameObject;
            this.piecesParent = this.board.transform.GetChild(2).gameObject;
            ClearPieces();

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    board.internBoard.board[x, y].CreateExtern(this.piecesParent);
                }
            }
        }

        private void ClearPieces()
        {
            while (piecesParent.transform.childCount > 0)
            {
                DestroyImmediate(piecesParent.transform.GetChild(0).gameObject);
            }
        }
    }
}