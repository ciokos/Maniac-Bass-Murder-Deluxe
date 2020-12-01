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

    public class Door
    {
        public int ypos;
        public int xpos;
        public int orientation; // 0 means north, -1 west, +1 east and -2 south

        public Door(int x, int y, int direction)
        {
            xpos = x;
            ypos = y;
            orientation = direction;
        }

        public int sum_orientation(int or1, int or2)
        {
            if (or1 + or2 < -2)
            {
                return or1 + or2 + 4;
            }
            else
            {
                if (or1 + or2 > 1)
                {
                    return or1 + or2 - 4;
                }
                else
                {
                    return or2 + or1;
                }
            }
        }
    }

    public class Position
    {
        public int x;
        public int y;

        public Position()
        {
            x = 0;
            y = 0;
        }

        public Position(int xv, int yv)
        {
            x = xv;
            y = yv;
        }

        public Position(int orientation, int xv, int yv)
        {
            switch (orientation)
            {
                case (0):
                    x = xv;
                    y = yv;
                    break;
                case (-1):
                    x = -yv;
                    y = xv;
                    break;
                case (1):
                    x = yv;
                    y = -xv;
                    break;
                case (-2):
                    x = -xv;
                    y = -yv;
                    break;
                default:
                    x = xv;
                    y = yv;
                    break;
            }
        }
    }

    public bool spawnTiles = true;

    public int startingColumns = 40;
    public int startingRows = 40;

    public int maxRecursionDepth = 1;

    public Count wallCount = new Count(50, 200);
    // putting prefabs in these arrays
    public GameObject[] wallTiles;
    public GameObject[] outerWallTiles;
    public GameObject[] ennemySpawnTiles;

    public int ennemyNumber = 2;

    public List<Door> doorList = new List<Door>();

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitializeList(int columns, int rows, int originX, int originY)
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

    void OuterWallsSetup(Transform boardholder, int originX, int originY, int depth, int orientation, int columns, int rows)
    {       
        // spawn floor
        /**GameObject toInstantiate = floor;
        Vector3 scaleChange = new Vector3(columns * 0.155f, rows * 0.155f, 0.0f);
        GameObject instance = Instantiate(toInstantiate, new Vector3(columns * 0.1f, rows * 0.1f, 0.0f), Quaternion.identity) as GameObject;
        instance.transform.localScale += scaleChange;
        instance.transform.SetParent(boardHolder);**/
        
        // spawn door
        bool isdoor()
        {
            return (Random.value < 0.05);
        }

        // spawn outer walls and doors
        // spawn south wall - no door in south wall - no wall if recursionDepth > 0
        if (depth == 0)
        {
            for (int x = -1; x < columns + 1; x++)
            {
                int y = -1;
                GameObject toInstantiate2;
                toInstantiate2 = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                Position rotatedPosition = new Position(orientation, x, y);
                GameObject instance2 = Instantiate(toInstantiate2, new Vector3(rotatedPosition.x / 5f, rotatedPosition.y / 5f, 0f), Quaternion.identity) as GameObject;
                instance2.transform.SetParent(boardHolder);
            }
        }
        

        // spawn west wall with door
        int westDoorCount = 1;
        int westDoorSet = 0;
        for (int y = -1; y < rows + 1; y++)
        {
            int x = -1;
            
            if (y > -1 && y < rows && isdoor() && westDoorSet < westDoorCount)
            {
                westDoorSet++;
                doorList.Add(new Door(x, y, -1));
            } 
            else
            {
                GameObject toInstantiate2 = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                Position rotatedPosition = new Position(orientation, x, y);
                GameObject instance2 = Instantiate(toInstantiate2, new Vector3(rotatedPosition.x / 5f, rotatedPosition.y / 5f, 0f), Quaternion.identity) as GameObject;
                instance2.transform.SetParent(boardHolder);
            }
        }

        // spawn east wall with door
        int eastDoorCount = 1;
        int eastDoorSet = 0;
        for (int y = -1; y < rows + 1; y++)
        {
            int x = columns + 1;
            
            if (y > -1 && y < rows && isdoor() && eastDoorSet < eastDoorCount)
            {
                eastDoorSet++;
                doorList.Add(new Door(x, y, 1));
            }
            else
            {
                GameObject toInstantiate2 = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                Position rotatedPosition = new Position(orientation, x, y);
                GameObject instance2 = Instantiate(toInstantiate2, new Vector3(rotatedPosition.x / 5f, rotatedPosition.y / 5f, 0f), Quaternion.identity) as GameObject;
                instance2.transform.SetParent(boardHolder);
            }
        }

        // spawn north wall with door
        int northDoorCount = 1;
        int northDoorSet = 0;
        for (int x = -1; x < columns + 1; x++)
        {
            int y = rows + 1;

            if (x > -1 && x < columns && isdoor() && northDoorSet < northDoorCount)
            {
                northDoorSet++;
                doorList.Add(new Door(x, y, 0));
            }
            else
            {
                GameObject toInstantiate2 = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                Position rotatedPosition = new Position(orientation, x, y);
                GameObject instance2 = Instantiate(toInstantiate2, new Vector3(rotatedPosition.x / 5f, rotatedPosition.y / 5f, 0f), Quaternion.identity) as GameObject;
                instance2.transform.SetParent(boardHolder);
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
        int recursionDepth = 0;
        boardHolder = new GameObject("board").transform;
        if (spawnTiles)
        {
            OuterWallsSetup(boardHolder, 0, 0, 0, 0, startingColumns, startingRows);
        }
        InitializeList(startingColumns, startingRows, 0, 0);
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(ennemySpawnTiles, ennemyNumber, ennemyNumber);
        while (recursionDepth < maxRecursionDepth) // add a criteria over doorList size
        {
            List<Door> fakeDoorList = new List<Door>(doorList);
            doorList.Clear();
            recursionDepth++;
            foreach (Door door in fakeDoorList)
            {
                int localColumns = Random.Range((int)(startingColumns / (recursionDepth + 1)), startingColumns);
                int localRows = Random.Range((int)(startingRows / (recursionDepth + 1)), startingRows);
                Position localOrigin = new Position(door.orientation, door.xpos, door.ypos); // TODO
                OuterWallsSetup(boardHolder, localOrigin.x, localOrigin.y, recursionDepth, door.orientation, localColumns, localRows);
            }


            
        }
    }
}
