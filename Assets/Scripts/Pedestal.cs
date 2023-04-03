using Microsoft.MixedReality.Toolkit;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using System.Linq;

public class Pedestal
{
    public Intern intern;
    public Extern @extern;
    public Pedestal()
    {
        intern = new Intern(this);
        @extern= new Extern(this);
    }
    public void AddPiece(Piece piece)
    {
        intern.pieces.Add(piece);
        piece.@extern.pieceGameObject.transform.SetParent(@extern.piecesGameObject.transform);
        @extern.UpdatePedestal();
    }
    public void AddPieces(List<Piece> pieces)
    {
        intern.pieces.AddRange(pieces);
        foreach (Piece piece in pieces)
        {
            piece.@extern.pieceGameObject.transform.SetParent(@extern.piecesGameObject.transform);
            @extern.UpdatePedestal();
        }
    }
    public class Intern
    {
        public Pedestal pedestal;
        public List<Piece> pieces;
        public Intern(Pedestal pedestal)
        {
            this.pedestal = pedestal;
            pieces = new List<Piece>();
        }

    }
    public class Extern
    {
        public Pedestal pedestal;
        public GameObject pedestalGameObject;
        public GameObject pedestalPlaySurfaceGameObject;
        public GameObject piecesGameObject;

        public Extern(Pedestal pedestal)
        {
            this.pedestal = pedestal;
            pedestalGameObject = Prefab.Instantiate(Prefab.Pedestal, null);
            pedestalPlaySurfaceGameObject = pedestalGameObject.GetNamedChild("PlaySurface");
            piecesGameObject = pedestalGameObject.GetNamedChild("Pieces");
        }

        public void UpdatePedestal()
        {
            int x = pedestal.intern.pieces.Count - 1;
            int y = 0;

            Vector2 size = (Vector2)pedestalPlaySurfaceGameObject.transform.localScale;

            //First count the amount of black and white pieces that are supposed be on the pedestal
            int amountWhitePieces = 0;
            int amountWhitePawns = 0;
            int amountBlackPawns = 0;
            foreach (Piece piece in pedestal.intern.pieces)
            {
                if (piece.GetColor() == typeof(Piece.White))
                {
                    amountWhitePieces++;
                }
                if (piece.GetType() == typeof(Piece.White.Pawn))
                {
                    amountWhitePawns++;
                }
                if (piece.GetType() == typeof(Piece.Black.Pawn))
                {
                    amountBlackPawns++;
                }
            }
            int amountBlackPieces = pedestal.intern.pieces.Count - amountWhitePieces;

            // The pedestal is divided in 4 sectors. Where the bottom two contain thge white and black pawns.
            // The top two sectors contain the rest. Left are the white pieces, on the right side are the black ones.
            int amountTopLeft = amountWhitePieces - amountWhitePawns;
            int amountTopRight = amountBlackPieces - amountBlackPawns;

            Piece latestCapturedPiece = pedestal.intern.pieces.Last();
            if (latestCapturedPiece.GetColor() == typeof(Piece.White))
            {
                if (latestCapturedPiece.GetType() == typeof(Piece.White.Pawn))
                {
                    x = amountWhitePawns - 1;
                    y = 1;
                }
                else
                {
                    x = amountTopLeft - 1;
                    y = 0;
                }
            // Just for readability
            } else if (latestCapturedPiece.GetColor() == typeof(Piece.Black))
            {
                if (latestCapturedPiece.GetType() == typeof(Piece.Black.Pawn))
                {
                    x = 8 + amountBlackPawns - 1;
                    y = 1;
                } else
                {
                    x = 8 + amountTopRight - 1;
                    y = 0;
                }
            }

            //Calculate and set the position
            Vector2 position = new Vector3((size.x / 32) - (8 * size.x / 16) + ((x * size.x / 16)), -size.y + (y * size.y * 2));
            latestCapturedPiece.@extern.pieceGameObject.transform.localPosition = position;
        }
    }
}
