using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public Item item;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !Inventory.instance.isFull)
        {
            Inventory.instance.AddItem(item);
            Destroy(gameObject);
        }
    }
}
