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
    }

    public int sumOrientation(int or1, int or2)
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

        public Position sumPosition(Position nextPos, int orientation)
        {
            switch (orientation)
            {
                case (0):
                    x += nextPos.x;
                    y += nextPos.y;
                    break;
                case (-1):
                    x += -nextPos.y;
                    y += nextPos.x;
                    break;
                case (1):
                    x += nextPos.y;
                    y += -nextPos.x;
                    break;
                case (-2):
                    x += -nextPos.x;
                    y += -nextPos.y;
                    break;
                default:
                    x += nextPos.x;
                    y += nextPos.y;
                    break;
            }
            return this;
        }
    }

    public bool spawnTiles = true;

    public int startingColumns = 40;
    public int startingRows = 40;

    public int maxRecursionDepth = 1;

    public float doorLuck = 0.1f;

    public Count wallCount = new Count(50, 200);
    // putting prefabs in these arrays
    public GameObject[] wallTiles;
    public GameObject[] outerWallTiles;
    public GameObject[] enemySpawnTiles;

    public int enemyNumber = 2;

    public List<Door> doorList = new List<Door>();

    public NavMeshSurface2d surface2D;
    public GameObject BoardHolder;

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

    void OuterWallsSetup(int originX, int originY, int depth, int orientation, int columns, int rows)
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
            return (Random.value < doorLuck);
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
                GameObject instance2 = Instantiate(toInstantiate2, new Vector3((rotatedPosition.x + originX) / 5f, (rotatedPosition.y + originY) / 5f, 0f), Quaternion.identity, BoardHolder.transform) as GameObject;
            }
        }
        

        // spawn west wall with door
        int westDoorCount = 1;
        int westDoorSet = 0;
        for (int y = -1; y < rows + 1; y++)
        {
            int x = -1;
            
            if (y > rows / 3 && y < 2 * rows / 3 && isdoor() && westDoorSet < westDoorCount)
            {
                westDoorSet++;
                Position newDoorPosition = new Position(orientation, originX, originY).sumPosition(new Position(x, y), 0);
                doorList.Add(new Door(newDoorPosition.x, newDoorPosition.y, sumOrientation(orientation, - 1)));
            } 
            else
            {
                GameObject toInstantiate2 = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                Position rotatedPosition = new Position(orientation, x, y);
                GameObject instance2 = Instantiate(toInstantiate2, new Vector3((rotatedPosition.x + originX) / 5f, (rotatedPosition.y + originY) / 5f, 0f), Quaternion.identity, BoardHolder.transform) as GameObject;
            }
        }

        // spawn east wall with door
        int eastDoorCount = 1;
        int eastDoorSet = 0;
        for (int y = -1; y < rows + 1; y++)
        {
            int x = columns + 1;
            
            if (y > rows / 3 && y < 2 * rows / 3 && isdoor() && eastDoorSet < eastDoorCount)
            {
                eastDoorSet++;
                Position newDoorPosition = new Position(orientation, originX, originY).sumPosition(new Position(x, y), 0);
                doorList.Add(new Door(newDoorPosition.x, newDoorPosition.y, sumOrientation(orientation, 1)));
            }
            else
            {
                GameObject toInstantiate2 = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                Position rotatedPosition = new Position(orientation, x, y);
                GameObject instance2 = Instantiate(toInstantiate2, new Vector3((rotatedPosition.x + originX) / 5f, (rotatedPosition.y + originY) / 5f, 0f), Quaternion.identity, BoardHolder.transform) as GameObject;

            }
        }

        // spawn north wall with door
        int northDoorCount = 1;
        int northDoorSet = 0;
        for (int x = -1; x < columns + 1; x++)
        {
            int y = rows + 1;

            if (x > columns / 3 && x < 2 * columns / 3 && isdoor() && northDoorSet < northDoorCount)
            {
                northDoorSet++;
                Position newDoorPosition = new Position(orientation, originX, originY).sumPosition(new Position(x, y), 0);
                doorList.Add(new Door(newDoorPosition.x, newDoorPosition.y, sumOrientation(orientation, 0)));
            }
            else
            {
                GameObject toInstantiate2 = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                Position rotatedPosition = new Position(orientation, x, y);
                GameObject instance2 = Instantiate(toInstantiate2, new Vector3((rotatedPosition.x + originX) / 5f, (rotatedPosition.y + originY) / 5f, 0f), Quaternion.identity, BoardHolder.transform) as GameObject;
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
        int recursionDepth = 0;
        if (spawnTiles)
        {
            OuterWallsSetup(0, 0, 0, 0, startingColumns, startingRows);
        }
        InitializeList(startingColumns, startingRows, 0, 0);
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(enemySpawnTiles, enemyNumber, enemyNumber);
        while (recursionDepth < maxRecursionDepth) // add a criteria over doorList size
        {
            List<Door> fakeDoorList = new List<Door>(doorList);
            doorList.Clear();
            recursionDepth++;
            foreach (Door door in fakeDoorList)
            {
                int localColumns = Random.Range((int)(startingColumns / (recursionDepth + 1)), 2 * startingColumns / 3);
                int localRows = Random.Range((int)(startingRows / (recursionDepth + 1)), startingRows);
                Position offset = new Position(door.orientation, 1, 1); // putting the room at the exact right position
                int localOriginX = door.xpos + offset.x;
                int localOriginY = door.ypos + offset.y;
                OuterWallsSetup(localOriginX, localOriginY, recursionDepth, door.orientation, localColumns, localRows);
            }
        }
        surface2D.BuildNavMesh();
    }
}
