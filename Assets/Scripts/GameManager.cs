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
<<<<<<< HEAD
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
=======
    private Marker markerManager;
    private Board board;
    private MoveManager moveManager;
>>>>>>> parent of 9871a0a (Refactoring Codebase 3)

    // Start is called before the first frame update
    void Start()
    {
<<<<<<< HEAD
        SetupBoard(CHESSBOARD);
    }

    public static void SetupBoard(GameObject CHESSBOARD)
=======
        board = new Board();
        UpdateExternBoard();
        markerManager = new Marker();

    }
   
    // Update all pieces on the board
    public void UpdateExternBoard()
    {
        for (int y = 0; y < board.intern.board.GetLength(1); y++)
        {
            for (int x = 0; x < board.intern.board.GetLength(0); x++)
            {
                Piece piece = board.intern.board[x, y];
                UpdateExternPosition(piece, new Vector2Int(x, y));
            }
        }
        UpdatePedestal();
    }

    private void UpdateExternPosition(Piece piece, Vector2Int internDestination)
    {
        //Calculate the destination for the external piece.
        Vector3 externDestination = ConvertInternToExternPosition(internDestination, );
        //Smoothly move the piece to the destination over a period of 0.1 seconds.
        StartCoroutine(piece.@extern.SmoothMove(externDestination, 0.1f));
    }

    // Refactor
    public void UpdatePedestal()
    {
        int x = board.intern.captured.Count - 1;
        int y = 0;
        int totalAmountOfWhitePieces = 0;
        int amountOfWhitePieces;
        int amountOfWhitePawns = 0;
        int totalAmountOfBlackPieces;
        int amountOfBlackPieces;
        int amountOfBlackPawns = 0;

        Vector2 size = board.@extern.playSurface.GetComponent<MeshFilter>().mesh.bounds.size * (Vector2)board.@extern.pedestalPlaySurface.transform.localScale;

        Piece capturedPiece;
        capturedPiece = board.intern.captured[board.intern.captured.Count - 1];

        foreach (Piece piece in board.intern.captured)
        {
            if (piece.intern.isWhite == true)
            {
                totalAmountOfWhitePieces++;
            }
            if (piece.GetType() == typeof(Piece.White_Pawn))
            {
                amountOfWhitePawns++;
            }
            if (piece.GetType() == typeof(Piece.Pawn))
            {
                amountOfBlackPawns++;
            }
        }

        amountOfWhitePieces = totalAmountOfWhitePieces - amountOfWhitePawns;
        totalAmountOfBlackPieces = board.intern.captured.Count - totalAmountOfWhitePieces;
        amountOfBlackPieces = totalAmountOfBlackPieces - amountOfBlackPawns;
        if (capturedPiece.intern.isWhite == true && capturedPiece.GetType() == typeof(Piece.White_Pawn))
        {
            x = amountOfWhitePawns - 1;
            y = 1;
        }
        if (capturedPiece.intern.isWhite == true && capturedPiece.GetType() != typeof(Piece.White_Pawn))
        {
            x = amountOfWhitePieces - 1;
            y = 0;
        }
        if (capturedPiece.intern.isWhite == false && capturedPiece.GetType() == typeof(Piece.Pawn))
        {
            x = 8 + amountOfBlackPawns - 1;
            y = 1;
        }
        if (capturedPiece.intern.isWhite == false && capturedPiece.GetType() != typeof(Piece.Pawn))
        {
            x = 8 + amountOfBlackPieces - 1;
            y = 0;
        }

        capturedPiece = board.intern.captured[board.intern.captured.Count - 1];
        capturedPiece.@extern.pieceGameObject.transform.parent = board.@extern.pedestalPlaySurface.transform;

        Vector2 position = new Vector3((size.x / 32) - (8 * size.x / 16) + ((x * size.x / 16)), -size.y + (y * size.y * 2));

        capturedPiece.@extern.pieceGameObject.transform.parent = board.@extern.pedestalPlaySurface.transform;
        capturedPiece.@extern.pieceGameObject.transform.localPosition = position;
    }
    
    // Calculate an internal position from an external position
    public Vector2Int ConvertExternToInternPosition(Vector3 externPosition)
    {
        Vector3 parentSize = board.@extern.playSurface.GetComponent<MeshFilter>().mesh.bounds.size;
        Vector2 internPosition = (((Vector2)externPosition) - ((Vector2)parentSize / 16) + ((Vector2)parentSize / 2)) / ((Vector2)parentSize / 8);
        return Vector2Int.RoundToInt(internPosition);
    }

    // Calculate an external position from an internal position
    public Vector3 ConvertInternToExternPosition(Vector2Int internDestination)
    {
        Vector3 parentSize = board.@extern.playSurface.GetComponent<MeshFilter>().mesh.bounds.size;
        Vector3 externDestination = (((Vector2)internDestination * (Vector2)parentSize/8) + ((Vector2)parentSize / 16) - ((Vector2)parentSize / 2));
        externDestination.z = 0;
        return externDestination;
    }


    public Transform GetExternBoardPosition()
    {
        return board.@extern.playSurface.transform;
    }


    // This function is assigned in the unity editor, and called when any object is grabbed.
    public void OnGrab(GameObject gameObject)
>>>>>>> parent of 9871a0a (Refactoring Codebase 3)
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
<<<<<<< HEAD
            for (int x = 0; x < board.internBoard.board.GetLength(0); x++)
            {
                Pieces piece = board.internBoard.board[x, y];
                UpdateExternPosition(piece, new Vector2Int(x, y));
            }
=======
            Move.Legal.Generate(board.intern);
            markerManager.hints.Visualize(this, grabedPiece);
>>>>>>> parent of 9871a0a (Refactoring Codebase 3)
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
        int amountOfWhitePieces;
        int amountOfWhitePawns = 0;
        int totalAmountOfBlackPieces;
        int amountOfBlackPieces;
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
<<<<<<< HEAD
            Vector2Int releasePosition = ConvertExternToInternPosition(gameObject.transform.localPosition, board.externBoard.playSurface.GetComponent<MeshFilter>().mesh.bounds.size);
            if (releasePosition.x >= 0 && releasePosition.y >= 0 && releasePosition.x < board.internBoard.board.GetLength(0) && releasePosition.y < board.internBoard.board.GetLength(1))
=======
            Vector2Int releasePosition = ConvertExternToInternPosition(gameObject.transform.localPosition);
            bool isInsideBounds = board.intern.IsInsideBounds(releasePosition);
            if (isInsideBounds)
>>>>>>> parent of 9871a0a (Refactoring Codebase 3)
            {
                foreach (Move legalMove in releasedPiece.internPiece.legalMoves)
                {
                    if (legalMove == new Move(releasedPiece.internPiece.position, releasePosition, board.internBoard))
                    {
<<<<<<< HEAD
                        board.internBoard.AddMove(legalMove);
=======
                        board.intern.AddMove(legalMove);
>>>>>>> parent of 9871a0a (Refactoring Codebase 3)
                        break;
                    }
                }
            }
<<<<<<< HEAD
            // Tijdelijk
            GameManager.UpdateExtern(GameManager.board);
            // To Do reset rotation
=======
            UpdateExternBoard();
>>>>>>> parent of 9871a0a (Refactoring Codebase 3)
        }
    }
}

