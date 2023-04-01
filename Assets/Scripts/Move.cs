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
    public MoveManager()
    {
        moves = new List<Move>();
    }
    public void AddMove(Move move)
    {
        moves.Add(move);
    }
    
    //This functions executes all moves in the list moves, and returns all the captured pieces.
    public List<Piece> ExecuteMoveQueue(Board board)
    {
        List<Piece> capturedPieces = new List<Piece>();
        foreach (Move move in moves)
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
                UpdateCastleAbility(buffer1, board.intern, currentMove);

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
        moves.Clear();
        return capturedPieces;
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