using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UIElements;


public class Board
{
    public Board.Intern internBoard;
    public Board.Extern externBoard;
    public Board()
    {
        internBoard = new Board.Intern();
        externBoard = new Board.Extern(this.internBoard);
    }
    public class Intern
    {
        public Pieces[,] board = new Pieces[8, 8];
        public bool whiteToMove;
        public CastleAbility castleAbility;
        public EnPassant enPassant;
        public int halfMoveCounter;
        public int fullMoveCounter;
        
        public Intern()
        {
            Fen.Apply(this, "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        }

        public class Fen
        {
            // To Do: If there is no piece to place. Put a None Piece in its place. (Piece.None)
            public static void Apply(Board.Intern board, string fenString)
            {
                board.castleAbility = new CastleAbility();
                board.enPassant = new EnPassant();

                int x = 0;
                int y = 0;
                int i = 0;
                bool boardDone = false;
                bool toMoveDone = false;
                bool castleDone = false;
                bool enPassantDone = false;
                bool halfCounterDone = false;
                bool fullCounterDone = false;

                Char[] letters = fenString.ToCharArray();

                foreach (char letter in letters)
                {
                    if (!boardDone)
                    {
                        switch (letter)
                        {
                            case 'K':
                                board.board[x, y] = new Pieces.White_King();
                                x++;
                                break;
                            case 'Q':
                                board.board[x, y] = new Pieces.White_Queen();

                                x++;
                                break;
                            case 'R':
                                board.board[x, y] = new Pieces.White_Rook();
                                x++;
                                break;
                            case 'B':   
                                board.board[x, y] = new Pieces.White_Bischop();
                                x++;
                                break;
                            case 'N':
                                board.board[x, y] = new Pieces.White_Knight();
                                x++;
                                break;
                            case 'P':
                                board.board[x, y] = new Pieces.White_Pawn();
                                x++;
                                break;
                            case 'k':
                                board.board[x, y] = new Pieces.Black_King();
                                x++;
                                break;
                            case 'q':
                                board.board[x, y] = new Pieces.Black_Queen();
                                x++;
                                break;
                            case 'r':
                                board.board[x, y] = new Pieces.Black_Rook();
                                x++;
                                break;
                            case 'b':
                                board.board[x, y] = new Pieces.Black_Bischop();
                                x++;
                                break;
                            case 'n':
                                board.board[x, y] = new Pieces.Black_Knight();
                                x++;
                                break;
                            case 'p':
                                board.board[x, y] = new Pieces.Black_Pawn();
                                x++;
                                break;
                            case '1':
                                i++;
                                break;
                            case '2':
                                i += 2;
                                break;
                            case '3':
                                i += 3;
                                break;
                            case '4':
                                i += 4;
                                break;
                            case '5':
                                i += 5;
                                break;
                            case '6':
                                i += 6;
                                break;
                            case '7':
                                i += 7;
                                break;
                            case '8':
                                i += 8;
                                break;
                            case '/':
                                x = 0;
                                y++;
                                break;
                            case ' ':
                                boardDone = true;
                                break;
                            default:
                                Debug.LogError("Fen String is malformed!");
                                break;
                        }
                        for(int u = 0; u < i; u++)
                        {
                            board.board[x, y] = new Pieces.None();
                            x++;
                        }
                        i = 0;
                    } else if (!toMoveDone)
                    {
                        switch (letter)
                        {
                            case 'w':
                                board.whiteToMove = true;
                                break;
                            case 'b':
                                board.whiteToMove = false;
                                break;
                            case ' ':
                                toMoveDone = true;
                                break;
                            default:
                                Debug.LogError("Fen String is malformed!");
                                break;
                        }
                    } else if (!castleDone)
                    {
                        switch (letter)
                        {
                            case 'K':
                                board.castleAbility.whiteKingSide = true;
                                break;
                            case 'Q':
                                board.castleAbility.whiteQueenSide = true;
                                break;
                            case 'k':
                                board.castleAbility.blackKingSide = true;
                                break;
                            case 'q':
                                board.castleAbility.blackQueenSide = true;
                                break;
                            case '-':
                                board.castleAbility.whiteKingSide = false;
                                board.castleAbility.whiteQueenSide = false;
                                board.castleAbility.whiteKingSide = false;
                                board.castleAbility.whiteKingSide = false;
                                break;
                            case ' ':
                                castleDone = true;
                                break;
                            default:
                                Debug.LogError("Fen String is malformed!");
                                break;
                        }
                    } else if (!enPassantDone)
                    {
                        switch (letter)
                        {
                            case '-':
                                board.enPassant.column = -1;
                                enPassantDone = true;
                                break;
                            case 'a':
                                board.enPassant.column = 0;
                                break;
                            case 'b':
                                board.enPassant.column = 1;
                                break;
                            case 'c':
                                board.enPassant.column = 2;
                                break;
                            case 'd':
                                board.enPassant.column = 3;
                                break;
                            case 'e':
                                board.enPassant.column = 4;
                                break;
                            case 'f':
                                board.enPassant.column = 5;
                                break;
                            case 'g':
                                board.enPassant.column = 6;
                                break;
                            case 'h':
                                board.enPassant.column = 7;
                                break;
                            case '6':
                                board.enPassant.forWhite = false;
                                break;
                            case '3':
                                board.enPassant.forWhite = true;
                                break;
                            case ' ':
                                enPassantDone = true;
                                break;
                            default:
                                Debug.LogError($"Fen String is malformed! {letter}");
                                break;
                        }
                    } else if (!halfCounterDone)
                    {
                        if (int.TryParse(letter.ToString(), out board.halfMoveCounter))
                            halfCounterDone = true;

                    } else if (!fullCounterDone)
                    {
                        if (letter == ' ')
                            break;
                        if (int.TryParse(letter.ToString(), out board.fullMoveCounter))
                            fullCounterDone = true;
                        else
                            Debug.LogError("Fen String is malformed!");
                    }   
                }
                
            }
        }
        public struct CastleAbility
        {
            public bool whiteKingSide;
            public bool whiteQueenSide;
            public bool blackKingSide;
            public bool blackQueenSide;
            public CastleAbility(bool whiteKingSide, bool whiteQueenSide, bool blackKingSide, bool blackQueenSide)
            {
                this.whiteKingSide = whiteKingSide;
                this.whiteQueenSide = whiteQueenSide;
                this.blackKingSide = blackKingSide;
                this.blackQueenSide = blackQueenSide;
            }
        }
        public struct EnPassant
        {
            public bool forWhite;
            public int column;
            public EnPassant(bool forWhite, int column)
            {
                this.forWhite = forWhite;
                this.column = column;
            }
        }
    }
    public class Extern
    {
        public GameObject board;
        public Vector3 boardOrigin;
        public Vector3 boardScale;
        
        public Extern(Board.Intern internBoard) 
        {
            this.board = MonoBehaviour.Instantiate(GameManager.instance.CHESSBOARD);
            this.board.SetActive(true);
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    internBoard.board[x, y].CreateExtern(this.board);
                }
            }
            boardScale = new Vector3(this.board.transform.localScale.x, this.board.transform.localScale.y, this.board.transform.localScale.z);
            boardOrigin = new Vector3(this.board.transform.position.x + 3.5f * 0.06f * this.board.transform.localScale.x, this.board.transform.position.z, this.board.transform.position.y - 3.5f * 0.06f * this.board.transform.localScale.y);
        }
    }
}
