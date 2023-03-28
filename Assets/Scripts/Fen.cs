using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Board.Intern;

public class Fen : MonoBehaviour
{
        // To Do: If there is no piece to place. Put a None Piece in its place. (Piece.None)
        public void Apply(Board board, string fenString)
        {
            board.internBoard.castleAbility = new CastleAbility();
            board.internBoard.enPassant = new EnPassant();

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
                            board.internBoard.board[x, y] = new Piece.White_King(board);
                            board.internBoard.board[x, y].internPiece.position = new Vector2Int((int)x, (int)y);
                            x++;
                            break;
                        case 'Q':
                            board.internBoard.board[x, y] = new Piece.White_Queen(board);
                            board.internBoard.board[x, y].internPiece.position = new Vector2Int((int)x, (int)y);
                            x++;
                            break;
                        case 'R':
                            board.internBoard.board[x, y] = new Piece.White_Rook(board);
                            board.internBoard.board[x, y].internPiece.position = new Vector2Int((int)x, (int)y);
                            x++;
                            break;
                        case 'B':
                            board.internBoard.board[x, y] = new Piece.White_Bischop(board);
                            board.internBoard.board[x, y].internPiece.position = new Vector2Int((int)x, (int)y);
                            x++;
                            break;
                        case 'N':
                            board.internBoard.board[x, y] = new Piece.White_Knight(board);
                            board.internBoard.board[x, y].internPiece.position = new Vector2Int((int)x, (int)y);
                            x++;
                            break;
                        case 'P':
                            board.internBoard.board[x, y] = new Piece.White_Pawn(board);
                            board.internBoard.board[x, y].internPiece.position = new Vector2Int((int)x, (int)y);
                            x++;
                            break;
                        case 'k':
                            board.internBoard.board[x, y] = new Piece.Black_King(board);
                            board.internBoard.board[x, y].internPiece.position = new Vector2Int((int)x, (int)y);
                            x++;
                            break;
                        case 'q':
                            board.internBoard.board[x, y] = new Piece.Black_Queen(board);
                            board.internBoard.board[x, y].internPiece.position = new Vector2Int((int)x, (int)y);
                            x++;
                            break;
                        case 'r':
                            board.internBoard.board[x, y] = new Piece.Black_Rook(board);
                            board.internBoard.board[x, y].internPiece.position = new Vector2Int((int)x, (int)y);
                            x++;
                            break;
                        case 'b':
                            board.internBoard.board[x, y] = new Piece.Black_Bischop(board);
                            board.internBoard.board[x, y].internPiece.position = new Vector2Int((int)x, (int)y);
                            x++;
                            break;
                        case 'n':
                            board.internBoard.board[x, y] = new Piece.Black_Knight(board);
                            board.internBoard.board[x, y].internPiece.position = new Vector2Int((int)x, (int)y);
                            x++;
                            break;
                        case 'p':
                            board.internBoard.board[x, y] = new Piece.Black_Pawn(board);
                            board.internBoard.board[x, y].internPiece.position = new Vector2Int((int)x, (int)y);
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
                    for (int u = 0; u < i; u++)
                    {
                        board.internBoard.board[x, y] = new Piece.None(board);
                        x++;
                    }
                    i = 0;
                }
                else if (!toMoveDone)
                {
                    switch (letter)
                    {
                        case 'w':
                            board.internBoard.whiteToMove = true;
                            break;
                        case 'b':
                            board.internBoard.whiteToMove = false;
                            break;
                        case ' ':
                            toMoveDone = true;
                            break;
                        default:
                            Debug.LogError("Fen String is malformed!");
                            break;
                    }
                }
                else if (!castleDone)
                {
                    switch (letter)
                    {
                        case 'K':
                            board.internBoard.castleAbility.whiteKingSide = true;
                            break;
                        case 'Q':
                            board.internBoard.castleAbility.whiteQueenSide = true;
                            break;
                        case 'k':
                            board.internBoard.castleAbility.blackKingSide = true;
                            break;
                        case 'q':
                            board.internBoard.castleAbility.blackQueenSide = true;
                            break;
                        case '-':
                            board.internBoard.castleAbility.whiteKingSide = false;
                            board.internBoard.castleAbility.whiteQueenSide = false;
                            board.internBoard.castleAbility.whiteKingSide = false;
                            board.internBoard.castleAbility.whiteKingSide = false;
                            break;
                        case ' ':
                            castleDone = true;
                            break;
                        default:
                            Debug.LogError("Fen String is malformed!");
                            break;
                    }
                }
                else if (!enPassantDone)
                {
                    switch (letter)
                    {
                        case '-':
                            board.internBoard.enPassant.column = -1;
                            enPassantDone = true;
                            break;
                        case 'a':
                            board.internBoard.enPassant.column = 0;
                            break;
                        case 'b':
                            board.internBoard.enPassant.column = 1;
                            break;
                        case 'c':
                            board.internBoard.enPassant.column = 2;
                            break;
                        case 'd':
                            board.internBoard.enPassant.column = 3;
                            break;
                        case 'e':
                            board.internBoard.enPassant.column = 4;
                            break;
                        case 'f':
                            board.internBoard.enPassant.column = 5;
                            break;
                        case 'g':
                            board.internBoard.enPassant.column = 6;
                            break;
                        case 'h':
                            board.internBoard.enPassant.column = 7;
                            break;
                        case '6':
                            board.internBoard.enPassant.forWhite = false;
                            break;
                        case '3':
                            board.internBoard.enPassant.forWhite = true;
                            break;
                        case ' ':
                            enPassantDone = true;
                            break;
                        default:
                            Debug.LogError($"Fen String is malformed! {letter}");
                            break;
                    }
                }
                else if (!halfCounterDone)
                {
                    if (int.TryParse(letter.ToString(), out board.internBoard.halfMoveCounter))
                        halfCounterDone = true;

                }
                else if (!fullCounterDone)
                {
                    if (letter == ' ')
                        break;
                    if (int.TryParse(letter.ToString(), out board.internBoard.fullMoveCounter))
                        fullCounterDone = true;
                    else
                        Debug.LogError("Fen String is malformed!");
                }
            }

        }
        public void BoardToFen(Board.Intern board)
        {
            string fenStringBuild = "";

            int emptySquareCounter = 0;
            bool wasEmptySquare = false;
            bool firstRound = true;

            //Check Piece position
            for (int y = 0; y < 8; y++)
            {
                if (firstRound == false)
                {
                    if (emptySquareCounter > 0)
                    {
                        fenStringBuild += emptySquareCounter.ToString();
                        emptySquareCounter = 0;
                    }
                    fenStringBuild += "/";
                }
                firstRound = false;
                for (int x = 0; x < 8; x++)
                {
                    switch (internboard.board[x, y].GetType().ToString())
                    {
                        case "Pieces+None":
                            emptySquareCounter++;
                            wasEmptySquare = true;
                            break;
                        case "Pieces+White_King":
                            if (wasEmptySquare == true)
                            {
                                wasEmptySquare = false;
                                if (emptySquareCounter > 0)
                                {
                                    fenStringBuild += emptySquareCounter.ToString();
                                    emptySquareCounter = 0;
                                }
                            }
                            fenStringBuild += "K";
                            break;
                        case "Pieces+White_Queen":
                            if (wasEmptySquare == true)
                            {
                                wasEmptySquare = false;
                                if (emptySquareCounter > 0)
                                {
                                    fenStringBuild += emptySquareCounter.ToString();
                                    emptySquareCounter = 0;
                                }
                            }
                            fenStringBuild += "Q";
                            break;
                        case "Pieces+White_Rook":
                            if (wasEmptySquare == true)
                            {
                                wasEmptySquare = false;
                                if (emptySquareCounter > 0)
                                {
                                    fenStringBuild += emptySquareCounter.ToString();
                                    emptySquareCounter = 0;
                                }
                            }
                            fenStringBuild += "R";
                            break;
                        case "Pieces+White_Bischop":
                            if (wasEmptySquare == true)
                            {
                                wasEmptySquare = false;
                                if (emptySquareCounter > 0)
                                {
                                    fenStringBuild += emptySquareCounter.ToString();
                                    emptySquareCounter = 0;
                                }
                            }
                            fenStringBuild += "B";
                            break;
                        case "Pieces+White_Knight":
                            if (wasEmptySquare == true)
                            {
                                wasEmptySquare = false;
                                if (emptySquareCounter > 0)
                                {
                                    fenStringBuild += emptySquareCounter.ToString();
                                    emptySquareCounter = 0;
                                }
                            }
                            fenStringBuild += "N";
                            break;
                        case "Pieces+White_Pawn":
                            if (wasEmptySquare == true)
                            {
                                wasEmptySquare = false;
                                if (emptySquareCounter > 0)
                                {
                                    fenStringBuild += emptySquareCounter.ToString();
                                    emptySquareCounter = 0;
                                }
                            }
                            fenStringBuild += "P";
                            break;
                        case "Pieces+Black_King":
                            if (wasEmptySquare == true)
                            {
                                wasEmptySquare = false;
                                if (emptySquareCounter > 0)
                                {
                                    fenStringBuild += emptySquareCounter.ToString();
                                    emptySquareCounter = 0;
                                }
                            }
                            fenStringBuild += "k";
                            break;
                        case "Pieces+Black_Queen":
                            if (wasEmptySquare == true)
                            {
                                wasEmptySquare = false;
                                if (emptySquareCounter > 0)
                                {
                                    fenStringBuild += emptySquareCounter.ToString();
                                    emptySquareCounter = 0;
                                }
                            }
                            fenStringBuild += "q";
                            break;
                        case "Pieces+Black_Rook":
                            if (wasEmptySquare == true)
                            {
                                wasEmptySquare = false;
                                if (emptySquareCounter > 0)
                                {
                                    fenStringBuild += emptySquareCounter.ToString();
                                    emptySquareCounter = 0;
                                }
                            }
                            fenStringBuild += "r";
                            break;
                        case "Pieces+Black_Bischop":
                            if (wasEmptySquare == true)
                            {
                                wasEmptySquare = false;
                                if (emptySquareCounter > 0)
                                {
                                    fenStringBuild += emptySquareCounter.ToString();
                                    emptySquareCounter = 0;
                                }
                            }
                            fenStringBuild += "b";
                            break;
                        case "Pieces+Black_Knight":
                            if (wasEmptySquare == true)
                            {
                                wasEmptySquare = false;
                                if (emptySquareCounter > 0)
                                {
                                    fenStringBuild += emptySquareCounter.ToString();
                                    emptySquareCounter = 0;
                                }
                            }
                            fenStringBuild += "n";
                            break;
                        case "Pieces+Black_Pawn":
                            if (wasEmptySquare == true)
                            {
                                wasEmptySquare = false;
                                if (emptySquareCounter > 0)
                                {
                                    fenStringBuild += emptySquareCounter.ToString();
                                    emptySquareCounter = 0;
                                }
                            }
                            fenStringBuild += "p";
                            break;
                    }
                }
            }
            //Check whose turn it is
            fenStringBuild += " ";
            if (internboard.halfMoveCounter % 2 == 0)
            {
                fenStringBuild += "w";
            }
            else
            {
                fenStringBuild += "b";
            }
            fenStringBuild += " ";

            //Check the castleability
            if (internboard.castleAbility.whiteKingSide == true)
            {
                fenStringBuild += "K";
            }
            if (internboard.castleAbility.whiteQueenSide == true)
            {
                fenStringBuild += "Q";
            }
            if (internboard.castleAbility.blackKingSide == true)
            {
                fenStringBuild += "k";
            }
            if (internboard.castleAbility.blackQueenSide == true)
            {
                fenStringBuild += "q";
            }
            if (internboard.castleAbility.whiteKingSide == false && internboard.castleAbility.whiteQueenSide == false && internboard.castleAbility.blackKingSide == false && internboard.castleAbility.blackQueenSide == false)
            {
                fenStringBuild += "-";
            }

            //Check for enpassent
            fenStringBuild += " ";
            fenStringBuild += enPassantCoordinates;
            fenStringBuild += " ";

            //Check the Move Count
            fenStringBuild += " ";
            fenStringBuild += internboard.halfMoveClockCounter.ToString();
            fenStringBuild += " ";
            fenStringBuild += internboard.fullMoveCounter.ToString();

            Debug.Log(fenStringBuild);
        }
}