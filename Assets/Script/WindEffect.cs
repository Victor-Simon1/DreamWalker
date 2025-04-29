using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffect : MonoBehaviour
{
    [SerializeField] PlayerMovement player;

    [Header("WindVariable")]
    [SerializeField] float windForce = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerMovement.instance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        player.Rb.AddForce(Vector2.left * windForce);
    }
}
