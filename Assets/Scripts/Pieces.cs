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
    static List<Pieces> instances = new List<Pieces>();

    static bool IsWhite()
    {
        return true;
    }

    public Vector2 position;
    public GameObject gameObject;

    public Pieces()
    {
        instances.Add(this);
    }


    //Classes for instantiating pieces.
    public class White_King : Pieces
    {
        public White_King()
        {
            gameObject = GameObject.Instantiate(GameManager.instance.WHITEKING);
        }
    }
    public class White_Queen : Pieces
    {
        public White_Queen()
        {
            gameObject = GameObject.Instantiate(GameManager.instance.WHITEQUEEN);
        }
    }
    public class White_Rook : Pieces
    {
        public White_Rook()
        {
            gameObject = GameObject.Instantiate(GameManager.instance.WHITEROOK);
        }
    }
    public class White_Bischop : Pieces
    {
        public White_Bischop()
        {
            gameObject = GameObject.Instantiate(GameManager.instance.WHITEBISCHOP);
        }
    }
    public class White_Knight : Pieces
    {
        public White_Knight()
        {
            gameObject = GameObject.Instantiate(GameManager.instance.WHITEKNIGHT);
        }
    }
    public class White_Pawn : Pieces
    {
        public White_Pawn()
        {
            gameObject = GameObject.Instantiate(GameManager.instance.WHITEPAWN);
        }
    }
    public class Black_King : Pieces
    {
        public Black_King()
        {
            gameObject = GameObject.Instantiate(GameManager.instance.BLACKKING);
        }
    }
    public class Black_Queen : Pieces
    {
        public Black_Queen()
        {
            gameObject = GameObject.Instantiate(GameManager.instance.BLACKQUEEN);
        }
    }
    public class Black_Rook : Pieces
    {
        public Black_Rook()
        {
            gameObject = GameObject.Instantiate(GameManager.instance.BLACKROOK);
        }
    }
    public class Black_Bischop : Pieces
    {
        public Black_Bischop()
        {
            gameObject = GameObject.Instantiate(GameManager.instance.BLACKBISCHOP);
        }
    }
    public class Black_Knight : Pieces
    {
        public Black_Knight()
        {
            gameObject = GameObject.Instantiate(GameManager.instance.BLACKKNIGHT);
        }
    }
    public class Black_Pawn : Pieces
    {
        public Black_Pawn()
        {
            gameObject = GameObject.Instantiate(GameManager.instance.BLACKPAWN);
        }
    }
}
