using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject chessBoard;

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
        Board myBoard = new Board();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetChessBoardOrigin()
    {
        Vector3 boardOrigin;
        boardOrigin = new Vector3(chessBoard.transform.position.x - 0.21f, 0, chessBoard.transform.position.y + 0.21f);
    }
    public Vector3 InternToExtern(Vector2 coordinate)
    {
        return coordinate.normalized;
    }
    public Vector2 ExternToIntern(Vector3 coordinate)
    {
        return coordinate;
    }
}
