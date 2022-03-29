using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairWalkPath : WalkablePath
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(PathPointPosition(false) , radius);
    }
}
