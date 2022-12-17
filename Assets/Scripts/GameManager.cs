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
        GameManager.board = board;
        UpdateExtern(board);
        Move.Legal.Generate(board.internBoard);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateExtern(Board board)
    {
        for (int y = 0; y < board.internBoard.board.GetLength(1); y++)
        {
            for (int x = 0; x < board.internBoard.board.GetLength(0); x++)
            {
                Pieces piece = board.internBoard.board[x, y];
                MovePiece(piece, new Vector2Int(x, y));
            }
        }
    }

    public static void MovePiece(Pieces piece, Vector2Int internDestination)
    {
        if (piece.GetType() == typeof(Pieces.Black_Pawn) || piece.GetType() == typeof(Pieces.White_Pawn))
        {
            //To Do: Update position in intern Piece
            Vector3 externDestination = ConvertInternToExternPosition(internDestination, board.externBoard.board.transform.GetChild(0).GetComponent<MeshFilter>().mesh.bounds.size);
            externDestination.z = 0.01f;
            board.internBoard.board[internDestination.x, internDestination.y].externPiece.pieceGameObject.transform.localPosition = externDestination;
        }
        else
        {
            Vector3 externDestination = ConvertInternToExternPosition(internDestination, board.externBoard.board.transform.GetChild(0).GetComponent<MeshFilter>().mesh.bounds.size);
            board.internBoard.board[internDestination.x, internDestination.y].externPiece.pieceGameObject.transform.localPosition = externDestination;
        }
    }
    
    public static void OnGrab(GameObject gameobject)
    {
        Pieces holdingPiece = Pieces.lookupTable[gameobject];
    }
    public static void OnRelease(GameObject gameobject)
    {
        Pieces releasedPiece = Pieces.lookupTable[gameobject];
        Vector2Int releasePosition = ConvertExternToInternPosition(gameobject.transform.localPosition, board.externBoard.board.transform.GetChild(0).GetComponent<MeshFilter>().mesh.bounds.size);
        Debug.Log(releasePosition);
        if (releasePosition.x >= 0 && releasePosition.y >= 0 && releasePosition.x < board.internBoard.board.GetLength(0) && releasePosition.y < board.internBoard.board.GetLength(1)) 
        {
            board.internBoard.AddMove(new Move(releasedPiece.internPiece.position, releasePosition));
        }
        // Tijdelijk
        GameManager.instance.UpdateExtern(GameManager.board);
    }
    
    public static Vector2Int ConvertExternToInternPosition(Vector3 externPosition, Vector3 parentSize)
    {
        Vector2 internPosition = (((Vector2)externPosition) - ((Vector2)parentSize / 16) + ((Vector2)parentSize / 2)) / ((Vector2)parentSize / 8);
        return Vector2Int.FloorToInt(internPosition);
    }
    public static Vector3 ConvertInternToExternPosition(Vector2Int internDestination, Vector3 parentSize)
    {
        Vector3 externDestination = (((Vector2)internDestination * (Vector2)parentSize/8) + ((Vector2)parentSize / 16) - ((Vector2)parentSize / 2));
        externDestination.z = 0;
        return externDestination;
    }
}

