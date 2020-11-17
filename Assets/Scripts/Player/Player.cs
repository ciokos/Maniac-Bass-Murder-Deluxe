using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public float maxHealth = 100f;
    public float currentHealth;
    public Text healthText;
    private Conductor conductor;

    // Start is called before the first frame update
    void Start()
    {
        conductor = FindObjectOfType<Conductor>();
        currentHealth = maxHealth;
        RefreshHealthText();
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        RefreshHealthText();
        if (currentHealth <= 0)
            GameOver();
    }

    private void RefreshHealthText()
    {
        healthText.text = ((int)currentHealth).ToString() + "/" + ((int)maxHealth).ToString();
    }

    private void GameOver()
    {
        SceneManager.LoadScene(2);
    }
}
