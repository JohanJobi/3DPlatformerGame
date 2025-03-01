using System.Collections;
using System.Linq; //used to get random numbers
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// change IEnumerator to void then remove yield return

public class MazeGeneration : MonoBehaviour
{
    public MazeCell mazeCell;
    public int mazewidth;
    public int mazeDepth;
    public PlayerInventory playerInventory;
    public Item keys;

    private MazeCell[,] mazeGrid;
    IEnumerator Start()
    {
        keys.Id = 7;
        
        mazeGrid = new MazeCell[mazewidth, mazeDepth];

        for (int x = 0; x < mazewidth; x++)
        {
            for (int z = 0 ; z < mazeDepth;  z++)
            {
                mazeGrid[x, z] = Instantiate(mazeCell, new Vector3(x, 0, z), Quaternion.identity, transform);
                mazeGrid[x, z].transform.localPosition = new Vector3(x, 0, z);
                //loops through every position in mazeGrid creates a cell and stores in the array
                //transform makes it able to scale depending on the parent game object
                // however this made all the cells overlap so I had to use localPosition so that the
                // algorithm uses world positions instead of positions local to the parent
                // also had to add the line after to change local position to the correct value
                // Instantiate uses world position I want this to be the localposition to the parent object 
            }
        }

        mazeGrid[0, 0].ClearLeftWall(); //makes an entrance
        mazeGrid[mazewidth - 1, mazeDepth - 1].ClearRightWall();//makes and exit
        yield return GenerateMaze(null, mazeGrid[0, 0]);// clears entrance cell
    }

    private IEnumerator GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        yield return new WaitForSeconds(0.05f);

        MazeCell nextCell;

        do // problem of not goint to every cell in maze solved 
        {
            nextCell = GetNextUnvisitedCell(currentCell);
            if (nextCell != null)
            {
                yield return GenerateMaze(currentCell, nextCell); //use yield return to call co-routine
            }
        } while (nextCell != null);
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell) 
    {
        // IEnumerable is a collection of objects that can be enumerated (iterated) sequentially
        // in this case the maze cells
        // this function checks all maze cells around the maze cell passed throught the function
        int x = (int)currentCell.transform.localPosition.x;
        int z = (int)currentCell.transform.localPosition.z;

        if (x + 1 < mazewidth)
        {
            var cellToRight = mazeGrid[x + 1, z];

            if (cellToRight.IsVisted == false)
            {
                yield return cellToRight;
            }
        }
        if (x - 1 >= 0)
        {
            var cellToLeft = mazeGrid[x - 1, z];
            if (cellToLeft.IsVisted == false)
            {
                yield return cellToLeft;
            }
        }

        if (z + 1 < mazeDepth)
        {
            var cellToFront = mazeGrid[x, z + 1];
            if (cellToFront.IsVisted == false)
            {
                yield return cellToFront;
            }
        }

        if (z - 1>= 0)
        {
            var cellToBack = mazeGrid[x, z - 1];

            if (cellToBack.IsVisted == false)
            {
                yield return cellToBack;
            }
        }
    }
    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvistedCells = GetUnvisitedCells(currentCell);

        return unvistedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();

    }
    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null)
        {
            return;
        }

        if (previousCell.transform.localPosition.x < currentCell.transform.localPosition.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }
        if (previousCell.transform.localPosition.x > currentCell.transform.localPosition.x)
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

        if (previousCell.transform.localPosition.z > currentCell.transform.localPosition.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }

    private void Update()
    {
        if (playerInventory.InEquipment(keys))
        {
            SceneManager.LoadScene("Level3");
        }
    }

}
