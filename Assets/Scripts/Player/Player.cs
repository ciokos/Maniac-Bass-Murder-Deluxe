using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Sprite halfHeart;

    private int maxHealth;
    private int currentHealth;
    private GameObject boardManager;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = hearts.Length * 2;
        currentHealth = maxHealth;
        boardManager = GameObject.Find("BoardManager");
    }

    private void Update()
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            if (2 * i + 1 == currentHealth)
            {
                hearts[hearts.Length - i - 1].sprite = halfHeart;
            }
            else if (i * 2 < currentHealth)
            {
                hearts[hearts.Length - i - 1].sprite = fullHeart;
            }
            else
            {
                hearts[hearts.Length - i - 1].sprite = emptyHeart;
            }
        }
        if (boardManager.GetComponent<BoardManager>().NoEnemies())
            GameOver(true);
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
            GameOver(false);
    }

    private void GameOver(bool won)
    {
        if (won)
            SceneManager.LoadScene(3);
        else
            SceneManager.LoadScene(2);
    }
}
