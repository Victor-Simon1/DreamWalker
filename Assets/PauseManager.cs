using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject pauseUI;
    bool isInPause;

    private void Update()
    {
        Pause();
    }
    public void Pause()
    {
        if(UserInput.instance.PauseInput)
        {
            isInPause = !isInPause;
            pauseUI.SetActive(isInPause);
        }
        
    }
}
