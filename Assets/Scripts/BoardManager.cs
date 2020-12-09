using System;   
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.AI;
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

    public bool spawnRooms = true;

    public int columns = 4;
    public int rows = 4;

    public Count roomCount = new Count(5, 10);
    // putting prefabs in these arrays
    public GameObject[] roomPrefab;
    private List<Vector3> RoomPositions = new List<Vector3>();

    public NavMeshSurface2d surface2D;
    public GameObject BoardHolder;

    private List<Vector3> gridPositions = new List<Vector3>();

    void SetupRooms()
    {
        int roomNumber = 0;
        // create first room
        GameObject toInstantiate2;
        toInstantiate2 = roomPrefab[Random.Range(0, roomPrefab.Length - 1)];
        GameObject instance2 = Instantiate(toInstantiate2, new Vector3(0f, 0f, 0f), Quaternion.identity, BoardHolder.transform) as GameObject;
        roomNumber++;
        // create other rooms
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if ((x != 0 || y != 0) && roomNumber < roomCount.maximum)
                {
                    toInstantiate2 = roomPrefab[Random.Range(0, roomPrefab.Length - 1)];
                    instance2 = Instantiate(toInstantiate2, new Vector3(x * 14, y * 14, 0f), Quaternion.identity, BoardHolder.transform) as GameObject;
                    roomNumber++;
                } 
            }
        }
    }

    void InitializeList()
    {
        gridPositions.Clear();
        for (int x = 1; x < columns -1; x++)
        {
            for (int y = 1; y < rows -1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
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
            GameObject instance = Instantiate(tileChoice, randomPosition, Quaternion.identity, BoardHolder.transform) as GameObject;
        }
    }

    public void SetupScene(int level)
    {
        if (spawnRooms)
        {
            SetupRooms();
        }
        surface2D.BuildNavMesh();
    }
}
