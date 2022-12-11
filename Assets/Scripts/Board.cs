using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Pieces[,] internalBoard = new Pieces[8, 8];

    public Board()
    {
        Fen.Apply(this, "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
    }
    public class Fen
    {
        public static void Apply(Board board, string fenString)
        {
            int x = 0;
            int y = 0;
            bool stop = false;

            Char[] letters = fenString.ToCharArray();
            
            foreach(char letter in letters)
            {
                if (stop) { break; }
                    switch (letter)
                    {
                        case 'K':
                            board.internalBoard[x, y] = new Pieces.White_King();
                            x++;
                            break;
                        case 'Q':
                            board.internalBoard[x, y] = new Pieces.White_Queen();

                            x++;
                            break;
                        case 'R':
                            board.internalBoard[x, y] = new Pieces.White_Rook();
                            x++;
                            break;
                        case 'B':
                            board.internalBoard[x, y] = new Pieces.White_Bischop();
                            x++;
                            break;
                        case 'N':
                            board.internalBoard[x, y] = new Pieces.White_Knight();
                            x++;
                            break;
                        case 'P':
                            board.internalBoard[x, y] = new Pieces.White_Pawn();
                            x++;
                            break;
                        case 'k':
                            board.internalBoard[x, y] = new Pieces.Black_King();
                            x++;
                            break;
                        case 'q':
                            board.internalBoard[x, y] = new Pieces.Black_Queen();
                            x++;
                            break;
                        case 'r':
                            board.internalBoard[x, y] = new Pieces.Black_Rook();
                            x++;
                            break;
                        case 'b':
                            board.internalBoard[x, y] = new Pieces.Black_Bischop();
                            x++;
                            break;
                        case 'n':
                            board.internalBoard[x, y] = new Pieces.Black_Knight();
                            x++;
                            break;
                        case 'p':
                            board.internalBoard[x, y] = new Pieces.Black_Pawn();
                            x++;
                            break;
                        case '1':
                            x++;
                            break;
                        case '2':
                            x += 2;
                            break;
                        case '3':
                            x += 3;
                            break;
                        case '4':
                            x += 4;
                            break;
                        case '5':
                            x += 5;
                            break;
                        case '6':
                            x += 6;
                            break;
                        case '7':
                            x += 7;
                            break;
                        case '8':
                            x += 8;
                            break;
                        case '/':
                            y++;
                            break;
                        case ' ':
                            stop = true;
                            break;
                    }
            }
        }
    }
}
