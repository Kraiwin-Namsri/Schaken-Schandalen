using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Board.Intern;

public class Fen
{
    public Board board;
    public Fen(Board board)
    {
        this.board = board;
    }
        // To Do: If there is no piece to place. Put a None Piece in its place. (Piece.None)
        public void Apply(string fenString)
        {
            board.intern.castleAbility = new CastleAbility();
            board.intern.enPassant = new EnPassant();

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
                            board.intern.array[x, y] = new Piece.White.King(board);
                            board.intern.array[x, y].intern.position = new Vector2Int((int)x, (int)y);
                            x++;
                            break;
                        case 'Q':
                            board.intern.array[x, y] = new Piece.White.Queen(board);
                            board.intern.array[x, y].intern.position = new Vector2Int((int)x, (int)y);
                            x++;
                            break;
                        case 'R':
                            board.intern.array[x, y] = new Piece.White.Rook(board);
                            board.intern.array[x, y].intern.position = new Vector2Int((int)x, (int)y);
                            x++;
                            break;
                        case 'B':
                            board.intern.array[x, y] = new Piece.White.Bischop(board);
                            board.intern.array[x, y].intern.position = new Vector2Int((int)x, (int)y);
                            x++;
                            break;
                        case 'N':
                            board.intern.array[x, y] = new Piece.White.Knight(board);
                            board.intern.array[x, y].intern.position = new Vector2Int((int)x, (int)y);
                            x++;
                            break;
                        case 'P':
                            board.intern.array[x, y] = new Piece.White.Pawn(board);
                            board.intern.array[x, y].intern.position = new Vector2Int((int)x, (int)y);
                            x++;
                            break;
                        case 'k':
                            board.intern.array[x, y] = new Piece.Black.King(board);
                            board.intern.array[x, y].intern.position = new Vector2Int((int)x, (int)y);
                            x++;
                            break;
                        case 'q':
                            board.intern.array[x, y] = new Piece.Black.Queen(board);
                            board.intern.array[x, y].intern.position = new Vector2Int((int)x, (int)y);
                            x++;
                            break;
                        case 'r':
                            board.intern.array[x, y] = new Piece.Black.Rook(board);
                            board.intern.array[x, y].intern.position = new Vector2Int((int)x, (int)y);
                            x++;
                            break;
                        case 'b':
                            board.intern.array[x, y] = new Piece.Black.Bischop(board);
                            board.intern.array[x, y].intern.position = new Vector2Int((int)x, (int)y);
                            x++;
                            break;
                        case 'n':
                            board.intern.array[x, y] = new Piece.Black.Knight(board);
                            board.intern.array[x, y].intern.position = new Vector2Int((int)x, (int)y);
                            x++;
                            break;
                        case 'p':
                            board.intern.array[x, y] = new Piece.Black.Pawn(board);
                            board.intern.array[x, y].intern.position = new Vector2Int((int)x, (int)y);
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
                        board.intern.array[x, y] = new Piece.None(board);
                        x++;
                    }
                    i = 0;
                }
                else if (!toMoveDone)
                {
                    switch (letter)
                    {
                        case 'w':
                            board.intern.whiteToMove = true;
                            break;
                        case 'b':
                            board.intern.whiteToMove = false;
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
                            board.intern.castleAbility.whiteKingSide = true;
                            break;
                        case 'Q':
                            board.intern.castleAbility.whiteQueenSide = true;
                            break;
                        case 'k':
                            board.intern.castleAbility.blackKingSide = true;
                            break;
                        case 'q':
                            board.intern.castleAbility.blackQueenSide = true;
                            break;
                        case '-':
                            board.intern.castleAbility.whiteKingSide = false;
                            board.intern.castleAbility.whiteQueenSide = false;
                            board.intern.castleAbility.whiteKingSide = false;
                            board.intern.castleAbility.whiteKingSide = false;
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
                    Vector2Int enPassantCoordinate = new Vector2Int();
                    switch (letter)
                    {
                        case '-':
                            enPassantCoordinate.x = -1;
                            enPassantCoordinate.y = -1;
                            enPassantDone = true;
                            break;
                        case 'a':
                            enPassantCoordinate.x = 0;
                            break;
                        case 'b':
                            enPassantCoordinate.x = 1;
                            break;
                        case 'c':
                            enPassantCoordinate.x = 2;
                            break;
                        case 'd':
                            enPassantCoordinate.x = 3;
                            break;
                        case 'e':
                            enPassantCoordinate.x = 4;
                            break;
                        case 'f':
                            enPassantCoordinate.x = 5;
                            break;
                        case 'g':
                            enPassantCoordinate.x = 6;
                            break;
                        case 'h':
                            enPassantCoordinate.x = 7;
                            break;
                        case '6':
                            enPassantCoordinate.y = 5;
                            break;
                        case '3':
                            enPassantCoordinate.y = 2;
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
                    if (int.TryParse(letter.ToString(), out board.intern.halfMoveCounter))
                        halfCounterDone = true;

                }
                else if (!fullCounterDone)
                {
                    if (letter == ' ')
                        break;
                    if (int.TryParse(letter.ToString(), out board.intern.fullMoveCounter))
                        fullCounterDone = true;
                    else
                        Debug.LogError("Fen String is malformed!");
                }
            }

        }
        public string BoardToFen()
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
                    switch (board.intern.array[x, y].GetType().ToString())
                    {
                        case "Piece+None":
                            emptySquareCounter++;
                            wasEmptySquare = true;
                            break;
                        case "Piece+White+King":
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
                        case "Piece+White+Queen":
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
                        case "Piece+White+Rook":
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
                        case "Piece+White+Bischop":
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
                        case "Piece+White+Knight":
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
                        case "Piece+White+Pawn":
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
                        case "Piece+Black+King":
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
                        case "Piece+Black+Queen":
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
                        case "Piece+Black+Rook":
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
                        case "Piece+Black+Bischop":
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
                        case "Piece+Black+Knight":
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
                        case "Piece+Black+Pawn":
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
            if (board.intern.halfMoveCounter % 2 == 0)
            {
                board.intern.whiteToMove = true;
                fenStringBuild += "w";
            }
            else
            {
                board.intern.whiteToMove = false;
                fenStringBuild += "b";
            }
            fenStringBuild += " ";

            //Check the castleability
            if (board.intern.castleAbility.whiteKingSide == true)
            {
                fenStringBuild += "K";
            }
            if (board.intern.castleAbility.whiteQueenSide == true)
            {
                fenStringBuild += "Q";
            }
            if (board.intern.castleAbility.blackKingSide == true)
            {
                fenStringBuild += "k";
            }
            if (board.intern.castleAbility.blackQueenSide == true)
            {
                fenStringBuild += "q";
            }
            if (board.intern.castleAbility.whiteKingSide == false && board.intern.castleAbility.whiteQueenSide == false && board.intern.castleAbility.blackKingSide == false && board.intern.castleAbility.blackQueenSide == false)
            {
                fenStringBuild += "-";
            }

        //To Do: convert vector2Int to ingame coordinate.
        Vector2Int enPassantCoordinate = board.intern.enPassant.coordinate;
        string enPassantCoordinatesString =  "";

        Debug.Log(enPassantCoordinate);

        if(enPassantCoordinate != new Vector2Int(-1,-1))
        {
            Debug.Log("door check");
            switch (enPassantCoordinate.x)
            {
                case 0:
                    enPassantCoordinatesString += "a";
                    break;
                case 1:
                    enPassantCoordinatesString += "b";
                    break;
                case 2:
                    enPassantCoordinatesString += "c";
                    break;
                case 3:
                    enPassantCoordinatesString += "d";
                    break;
                case 4:
                    enPassantCoordinatesString += "e";
                    break;
                case 5:
                    enPassantCoordinatesString += "f";
                    break;
                case 6:
                    enPassantCoordinatesString += "g";
                    break;
                case 7:
                    enPassantCoordinatesString += "h";
                    break;
            }
            enPassantCoordinatesString += (9 - enPassantCoordinate.y - 1).ToString();
        }
        else
        {
            enPassantCoordinatesString = "-";
        }
        
        //Check for enpassent
        fenStringBuild += " ";
        fenStringBuild += enPassantCoordinatesString;
        fenStringBuild += " ";

        //Check the Move Count
        fenStringBuild += board.intern.halfMoveClockCounter.ToString();
        fenStringBuild += " ";
        fenStringBuild += board.intern.fullMoveCounter.ToString();
        Debug.Log(fenStringBuild);
        return fenStringBuild;
        }
}