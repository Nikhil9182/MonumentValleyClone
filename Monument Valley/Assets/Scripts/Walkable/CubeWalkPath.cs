using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeWalkPath : WalkablePath
{
    [Header("Different Paths")]

    public List<NearbyCube> possiblePaths = new List<NearbyCube>();
    public bool isEdgeCube;
    [SerializeField] LayerMask walkableCubeLayer;

    [Space]

    [Header("Previous Cube")]

    public Transform previousBlock;

    Vector3 position;
    Vector3 centre;

    private void Start()
    {
        position = PathPointPosition(true);
        centre = transform.position - (transform.forward * offset / 2) + (transform.right * offset / 2) + (transform.up * offset / 2);
        SetPaths();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        Vector3 pos = PathPointPosition(true);
        Gizmos.DrawSphere(pos, radius);

        Gizmos.color = Color.red;
        centre = transform.position - (transform.forward * offset / 2) + (transform.right * offset / 2) + (transform.up * offset / 2);
        Gizmos.DrawSphere(centre, sphereRadius);
    }

    public void SetPaths()
    {
        Collider[] col = Physics.OverlapSphere(centre, sphereRadius, walkableCubeLayer);

        if (col.Length > 0)
        {
            for (int i = 0; i < col.Length; i++)
            {
                if(col[i].gameObject != this.gameObject)
                {
                    NearbyCube cube = new NearbyCube();
                    cube.nearbyCube = col[i].gameObject.transform;
                    cube.isAvailable = true;
                    possiblePaths.Add(cube);
                }
            }
        }
    }
}

[System.Serializable]
public class NearbyCube
{
    public Transform nearbyCube;
    public bool isAvailable;
}
