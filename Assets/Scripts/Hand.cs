using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hand : MonoBehaviour
{

    public static Dictionary<GameObject, Vector2Int> handCoordinates = new Dictionary<GameObject, Vector2Int>();

    public Hand()
    {

    }

    private void Start()
    {
        //GameManager.setParentPointerfinger(this);
    }
    public void OnTriggerEnter(Collider collision)
    {
        //GameManager.OnPointerfingerCollisionEnter(this);
    }
    public void OnTriggerExit(Collider collision)
    {
        //GameManager.OnPointerfingerCollisionExit(this);
    }
}
