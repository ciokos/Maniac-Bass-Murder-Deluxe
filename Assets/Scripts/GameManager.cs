using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BoardManager boardScript;

    public bool spawnBoard = true;

    private int level = 1;
    
    // Use this for initialization
    void Awake()
    {
        if (spawnBoard) {
            boardScript = GetComponent<BoardManager>();
            InitGame();
        }
    }

    void InitGame()
    {
        boardScript.SetupScene(level);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
