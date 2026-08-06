using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private MazeCell mazeCell;

    [SerializeField]
    private int mazeWidth;

    [SerializeField]
    private int mazeDepth;

    private MazeCell[,] mazeGrid;

    [SerializeField]
    private int size;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        mazeWidth *= size;
        mazeDepth *= size;

        mazeGrid = new MazeCell[mazeWidth, mazeDepth];

        for (int i = 0; i < mazeWidth; i++)
        {
            for (int j = 0; j < mazeDepth; j++)
            {
                MazeCell cell = Instantiate(mazeCell, new Vector3(i * size, 0, j * size), Quaternion.identity);
                mazeGrid[i, j] = cell;
            }
        }

        yield return GenerateMaze(null, mazeGrid[0, 0]);
    }

    private IEnumerator GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.visit();
        // Implement maze generation algorithm here
        ClearWalls(previousCell, currentCell);

        var nextCell = GetNextUnvisitedCell(currentCell);

        if (nextCell != null)
        {
            yield return GenerateMaze(currentCell, nextCell);
        }

    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {

        var unvisitedCells = GetUnvisitedNeighbors(currentCell);

        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedNeighbors(MazeCell currentCell)
    {
        int x = (int)((int)currentCell.transform.position.x / size);
        int z = (int)((int)currentCell.transform.position.z / size);

        if (x + 1 < mazeWidth)
        {
            var cellToRight = mazeGrid[x + 1, z];

            if (cellToRight.isVisited == false)
            {
                yield return cellToRight;

            }
        }

        if (x - 1 >= 0)
        {
            var cellToLeft = mazeGrid[x - 1, z];

            if (cellToLeft.isVisited == false)
            {
                yield return cellToLeft;
            }
        }

        if (z + 1 < mazeDepth)
        {
            var cellToFront = mazeGrid[x, z + 1];

            if (cellToFront.isVisited == false)
            {
                yield return cellToFront;
            }
        }

        if (z - 1 >= 0)
        {
            var cellToBack = mazeGrid[x, z - 1];
            if (cellToBack.isVisited == false)
            {
                yield return cellToBack;
            }
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell) 
    {
        if (previousCell == null) {

            Debug.Log("Previous cell is null");
        return;
            }

        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }


    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
