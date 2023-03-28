using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    public static GameObject PREFAB_highlight;
    public static GameObject PREFAB_hint;
    public static GameObject PREFAB_marker;

    public Hint hints;

    public Marker()
    {
        hints = new Hint();
    }
    public class Hint
    {
        private List<GameObject> hints = new List<GameObject>();
        public void Visualize(GameManager gameManager, Piece piece)
        {
            Delete();
            foreach (Move legalMove in piece.internPiece.legalMoves)
            {
                Vector3 destination = gameManager.ConvertInternToExternPosition(legalMove.endPosition);
                GameObject hint = Instantiate(PREFAB_hint, gameManager.GetExternBoardPosition());
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
                Destroy(hint);
            }
            hints.Clear();
        }
    }
}
