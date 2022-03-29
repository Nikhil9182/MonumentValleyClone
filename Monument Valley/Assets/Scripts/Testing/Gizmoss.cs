using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmoss : MonoBehaviour
{
    [Header("Variables")]
    public float gizmosRadius;
    public Color gizmoColor;
    public Color detectedGizmoColor;
    public GameObject target;

    private bool isDetected = false;
    private Camera cam;
    private void Start()
    {
        cam = Camera.main;
    }
    private void Update()
    {
        if(target != null)
        {
            Vector3 selfPos = cam.WorldToScreenPoint(transform.position);
            Vector3 targetPos = cam.WorldToScreenPoint(target.transform.position);
            if(Vector3.Distance(selfPos, targetPos) < 2f)
            {
                Debug.Log(Vector3.Distance(selfPos, targetPos));
                isDetected = true;
            }
            else
            {
                Debug.Log(Vector3.Distance(selfPos, targetPos));
                isDetected = false;
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (isDetected)
        {
            Gizmos.color = detectedGizmoColor;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawSphere(transform.position, gizmosRadius);
    }

}
