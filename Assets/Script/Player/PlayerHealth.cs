using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    private int maxHealth = 100;
    private int currentHealth;

    private bool isInvisible = false;
    public SpriteRenderer graphics;
    public HealthBar healthBar;
    private float invisiblityFlashDelay = 0.3f;

    public static PlayerHealth instance = null;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("PlayerHealth is already set ");
            return;
        }
        instance = this;

    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMax(maxHealth);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
            TakeDamage(70);
    }
    public void TakeDamage(int damage)
    {
        if (!isInvisible)
        {
            Debug.Log("Player take " + damage + " of damage");
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
            if(currentHealth<=0)
            {
                Death();
                return;
            }
            isInvisible = true;
            StartCoroutine(InvisibilityFlash());
            StartCoroutine(HandleInvisibilityDelay());
        }
    }

    private void Death()
    {
        Debug.Log("Le joueur est mort !");
        //block movements
        PlayerMovement.instance.enabled = false;
        //block interactions
    }
    public void HealPlayer(int amount)
    {
        currentHealth += amount;
        currentHealth = (currentHealth > maxHealth) ? maxHealth :currentHealth;
        healthBar.SetHealth(currentHealth);
    }

    public IEnumerator InvisibilityFlash()
    {
        while(isInvisible) 
        {
            graphics.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(invisiblityFlashDelay);
            graphics.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(invisiblityFlashDelay);
        }
    }
    public IEnumerator HandleInvisibilityDelay()
    {
        yield return new WaitForSeconds(3f);
        isInvisible = false;
    }
}
