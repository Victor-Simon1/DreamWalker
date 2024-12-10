using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] bool isOnCheckpoint;
    [SerializeField] bool isClickable = true;
    private void Update()
    {
        if(isOnCheckpoint && UserInput.instance.ChangeCheckpointInput)
        {
            ChangeCheckpoint();
        }
    }

    public void ChangeCheckpoint()
    {
        PlayerMovement.instance.SetCheckpoint(gameObject);
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
