using System.Collections;
using UnityEngine;


public class makePlaneGrid : MonoBehaviour
{

    public int xSize, ySize, gridSize;
    public Transform gridPlane;

    private void Awake()
    {
        Generate();
    }

    private void Generate()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (i == 0 && j == 0)
                {
                    Instantiate(gridPlane, new Vector3(i * xSize, 0, j * ySize), Quaternion.identity);
                }
                else if (j == 0)
                {
                    Instantiate(gridPlane, new Vector3(i * xSize, 0, j * ySize), Quaternion.identity);
                    Instantiate(gridPlane, new Vector3(i * -xSize, 0, j * ySize), Quaternion.identity);
                }
                else if (i == 0)
                {
                    Instantiate(gridPlane, new Vector3(i * xSize, 0, j * ySize), Quaternion.identity);
                    Instantiate(gridPlane, new Vector3(i * xSize, 0, j * -ySize), Quaternion.identity);
                }
                else
                {
                    Instantiate(gridPlane, new Vector3(i * xSize, 0, j * ySize), Quaternion.identity);
                    Instantiate(gridPlane, new Vector3(i * xSize, 0, j * -ySize), Quaternion.identity);
                    Instantiate(gridPlane, new Vector3(i * -xSize, 0, j * ySize), Quaternion.identity);
                    Instantiate(gridPlane, new Vector3(i * -xSize, 0, j * -ySize), Quaternion.identity);
                }
            }
        }

    }
}