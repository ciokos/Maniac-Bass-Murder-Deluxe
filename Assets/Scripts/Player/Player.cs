using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public float maxHealth = 100f;
    public float currentHealth;
    private int maxHearts = 5;
    private int currentHearts = 5;
    public Text healthText;
    private Conductor conductor;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    // Start is called before the first frame update
    void Start()
    {
        conductor = FindObjectOfType<Conductor>();
        currentHealth = maxHealth;
        SetHearts();
    }

    private void Update()
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            if(i < currentHearts)
            {
                hearts[hearts.Length-i-1].sprite = fullHeart;
            }
            else
            {
                hearts[hearts.Length - i - 1].sprite = emptyHeart;
            }
        }
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        SetHearts();
        if (currentHealth <= 0)
            GameOver();
    }

    private void GameOver()
    {
        SceneManager.LoadScene(2);
    }

    private void SetHearts()
    {
        currentHearts = (int)Mathf.Ceil(currentHealth * maxHearts / maxHealth);
    }
}
