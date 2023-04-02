using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Piece
{
    private static Dictionary<GameObject, Piece> lookupTable = new Dictionary<GameObject, Piece>();
    public Board board;
    public Intern intern;
    public Extern @extern;
    public static Piece Lookup(GameObject gameobject)
    {
        if (lookupTable.ContainsKey(gameobject))
        {
            return lookupTable[gameobject];
        }
        return null;
    }
    public Piece(Board board)
    {
        this.board = board;
    }
    public Type GetColor()
    {
        if (GetType().IsSubclassOf(typeof(White)))
        {
            return typeof(White);
        }
        if (GetType().IsSubclassOf(typeof(Black)))
        {
            return typeof(Black);
        }
        return typeof(None);
    }

    public class None : Piece
    {
        public None(Board board) : base (board)
        {
            List<Vector2Int> moveOffsets = new List<Vector2Int>();
            intern = new Intern(this, moveOffsets);
            @extern = new Extern(this, Prefab.None, board);
        }
    }
    public class White : Piece {
        public White(Board board) : base(board)
        {

        }
        public class King : White
        {
            public bool isFirstMove = true;
            public King(Board board) : base(board)
            {
                List<Vector2Int> moveOffsets = new List<Vector2Int>()
                {
                    new Vector2Int(0, 1),
                    new Vector2Int(1, 1),
                    new Vector2Int(1, 0),
                    new Vector2Int(1, -1),
                    new Vector2Int(0, -1),
                    new Vector2Int(-1, -1),
                    new Vector2Int(-1, 0),
                    new Vector2Int(-1, 1)
                };
                intern = new Intern(this, moveOffsets);
                @extern = new Extern(this, Prefab.WhiteKing, board);
                board.intern.whiteKing = this;
            }
        }
        public class Queen : White
        {
            public Queen(Board board) : base(board)
            {
                List<Vector2Int> moveOffsets = new List<Vector2Int>()
                {
                    new Vector2Int(0, 1),
                    new Vector2Int(1, 1),
                    new Vector2Int(1, 0),
                    new Vector2Int(1, -1),
                    new Vector2Int(0, -1),
                    new Vector2Int(-1, -1),
                    new Vector2Int(-1, 0),
                    new Vector2Int(-1, 1)
                };
                intern = new Intern(this, moveOffsets);
                @extern = new Extern(this, Prefab.WhiteQueen, board);
            }
        }
        public class Rook : White
        {
            public bool isFirstMove = true;
            public Rook(Board board) : base(board)
            {
                List<Vector2Int> moveOffsets = new List<Vector2Int>()
                {
                    new Vector2Int(0, 1),
                    new Vector2Int(1, 0),
                    new Vector2Int(0, -1),
                    new Vector2Int(-1, 0)
                };
                intern = new Intern(this, moveOffsets);
                @extern = new Extern(this, Prefab.WhiteRook, board);
            }
        }
        public class Bischop : White
        {
            public Bischop(Board board) : base(board)
            {
                List<Vector2Int> moveOffsets = new List<Vector2Int>()
                {
                    new Vector2Int(1, 1),
                    new Vector2Int(1, -1),
                    new Vector2Int(-1, -1),
                    new Vector2Int(-1, 1)
                };
                intern = new Intern(this, moveOffsets);
                @extern = new Extern(this, Prefab.WhiteBischop, board);
            }
        }
        public class Knight : White
        {
            public Knight(Board board) : base(board)
            {
                List<Vector2Int> moveOffsets = new List<Vector2Int>()
                {
                    new Vector2Int(1, 2),
                    new Vector2Int(2, -1),
                    new Vector2Int(2, 1),
                    new Vector2Int(1, -2),
                    new Vector2Int(-1, -2),
                    new Vector2Int(-2, -1),
                    new Vector2Int(-2, 1),
                    new Vector2Int(-1, 2)
                };
                intern = new Intern(this, moveOffsets);   
                @extern = new Extern(this, Prefab.WhiteKnight, board);
            }
        }
        public class Pawn : White
        {
            public bool isFirstMove = true;
            public Pawn(Board board) : base(board)
            {
                List<Vector2Int> moveOffsets = new List<Vector2Int>()
                {
                    new Vector2Int(0, -1),
                    new Vector2Int(0, -2)
                };
                intern = new Intern(this, moveOffsets);
                @extern = new Extern(this, Prefab.WhitePawn, board);
            }
            public void RemoveDoublePawnPush()
            {
                intern.moveOffsets.Remove(new Vector2Int(0, -2));
            }
        }
    }
    public class Black : Piece {
        public Black(Board board) : base(board)
        {

        }
        public class King : Black
        {
            public bool isFirstMove = true;
            public King(Board board) : base(board)
            {
                List<Vector2Int> moveOffsets = new List<Vector2Int>()
                {
                    new Vector2Int(0, 1),
                    new Vector2Int(1, 1),
                    new Vector2Int(1, 0),
                    new Vector2Int(1, -1),
                    new Vector2Int(0, -1),
                    new Vector2Int(-1, -1),
                    new Vector2Int(-1, 0),
                    new Vector2Int(-1, 1)
                };
                intern = new Intern(this, moveOffsets);
                @extern = new Extern(this, Prefab.BlackKing, board);
                board.intern.blackKing = this;
            }
        }
        public class Queen : Black
        {
            public Queen(Board board) : base(board)
            {
                List<Vector2Int> moveOffsets = new List<Vector2Int>()
                {
                    new Vector2Int(0, 1),
                    new Vector2Int(1, 1),
                    new Vector2Int(1, 0),
                    new Vector2Int(1, -1),
                    new Vector2Int(0, -1),
                    new Vector2Int(-1, -1),
                    new Vector2Int(-1, 0),
                    new Vector2Int(-1, 1)
                };
                intern = new Intern(this, moveOffsets);
                @extern = new Extern(this, Prefab.BlackQueen, board);
            }
        }
        public class Rook : Black
        {
            public bool isFirstMove = true;
            public Rook(Board board) : base(board)
            {
                List<Vector2Int> moveOffsets = new List<Vector2Int>()
                {
                    new Vector2Int(0, 1),
                    new Vector2Int(1, 0),
                    new Vector2Int(0, -1),
                    new Vector2Int(-1, 0)
                };
                intern = new Intern(this, moveOffsets);
                @extern = new Extern(this, Prefab.BlackRook, board);
            }
        }
        public class Bischop : Black
        {
            public Bischop(Board board) : base(board)
            {
                List<Vector2Int> moveOffsets = new List<Vector2Int>() 
                {
                    new Vector2Int(1, 1),
                    new Vector2Int(1, -1),
                    new Vector2Int(-1, -1),
                    new Vector2Int(-1, 1)
                };
                intern = new Intern(this, moveOffsets);
                @extern = new Extern(this, Prefab.BlackBischop, board);

            }
        }
        public class Knight : Black
        {
            public Knight(Board board) : base(board)
            {
                List<Vector2Int> moveOffsets = new List<Vector2Int>() 
                    {
                    new Vector2Int(1, 2),
                    new Vector2Int(2, -1),
                    new Vector2Int(2, 1),
                    new Vector2Int(1, -2),
                    new Vector2Int (-1, -2),
                    new Vector2Int(-2, -1),
                    new Vector2Int(-2, 1),
                    new Vector2Int(-1, 2)
                    };
                intern = new Intern(this, moveOffsets);
                @extern = new Extern(this, Prefab.BlackKnight, board);
            }
        }
        public class Pawn : Black
        {
            public Pawn(Board board) : base(board)
            {
                List<Vector2Int> moveOffsets = new List<Vector2Int>() 
                {
                    new Vector2Int(0,1),
                    new Vector2Int(0, 2)
                };
                intern = new Intern(this, moveOffsets);
                @extern = new Extern(this, Prefab.BlackPawn, board);
            }
            public void RemoveDoublePawnPush()
            {
                intern.moveOffsets.Remove(new Vector2Int(0,2));
            }
        }
    }
    public class Intern
    {
        Piece piece;
        public Vector2Int position;
        public readonly List<Vector2Int> moveOffsets;
        public List<Move> legalMoves;
        public Intern(Piece piece, List<Vector2Int> moveOffsets)
        {
            this.piece = piece;
            this.moveOffsets = moveOffsets;
        }
        public bool IsSliding()
        {
            if (piece.GetType() == typeof(White.Bischop) | piece.GetType() == typeof(Black.Bischop))
            {
                return true;
            }
            if (piece.GetType() == typeof(White.Rook) | piece.GetType() == typeof(Black.Rook))
            {
                return true;
            }
            if (piece.GetType() == typeof(White.Queen) | piece.GetType() == typeof(Black.Queen))
            {
                return true;
            }
            return false;
        }
        public Type GetColor()
        {
            if (GetType().IsSubclassOf(typeof(White)))
            {
                return typeof(White);
            }
            if (GetType().IsSubclassOf(typeof(Black)))
            {
                return typeof(Black);
            }
            return typeof(None);
        }
    }
    public class Extern
    {
        public Piece piece;
        public Board board;
        public GameObject pieceGameObject;
        public Extern(Piece piece, GameObject prefabPiece, Board board)
        {
            this.piece = piece;
            this.board = board;
            pieceGameObject = Prefab.Instantiate(prefabPiece, board.@extern.boardGameObject.transform);
            lookupTable.Add(pieceGameObject, piece);
        }
        //To Do also smoothly return to standard rotation
        public IEnumerator Update(float journeyTime)
        {
            Vector3 destination = board.intern.ToExtern(this.piece.intern.position);
            float startTime = Time.time;
            Vector3 startPosition = pieceGameObject.transform.localPosition;
            float fracComplete = (Time.time - startTime) / journeyTime;

            while (fracComplete < 1f)
            {
                fracComplete = (Time.time - startTime) / journeyTime;
                pieceGameObject.transform.localPosition = Vector3.Slerp(startPosition, destination, fracComplete);
                yield return null;
            }
            pieceGameObject.transform.localPosition = destination;
            yield return null;
        }
    }
}
