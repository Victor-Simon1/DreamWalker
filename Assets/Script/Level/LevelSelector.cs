using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public string levelName;

    private void Start()
    {
        TextMeshProUGUI txt = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        txt.text = levelName;
    }
    public void LoadLevel()
    {
        SceneManager.LoadScene(levelName); 
    }
}
