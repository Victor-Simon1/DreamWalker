using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndDoor : MonoBehaviour
{
    private bool alreadyOpen = false;
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!alreadyOpen) 
        {
            GameObject[] scoreGO = GameObject.FindObjectsOfType<GameObject>(true).Where(sr => !sr.gameObject.activeInHierarchy && sr.CompareTag("ScoreScreen")).ToArray();
            scoreGO[0].gameObject.SetActive(true);
            PlayerMovement.instance.FreezePlayer();
            alreadyOpen = true;
        }
    
    }
}