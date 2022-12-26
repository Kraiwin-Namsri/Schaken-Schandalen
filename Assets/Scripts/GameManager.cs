using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.OpenXR.Input;


public class GameManager : MonoBehaviour
{
    public GameObject CHESSBOARD;
    public GameObject PEDESTAL;

    public GameObject HIGHLIGHTSQUARE;
    public GameObject HINT;
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
    private static Board board;
    GameManager() { 
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Board board = new Board();
        GameManager.board = board;
        GameManager.UpdateExtern(board);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void UpdateExtern(Board board)
    {
        for (int y = 0; y < board.internBoard.board.GetLength(1); y++)
        {
            for (int x = 0; x < board.internBoard.board.GetLength(0); x++)
            {
                Pieces piece = board.internBoard.board[x, y];
                UpdateExternPosition(piece, new Vector2Int(x, y));
            }
        }
    }

    private static void UpdateExternPosition(Pieces piece, Vector2Int internDestination)
    {
        if (piece.GetType() == typeof(Pieces.Black_Pawn) || piece.GetType() == typeof(Pieces.White_Pawn))
        {
            //To Do: Update position in intern Piece
            Vector3 externDestination = ConvertInternToExternPosition(internDestination, board.externBoard.playSurface.GetComponent<MeshFilter>().mesh.bounds.size);
            externDestination.z = 0.01f;
            piece.externPiece.Move(externDestination);
        }
        else
        {
            Vector3 externDestination = ConvertInternToExternPosition(internDestination, board.externBoard.playSurface.GetComponent<MeshFilter>().mesh.bounds.size);
            piece.externPiece.Move(externDestination);
        }
    }
    public static void UpdatePedestal()
    {
        int x = board.internBoard.captured.Count - 1;
        int y = 0;
        int totalAmountOfWhitePieces = 0;
        int amountOfWhitePieces= 0;
        int amountOfWhitePawns = 0;
        int totalAmountOfBlackPieces = 0;
        int amountOfBlackPieces = 0;
        int amountOfBlackPawns = 0;


        Vector2 size = board.externBoard.playSurface.GetComponent<MeshFilter>().mesh.bounds.size * (Vector2)board.externBoard.pedestalPlaySurface.transform.localScale;

        Pieces capturedPiece;
        capturedPiece = board.internBoard.captured[board.internBoard.captured.Count - 1];

        foreach (Pieces piece in board.internBoard.captured)
        {
            if(piece.internPiece.isWhite == true)
            {
                totalAmountOfWhitePieces ++;
            }
            if(piece.GetType() == typeof(Pieces.White_Pawn))
            {
                amountOfWhitePawns++;
            }
            if (piece.GetType() == typeof(Pieces.Black_Pawn))
            {
                amountOfBlackPawns++;
            }
        }
        amountOfWhitePieces = totalAmountOfWhitePieces - amountOfWhitePawns;

        totalAmountOfBlackPieces = board.internBoard.captured.Count - totalAmountOfWhitePieces;
        amountOfBlackPieces = totalAmountOfBlackPieces - amountOfBlackPawns;

        if (capturedPiece.internPiece.isWhite == true && capturedPiece.GetType() == typeof(Pieces.White_Pawn))
        {
            x = amountOfWhitePawns -1;
            y = 1;
        }
        if(capturedPiece.internPiece.isWhite == true && capturedPiece.GetType() != typeof(Pieces.White_Pawn))
        {
            x = amountOfWhitePieces - 1;
            y = 0;
        }
        if(capturedPiece.internPiece.isWhite == false && capturedPiece.GetType() == typeof(Pieces.Black_Pawn))
        {
            x = 8 + amountOfBlackPawns -1;
            y = 1;
        }
        if(capturedPiece.internPiece.isWhite == false && capturedPiece.GetType() != typeof(Pieces.Black_Pawn))
        {
            x = 8 + amountOfBlackPieces - 1;
            y = 0;
        }
        



        capturedPiece = board.internBoard.captured[board.internBoard.captured.Count - 1];
        capturedPiece.externPiece.pieceGameObject.transform.parent = board.externBoard.pedestalPlaySurface.transform;

        Vector2 position = new Vector3((size.x / 32) - (8 * size.x / 16) + ((x * size.x / 16)), -size.y + (y * size.y * 2));

        capturedPiece.externPiece.pieceGameObject.transform.parent = board.externBoard.pedestalPlaySurface.transform;
        capturedPiece.externPiece.pieceGameObject.transform.localPosition = position;
    }

    private static Vector2Int ConvertExternToInternPosition(Vector3 externPosition, Vector3 parentSize)
    {
        Vector2 internPosition = (((Vector2)externPosition) - ((Vector2)parentSize / 16) + ((Vector2)parentSize / 2)) / ((Vector2)parentSize / 8);
        return Vector2Int.RoundToInt(internPosition);
    }
    private static Vector3 ConvertInternToExternPosition(Vector2Int internDestination, Vector3 parentSize)
    {
        Vector3 externDestination = (((Vector2)internDestination * (Vector2)parentSize/8) + ((Vector2)parentSize / 16) - ((Vector2)parentSize / 2));
        externDestination.z = 0;
        return externDestination;
    }

    static List<GameObject> visualizeGameobject = new List<GameObject>();
    private static void VisualizeLegalMoves(Pieces piece)
    {
        DeleteVisualization();
        Move.Legal.Generate(board.internBoard);
        foreach (Move legalMove in piece.internPiece.legalMoves)
        {
            Vector3 destination = ConvertInternToExternPosition(legalMove.endPosition, board.externBoard.playSurface.GetComponent<MeshFilter>().mesh.bounds.size);
            GameObject hint = Instantiate(GameManager.instance.HINT, board.externBoard.playSurface.transform);
            hint.SetActive(true);
            visualizeGameobject.Add(hint);
            hint.transform.localPosition = destination;
        }
    }

    private static void DeleteVisualization()
    {
        foreach (GameObject go in visualizeGameobject)
        {
            Destroy(go);
        }
        visualizeGameobject.Clear();
    }


    public static void OnGrab(GameObject gameobject)
    {
        Pieces grabedPiece = Pieces.lookupTable[gameobject];
        VisualizeLegalMoves(grabedPiece);

        GameObject square = Instantiate(GameManager.instance.HIGHLIGHTSQUARE, board.externBoard.playSurface.transform);
        square.transform.localPosition = new Vector3(gameobject.transform.localPosition.x, gameobject.transform.localPosition.y, 0.0f);
        square.SetActive(true);
        visualizeGameobject.Add(square);
    }
    public static void OnRelease(GameObject gameobject)
    {
        DeleteVisualization();

        if (Pieces.lookupTable.ContainsKey(gameobject))
        {
            Pieces releasedPiece = Pieces.lookupTable[gameobject];
            Vector2Int releasePosition = ConvertExternToInternPosition(gameobject.transform.localPosition, board.externBoard.playSurface.GetComponent<MeshFilter>().mesh.bounds.size);
            if (releasePosition.x >= 0 && releasePosition.y >= 0 && releasePosition.x < board.internBoard.board.GetLength(0) && releasePosition.y < board.internBoard.board.GetLength(1))
            {
                board.internBoard.AddMove(new Move(releasedPiece.internPiece.position, releasePosition, board.internBoard));
            }
            // Tijdelijk
            GameManager.UpdateExtern(GameManager.board);
        }
    }
}

