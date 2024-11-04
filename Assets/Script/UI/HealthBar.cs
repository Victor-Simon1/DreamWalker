using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{

    [SerializeField] Slider healthbar;
    // Start is called before the first frame update
    void Awake()
    {
        healthbar = GetComponent<Slider>();
    }

    public void SetMax(int maxHealth)
    {
        healthbar.maxValue =  maxHealth;
        healthbar.value = maxHealth;
    }
    public void SetHealth(int health)
    {
        healthbar.value = health;
    }
 
}
