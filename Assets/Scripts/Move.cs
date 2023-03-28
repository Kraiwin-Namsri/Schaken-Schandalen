using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class MoveManager
{
    private List<Move> moves;
    MoveManager()
    {
        moves = new List<Move>();
    }
    //To Do: when changing position of one of pieces. It should automatically update the piece as wel as on the board.
    public void ExecuteMoves(Board board)
    {
        foreach (Move move in moves)
        {
            //Buffer for appended moves.
            Move currentMove = move;
            do
            {
                //If one of the moves is illegal stop executing.
                if (!Legal.IsLegal(currentMove, board.intern))
                {
                    moves.Clear();
                    return;
                }
                //Store the two pieces on the start and end position
                Piece buffer1 = board.intern.board[currentMove.startPosition.x, currentMove.startPosition.y];
                Piece buffer2 = board.intern.board[currentMove.endPosition.x, currentMove.endPosition.y];

                //Update rules
                if (buffer1.GetType() == typeof(Piece.White.Pawn) | buffer1.GetType() == typeof(Piece.Black.Pawn))
                {
                    UpdatePawnFirstMove(buffer1);
                }
                UpdateCastleAbility(buffer1, board.intern, currentMove);

                //Set the internal position to the endposition of the move
                buffer1.intern.position = currentMove.endPosition;

                if (Legal.IsCapture(currentMove, board.intern))
                {
                    Piece buffer3 = new Piece.None(board);

                    buffer3.intern.position = currentMove.startPosition;

                    board.intern.board[buffer1.intern.position.x, buffer1.intern.position.y] = buffer1;
                    board.intern.board[buffer3.intern.position.x, buffer3.intern.position.y] = buffer3;
                    board.intern.captured.Add(buffer2);
                }
                else
                {
                    // Simply switch the piece+none
                    buffer2.intern.position = currentMove.startPosition;

                    board.intern.board[buffer1.intern.position.x, buffer1.intern.position.y] = buffer1;
                    board.intern.board[buffer2.intern.position.x, buffer2.intern.position.y] = buffer2;
                }
                
                currentMove = currentMove.appendedMove;
            } while (currentMove is not null);
        }
        moves.Clear();
    }
    private void UpdatePawnFirstMove(dynamic piece)
    {
        if (piece.intern.isFirstMove)
        {
            piece.intern.moveOffsets.RemoveAt(1);
            piece.intern.isFirstMove = false;
        }
    }
    private void UpdateCastleAbility(Piece piece, Board.Intern internboard, Move currentMove)
    {
        if (piece.GetType() == typeof(Piece.White.King))
        {
            internboard.castleAbility.whiteKingSide = false;
            internboard.castleAbility.whiteQueenSide = false;
        }
        else if (piece.GetType() == typeof(Piece.Black.King))
        {
            internboard.castleAbility.blackKingSide = false;
            internboard.castleAbility.blackQueenSide = false;
        }
        else if (piece.GetType() == typeof(Piece.White.Rook))
        {
            if (new Vector2(currentMove.startPosition.x, currentMove.startPosition.y) == new Vector2(7, 7))
            {
                internboard.castleAbility.whiteKingSide = false;
            }
            else if (new Vector2(currentMove.startPosition.x, currentMove.startPosition.y) == new Vector2(0, 7))
            {
                internboard.castleAbility.whiteQueenSide = false;
            }
        }
        else if (piece.GetType() == typeof(Piece.Black.Rook))
        {
            if (new Vector2(currentMove.startPosition.x, currentMove.startPosition.y) == new Vector2(0, 0))
            {
                internboard.castleAbility.blackQueenSide = false;
            }
            else if (new Vector2(currentMove.startPosition.x, currentMove.startPosition.y) == new Vector2(7, 0))
            {
                internboard.castleAbility.blackKingSide = false;
            }
        }

    }
}
public class Move
{
    public MoveManager moveManager;
    public Vector2Int startPosition;
    public Vector2Int endPosition;
    public Move appendedMove;

    public Move(Vector2Int startPosition, Vector2Int endPosition, Board.Intern board, Move appendMove = null)
    {
        this.startPosition = startPosition;
        this.endPosition = endPosition;
        this.appendedMove = appendMove;
    }

    //For comparing Moves
    public static bool Contains(List<Move> moves, Move moveToCheck)
    {
        foreach (Move move in moves)
        {
            if (move == moveToCheck) { return true; }
        }
        return false;
    }
    public static bool operator ==(Move move1, Move move2)
    {
        return (move1.startPosition == move2.startPosition && move1.endPosition == move2.endPosition);
    }
    public static bool operator !=(Move move1, Move move2)
    {
        return (move1.startPosition != move2.startPosition | move1.endPosition != move2.endPosition);
    }
}