using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Pieces[,] board = new Pieces[8, 8];
    public Board()
    {
        Fen.Apply(this, "");
    }
    public class Fen
    {
        public static void Apply(Board board, string fenString)
        {

        }
        Pieces ParseFen()
        {
            Pieces piece = new Pieces.White_King();
            return piece;
        }
    }
}
