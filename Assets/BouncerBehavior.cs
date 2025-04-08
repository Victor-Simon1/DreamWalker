using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncerBehavior : MonoBehaviour
{
    float force = 550;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Bounce");
            PlayerMovement.instance.Rb.AddForce(new Vector2(0, force));
        }
    }
}
