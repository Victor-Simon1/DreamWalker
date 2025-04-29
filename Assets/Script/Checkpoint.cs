using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] bool isOnCheckpoint;
    [SerializeField] bool isClickable = true;
    ParticleSystem effect;

    private void Start()
    {
        effect = transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
    }
    private void Update()
    {
        if(isOnCheckpoint && UserInput.instance.ChangeCheckpointInput)
        {
            Debug.Log("New checkpoint");
            ChangeCheckpoint();
        }
    }

    public void ChangeCheckpoint()
    {
        PlayerMovement.instance.SetCheckpoint(gameObject);
        StartAnimationCheckPoint();
    }

    private void StartAnimationCheckPoint()
    {
        effect.Play();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isClickable)
            isOnCheckpoint = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isClickable)
            isOnCheckpoint = false;
    }
}
