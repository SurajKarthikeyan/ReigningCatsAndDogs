using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Text healthText;
    public float maxHealth = 10.0f;
    public float currentHealth;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        if (this.gameObject.tag == "Player" && healthText)
        {
            healthText.text = ("Health: " + currentHealth.ToString());
        }
    }

    public bool IsDead()
    {
        return !isDead && currentHealth <= 0;
    }

    public void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (this.gameObject.tag == "Player" && healthText)
        {
            healthText.text = ("Health: " + currentHealth.ToString());
        }


        if (this.gameObject.tag == "Player" && IsDead())
        {
            // Temporary until Game Restart UI is in, just reload level
            Application.LoadLevel(Application.loadedLevel);
        }
        else if (this.gameObject.tag == "Cat" && IsDead())
        {
            this.gameObject.GetComponent<Cat>().EmptySeat();
            Die();
        }
        else
        {
            if (IsDead())
                Die();
        }
    }

    public void Heal(float amount)
    {
        if (currentHealth + amount <= maxHealth)
            currentHealth += amount;
        else
            currentHealth = maxHealth;

        if (this.gameObject.tag == "Player" && healthText)
        {
            healthText.text = ("Health: " + currentHealth.ToString());
        }
    }


}
