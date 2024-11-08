using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPowerUp : MonoBehaviour
{

    private int amount = 15;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            PlayerHealth.instance.HealPlayer(amount);
            Destroy(gameObject);
        }

    }
}
