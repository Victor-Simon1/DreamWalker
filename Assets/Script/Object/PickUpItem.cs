using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public Item item;
    /// <summary>
    /// Add item to the inventory if is not full
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !Inventory.instance.isFull)
        {
            Inventory.instance.AddItem(item);
            Destroy(gameObject);
        }
    }
}
