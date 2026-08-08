using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class ClearingGen : MonoBehaviour
{
    [SerializeField]
    private MazeGenerator m_Generator;

    [SerializeField]
    private MazeCell mazeCell;

    [SerializeField]
    private GameObject wallPrefab;

    [SerializeField]
    private GameObject gatePrefab;

    // side length in cells for the square to generate
    public int cellWidth = 3;

    void Start()
    {
        // center this object on the maze (optional)
        transform.position = new Vector3((m_Generator._mazeWidth * m_Generator.size) / 2f, 0f, (m_Generator._mazeDepth * m_Generator.size) / 2f);

        // Set the cellWidth to the central clear size defined in the MazeGenerator
        cellWidth = m_Generator._centralClearSize;

        // Generate the base walls of the clearing
        genBaseWalls();
    }

    private void genBaseWalls()
    {
        if (cellWidth <= 0 || m_Generator == null || wallPrefab == null) return;

        int centerX = m_Generator._mazeWidth / 2;
        int centerZ = m_Generator._mazeDepth / 2;
        int half = cellWidth / 2;

        int startX = Mathf.Clamp(centerX - half, 0, m_Generator._mazeWidth - 1);
        int startZ = Mathf.Clamp(centerZ - half, 0, m_Generator._mazeDepth - 1);

        for (int dx = 0; dx < cellWidth; dx++)
        {
            for (int dz = 0; dz < cellWidth; dz++)
            {
                int gridX = startX + dx;
                int gridZ = startZ + dz;

                if (gridX < 0 || gridX >= m_Generator._mazeWidth || gridZ < 0 || gridZ >= m_Generator._mazeDepth)
                    continue;

                Vector3 worldPos = new Vector3(gridX * m_Generator.size, 0f, gridZ * m_Generator.size);

                // Only instantiate walls on the perimeter of the square
                if (worldPos.x == startX * m_Generator.size || worldPos.x == (startX + cellWidth - 1) * m_Generator.size ||
                    worldPos.z == startZ * m_Generator.size || worldPos.z == (startZ + cellWidth - 1) * m_Generator.size)
                {
                    MazeCell clone;

                    // dont instantiate a cell in the center of the square to leave a gap for the player to walk through
                    if (worldPos.x != startX * m_Generator.size + half * m_Generator.size)
                    {
                        clone = Instantiate(mazeCell, worldPos, Quaternion.identity, transform);

                        // clear the top right corner to reduce the number of walls in the maze
                        if (worldPos.z == (startZ + cellWidth - 1) * m_Generator.size && worldPos.x == (startX + cellWidth - 1) * m_Generator.size)
                        {
                            clone.Visit();
                            clone.ClearLeftWall();
                            clone.ClearBackWall();
                        }
                        // clear the top left corner to reduce the number of walls in the maze
                        else if (worldPos.z == (startZ + cellWidth - 1) * m_Generator.size && worldPos.x == startX * m_Generator.size)
                        {
                            clone.Visit();
                            clone.ClearRightWall();
                            clone.ClearBackWall();
                        }
                        // clear the bottom right corner to reduce the number of walls in the maze
                        else if (worldPos.z == startZ * m_Generator.size && worldPos.x == (startX + cellWidth - 1) * m_Generator.size)
                        {
                            clone.Visit();
                            clone.ClearLeftWall();
                            clone.ClearFrontWall();
                        }
                        // clear the bottom left corner to reduce the number of walls in the maze
                        else if (worldPos.z == startZ * m_Generator.size && worldPos.x == startX * m_Generator.size)
                        {
                            clone.Visit();
                            clone.ClearRightWall();
                            clone.ClearFrontWall();
                        }
                        // clear the left wall of the leftmost cells to reduce the number of walls in the maze
                        else if (worldPos.x == startX * m_Generator.size)
                        {
                            clone.Visit();
                            clone.ClearRightWall();
                            clone.ClearFrontWall();
                            clone.ClearBackWall();
                        }
                        // clear the right wall of the rightmost cells to reduce the number of walls in the maze
                        else if (worldPos.x == (startX + cellWidth - 1) * m_Generator.size)
                        {
                            clone.Visit();
                            clone.ClearLeftWall();
                            clone.ClearFrontWall();
                            clone.ClearBackWall();
                        }
                        // clear the front wall of the bottom cells to reduce the number of walls in the maze
                        else if (worldPos.z == startZ * m_Generator.size)
                        {
                            clone.Visit();
                            clone.ClearLeftWall();
                            clone.ClearRightWall();
                            clone.ClearFrontWall();
                        }
                        // clear the back wall of the top cells to reduce the number of walls in the maze
                        else if (worldPos.z == (startZ + cellWidth - 1) * m_Generator.size)
                        {
                            clone.Visit();
                            clone.ClearLeftWall();
                            clone.ClearRightWall();
                            clone.ClearBackWall();

                        }




                        
                    }
                    else
                    {
                        GameObject clone2;
                        clone2 = Instantiate(gatePrefab, worldPos, Quaternion.identity, transform);

                        MazeCell gate = clone2.GetComponent<MazeCell>();

                        if (worldPos.z == startZ * m_Generator.size)
                        {
                            gate.Visit();
                            gate.ClearLeftWall();
                            gate.ClearRightWall();
                            gate.ClearFrontWall();
                        }
                        else if (worldPos.z == (startZ + cellWidth - 1) * m_Generator.size)
                        {
                            gate.Visit();
                            gate.ClearLeftWall();
                            gate.ClearRightWall();
                            gate.ClearBackWall();

                        }

                    }
                }
            }
        }
    }

    private void spawnGate(Vector3 worldPos)
    {
        



    }

}    
