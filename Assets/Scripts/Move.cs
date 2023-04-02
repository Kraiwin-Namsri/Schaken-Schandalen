using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class MoveManager
{
    private List<Move> moves;
    private List<Move> moveQueue;
    public MoveManager()
    {
        moves = new List<Move>();
        moveQueue = new List<Move>();
    }
    public void AddMove(Move move)
    {
        moves.Add(move);
        moveQueue.Add(move);
    }
    
    //This functions executes all moves in the list moves, and returns all the captured pieces.
    public List<Piece> ExecuteMoveQueue(Board board)
    {
        List<Piece> capturedPieces = new List<Piece>();
        foreach (Move move in moveQueue)
        {
            //Buffer for appended moves.
            Move currentMove = move;
            do
            {
                //Store the two pieces on the start and end position
                Piece buffer1 = board.intern.array[currentMove.startPosition.x, currentMove.startPosition.y];
                Piece buffer2 = board.intern.array[currentMove.endPosition.x, currentMove.endPosition.y];



                //Update rules
                if (buffer1.GetType() == typeof(Piece.White.Pawn) | buffer1.GetType() == typeof(Piece.Black.Pawn))
                {
                    ((dynamic) buffer1).RemoveDoublePawnPush();
                }
                board.intern.UpdateEnPassant(buffer1, move);
                board.intern.UpdateCastleAbility(buffer1, currentMove);
                board.intern.UpdateHalfMove();
                board.intern.UpdateHalfMoveClock(Legal.IsCapture(currentMove, board.intern));



                //Set the internal position to the endposition of the move
                buffer1.intern.position = currentMove.endPosition;

                if (Legal.IsCapture(currentMove, board.intern))
                {
                    Piece buffer3 = new Piece.None(board);

                    buffer3.intern.position = currentMove.startPosition;

                    board.intern.array[buffer1.intern.position.x, buffer1.intern.position.y] = buffer1;
                    board.intern.array[buffer3.intern.position.x, buffer3.intern.position.y] = buffer3;
                    capturedPieces.Add(buffer2);
                }
                else
                {
                    buffer2.intern.position = currentMove.startPosition;
                    board.intern.array[buffer1.intern.position.x, buffer1.intern.position.y] = buffer1;
                    board.intern.array[buffer2.intern.position.x, buffer2.intern.position.y] = buffer2;
                }
                
                currentMove = currentMove.appendedMove;
            } while (currentMove is not null);
        }
        moveQueue.Clear();
        return capturedPieces;
    }
    
}
public class Move
{
    public Vector2Int startPosition;
    public Vector2Int endPosition;
    public bool fromWhite;
    public Move appendedMove;

    public Move(Vector2Int startPosition, Vector2Int endPosition, Board.Intern board, Move appendMove = null, bool fromWhite = false)
    {
        this.startPosition = startPosition;
        this.endPosition = endPosition;
        this.fromWhite = fromWhite;
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