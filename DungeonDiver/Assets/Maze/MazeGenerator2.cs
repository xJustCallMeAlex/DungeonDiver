using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using NavMeshPlus.Components;

public class MazeGenerator2 : MonoBehaviour
{
    [SerializeField] MazeTile tilePrefab;
    [SerializeField] GameObject player;
    [SerializeField] GameObject tunnel;
    [SerializeField] Vector2Int mazeSize;
    [SerializeField] bool ShowcaseOn;
    float playerPosX;
    float playerPosY;
    [SerializeField] List<MazeTile> tiles;

    public NavMeshSurface surface;

    // Start is called before the first frame update
    void Start()
    {
        if (ShowcaseOn)
        {
            StartCoroutine(GenerateMaze(mazeSize));
        }
        else
        {
            GenerateMazeInstant(mazeSize);
            surface.BuildNavMesh();
        }
    }

    void GenerateMazeInstant(Vector2Int size)
    {
        tiles = new List<MazeTile>();

        int tunnelIndex = Random.Range(1, size.x * size.y);
        int counter = 0;

        // Create nodes
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3 tilePos = new Vector3((x - (size.x / 2f))*2, (y - (size.y / 2f)) * 2, 0);
                //tilePos = tilePos * 5;
                MazeTile tile = Instantiate(tilePrefab, tilePos, Quaternion.identity, transform);
                tiles.Add(tile);

                if(counter == tunnelIndex - 1)
                {
                    tunnel.transform.position = tilePos;
                }
                counter++;
            }
        }


        List<MazeTile> currentPath = new List<MazeTile>();
        List<MazeTile> completedTiles = new List<MazeTile>();

        // Choose starting node
        // Note: Dont Choose Random, but use the Node the Player starts in
        currentPath.Add(tiles[Random.Range(0, tiles.Count)]);
        currentPath[0].SetState(NodeState.Current);

        while (completedTiles.Count < tiles.Count)
        {
            // Check nodes next to the current node
            List<int> possibleNextTiles = new List<int>();
            List<int> possibleDirections = new List<int>();

            int currentTileIndex = tiles.IndexOf(currentPath[currentPath.Count - 1]);
            int currentTileXPos = currentTileIndex / size.y;
            int currentTileYPos = currentTileIndex % size.y;

            if (currentTileXPos < size.x - 1)
            {
                // Check node to the right of the current node
                if (!completedTiles.Contains(tiles[currentTileIndex + size.y]) &&
                    !currentPath.Contains(tiles[currentTileIndex + size.y]))
                {
                    possibleDirections.Add(1);
                    possibleNextTiles.Add(currentTileIndex + size.y);
                }
            }
            if (currentTileXPos > 0)
            {
                // Check node to the left of the current node
                if (!completedTiles.Contains(tiles[currentTileIndex - size.y]) &&
                    !currentPath.Contains(tiles[currentTileIndex - size.y]))
                {
                    possibleDirections.Add(2);
                    possibleNextTiles.Add(currentTileIndex - size.y);
                }
            }
            if (currentTileYPos < size.y - 1)
            {
                // Check node above the current node
                if (!completedTiles.Contains(tiles[currentTileIndex + 1]) &&
                    !currentPath.Contains(tiles[currentTileIndex + 1]))
                {
                    possibleDirections.Add(3);
                    possibleNextTiles.Add(currentTileIndex + 1);
                }
            }
            if (currentTileYPos > 0)
            {
                // Check node below the current node
                if (!completedTiles.Contains(tiles[currentTileIndex - 1]) &&
                    !currentPath.Contains(tiles[currentTileIndex - 1]))
                {
                    possibleDirections.Add(4);
                    possibleNextTiles.Add(currentTileIndex - 1);
                }
            }

            // Choose next node
            if (possibleDirections.Count > 0)
            {
                int chosenDirection = Random.Range(0, possibleDirections.Count);
                MazeTile chosenTile = tiles[possibleNextTiles[chosenDirection]];

                switch (possibleDirections[chosenDirection])
                {
                    case 1:
                        chosenTile.RemoveWall(1);
                        currentPath[currentPath.Count - 1].RemoveWall(0);
                        break;
                    case 2:
                        chosenTile.RemoveWall(0);
                        currentPath[currentPath.Count - 1].RemoveWall(1);
                        break;
                    case 3:
                        chosenTile.RemoveWall(3);
                        currentPath[currentPath.Count - 1].RemoveWall(2);
                        break;
                    case 4:
                        chosenTile.RemoveWall(2);
                        currentPath[currentPath.Count - 1].RemoveWall(3);
                        break;
                }

                currentPath.Add(chosenTile);
                chosenTile.SetState(NodeState.Current);
            }
            else
            {
                completedTiles.Add(currentPath[currentPath.Count - 1]);

                currentPath[currentPath.Count - 1].SetState(NodeState.Completed);
                currentPath.RemoveAt(currentPath.Count - 1);
            }
        }
        //Vector3 playerPos = new Vector3(playerPosX, playerPosY, 0);
        //Instantiate(player, playerPos, Quaternion.identity, transform);
    }

    IEnumerator GenerateMaze(Vector2Int size)
    {
        tiles = new List<MazeTile>();

        int tunnelIndex = Random.Range(1, size.x * size.y);
        int counter = 0;

        // Create nodes
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3 tilePos = new Vector3(x - (size.x / 2f), y - (size.y / 2f), 0);
                MazeTile tile = Instantiate(tilePrefab, tilePos, Quaternion.identity, transform);
                tiles.Add(tile);

                if (counter == tunnelIndex -1) 
                {
                    tunnel.transform.position = tilePos;
                }
                counter++;

                yield return null;
            }
        }


        List<MazeTile> currentPath = new List<MazeTile>();
        List<MazeTile> completedTiles = new List<MazeTile>();

        // Choose starting node
        // Note: Dont Choose Random, but use the Node the Player starts in
        currentPath.Add(tiles[Random.Range(0, tiles.Count)]);
        currentPath[0].SetState(NodeState.Current);

        while (completedTiles.Count < tiles.Count)
        {
            // Check nodes next to the current node
            List<int> possibleNextTiles = new List<int>();
            List<int> possibleDirections = new List<int>();

            int currentTileIndex = tiles.IndexOf(currentPath[currentPath.Count - 1]);
            int currentTileXPos = currentTileIndex / size.y;
            int currentTileYPos = currentTileIndex % size.y;

            if (currentTileXPos < size.x - 1)
            {
                // Check node to the right of the current node
                if (!completedTiles.Contains(tiles[currentTileIndex + size.y]) &&
                    !currentPath.Contains(tiles[currentTileIndex + size.y]))
                {
                    possibleDirections.Add(1);
                    possibleNextTiles.Add(currentTileIndex + size.y);
                }
            }
            if (currentTileXPos > 0)
            {
                // Check node to the left of the current node
                if (!completedTiles.Contains(tiles[currentTileIndex - size.y]) &&
                    !currentPath.Contains(tiles[currentTileIndex - size.y]))
                {
                    possibleDirections.Add(2);
                    possibleNextTiles.Add(currentTileIndex - size.y);
                }
            }
            if (currentTileYPos < size.y - 1)
            {
                // Check node above the current node
                if (!completedTiles.Contains(tiles[currentTileIndex + 1]) &&
                    !currentPath.Contains(tiles[currentTileIndex + 1]))
                {
                    possibleDirections.Add(3);
                    possibleNextTiles.Add(currentTileIndex + 1);
                }
            }
            if (currentTileYPos > 0)
            {
                // Check node below the current node
                if (!completedTiles.Contains(tiles[currentTileIndex - 1]) &&
                    !currentPath.Contains(tiles[currentTileIndex - 1]))
                {
                    possibleDirections.Add(4);
                    possibleNextTiles.Add(currentTileIndex - 1);
                }
            }

            // Choose next node
            if (possibleDirections.Count > 0)
            {
                int chosenDirection = Random.Range(0, possibleDirections.Count);
                MazeTile chosenTile = tiles[possibleNextTiles[chosenDirection]];

                switch (possibleDirections[chosenDirection])
                {
                    case 1:
                        chosenTile.RemoveWall(1);
                        currentPath[currentPath.Count - 1].RemoveWall(0);
                        break;
                    case 2:
                        chosenTile.RemoveWall(0);
                        currentPath[currentPath.Count - 1].RemoveWall(1);
                        break;
                    case 3:
                        chosenTile.RemoveWall(3);
                        currentPath[currentPath.Count - 1].RemoveWall(2);
                        break;
                    case 4:
                        chosenTile.RemoveWall(2);
                        currentPath[currentPath.Count - 1].RemoveWall(3);
                        break;
                }

                currentPath.Add(chosenTile);
                chosenTile.SetState(NodeState.Current);
            }
            else
            {
                completedTiles.Add(currentPath[currentPath.Count - 1]);

                currentPath[currentPath.Count - 1].SetState(NodeState.Completed);
                currentPath.RemoveAt(currentPath.Count - 1);
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void endCurrentLevel()
    {
        playerPosX = player.transform.position.x;
        playerPosY = player.transform.position.y;
        //Destroy(tunnel);

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        GenerateMazeInstant(mazeSize);

        //SceneManager.LoadScene(1);

    }
}
