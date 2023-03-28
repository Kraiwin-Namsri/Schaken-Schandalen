using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Piece
{
    public static GameObject none;

    public static GameObject PREFAB_whiteking;
    public static GameObject PREFAB_whitequeen;
    public static GameObject PREFAB_whiterook;
    public static GameObject PREFAB_whitebischop;
    public static GameObject PREFAB_whiteknight;
    public static GameObject PREFAB_whitepawn;

    public static GameObject PREFAB_blackking;
    public static GameObject PREFAB_blackqueen;
    public static GameObject PREFAB_blackrook;
    public static GameObject PREFAB_blackbischop;
    public static GameObject PREFAB_blackknight;
    public static GameObject PREFAB_blackpawn;

    private static Dictionary<GameObject, Piece> lookupTable;

    public Intern intern;
    public Extern @extern;

    public static Piece Lookup(GameObject gameobject)
    {
        if (Piece.lookupTable.ContainsKey(gameobject))
        {
            return Piece.lookupTable[gameobject];
        }
        return null;
    }
    public Type GetColor()
    {
        if (this.GetType().IsSubclassOf(typeof(White)))
        {
            return typeof(White);
        }
        if (this.GetType().IsSubclassOf(typeof(Black)))
        {
            return typeof(Black);
        }
        return typeof(None);
    }
    public class None : Piece
    {
        public None(Board board)
        {
            List<Vector2Int> moveOffsets = new List<Vector2Int>();
            intern = new Intern(moveOffsets);
            @extern = new Extern(none, board.@extern);
        }
    }
    public class White : Piece {
        public class King : White
        {
            public King(Board board)
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
                intern = new Intern(moveOffsets);
                @extern = new Extern(PREFAB_whiteking, board.@extern);
            }
        }
        public class Queen : White
        {
            public Queen(Board board)
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
                intern = new Intern(moveOffsets);
                @extern = new Extern(PREFAB_whitequeen, board.@extern);
            }
        }
        public class Rook : White
        {
            public Rook(Board board)
            {
                List<Vector2Int> moveOffsets = new List<Vector2Int>()
                {
                    new Vector2Int(0, 1),
                    new Vector2Int(1, 0),
                    new Vector2Int(0, -1),
                    new Vector2Int(-1, 0)
                };
                intern = new Intern(moveOffsets);
                @extern = new Extern(PREFAB_whiterook, board.@extern);
            }
        }
        public class Bischop : White
        {
            public Bischop(Board board)
            {
                List<Vector2Int> moveOffsets = new List<Vector2Int>()
                {
                    new Vector2Int(1, 1),
                    new Vector2Int(1, -1),
                    new Vector2Int(-1, -1),
                    new Vector2Int(-1, 1)
                };
                intern = new Intern(moveOffsets);
                @extern = new Extern(PREFAB_whiterook, board.@extern);
            }
        }
        public class Knight : White
        {
            public Knight(Board board)
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
                intern = new Intern(moveOffsets);
                @extern = new Extern(PREFAB_whiteknight, board.@extern);
            }
        }
        public class Pawn : White
        {
            public bool isFirstMove = true;
            public Pawn(Board board)
            {
                List<Vector2Int> moveOffsets = new List<Vector2Int>()
                {
                    new Vector2Int(0, -1),
                    new Vector2Int(0, -2)
                };
                intern = new Intern(moveOffsets);
                @extern = new Extern(PREFAB_whitepawn, board.@extern);
            }
        }
    }
    public class Black : Piece {
        public class King : Black
        {
            public King(Board board)
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
                intern = new Intern(moveOffsets);
            }
        }
        public class Queen : Black
        {
            public Queen(Board board)
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
                intern = new Intern(moveOffsets);
            }
        }
        public class Rook : Black
        {
            public Rook(Board board)
            {
                List<Vector2Int> moveOffsets = new List<Vector2Int>()
                {
                    new Vector2Int(0, 1),
                    new Vector2Int(1, 0),
                    new Vector2Int(0, -1),
                    new Vector2Int(-1, 0)
                };
                intern = new Intern(moveOffsets);
            }
        }
        public class Bischop : Black
        {
            public Bischop(Board board)
            {
                List<Vector2Int> moveOffsets = new List<Vector2Int>() 
                {
                    new Vector2Int(1, 1),
                    new Vector2Int(1, -1),
                    new Vector2Int(-1, -1),
                    new Vector2Int(-1, 1)
                };
                intern = new Intern(moveOffsets);
            }
        }
        public class Knight : Black
        {
            public Knight(Board board)
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
                intern = new Intern(moveOffsets);
            }
        }
        public class Pawn : Black
        {
            public bool isFirstMove = true;
            public Pawn(Board board)
            {
                List<Vector2Int> moveOffsets = new List<Vector2Int>() 
                {
                    new Vector2Int(0,1),
                    new Vector2Int(0, 2)
                };
                intern = new Intern(moveOffsets);
            }
        }
    }
    public class Intern
    {
        public Vector2Int position;
        public readonly List<Vector2Int> moveOffsets;
        public List<Move> legalMoves;
        public Intern(List<Vector2Int> moveOffsets)
        {
            this.moveOffsets = moveOffsets;
        }
    }
    public class Extern
    {
        public GameObject pieceGameObject;
        public Extern(GameObject prefabPiece, Board.Extern @extern)
        {
            pieceGameObject = MonoBehaviour.Instantiate(prefabPiece, @extern.board.transform);
            pieceGameObject.SetActive(true);
        }
        public void Move(Vector3 destination)
        {
            pieceGameObject.transform.localPosition = destination;
            pieceGameObject.transform.localRotation = Quaternion.identity;
        }
        //To Do also smoothly return to standard rotation
        public IEnumerator SmoothMove(Vector3 destination, float journeyTime)
        {
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
