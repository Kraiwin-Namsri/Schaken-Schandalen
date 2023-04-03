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
            Piece startPiece = board.intern.array[move.startPosition.x, move.startPosition.y];
                
            //Update rules
            if (startPiece.GetType() == typeof(Piece.White.Pawn) | startPiece.GetType() == typeof(Piece.Black.Pawn))
            {
                ((dynamic) startPiece).RemoveDoublePawnPush();
            }
            board.intern.legal.enPassant.Update(startPiece, move);
            board.intern.legal.castleAbility.Update(startPiece, move);
            board.intern.legal.remise.UpdateHalfMove();
            board.intern.legal.remise.UpdateHalfMoveClock(move);
            board.intern.legal.remise.UpdateFullMove();

            ExecuteMove(move, board.intern.array, board);
        }
        moveQueue.Clear();
        return capturedPieces;
    }
    public static void ExecuteMove(Move move, Piece[,] array, Board board)
    {
        Piece startPiece = array[move.startPosition.x, move.startPosition.y];
        
        Move currentMove = move;
        do
        {
            Piece buffer = array[currentMove.endPosition.x, currentMove.endPosition.y];
            startPiece.intern.position = currentMove.endPosition;
            if (Legal.IsCapture(currentMove, array))
            {
                Piece buffer3 = new Piece.None(board);

                buffer3.intern.position = currentMove.startPosition;

                array[startPiece.intern.position.x, startPiece.intern.position.y] = startPiece;
                array[buffer3.intern.position.x, buffer3.intern.position.y] = buffer3;
            }
            else
            {
                buffer.intern.position = currentMove.startPosition;
                array[startPiece.intern.position.x, startPiece.intern.position.y] = startPiece;
                array[buffer.intern.position.x, buffer.intern.position.y] = buffer;
            }

            currentMove = currentMove.appendedMove;
        } while (currentMove is not null);
    }
    
}
public class Move
{
    public Vector2Int startPosition;
    public Vector2Int endPosition;
    public Move appendedMove;

    public Move(Vector2Int startPosition, Vector2Int endPosition, Move appendMove = null)
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