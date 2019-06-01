using System.Collections;
using UnityEngine;


public class makePlaneGrid : MonoBehaviour
{

    public int planeSize, gridSize;
    public Transform gridPlane;

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
                if (i == 0 && j == 0)
                {
                    Instantiate(gridPlane, new Vector3(i * planeSize * 10, 0, j * planeSize * 10), Quaternion.identity).transform.localScale
                        = new Vector3(planeSize, 2, planeSize);
                }
                else if (j == 0)
                {
                    Instantiate(gridPlane, new Vector3(i * planeSize * 10, 0, j * planeSize * 10), Quaternion.identity).transform.localScale
                        = new Vector3(planeSize, 2, planeSize);
                    Instantiate(gridPlane, new Vector3(i * -planeSize * 10, 0, j * planeSize * 10), Quaternion.identity).transform.localScale
                        = new Vector3(planeSize, 2, planeSize);
                }
                else if (i == 0)
                {
                    Instantiate(gridPlane, new Vector3(i * planeSize * 10, 0, j * planeSize * 10), Quaternion.identity).transform.localScale
                        = new Vector3(planeSize, 2, planeSize);
                    Instantiate(gridPlane, new Vector3(i * planeSize * 10, 0, j * -planeSize * 10), Quaternion.identity).transform.localScale
                        = new Vector3(planeSize, 2, planeSize);
                }
                else
                {
                    Instantiate(gridPlane, new Vector3(i * planeSize * 10, 0, j * planeSize * 10), Quaternion.identity).transform.localScale
                        = new Vector3(planeSize, 2, planeSize);
                    Instantiate(gridPlane, new Vector3(i * planeSize * 10, 0, j * -planeSize * 10), Quaternion.identity).transform.localScale
                        = new Vector3(planeSize, 2, planeSize);
                    Instantiate(gridPlane, new Vector3(i * -planeSize * 10, 0, j * planeSize * 10), Quaternion.identity).transform.localScale
                        = new Vector3(planeSize, 2, planeSize);
                    Instantiate(gridPlane, new Vector3(i * -planeSize * 10, 0, j * -planeSize * 10), Quaternion.identity).transform.localScale
                        = new Vector3(planeSize, 2, planeSize);
                }
            }
        }

    }
}