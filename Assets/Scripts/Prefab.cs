using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefab : MonoBehaviour
{
    [Header("Piece GameObjects")]
    [SerializeField] public GameObject none;
    [SerializeField] public GameObject whiteKing;
    [SerializeField] public GameObject whiteQueen;
    [SerializeField] public GameObject whiteRook;
    [SerializeField] public GameObject whiteBischop;
    [SerializeField] public GameObject whiteKnight;
    [SerializeField] public GameObject whitePawn;
    [SerializeField] public GameObject blackKing;
    [SerializeField] public GameObject blackQueen;
    [SerializeField] public GameObject blackRook;
    [SerializeField] public GameObject blackBischop;
    [SerializeField] public GameObject blackKnight;
    [SerializeField] public GameObject blackPawn;

    public static GameObject None;
    public static GameObject WhiteKing;
    public static GameObject WhiteQueen;
    public static GameObject WhiteRook;
    public static GameObject WhiteBischop;
    public static GameObject WhiteKnight;
    public static GameObject WhitePawn;
    public static GameObject BlackKing;
    public static GameObject BlackQueen;
    public static GameObject BlackRook;
    public static GameObject BlackBischop;
    public static GameObject BlackKnight;
    public static GameObject BlackPawn;
    
    [Header("Board GameObjects")]
    [SerializeField] public GameObject chessBoard;
    [SerializeField] public GameObject pedestal;

    public static GameObject ChessBoard;
    public static GameObject Pedestal;

    [Header("Marker GameObjects")]
    [SerializeField] public GameObject highlight;
    [SerializeField] public GameObject hint;
    [SerializeField] public GameObject marker;

    public static GameObject Highlight;
    public static GameObject Hint;
    public static GameObject Marker;
    public void Start()
    {
        None = none;
        WhiteKing = whiteKing;
        WhiteQueen = whiteQueen;
        WhiteRook = whiteRook;
        WhiteBischop = whiteBischop;
        WhiteKnight = whiteKnight;
        WhitePawn = whitePawn;
        BlackKing = blackKing;
        BlackQueen = blackQueen;
        BlackRook = blackRook;
        BlackBischop = blackBischop;
        BlackKnight = blackKnight;
        BlackPawn = blackPawn;
        
        none.SetActive(false);
        whiteKing.SetActive(false);
        whiteQueen.SetActive(false);
        whiteRook.SetActive(false);
        whiteBischop.SetActive(false);
        whiteKnight.SetActive(false);
        whitePawn.SetActive(false);
        blackKing.SetActive(false);
        BlackQueen.SetActive(false);
        BlackRook.SetActive(false);
        BlackBischop.SetActive(false);
        blackKnight.SetActive(false);
        blackPawn.SetActive(false);

        ChessBoard = chessBoard;
        Pedestal = pedestal;

        chessBoard.SetActive(false);
        pedestal.SetActive(false);

        Highlight = highlight;
        Hint = hint;
        Marker = marker;

        highlight.SetActive(false);
        hint.SetActive(false);
        marker.SetActive(false);
    }

    public static GameObject Instantiate(GameObject gameObject, Transform parent)
    {
        GameObject instantiated = MonoBehaviour.Instantiate(gameObject);
        instantiated.transform.SetParent(parent);
        instantiated.SetActive(true);   
        instantiated.transform.SetParent(parent, false);
        return instantiated;
    }
}
