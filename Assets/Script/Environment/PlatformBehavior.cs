using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlatformBehavior : MonoBehaviour
{

    public Rigidbody2D rb;
    float moveSpeed = 2f;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private Vector3 nextPos = Vector3.zero;
  
    // Start is called before the first frame update
    void Start()
    {
        nextPos = pointB.position;
    }

    // Update is called once per frame
    void Update()
    {
        GoToPos();
    }

    //Fonctions

    private void GoToPos()
    {
        transform.position = Vector2.MoveTowards(transform.position, nextPos, moveSpeed *Time.deltaTime);
        if(transform.position == nextPos)
            nextPos = (nextPos == pointA.position) ? pointB.position : pointA.position;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
            collision.gameObject.transform.parent = transform;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
            collision.gameObject.transform.parent = null;
    }
}
