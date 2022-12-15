using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Pieces
{
    public static Dictionary<Pieces.Intern, Pieces.Extern> lookupTableInternToExtern = new Dictionary<Pieces.Intern, Pieces.Extern>();
    public static Dictionary<Pieces.Extern, Pieces.Intern> lookupTableExternToIntern = new Dictionary<Pieces.Extern, Pieces.Intern>();
    static List<Pieces> instances = new List<Pieces>();
    public Pieces.Intern internPiece;
    public Pieces.Extern externPiece;
    public GameObject prefabPiece;

    public Pieces()
    {
        instances.Add(this);
    }
    public void CreateExtern(GameObject parent)
    {
        this.externPiece = new Pieces.Extern(this, parent);
        lookupTableInternToExtern.Add(this.internPiece, this.externPiece);
        lookupTableExternToIntern.Add(this.externPiece, this.internPiece);
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
            prefabPiece = GameManager.instance.WHITEKING;
        }
    }
    public class White_Queen : Pieces
    {
        public White_Queen()
        {
            internPiece = new Pieces.Intern();
            prefabPiece = GameManager.instance.WHITEQUEEN;
        }
    }
    public class White_Rook : Pieces
    {
        public White_Rook()
        {
            internPiece = new Pieces.Intern();
            prefabPiece = GameManager.instance.WHITEROOK;
        }
    }
    public class White_Bischop : Pieces
    {
        public White_Bischop()
        {
            internPiece = new Pieces.Intern();
            prefabPiece = GameManager.instance.WHITEBISCHOP;
        }
    }
    public class White_Knight : Pieces
    {
        public White_Knight()
        {
            internPiece = new Pieces.Intern();
            prefabPiece = GameManager.instance.WHITEKNIGHT;
        }
    }
    public class White_Pawn : Pieces
    {
        public White_Pawn()
        {
            internPiece = new Pieces.Intern();
            prefabPiece = GameManager.instance.WHITEPAWN;
        }
    }
    public class Black_King : Pieces
    {
        public Black_King()
        {
            internPiece = new Pieces.Intern();
            prefabPiece = GameManager.instance.BLACKKING;
        }
    }
    public class Black_Queen : Pieces
    {
        public Black_Queen()
        {
            internPiece = new Pieces.Intern();
            prefabPiece = GameManager.instance.BLACKQUEEN;
        }
    }
    public class Black_Rook : Pieces
    {
        public Black_Rook()
        {
            internPiece = new Pieces.Intern();
            prefabPiece = GameManager.instance.BLACKROOK;
        }
    }
    public class Black_Bischop : Pieces
    {
        public Black_Bischop()
        {
            internPiece = new Pieces.Intern();
            prefabPiece = GameManager.instance.BLACKBISCHOP;
        }
    }
    public class Black_Knight : Pieces
    {
        public Black_Knight()
        {
            internPiece = new Pieces.Intern();
            prefabPiece = GameManager.instance.BLACKKNIGHT;
        }
    }
    public class Black_Pawn : Pieces
    {
        public Black_Pawn()
        {
            internPiece = new Pieces.Intern();
            prefabPiece = GameManager.instance.BLACKPAWN;
        }
    }
    public class Intern
    {
        Vector2Int position;
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
