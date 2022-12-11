using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        
        public Intern()
        {
            Fen.Apply(this, "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        }
        public class Fen
        {
            // To Do: If there is no piece to place. Put a None Piece in its place. (Piece.None)
            public static void Apply(Board.Intern board, string fenString)
            {
                int x = 0;
                int y = 0;
                int i = 0;
                bool stop = false;

                Char[] letters = fenString.ToCharArray();

                foreach (char letter in letters)
                {
                    if (stop) { break; }
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
                            stop = true;
                            break;
                    }
                    for(int u = 0; u < i; u++)
                    {
                        board.board[x, y] = new Pieces.None();
                        x++;
                    }
                    i = 0;
                }
                
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
