using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerManager
{
    public Hint hints;

    public MarkerManager()
    {
        hints = new Hint();
    }
    public class Hint
    {
        private List<GameObject> hints = new List<GameObject>();
        public void Visualize(Board board, Piece piece)
        {
            Delete();
            foreach (Move legalMove in piece.intern.legalMoves)
            {
                Vector3 destination = board.intern.ToExtern(legalMove.endPosition);
                GameObject hint = Prefab.Instantiate(Prefab.Hint, board.@extern.boardPlaySurfaceGameObject.transform);
                hint.transform.localPosition = destination;
                hints.Add(hint);
                hint.SetActive(true);
            }
        }
        // Refactor
        public void Delete()
        {
            foreach (GameObject hint in hints)
            {
                MonoBehaviour.Destroy(hint);
            }
            hints.Clear();
        }
    }
}
