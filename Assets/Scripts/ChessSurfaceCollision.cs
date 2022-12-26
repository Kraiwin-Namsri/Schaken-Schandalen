using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessSurfaceCollision : MonoBehaviour
{
    public void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Colission detected");
        if(collision.gameObject.name == "L_Pointer_end")
        {
            Debug.Log("With pointer FInger");
            GameManager.OnPointerfingerCollision(collision.gameObject);
            
        }
    }
}
