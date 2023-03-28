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

    private static Dictionary<GameObject, Piece> lookupTable = new Dictionary<GameObject, Piece>();

    public Board board;
    public Piece pieceObject;
    public Piece.Intern internPiece;
    public Piece.Extern externPiece;
    private GameObject prefabPiece;

    public Piece(Board board)
    {
        pieceObject = this;
        this.board = board;
    }
    public void CreateExtern(GameObject parent)
    {
        this.externPiece = new Piece.Extern(this, parent);
        lookupTable.Add(this.externPiece.pieceGameObject, this);
    }
    public class None : Piece
    {
        public None(Board parent) : base (parent)
        {
            internPiece = new Piece.Intern(this);
            prefabPiece = GameManager.instance.NONE;
        }
    }
    public class White_King : Piece
    {
        public White_King(Board parent) : base(parent)
        {
            internPiece = new Piece.Intern(this);
            internPiece.isWhite = true;

            internPiece.isSlidingPiece = false;
            internPiece.moveOffsets.Add(new Vector2Int(0, 1));
            internPiece.moveOffsets.Add(new Vector2Int(1, 1));
            internPiece.moveOffsets.Add(new Vector2Int(1, 0));
            internPiece.moveOffsets.Add(new Vector2Int(1, -1));
            internPiece.moveOffsets.Add(new Vector2Int(0, -1));
            internPiece.moveOffsets.Add(new Vector2Int(-1, -1));
            internPiece.moveOffsets.Add(new Vector2Int(-1, 0));
            internPiece.moveOffsets.Add(new Vector2Int(-1, 1));

            prefabPiece = GameManager.instance.WHITEKING;
        }
    }
    public class White_Queen : Piece
    {
        public White_Queen(Board parent) : base(parent)
        {
            internPiece = new Piece.Intern(this);
            internPiece.isWhite = true;


            internPiece.isSlidingPiece = true;
            internPiece.moveOffsets.Add(new Vector2Int(0, 1));
            internPiece.moveOffsets.Add(new Vector2Int(1, 1));
            internPiece.moveOffsets.Add(new Vector2Int(1, 0));
            internPiece.moveOffsets.Add(new Vector2Int(1, -1));
            internPiece.moveOffsets.Add(new Vector2Int(0, -1));
            internPiece.moveOffsets.Add(new Vector2Int(-1, -1));
            internPiece.moveOffsets.Add(new Vector2Int(-1, 0));
            internPiece.moveOffsets.Add(new Vector2Int(-1, 1));

            prefabPiece = GameManager.instance.WHITEQUEEN;
        }
    }
    public class White_Rook : Piece
    {
        public White_Rook(Board parent) : base(parent)
        {
            internPiece = new Piece.Intern(this);
            internPiece.isWhite = true;


            internPiece.isSlidingPiece = true;
            internPiece.moveOffsets.Add(new Vector2Int(0, 1));
            internPiece.moveOffsets.Add(new Vector2Int(1, 0));
            internPiece.moveOffsets.Add(new Vector2Int(0, -1));
            internPiece.moveOffsets.Add(new Vector2Int(-1, 0));

            prefabPiece = GameManager.instance.WHITEROOK;
        }
    }
    public class White_Bischop : Piece
    {
        public White_Bischop(Board parent) : base(parent)
        {
            internPiece = new Piece.Intern(this);
            internPiece.isWhite = true;


            internPiece.isSlidingPiece = true;
            internPiece.moveOffsets.Add(new Vector2Int(1, 1));
            internPiece.moveOffsets.Add(new Vector2Int(1, -1));
            internPiece.moveOffsets.Add(new Vector2Int(-1, -1));
            internPiece.moveOffsets.Add(new Vector2Int(-1, 1));

            prefabPiece = GameManager.instance.WHITEBISCHOP;
        }
    }
    public class White_Knight : Piece
    {
        public White_Knight(Board parent) : base(parent)
        {
            internPiece = new Piece.Intern(this);
            internPiece.isWhite = true;


            internPiece.isSlidingPiece = false;
            internPiece.moveOffsets.Add(new Vector2Int(1, 2));
            internPiece.moveOffsets.Add(new Vector2Int(2, -1));
            internPiece.moveOffsets.Add(new Vector2Int(2, 1));
            internPiece.moveOffsets.Add(new Vector2Int(1, -2));
            internPiece.moveOffsets.Add(new Vector2Int(-1, -2));
            internPiece.moveOffsets.Add(new Vector2Int(-2, -1));
            internPiece.moveOffsets.Add(new Vector2Int(-2, 1));
            internPiece.moveOffsets.Add(new Vector2Int(-1, 2));

            prefabPiece = GameManager.instance.WHITEKNIGHT;
        }
    }
    public class White_Pawn : Piece
    {
        public White_Pawn(Board parent) : base(parent)
        {
            internPiece = new Piece.Intern(this);
            internPiece.isWhite = true;

            internPiece.isSlidingPiece = false;
            internPiece.moveOffsets.Add(new Vector2Int(0, -1));
            internPiece.moveOffsets.Add(new Vector2Int(0, -2));

            prefabPiece = GameManager.instance.WHITEPAWN;
        }
    }
    public class Black_King : Piece
    {
        public Black_King(Board parent) : base(parent)
        {
            internPiece = new Piece.Intern(this);
            internPiece.isWhite = false;


            internPiece.isSlidingPiece = false;
            internPiece.moveOffsets.Add(new Vector2Int(0, 1));
            internPiece.moveOffsets.Add(new Vector2Int(1, 1));
            internPiece.moveOffsets.Add(new Vector2Int(1, 0));
            internPiece.moveOffsets.Add(new Vector2Int(1, -1));
            internPiece.moveOffsets.Add(new Vector2Int(0, -1));
            internPiece.moveOffsets.Add(new Vector2Int(-1, -1));
            internPiece.moveOffsets.Add(new Vector2Int(-1, 0));
            internPiece.moveOffsets.Add(new Vector2Int(-1, 1));

            prefabPiece = GameManager.instance.BLACKKING;
        }
    }
    public class Black_Queen : Piece
    {
        public Black_Queen(Board parent) : base(parent)
        {
            internPiece = new Piece.Intern(this);
            internPiece.isWhite = false;

            internPiece.isSlidingPiece = true;
            internPiece.moveOffsets.Add(new Vector2Int(0, 1));
            internPiece.moveOffsets.Add(new Vector2Int(1, 1));
            internPiece.moveOffsets.Add(new Vector2Int(1, 0));
            internPiece.moveOffsets.Add(new Vector2Int(1, -1));
            internPiece.moveOffsets.Add(new Vector2Int(0, -1));
            internPiece.moveOffsets.Add(new Vector2Int(-1, -1));
            internPiece.moveOffsets.Add(new Vector2Int(-1, 0));
            internPiece.moveOffsets.Add(new Vector2Int(-1, 1));

            prefabPiece = GameManager.instance.BLACKQUEEN;
        }
    }
    public class Black_Rook : Piece
    {
        public Black_Rook(Board parent) : base(parent)
        {
            internPiece = new Piece.Intern(this);
            internPiece.isWhite = false;

            internPiece.isSlidingPiece = true;
            internPiece.moveOffsets.Add(new Vector2Int(0, 1));
            internPiece.moveOffsets.Add(new Vector2Int(1, 0));
            internPiece.moveOffsets.Add(new Vector2Int(0, -1));
            internPiece.moveOffsets.Add(new Vector2Int(-1, 0));

            prefabPiece = GameManager.instance.BLACKROOK;
        }
    }
    public class Black_Bischop : Piece
    {
        public Black_Bischop(Board parent) : base(parent)
        {
            internPiece = new Piece.Intern(this);
            internPiece.isWhite = false;

            internPiece.isSlidingPiece = true;
            internPiece.moveOffsets.Add(new Vector2Int(1, 1));
            internPiece.moveOffsets.Add(new Vector2Int(1, -1));
            internPiece.moveOffsets.Add(new Vector2Int(-1, -1));
            internPiece.moveOffsets.Add(new Vector2Int(-1, 1));

            prefabPiece = GameManager.instance.BLACKBISCHOP;
        }
    }
    public class Black_Knight : Piece
    {
        public Black_Knight(Board parent) : base(parent)
        {
            internPiece = new Piece.Intern(this);
            internPiece.isWhite = false;

            internPiece.isSlidingPiece = false;
            internPiece.moveOffsets.Add(new Vector2Int(1, 2));
            internPiece.moveOffsets.Add(new Vector2Int(2, -1));
            internPiece.moveOffsets.Add(new Vector2Int(2, 1));
            internPiece.moveOffsets.Add(new Vector2Int(1, -2));
            internPiece.moveOffsets.Add(new Vector2Int (-1, -2));
            internPiece.moveOffsets.Add(new Vector2Int(-2, -1));
            internPiece.moveOffsets.Add(new Vector2Int(-2, 1));
            internPiece.moveOffsets.Add(new Vector2Int(-1, 2));

            prefabPiece = GameManager.instance.BLACKKNIGHT;
        }
    }
    public class Black_Pawn : Piece
    {
        public Black_Pawn(Board parent) : base(parent)
        {
            internPiece = new Piece.Intern(this);
            internPiece.isWhite = false;

            internPiece.isSlidingPiece = false;
            internPiece.moveOffsets.Add(new Vector2Int(0,1));
            internPiece.moveOffsets.Add(new Vector2Int(0, 2));

            prefabPiece = GameManager.instance.BLACKPAWN;
        }
    }
    public class Intern
    {
        public Vector2Int position;

        public bool isWhite;
        public bool isSlidingPiece;

        public readonly List<Vector2Int> moveOffsets = new List<Vector2Int>();
        public List<Move> legalMoves;

        public bool isFirstMove = true;
        public Intern(Piece piece)
        {

        }
    }
    public class Extern
    {
        public GameObject pieceGameObject;
        public Extern(Piece piece, GameObject parent)
        {
            pieceGameObject = MonoBehaviour.Instantiate(piece.prefabPiece, parent.transform);
            pieceGameObject.SetActive(true);
        }
        public void Move(Vector3 destination)
        {
            pieceGameObject.transform.localPosition = destination;
            pieceGameObject.transform.localRotation = Quaternion.identity;
        }

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

    public static Piece Lookup(GameObject gameobject)
    {
        if (gameobject == null)
        {
            Debug.Log("Piece requested is null!");
            return null;
        }
        if (Piece.lookupTable.ContainsKey(gameobject))
        {
            return Piece.lookupTable[gameobject];
        }
        else
        {
            Debug.LogError("Piece not in Lookuptable!");
            return null;
        }
    }
}
