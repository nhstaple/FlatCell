using System.Collections;
using UnityEngine;


public class makePlaneGrid : MonoBehaviour
{

    public int planeSize, gridSize;
    public Transform gridPlane;
    public Transform boundary;

    private void Awake()
    {
        Physics.gravity = new Vector3(0, -5.0F, 0);
        Generate();
    }

    private void Generate()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (i == gridSize - 1 || j == gridSize - 1)
                {
                    Instantiate(boundary, new Vector3(i * planeSize * 10, 0, j * planeSize * 10), Quaternion.identity).transform.localScale
                        = new Vector3(planeSize * 10, 100, planeSize * 10);
                    Instantiate(boundary, new Vector3(i * planeSize * -10, 0, j * planeSize * 10), Quaternion.identity).transform.localScale
                        = new Vector3(planeSize * 10, 100, planeSize * 10);
                    Instantiate(boundary, new Vector3(i * planeSize * 10, 0, j * planeSize * -10), Quaternion.identity).transform.localScale
                        = new Vector3(planeSize * 10, 100, planeSize * 10);
                    Instantiate(boundary, new Vector3(i * planeSize * -10, 0, j * planeSize * -10), Quaternion.identity).transform.localScale
                        = new Vector3(planeSize * 10, 100, planeSize * 10);
                }
                if (i == 0 && j == 0)
                {
                    Instantiate(gridPlane, new Vector3(i * planeSize * 10, 0, j * planeSize * 10), Quaternion.identity).transform.localScale
                        = new Vector3(planeSize, 10, planeSize);
                }
                else if (j == 0)
                {
                    Instantiate(gridPlane, new Vector3(i * planeSize * 10, 0, j * planeSize * 10), Quaternion.identity).transform.localScale
                        = new Vector3(planeSize, 10, planeSize);
                    Instantiate(gridPlane, new Vector3(i * -planeSize * 10, 0, j * planeSize * 10), Quaternion.identity).transform.localScale
                        = new Vector3(planeSize, 10, planeSize);
                }
                else if (i == 0)
                {
                    Instantiate(gridPlane, new Vector3(i * planeSize * 10, 0, j * planeSize * 10), Quaternion.identity).transform.localScale
                        = new Vector3(planeSize, 10, planeSize);
                    Instantiate(gridPlane, new Vector3(i * planeSize * 10, 0, j * -planeSize * 10), Quaternion.identity).transform.localScale
                        = new Vector3(planeSize, 10, planeSize);
                }
                else
                {
                    Instantiate(gridPlane, new Vector3(i * planeSize * 10, 0, j * planeSize * 10), Quaternion.identity).transform.localScale
                        = new Vector3(planeSize, 10, planeSize);
                    Instantiate(gridPlane, new Vector3(i * planeSize * 10, 0, j * -planeSize * 10), Quaternion.identity).transform.localScale
                        = new Vector3(planeSize, 10, planeSize);
                    Instantiate(gridPlane, new Vector3(i * -planeSize * 10, 0, j * planeSize * 10), Quaternion.identity).transform.localScale
                        = new Vector3(planeSize, 10, planeSize);
                    Instantiate(gridPlane, new Vector3(i * -planeSize * 10, 0, j * -planeSize * 10), Quaternion.identity).transform.localScale
                        = new Vector3(planeSize, 10, planeSize);
                }
            }
        }

    }
}