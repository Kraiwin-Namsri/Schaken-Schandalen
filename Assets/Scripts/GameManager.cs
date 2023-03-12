using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.OpenXR.Input;
using static Move;

public class GameManager : MonoBehaviour
{
    public GameObject CHESSBOARD;
    public GameObject PEDESTAL;
    public GameObject HIGHLIGHT;
    public GameObject HINT;
    public GameObject MARKER;

    public GameObject PARENT;

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
        SetupBoard(CHESSBOARD);
    }

    public static void SetupBoard(GameObject CHESSBOARD)
    {
        CHESSBOARD.SetActive(false);
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

        Vector3 externDestination = ConvertInternToExternPosition(internDestination, board.externBoard.playSurface.GetComponent<MeshFilter>().mesh.bounds.size);
        //piece.externPiece.Move(externDestination);
        instance.StartCoroutine(piece.externPiece.SmoothMove(externDestination, 0.1f));
    }
    public static void UpdatePedestal()
    {
        int x = board.internBoard.captured.Count - 1;
        int y = 0;
        int totalAmountOfWhitePieces = 0;
        int amountOfWhitePieces = 0;
        int amountOfWhitePawns = 0;
        int totalAmountOfBlackPieces = 0;
        int amountOfBlackPieces = 0;
        int amountOfBlackPawns = 0;

        Vector2 size = board.externBoard.playSurface.GetComponent<MeshFilter>().mesh.bounds.size * (Vector2)board.externBoard.pedestalPlaySurface.transform.localScale;

        Pieces capturedPiece;
        capturedPiece = board.internBoard.captured[board.internBoard.captured.Count - 1];

        foreach (Pieces piece in board.internBoard.captured)
        {
            if (piece.internPiece.isWhite == true)
            {
                totalAmountOfWhitePieces++;
            }
            if (piece.GetType() == typeof(Pieces.White_Pawn))
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
            x = amountOfWhitePawns - 1;
            y = 1;
        }
        if (capturedPiece.internPiece.isWhite == true && capturedPiece.GetType() != typeof(Pieces.White_Pawn))
        {
            x = amountOfWhitePieces - 1;
            y = 0;
        }
        if (capturedPiece.internPiece.isWhite == false && capturedPiece.GetType() == typeof(Pieces.Black_Pawn))
        {
            x = 8 + amountOfBlackPawns - 1;
            y = 1;
        }
        if (capturedPiece.internPiece.isWhite == false && capturedPiece.GetType() != typeof(Pieces.Black_Pawn))
        {
            x = 8 + amountOfBlackPieces - 1;
            y = 0;
        }

        capturedPiece = board.internBoard.captured[board.internBoard.captured.Count - 1];
        capturedPiece.externPiece.pieceGameObject.transform.parent = board.externBoard.pedestalPlaySurface.transform;

        Vector2 position = new Vector3((size.x / 32) - (8 * size.x / 16) + ((x * size.x / 16)), -size.y + (y * size.y * 2));

        capturedPiece.externPiece.pieceGameObject.transform.parent = board.externBoard.pedestalPlaySurface.transform;
        //capturedPiece.externPiece.pieceGameObject.transform.localScale = new Vector3(1,1,1);
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


    public static void OnGrab(GameObject gameObject)
    {
        Pieces grabedPiece = Pieces.Lookup(gameObject);
        if (gameObject != null & grabedPiece != null)
        {
            VisualizeLegalMoves(grabedPiece);

            GameObject square = Instantiate(GameManager.instance.HIGHLIGHT, board.externBoard.playSurface.transform);
            square.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, 0);
            square.SetActive(true);
            visualizeGameobject.Add(square);
        }
    }
    public static void OnRelease(GameObject gameObject)
    {
        DeleteVisualization();
        Pieces releasedPiece = Pieces.Lookup(gameObject);

        if (gameObject != null & releasedPiece != null)
        {
            Vector2Int releasePosition = ConvertExternToInternPosition(gameObject.transform.localPosition, board.externBoard.playSurface.GetComponent<MeshFilter>().mesh.bounds.size);
            if (releasePosition.x >= 0 && releasePosition.y >= 0 && releasePosition.x < board.internBoard.board.GetLength(0) && releasePosition.y < board.internBoard.board.GetLength(1))
            {
                foreach (Move legalMove in releasedPiece.internPiece.legalMoves)
                {
                    if (legalMove == new Move(releasedPiece.internPiece.position, releasePosition, board.internBoard))
                    {
                        board.internBoard.AddMove(legalMove);
                        break;
                    }
                }
                
            }
            // Tijdelijk
            GameManager.UpdateExtern(GameManager.board);
            // To Do reset rotation
        }
    }

    public static void setParentPointerfinger(Hand pointerfinger)
    {
        pointerfinger.gameObject.transform.parent = board.externBoard.piecesParent.transform; 
    }
    public static void OnPointerfingerCollisionEnter(Hand hand)
    {
        Vector3 localPosition = board.externBoard.playSurface.transform.InverseTransformPoint(hand.gameObject.transform.position);
        Debug.Log($"position: {localPosition}");

        Vector2Int position = ConvertExternToInternPosition(localPosition, board.externBoard.playSurface.GetComponent<MeshFilter>().mesh.bounds.size);
        if (position.x >= 0 && position.y >= 0 && position.x < board.internBoard.board.GetLength(0) && position.y < board.internBoard.board.GetLength(1))
        {
            Debug.Log($"Is inside bounds!");
            MarkSquare(position);
        }
    }

    public static void OnPointerfingerCollisionExit(Hand hand)
    {
        Vector2Int enterCoordinates = Hand.handCoordinates[hand.gameObject];
        Debug.Log(enterCoordinates);

        Vector2Int exitCoordinates = ConvertExternToInternPosition(hand.gameObject.transform.localPosition, board.externBoard.playSurface.GetComponent<MeshFilter>().mesh.bounds.size);
        Debug.Log(exitCoordinates);


        if(enterCoordinates == exitCoordinates)
        {
            MarkSquare(enterCoordinates);
        }
        else
        {
            DrawArrow(enterCoordinates, exitCoordinates);
        }
        Hand.handCoordinates.Remove(hand.gameObject);
    }
    public static void MarkSquare(Vector2Int coordinate)
    {
        Vector3 destination = ConvertInternToExternPosition(coordinate, board.externBoard.playSurface.GetComponent<MeshFilter>().mesh.bounds.size);
        GameObject marker = Instantiate(GameManager.instance.MARKER, board.externBoard.playSurface.transform);
        marker.SetActive(true);
        marker.transform.localPosition = destination;
    }

    public static void DrawArrow(Vector2Int startCoordinate, Vector2Int endCoordinate)
    {

    }
}

