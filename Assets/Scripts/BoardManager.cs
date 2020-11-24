using System;   
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count (int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public bool spawnTiles = true;

    public int columns = 40;
    public int rows = 40;
    public Count wallCount = new Count(50, 200);
    // putting prefabs in these arrays
    public GameObject[] floor;
    public GameObject[] wallTiles;
    public GameObject[] outerWallTiles;
    public GameObject[] ennemySpawnTiles;

    public int ennemyNumber = 2;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitializeList()
    {
        gridPositions.Clear();
        for (int x = 1; x < columns -1; x++)
        {
            for (int y = 1; y < rows -1; y++)
            {
                gridPositions.Add(new Vector3(x / 5f, y / 5f, 0f));
            }
        }
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("board").transform;
        // spawn floor
        GameObject toInstantiate = floor[Random.Range(0, floor.Length)];
        Vector3 scaleChange = new Vector3(columns * 0.155f, rows * 0.155f, 0.0f);
        GameObject instance = Instantiate(toInstantiate, new Vector3(columns * 0.1f, rows * 0.1f, 0.0f), Quaternion.identity) as GameObject;
        instance.transform.localScale += scaleChange;
        instance.transform.SetParent(boardHolder);
        // spawn outer walls
        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    GameObject toInstantiate2 = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                    GameObject instance2 = Instantiate(toInstantiate2, new Vector3(x / 5f, y / 5f, 0f), Quaternion.identity) as GameObject;
                    instance2.transform.SetParent(boardHolder);
                } 
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPositions = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPositions;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);
        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            GameObject instance = Instantiate(tileChoice, randomPosition, Quaternion.identity) as GameObject;
            instance.transform.SetParent(boardHolder);
        }
    }

    public void SetupScene(int level)
    {
        if (spawnTiles)
        {
            BoardSetup();
        }
        InitializeList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(ennemySpawnTiles, ennemyNumber, ennemyNumber);
    }
}
