using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    public Transform currentCube;
    public Transform targetCube;

    [Space]

    [Header("Path")]
    [SerializeField] List<Transform> path = new List<Transform>();

    [Space]

    [Header("Position Update Variable")]
    [SerializeField] float cubeSpeed;
    [SerializeField] float stairSpeed;
    [SerializeField] float ladderSpeed;
    [SerializeField] Coroutine Move;


    private void Start()
    {
        CurrentCubeDetection();
    }

    private void Update()
    {
        ClickMovement();
    }

    public void ClickMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                if (hit.transform.GetComponent<WalkablePath>() != null)
                {
                    targetCube = hit.transform;
                    FindPath();
                }
            }
        }
    }

    public void CurrentCubeDetection()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.GetComponent<WalkablePath>() != null)
            {
                currentCube = hit.transform;
            }
        }
    }

    public void FindPath()
    {
        List<Transform> nextCubes = new List<Transform>();
        List<Transform> pastCubes = new List<Transform>();

        foreach (NearbyCube cube in currentCube.GetComponent<CubeWalkPath>().possiblePaths)
        {
            if (cube.isAvailable)
            {
                nextCubes.Add(cube.nearbyCube);
                cube.nearbyCube.GetComponent<CubeWalkPath>().previousBlock = currentCube;
            }
        }

        pastCubes.Add(currentCube);
        ExploreCubes(nextCubes, pastCubes);
        FollowPath();
    }

    public void ExploreCubes(List<Transform> nextCubes, List<Transform> visitedCubes)
    {
        Transform current = nextCubes[0];
        nextCubes.Remove(current);

        if (current == targetCube)
        {
            return;
        }

        foreach (NearbyCube cube in current.GetComponent<CubeWalkPath>().possiblePaths)
        {
            if (!visitedCubes.Contains(cube.nearbyCube) && cube.isAvailable)
            {
                nextCubes.Add(cube.nearbyCube);
                cube.nearbyCube.GetComponent<CubeWalkPath>().previousBlock = current;
            }
        }

        visitedCubes.Add(current);

        if (nextCubes.Any())
        {
            ExploreCubes(nextCubes, visitedCubes);
        }
    }

    private void FollowPath()
    {
        Transform cube = targetCube;
        while (cube != currentCube)
        {
            path.Add(cube);
            if (cube.GetComponent<CubeWalkPath>().previousBlock != null)
            {
                cube = cube.GetComponent<CubeWalkPath>().previousBlock;
            }
            else
            {
                return;
            }
        }
        StartCoroutine(MoveObject());
    }

    IEnumerator MoveObject()
    {
        for (int i = path.Count-1; i >= 0; i--)
        {
            Move = StartCoroutine(UpdatePosition(i));
            yield return Move;   
        }
        Clear();
    }

    IEnumerator UpdatePosition(int i)
    {
        float speed = 0;
        if (path[i].GetComponent<CubeWalkPath>() != null)
        {
            speed = cubeSpeed;
        }
        else if (path[i].GetComponent<StairWalkPath>() != null)
        {
            speed = stairSpeed;
        }
        while (transform.position != path[i].position)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[i].position, speed * Time.deltaTime);
            yield return null;
        }
    }

    public void Clear()
    {
        for (int i = 0; i < path.Count; i++)
        {
            path[i].GetComponent<CubeWalkPath>().previousBlock = null;
        }
        path.Clear();
    }
}
