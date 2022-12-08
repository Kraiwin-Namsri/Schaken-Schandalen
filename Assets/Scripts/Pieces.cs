using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieces : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static int _NONE = 0;
    static int _KING = 1;
    static int _QUEEN = 2;
    static int _ROOK = 3;
    static int _BISCHOP = 4;
    static int _KNIGHT = 5;
    static int _PAWN = 6;
    static int _WHITE = 8;
    static int _BLACK = 16;
    static List<Pieces> instances = new List<Pieces>();

    static bool IsWhite()
    {
        return true;
    }

    public Position position;
    public int pieceValue;

    public Pieces()
    {
        instances.Add(this);
    }
    public struct Position
    {
        int x;
        int y;
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }


    //Classes for instantiating pieces.
    public class White_King : Pieces
    {
        public White_King()
        {
            this.pieceValue = _WHITE + _KING;
        }
    }
    public class White_Queen : Pieces
    {
        public White_Queen()
        {
            this.pieceValue = _WHITE + _QUEEN;
        }
    }
    public class White_Rook : Pieces
    {
        public White_Rook()
        {
            this.pieceValue = _WHITE + _ROOK;
        }
    }
    public class White_Bischop : Pieces
    {
        public White_Bischop()
        {
            this.pieceValue = _WHITE + _BISCHOP;
        }
    }
    public class White_Knight : Pieces
    {
        public White_Knight()
        {
            this.pieceValue = _WHITE + _KNIGHT;
        }
    }
    public class White_Pawn : Pieces
    {
        public White_Pawn()
        {
            this.pieceValue = _WHITE + _PAWN;
        }
    }
    public class Black_King : Pieces
    {
        public Black_King()
        {
            this.pieceValue = _BLACK + _KING;
        }
    }
    public class Black_Queen : Pieces
    {
        public Black_Queen()
        {
            this.pieceValue = _BLACK + _QUEEN;
        }
    }
    public class Black_Rook : Pieces
    {
        public Black_Rook()
        {
            this.pieceValue = _BLACK + _ROOK;
        }
    }
    public class Black_Bischop : Pieces
    {
        public Black_Bischop()
        {
            this.pieceValue = _BLACK + _BISCHOP;
        }
    }
    public class Black_Knight : Pieces
    {
        public Black_Knight()
        {
            this.pieceValue = _BLACK + _KNIGHT;
        }
    }
    public class Black_Pawn : Pieces
    {
        public Black_Pawn()
        {
            this.pieceValue = _BLACK + _PAWN;
        }
    }
}
