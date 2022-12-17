using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Pieces
{
    static List<Pieces> instances = new List<Pieces>();
    public static Dictionary<GameObject, Pieces> lookupTable = new Dictionary<GameObject, Pieces>();

    public Board board;
    public Pieces pieceObject;
    public Pieces.Intern internPiece;
    public Pieces.Extern externPiece;
    public GameObject prefabPiece;

    public Pieces(Board board)
    {
        pieceObject = this;
        instances.Add(this);
        this.board = board;
    }
    public void CreateExtern(GameObject parent)
    {
        this.externPiece = new Pieces.Extern(this, parent);
        lookupTable.Add(this.externPiece.pieceGameObject, this);
    }
    public class None : Pieces
    {
        public None(Board parent) : base (parent)
        {
            internPiece = new Pieces.Intern();
            prefabPiece = GameManager.instance.NONE;
        }
    }
    public class White_King : Pieces
    {
        public White_King(Board parent) : base(parent)
        {
            internPiece = new Pieces.Intern();
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
    public class White_Queen : Pieces
    {
        public White_Queen(Board parent) : base(parent)
        {
            internPiece = new Pieces.Intern();
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
    public class White_Rook : Pieces
    {
        public White_Rook(Board parent) : base(parent)
        {
            internPiece = new Pieces.Intern();
            internPiece.isWhite = true;


            internPiece.isSlidingPiece = true;
            internPiece.moveOffsets.Add(new Vector2Int(0, 1));
            internPiece.moveOffsets.Add(new Vector2Int(1, 0));
            internPiece.moveOffsets.Add(new Vector2Int(0, -1));
            internPiece.moveOffsets.Add(new Vector2Int(-1, 0));

            prefabPiece = GameManager.instance.WHITEROOK;
        }
    }
    public class White_Bischop : Pieces
    {
        public White_Bischop(Board parent) : base(parent)
        {
            internPiece = new Pieces.Intern();
            internPiece.isWhite = true;


            internPiece.isSlidingPiece = true;
            internPiece.moveOffsets.Add(new Vector2Int(1, 1));
            internPiece.moveOffsets.Add(new Vector2Int(1, -1));
            internPiece.moveOffsets.Add(new Vector2Int(-1, -1));
            internPiece.moveOffsets.Add(new Vector2Int(-1, 1));

            prefabPiece = GameManager.instance.WHITEBISCHOP;
        }
    }
    public class White_Knight : Pieces
    {
        public White_Knight(Board parent) : base(parent)
        {
            internPiece = new Pieces.Intern();
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
    public class White_Pawn : Pieces
    {
        public White_Pawn(Board parent) : base(parent)
        {
            internPiece = new Pieces.Intern();
            internPiece.isWhite = true;


            internPiece.isSlidingPiece = false;
            internPiece.moveOffsets.Add(new Vector2Int(0, -1));

            prefabPiece = GameManager.instance.WHITEPAWN;
        }
    }
    public class Black_King : Pieces
    {
        public Black_King(Board parent) : base(parent)
        {
            internPiece = new Pieces.Intern();
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
    public class Black_Queen : Pieces
    {
        public Black_Queen(Board parent) : base(parent)
        {
            internPiece = new Pieces.Intern();
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
    public class Black_Rook : Pieces
    {
        public Black_Rook(Board parent) : base(parent)
        {
            internPiece = new Pieces.Intern();
            internPiece.isWhite = false;

            internPiece.isSlidingPiece = true;
            internPiece.moveOffsets.Add(new Vector2Int(0, 1));
            internPiece.moveOffsets.Add(new Vector2Int(1, 0));
            internPiece.moveOffsets.Add(new Vector2Int(0, -1));
            internPiece.moveOffsets.Add(new Vector2Int(-1, 0));

            prefabPiece = GameManager.instance.BLACKROOK;
        }
    }
    public class Black_Bischop : Pieces
    {
        public Black_Bischop(Board parent) : base(parent)
        {
            internPiece = new Pieces.Intern();
            internPiece.isWhite = false;

            internPiece.isSlidingPiece = true;
            internPiece.moveOffsets.Add(new Vector2Int(1, 1));
            internPiece.moveOffsets.Add(new Vector2Int(1, -1));
            internPiece.moveOffsets.Add(new Vector2Int(-1, -1));
            internPiece.moveOffsets.Add(new Vector2Int(-1, 1));

            prefabPiece = GameManager.instance.BLACKBISCHOP;
        }
    }
    public class Black_Knight : Pieces
    {
        public Black_Knight(Board parent) : base(parent)
        {
            internPiece = new Pieces.Intern();
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
    public class Black_Pawn : Pieces
    {
        public Black_Pawn(Board parent) : base(parent)
        {
            internPiece = new Pieces.Intern();
            internPiece.isWhite = false;

            internPiece.isSlidingPiece = false;
            internPiece.moveOffsets.Add(new Vector2Int(0,1));

            prefabPiece = GameManager.instance.BLACKPAWN;
        }
    }
    public class Intern
    {
        public bool isWhite;
        public Vector2Int position;

        public bool isSlidingPiece;
        public List<Vector2Int> moveOffsets = new List<Vector2Int>();

        public List<Move> legalMoves;
        public Intern()
        {
            
        }
    }
    public class Extern
    {
        public GameObject pieceGameObject;
        public Extern(Pieces piece, GameObject parent)
        {
            pieceGameObject = MonoBehaviour.Instantiate(piece.prefabPiece, parent.transform);
            pieceGameObject.SetActive(true);
        }
    }
}
