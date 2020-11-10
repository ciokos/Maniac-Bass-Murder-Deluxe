using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public float maxHealth = 100f;
    public float currentHealth;
    public Text healthText;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        RefreshHealthText();
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        RefreshHealthText();
    }

    private void RefreshHealthText()
    {
        healthText.text = ((int)currentHealth).ToString() + "/" + ((int)maxHealth).ToString();
    }
}
