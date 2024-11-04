using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyBehavior : MonoBehaviour
{

    [Header("Movement")]
    private int direction = 1;
    private float actualSpeed;
    private float normalSpeed = 2.2f;
    private float targetSpeed = 3.2f;
    private Transform target;
    [SerializeField] int destPoints = 0;
    [SerializeField] Transform[] waypoints;
    [Header("Player")]
    [SerializeField] GameObject player;
    [Header("Attributes")]
    int damage = 36;
    #region UNITY_FUNCTION
    // Start is called before the first frame update
    void Start()
    {
        actualSpeed = normalSpeed;
        player = GameObject.Find("Player");

        target = waypoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        Move();  
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth)
        {
            playerHealth.TakeDamage(damage);

        }
    }
    #endregion
    private void Move()
    {
        Vector3 dir = target.position - transform.position; 
        transform.Translate(dir.normalized * actualSpeed*Time.deltaTime,Space.World);
        if(Vector3.Distance(transform.position,target.position)<0.3f)
        {
            destPoints = (destPoints + 1) % waypoints.Length;
            target = waypoints[destPoints];
            Flip();
        }
    }
  
    private void Flip()
    {
        direction *= -1;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void Death()
    {
        Destroy(gameObject);
    }

  
}
