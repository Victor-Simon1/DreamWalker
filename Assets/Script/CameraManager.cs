using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] float offsetZ = -10f;


    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + new Vector3(0,0, offsetZ);
    }
}
