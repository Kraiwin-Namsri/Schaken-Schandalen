using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.OpenXR.Input;

public class GameManager : MonoBehaviour
{
    public GameObject CHESSBOARD;

    public GameObject NONE;

    public GameObject WHITEKING;
    public GameObject WHITEQUEEN;
    public GameObject WHITEROOK;
    public GameObject WHITEBISCHOP;
    public GameObject WHITEKNIGHT;
    public GameObject WHITEPAWN;

    public GameObject BLACKKING;
    public GameObject BLACKQUEEN;
    public GameObject BLACKROOK;
    public GameObject BLACKBISCHOP;
    public GameObject BLACKKNIGHT;
    public GameObject BLACKPAWN;

    public static GameManager instance;
    public static Board board;
    GameManager() { 
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Board board = new Board();
        UpdateExtern(board);
        Move.Legal.Generate(board.internBoard);
        Debug.Log(board.internBoard.board[1,0].internPiece.legalMoves.Count);
        GameManager.board = board;
        UpdateExtern(board); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateExtern(Board board)
    {
        float posx = (board.externBoard.board.transform.GetChild(0).GetComponent<MeshFilter>().mesh.bounds.size.x) / 8;
        float posy = (board.externBoard.board.transform.GetChild(0).GetComponent<MeshFilter>().mesh.bounds.size.y) / 8;
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                if (board.internBoard.board[x, y].GetType() == typeof(Pieces.Black_Pawn) || board.internBoard.board[x,y].GetType() == typeof(Pieces.White_Pawn))
                {
                    Vector3 position = new Vector3((posx*x)+posx/2 - (posx*4), (posy * y) + posy/2 - (posy * 4), 0.01f);
                    board.internBoard.board[x, y].externPiece.pieceGameObject.transform.localPosition = position;
                }
                else
                {
                    Vector3 position = new Vector3((posx * x) + posx/2 - (posx * 4), (posy * y) + posy / 2 - (posy * 4),0);
                    board.internBoard.board[x, y].externPiece.pieceGameObject.transform.localPosition = position;
                }
            }
        }
    }
    
    public static List<Pieces> holding = new List<Pieces>();
    public static void OnGrab(GameObject gameobject)
    {
        float posx = (board.externBoard.board.transform.GetChild(0).GetComponent<MeshFilter>().mesh.bounds.size.x) / 8;
        float posy = (board.externBoard.board.transform.GetChild(0).GetComponent<MeshFilter>().mesh.bounds.size.y) / 8;

        int coordinateX = (int)Math.Round((gameobject.transform.localPosition.x - (posx / 2) + (posx * 4)) / posx);
        int coordinateY = (int)Math.Round((gameobject.transform.localPosition.x - (posy / 2) + (posy * 4)) / posy);

        holding.Add(board.internBoard.board[coordinateX, coordinateY]);
        board.internBoard.board[coordinateX, coordinateY] = new Pieces.None();

    }
    public static void OnRelease(GameObject gameobject)
    {
        float posx = (board.externBoard.board.transform.GetChild(0).GetComponent<MeshFilter>().mesh.bounds.size.x) / 8;
        float posy = (board.externBoard.board.transform.GetChild(0).GetComponent<MeshFilter>().mesh.bounds.size.y) / 8;

        int coordinateX = (int)Math.Round((gameobject.transform.localPosition.x - (posx / 2) + (posx * 4)) / posx);
        int coordinateY = (int)Math.Round((gameobject.transform.localPosition.y - (posy / 2) + (posy * 4)) / posy);


        Debug.Log("x" + coordinateX);
        Debug.Log("y" + coordinateY);

        foreach (Pieces piece in holding)
        {
            Debug.Log("Before if");
            if (piece.externPiece.pieceGameObject = gameobject)
            {
                Debug.Log("If opened");
                board.internBoard.board[coordinateX, coordinateY] = piece;
                Debug.Log("after = piece");
                gameobject.transform.localPosition = new Vector3((posx * coordinateX) + posx / 2 - (posx * 4), (posy * coordinateY) + posy / 2 - (posy * 4), 0);
                Debug.Log("After transform");
                holding.Remove(piece);
                Debug.Log("After assign");
            }
        }
    }

    public void Snapping()
    {

    }

}
