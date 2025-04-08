using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class DestructibleTile : MonoBehaviour
{
    public Tilemap destructibleTilemap;

    private void Start()
    {
        destructibleTilemap = GetComponent<Tilemap>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Vector3 hitPos = Vector3.zero;
            foreach (ContactPoint2D hit in collision.contacts) 
            {
                hitPos.x = hit.point.x+1 /*- 0.01f * hit.normal.x*/;
                hitPos.y = hit.point.y /*- 0.01f * hit.normal.y*/;
                Vector3Int tile = destructibleTilemap.WorldToCell(hitPos);
                destructibleTilemap.SetTile(destructibleTilemap.WorldToCell(hitPos), null);
            }
        }
    }
}
