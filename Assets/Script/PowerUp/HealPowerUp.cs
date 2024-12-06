using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPowerUp : MonoBehaviour
{

    private int amount = 15;
    /// <summary>
    /// Add the power up to the player when collide
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            PlayerHealth.instance.HealPlayer(amount);
            Destroy(gameObject);
        }

    }
}
