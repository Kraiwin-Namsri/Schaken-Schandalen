using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateExtern(Board board)
    {
        for(int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                if (board.internBoard.board[x, y].GetType() == typeof(Pieces.Black_Pawn) || board.internBoard.board[x,y].GetType() == typeof(Pieces.White_Pawn))
                {
                    Vector3 position = new Vector3(board.externBoard.boardOrigin.x - 0.06f * x, board.externBoard.boardOrigin.y + 0.01f, board.externBoard.boardOrigin.z + 0.06f * y);
                    board.internBoard.board[x, y].externPiece.pieceGameObject.transform.position = position;
                }
                else
                {
                    Vector3 position = new Vector3(board.externBoard.boardOrigin.x - 0.06f * x, board.externBoard.boardOrigin.y, board.externBoard.boardOrigin.z + 0.06f * y);
                    board.internBoard.board[x, y].externPiece.pieceGameObject.transform.position = position;
                }
            }
        }
    }
    public void UpdateIntern(Board board)
    {
 
    }
}
