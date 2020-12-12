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

    public float roomLuck = 0.5f;

    public Count roomCount = new Count(5, 10);
    // putting prefabs in these arrays
    public GameObject[] roomPrefab;
    public GameObject wallPrefab;
    public GameObject[] enemyPrefab;
    public List<Vector3> RoomPositions = new List<Vector3>();
    private List<GameObject> enemyList = new List<GameObject>();

    public NavMeshSurface2d surface2D;
    public GameObject BoardHolder;

    public bool NoEnemies()
    {
        foreach (GameObject enemy in enemyList)
        {
            if (enemy != null)
                return false;
        }
        return true;
    }
    bool randomRoom()
    {
        return (Random.value < roomLuck);
    }

    bool RoomAdjacent(List<Vector3> positions, int x, int y)
    {
        foreach (Vector3 position in positions)
        {
            if ((position.x + 1 == x && position.y == y) || (position.x - 1 == x && position.y == y) || (position.x == x && position.y - 1 == y) || (position.x == x && position.y + 1 == y))
            {
                return true;
            }
        }
        return false;
    }

    void CloseDoor(List<Vector3> positions)
    {
        foreach (Vector3 position in positions)
        {
            GameObject toInstantiate2 = wallPrefab;
            // east
            if (!positions.Contains(new Vector3(position.x + 1, position.y, 0f)))
            {
                GameObject instance2 = Instantiate(toInstantiate2, new Vector3(position.x * 24 + 24, position.y * 24 + 11, 0f), Quaternion.identity, BoardHolder.transform) as GameObject;
            }
            // west
            if (!positions.Contains(new Vector3(position.x - 1, position.y, 0f)))
            {
                GameObject instance2 = Instantiate(toInstantiate2, new Vector3(position.x * 24, position.y * 24 + 11, 0f), Quaternion.identity, BoardHolder.transform) as GameObject;
            }
            // north
            if (!positions.Contains(new Vector3(position.x, position.y + 1, 0f)))
            {
                GameObject instance2 = Instantiate(toInstantiate2, new Vector3(position.x * 24 + 12, position.y * 24 + 23, 0f), Quaternion.identity, BoardHolder.transform) as GameObject;
            }
            // south
            if (!positions.Contains(new Vector3(position.x, position.y - 1, 0f)))
            {
                GameObject instance2 = Instantiate(toInstantiate2, new Vector3(position.x * 24 + 12, position.y * 24 - 1, 0f), Quaternion.identity, BoardHolder.transform) as GameObject;
            }
        }
    }

    void SetupRooms()
    {
        int roomNumber = 0;
        // create first room
        GameObject toInstantiate2;
        toInstantiate2 = roomPrefab[Random.Range(0, roomPrefab.Length - 1)];
        GameObject instance2 = Instantiate(toInstantiate2, new Vector3(0f, 0f, 0f), Quaternion.identity, BoardHolder.transform) as GameObject;
        roomNumber++;
        RoomPositions.Add(new Vector3(0f, 0f, 0f));
        // create other rooms
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if ((x != 0 || y != 0) && roomNumber < roomCount.maximum && randomRoom() && RoomAdjacent(RoomPositions, x, y))
                {
                    toInstantiate2 = roomPrefab[Random.Range(0, roomPrefab.Length - 1)];
                    instance2 = Instantiate(toInstantiate2, new Vector3(x * 24, y * 24, 0f), Quaternion.identity, BoardHolder.transform) as GameObject;
                    roomNumber++;
                    RoomPositions.Add(new Vector3(x, y, 0f));
                } 
            }
        }
    }

    void SpawnEnnemies()
    {
        // getting the spawn positions from the prefabs
        GameObject[] spawnArray = GameObject.FindGameObjectsWithTag("SpawnLocation");
        List<Vector3> spawnLocationList = new List<Vector3>();
        foreach (GameObject spawnL in spawnArray)
        {
            Vector3 spawnPoint = spawnL.transform.position;
            GameObject toInstantiate3;
            toInstantiate3 = enemyPrefab[Random.Range(0, enemyPrefab.Length - 1)];
            GameObject instance3 = Instantiate(toInstantiate3, spawnPoint, Quaternion.identity, BoardHolder.transform) as GameObject;
            enemyList.Add(instance3);
        }
        
    }

    public void SetupScene(int level)
    {
        if (spawnRooms)
        {
            SetupRooms();
            CloseDoor(RoomPositions);
            SpawnEnnemies();
        }
        surface2D.BuildNavMesh();
    }
}
