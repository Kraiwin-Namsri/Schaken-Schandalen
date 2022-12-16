using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Pieces
{
    static List<Pieces> instances = new List<Pieces>();

    public Pieces pieceObject;
    public Pieces.Intern internPiece;
    public Pieces.Extern externPiece;
    public GameObject prefabPiece;

    public Pieces()
    {
        pieceObject = this;
        instances.Add(this);
    }
    public void CreateExtern(GameObject parent)
    {
        this.externPiece = new Pieces.Extern(this, parent);
    }
    public class None : Pieces
    {
        public None()
        {
            internPiece = new Pieces.Intern();
            prefabPiece = null;
        }
    }
    public class White_King : Pieces
    {
        public White_King()
        {
            internPiece = new Pieces.Intern();
            internPiece.isWhite = true;

            internPiece.isSlidingPiece = false;
            internPiece.moveOffsets.Add(new Tuple<int, int>(0, 1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(1, 1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(1, 0));
            internPiece.moveOffsets.Add(new Tuple<int, int>(1, -1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(0, -1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(-1, -1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(-1, 0));
            internPiece.moveOffsets.Add(new Tuple<int, int>(-1, 1));

            prefabPiece = GameManager.instance.WHITEKING;
        }
    }
    public class White_Queen : Pieces
    {
        public White_Queen()
        {
            internPiece = new Pieces.Intern();
            internPiece.isWhite = true;


            internPiece.isSlidingPiece = true;
            internPiece.moveOffsets.Add(new Tuple<int, int>(0, 1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(1, 1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(1, 0));
            internPiece.moveOffsets.Add(new Tuple<int, int>(1, -1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(0, -1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(-1, -1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(-1, 0));
            internPiece.moveOffsets.Add(new Tuple<int, int>(-1, 1));

            prefabPiece = GameManager.instance.WHITEQUEEN;
        }
    }
    public class White_Rook : Pieces
    {
        public White_Rook()
        {
            internPiece = new Pieces.Intern();
            internPiece.isWhite = true;


            internPiece.isSlidingPiece = true;
            internPiece.moveOffsets.Add(new Tuple<int, int>(0, 1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(1, 0));
            internPiece.moveOffsets.Add(new Tuple<int, int>(0, -1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(-1, 0));

            prefabPiece = GameManager.instance.WHITEROOK;
        }
    }
    public class White_Bischop : Pieces
    {
        public White_Bischop()
        {
            internPiece = new Pieces.Intern();
            internPiece.isWhite = true;


            internPiece.isSlidingPiece = true;
            internPiece.moveOffsets.Add(new Tuple<int, int>(1, 1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(1, -1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(-1, -1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(-1, 1));

            prefabPiece = GameManager.instance.WHITEBISCHOP;
        }
    }
    public class White_Knight : Pieces
    {
        public White_Knight()
        {
            internPiece = new Pieces.Intern();
            internPiece.isWhite = true;


            internPiece.isSlidingPiece = false;
            internPiece.moveOffsets.Add(new Tuple<int, int>(1, 2));
            internPiece.moveOffsets.Add(new Tuple<int, int>(2, -1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(2, 1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(1, -2));
            internPiece.moveOffsets.Add(new Tuple<int, int>(-1, -2));
            internPiece.moveOffsets.Add(new Tuple<int, int>(-2, -1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(-2, 1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(-1, 2));

            prefabPiece = GameManager.instance.WHITEKNIGHT;
        }
    }
    public class White_Pawn : Pieces
    {
        public White_Pawn()
        {
            internPiece = new Pieces.Intern();
            internPiece.isWhite = true;


            internPiece.isSlidingPiece = false;
            internPiece.moveOffsets.Add(new Tuple<int, int>(0, -1));

            prefabPiece = GameManager.instance.WHITEPAWN;
        }
    }
    public class Black_King : Pieces
    {
        public Black_King()
        {
            internPiece = new Pieces.Intern();
            internPiece.isWhite = false;


            internPiece.isSlidingPiece = false;
            internPiece.moveOffsets.Add(new Tuple<int, int>(0, 1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(1, 1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(1, 0));
            internPiece.moveOffsets.Add(new Tuple<int, int>(1, -1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(0, -1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(-1, -1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(-1, 0));
            internPiece.moveOffsets.Add(new Tuple<int, int>(-1, 1));

            prefabPiece = GameManager.instance.BLACKKING;
        }
    }
    public class Black_Queen : Pieces
    {
        public Black_Queen()
        {
            internPiece = new Pieces.Intern();
            internPiece.isWhite = false;

            internPiece.isSlidingPiece = true;
            internPiece.moveOffsets.Add(new Tuple<int, int>(0, 1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(1, 1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(1, 0));
            internPiece.moveOffsets.Add(new Tuple<int, int>(1, -1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(0, -1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(-1, -1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(-1, 0));
            internPiece.moveOffsets.Add(new Tuple<int, int>(-1, 1));

            prefabPiece = GameManager.instance.BLACKQUEEN;
        }
    }
    public class Black_Rook : Pieces
    {
        public Black_Rook()
        {
            internPiece = new Pieces.Intern();
            internPiece.isWhite = false;

            internPiece.isSlidingPiece = true;
            internPiece.moveOffsets.Add(new Tuple<int, int>(0, 1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(1, 0));
            internPiece.moveOffsets.Add(new Tuple<int, int>(0, -1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(-1, 0));

            prefabPiece = GameManager.instance.BLACKROOK;
        }
    }
    public class Black_Bischop : Pieces
    {
        public Black_Bischop()
        {
            internPiece = new Pieces.Intern();
            internPiece.isWhite = false;

            internPiece.isSlidingPiece = true;
            internPiece.moveOffsets.Add(new Tuple<int, int>(1, 1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(1, -1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(-1, -1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(-1, 1));

            prefabPiece = GameManager.instance.BLACKBISCHOP;
        }
    }
    public class Black_Knight : Pieces
    {
        public Black_Knight()
        {
            internPiece = new Pieces.Intern();
            internPiece.isWhite = false;

            internPiece.isSlidingPiece = false;
            internPiece.moveOffsets.Add(new Tuple<int, int>(1, 2));
            internPiece.moveOffsets.Add(new Tuple<int, int>(2, -1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(2, 1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(1, -2));
            internPiece.moveOffsets.Add(new Tuple<int, int>(-1, -2));
            internPiece.moveOffsets.Add(new Tuple<int, int>(-2, -1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(-2, 1));
            internPiece.moveOffsets.Add(new Tuple<int, int>(-1, 2));

            prefabPiece = GameManager.instance.BLACKKNIGHT;
        }
    }
    public class Black_Pawn : Pieces
    {
        public Black_Pawn()
        {
            internPiece = new Pieces.Intern();
            internPiece.isWhite = false;

            internPiece.isSlidingPiece = false;
            internPiece.moveOffsets.Add(new Tuple<int, int>(0,1));

            prefabPiece = GameManager.instance.BLACKPAWN;
        }
    }
    public class Intern
    {
        public bool isWhite;
        public Vector2Int position;

        public bool isSlidingPiece;
        public List<Tuple<int, int>> moveOffsets = new List<Tuple<int, int>>();

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
            if (piece.prefabPiece != null)
            {
                pieceGameObject = MonoBehaviour.Instantiate(piece.prefabPiece, parent.transform);
                pieceGameObject.SetActive(true);
            } else
            {
                pieceGameObject = GameManager.instance.NONE;
            }
        }
    }
}
