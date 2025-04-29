using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public string levelName;
    public Color color;

    private void Start()
    {
        TextMeshProUGUI txt = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        txt.text = levelName;
        transform.GetComponent<Image>().color = color;
    }
    public void LoadLevel()
    {
        SceneManager.LoadScene(levelName); 
    }
}
