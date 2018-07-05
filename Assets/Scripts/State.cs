using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Orientation
{
    RightZ,
    RightX,
    LeftZ,
    LeftX
}
public class State : MonoBehaviour
{
    [HideInInspector]
    public bool rotate;
    public Transform topHorizontalTransform;
    public Transform centralHorizontalTransform;
    public Transform bottomHorizontalTransform;
    public Transform leftVerticalTransform;
    public Transform centralVerticalXTransform;
    public Transform centralVerticalZTransform;
    public Transform rightVerticalTransform;
    public HashSet<RotateBehaviour> cubes = new HashSet<RotateBehaviour>();
    public Orientation GetOrientation()
    {
        Vector3 dir = Camera.main.transform.TransformDirection(Vector3.right);
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
        {
            if (dir.x > 0) return Orientation.RightX;
            return Orientation.LeftX;
        }
        if (dir.z > 0) return Orientation.RightZ;
        return Orientation.LeftZ;
    }
    // Use this for initialization
    void Start()
    {
        rotate = false;
    }

    internal void SetTransforms()
    {
        foreach (var cube in cubes)
        {
            if (cube.transform.position.y > 0.9f &&
                Mathf.Abs(cube.transform.position.x) < 0.1f &&
                Mathf.Abs(cube.transform.position.z) < 0.1f)
            {
                topHorizontalTransform = cube.transform;
                continue;
            }
            if (cube.transform.position.y < -0.9f &&
                Mathf.Abs(cube.transform.position.x) < 0.1f &&
                Mathf.Abs(cube.transform.position.z) < 0.1f)
            {
                bottomHorizontalTransform = cube.transform;
                continue;
            }
            switch (GetOrientation())
            {
                case Orientation.RightX:
                    if (cube.transform.position.x > 0.9f &&
                Mathf.Abs(cube.transform.position.y) < 0.1f &&
                Mathf.Abs(cube.transform.position.z) < 0.1f)
                    {
                        rightVerticalTransform = cube.transform;
                        continue;
                    }
                    if (cube.transform.position.x < -0.9f &&
                        Mathf.Abs(cube.transform.position.y) < 0.1f &&
                        Mathf.Abs(cube.transform.position.z) < 0.1f)
                        leftVerticalTransform = cube.transform;
                    break;
                case Orientation.LeftX:
                    if (cube.transform.position.x > 0.9f &&
                Mathf.Abs(cube.transform.position.y) < 0.1f &&
                Mathf.Abs(cube.transform.position.z) < 0.1f)
                    {
                        leftVerticalTransform = cube.transform;
                        continue;
                    }
                    if (cube.transform.position.x < -0.9f &&
                        Mathf.Abs(cube.transform.position.y) < 0.1f &&
                        Mathf.Abs(cube.transform.position.z) < 0.1f)
                        rightVerticalTransform = cube.transform;
                    break;
                case Orientation.RightZ:
                    if (cube.transform.position.z > 0.9f &&
                Mathf.Abs(cube.transform.position.y) < 0.1f &&
                Mathf.Abs(cube.transform.position.x) < 0.1f)
                    {
                        rightVerticalTransform = cube.transform;
                        continue;
                    }
                    if (cube.transform.position.z < -0.9f &&
                        Mathf.Abs(cube.transform.position.y) < 0.1f &&
                        Mathf.Abs(cube.transform.position.x) < 0.1f)
                        leftVerticalTransform = cube.transform;
                    break;
                case Orientation.LeftZ:
                    if (cube.transform.position.z > 0.9f &&
                Mathf.Abs(cube.transform.position.y) < 0.1f &&
                Mathf.Abs(cube.transform.position.x) < 0.1f)
                    {
                        leftVerticalTransform = cube.transform;
                        continue;
                    }
                    if (cube.transform.position.z < -0.9f &&
                        Mathf.Abs(cube.transform.position.y) < 0.1f &&
                        Mathf.Abs(cube.transform.position.x) < 0.1f)
                        rightVerticalTransform = cube.transform;
                    break;
            }
        }
    }
}
