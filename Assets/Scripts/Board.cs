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
    public Intern internBoard;
    public Extern externBoard;
    public Board()
    {
        internBoard = new Intern(this);
        externBoard = new Extern(this);
    }
    public class Intern
    {
        public Pieces[,] board = new Pieces[8, 8];
        public bool whiteToMove;
        public CastleAbility castleAbility;
        public EnPassant enPassant;
        public int halfMoveCounter;
        public int fullMoveCounter;

        public List<Move> moves = new List<Move>();
        
        public Intern(Board board)
        {
            Fen.Apply(this, board, "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        }

        public void AddMove(Move move)
        {
            moves.Add(move);
            if (board[move.endPosition.x, move.endPosition.y].GetType() == typeof(Pieces.None))
                move.isCapture = false;
            else
                move.isCapture = (board[move.startPosition.x, move.startPosition.y].internPiece.isWhite != board[move.endPosition.x, move.endPosition.y].internPiece.isWhite);
            ExecuteMoves();
        }
        public void ExecuteMoves()
        {
            foreach (Move move in moves)
            {
                //Debug.Log($"{move.startPosition.x}, {move.startPosition.y}: {board[move.startPosition.x, move.startPosition.y]}, {move.endPosition.x}, {move.endPosition.y} : {board[move.endPosition.x, move.endPosition.y]}, {move.isCapture}");
                if (move.isCapture)
                {
                    Pieces buffer1 = board[move.startPosition.x, move.startPosition.y];
                    Pieces buffer2 = new Pieces.None(board[move.startPosition.x, move.startPosition.y].board);
                    
                    buffer1.internPiece.position = move.endPosition;
                    buffer2.internPiece.position = move.startPosition;
                    buffer2.CreateExtern(buffer1.board.externBoard.board);

                    board[buffer1.internPiece.position.x, buffer1.internPiece.position.y] = buffer1;
                    board[buffer2.internPiece.position.x, buffer2.internPiece.position.y] = buffer2;
                }
                else
                {
                    Pieces buffer1 = board[move.startPosition.x, move.startPosition.y];
                    Pieces buffer2 = board[move.endPosition.x, move.endPosition.y];

                    buffer1.internPiece.position = move.endPosition;
                    buffer2.internPiece.position = move.startPosition;

                    board[buffer1.internPiece.position.x, buffer1.internPiece.position.y] = buffer1;
                    board[buffer2.internPiece.position.x, buffer2.internPiece.position.y] = buffer2;
                }
            }
            moves.Clear();
        }

        public static class Fen
        {
            // To Do: If there is no piece to place. Put a None Piece in its place. (Piece.None)
            public static void Apply(Board.Intern internBoard, Board board, string fenString)
            {
                internBoard.castleAbility = new CastleAbility();
                internBoard.enPassant = new EnPassant();

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
                                internBoard.board[x, y] = new Pieces.White_King(board);
                                internBoard.board[x, y].internPiece.position = new Vector2Int((int)x, (int)y);
                                x++;
                                break;
                            case 'Q':
                                internBoard.board[x, y] = new Pieces.White_Queen(board);
                                internBoard.board[x, y].internPiece.position = new Vector2Int((int)x, (int)y);
                                x++;
                                break;
                            case 'R':
                                internBoard.board[x, y] = new Pieces.White_Rook(board);
                                internBoard.board[x, y].internPiece.position = new Vector2Int((int)x, (int)y);
                                x++;
                                break;
                            case 'B':   
                                internBoard.board[x, y] = new Pieces.White_Bischop(board);
                                internBoard.board[x, y].internPiece.position = new Vector2Int((int)x, (int)y);
                                x++;
                                break;
                            case 'N':
                                internBoard.board[x, y] = new Pieces.White_Knight(board);
                                internBoard.board[x, y].internPiece.position = new Vector2Int((int)x, (int)y);
                                x++;
                                break;
                            case 'P':
                                internBoard.board[x, y] = new Pieces.White_Pawn(board);
                                internBoard.board[x, y].internPiece.position = new Vector2Int((int)x, (int)y);
                                x++;
                                break;
                            case 'k':
                                internBoard.board[x, y] = new Pieces.Black_King(board);
                                internBoard.board[x, y].internPiece.position = new Vector2Int((int)x, (int)y);
                                x++;
                                break;
                            case 'q':
                                internBoard.board[x, y] = new Pieces.Black_Queen(board);
                                internBoard.board[x, y].internPiece.position = new Vector2Int((int)x, (int)y);
                                x++;
                                break;
                            case 'r':
                                internBoard.board[x, y] = new Pieces.Black_Rook(board);
                                internBoard.board[x, y].internPiece.position = new Vector2Int((int)x, (int)y);
                                x++;
                                break;
                            case 'b':
                                internBoard.board[x, y] = new Pieces.Black_Bischop(board);
                                internBoard.board[x, y].internPiece.position = new Vector2Int((int)x, (int)y);
                                x++;
                                break;
                            case 'n':
                                internBoard.board[x, y] = new Pieces.Black_Knight(board);
                                internBoard.board[x, y].internPiece.position = new Vector2Int((int)x, (int)y);
                                x++;
                                break;
                            case 'p':
                                internBoard.board[x, y] = new Pieces.Black_Pawn(board);
                                internBoard.board[x, y].internPiece.position = new Vector2Int((int)x, (int)y);
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
                            internBoard.board[x, y] = new Pieces.None(board);
                            x++;
                        }
                        i = 0;
                    } else if (!toMoveDone)
                    {
                        switch (letter)
                        {
                            case 'w':
                                internBoard.whiteToMove = true;
                                break;
                            case 'b':
                                internBoard.whiteToMove = false;
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
                                internBoard.castleAbility.whiteKingSide = true;
                                break;
                            case 'Q':
                                internBoard.castleAbility.whiteQueenSide = true;
                                break;
                            case 'k':
                                internBoard.castleAbility.blackKingSide = true;
                                break;
                            case 'q':
                                internBoard.castleAbility.blackQueenSide = true;
                                break;
                            case '-':
                                internBoard.castleAbility.whiteKingSide = false;
                                internBoard.castleAbility.whiteQueenSide = false;
                                internBoard.castleAbility.whiteKingSide = false;
                                internBoard.castleAbility.whiteKingSide = false;
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
                                internBoard.enPassant.column = -1;
                                enPassantDone = true;
                                break;
                            case 'a':
                                internBoard.enPassant.column = 0;
                                break;
                            case 'b':
                                internBoard.enPassant.column = 1;
                                break;
                            case 'c':
                                internBoard.enPassant.column = 2;
                                break;
                            case 'd':
                                internBoard.enPassant.column = 3;
                                break;
                            case 'e':
                                internBoard.enPassant.column = 4;
                                break;
                            case 'f':
                                internBoard.enPassant.column = 5;
                                break;
                            case 'g':
                                internBoard.enPassant.column = 6;
                                break;
                            case 'h':
                                internBoard.enPassant.column = 7;
                                break;
                            case '6':
                                internBoard.enPassant.forWhite = false;
                                break;
                            case '3':
                                internBoard.enPassant.forWhite = true;
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
                        if (int.TryParse(letter.ToString(), out internBoard.halfMoveCounter))
                            halfCounterDone = true;

                    } else if (!fullCounterDone)
                    {
                        if (letter == ' ')
                            break;
                        if (int.TryParse(letter.ToString(), out internBoard.fullMoveCounter))
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
        public GameObject playSurface;

        public Vector3 boardOrigin;
        public Vector3 boardScale;
        
        public Extern(Board board) 
        {
            this.board = MonoBehaviour.Instantiate(GameManager.instance.CHESSBOARD);
            this.board.SetActive(true);
            this.playSurface = this.board.transform.GetChild(0).gameObject;
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    board.internBoard.board[x, y].CreateExtern(this.board);
                }
            }
        }
    }
}
