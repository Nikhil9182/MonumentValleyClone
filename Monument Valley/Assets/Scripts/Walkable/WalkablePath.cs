using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WalkablePath : MonoBehaviour
{
    [Header("Offsets")]
    [Tooltip("Set Offset for point in cube or stair")]
    [SerializeField] protected float offset;

    [Space]

    [Header("Points Radius")]
    [SerializeField] protected float radius;
    [SerializeField] protected float sphereRadius;

    public Vector3 PathPointPosition(bool isCube)
    {
        if(isCube)
        {
            return (transform.position + (transform.up * offset) + (transform.right * offset / 2) + (transform.forward * offset * (-1) / 2));
        }
        else
        {
            return (transform.position + (transform.up * (offset + 0.2f)/2)) - (transform.right * offset / 2) - (transform.forward * offset * (-1) / 2);
        }
    }
}
