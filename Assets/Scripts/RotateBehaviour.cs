using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBehaviour : MonoBehaviour
{
    public State state;
    [Range(0f, 1f)]
    public float rotateSpeed = 0.1f;
    private Vector3 forward;
    private Vector3 right;

    // Use this for initialization
    void Start()
    {
        if (state)
            state.cubes.Add(this);
        forward = Vector3.up;
        if (transform.position.y < -0.5f)
            forward = Vector3.down;
        if (transform.position.x > 0.5f)
            forward = Vector3.right;
        if (transform.position.x < -0.5f)
            forward = Vector3.left;
        if (transform.position.z > 0.5f)
            forward = Vector3.forward;
        if (transform.position.z < -0.5f)
            forward = Vector3.back;
    }

    // Update is called once per frame
    void Update()
    {
        if (state)
            if (!state.rotate)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hitInfo;
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        if (hitInfo.transform == transform)
                        {
                            RotateHorizontal();
                        }
                    }
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hitInfo;
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        if (hitInfo.transform == transform)
                        {
                            RotateVertical();
                        }
                    }
                }

            }
    }

    private void RotateHorizontal()
    {
        state.rotate = true;
        foreach (var cube in state.cubes)
        {
            cube.transform.SetParent(null);
        }
        state.SetTransforms();
        foreach (var cube in state.cubes)
        {
            if (Mathf.Abs(cube.transform.position.y - transform.position.y) < 0.1f)
            {
                if (cube.transform.position.y > .5)
                    cube.transform.SetParent(state.topHorizontalTransform);
                else if (cube.transform.position.y < -.5)
                    cube.transform.SetParent(state.bottomHorizontalTransform);
                else
                    cube.transform.SetParent(state.centralHorizontalTransform);
            }
        }
        StartCoroutine(RotateHorizontalCoroutine());
    }



    public IEnumerator RotateHorizontalCoroutine()
    {
        Quaternion rotation;
        Vector3 axis;
        Quaternion currentRotation;
        if (transform.parent == null)
        {
            rotation = transform.rotation;
            axis = forward;
        }
        else
        {
            rotation = transform.parent.rotation;
            var x = transform.parent.GetComponent<RotateBehaviour>();
            if (x != null)
                axis = x.forward;
            else
                axis = Vector3.up;
        }
        currentRotation = rotation;
        rotation *= Quaternion.AngleAxis(90f, axis);
        float t = 0f;
        while (currentRotation != rotation)
        {
            Debug.LogFormat("Horizontal t:{0}", t);
            if (transform.parent == null)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed);
                currentRotation = transform.rotation;
            }
            else
            {
                transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, rotation, rotateSpeed);
                currentRotation = transform.parent.rotation;
            }
            t = Mathf.Clamp01(t + rotateSpeed);
            yield return null;
        }
        Debug.LogFormat("Horizontal t:{0}", t);
        if (transform.parent == null)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, t);
            currentRotation = transform.rotation;
        }
        else
        {
            transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, rotation, t);
            currentRotation = transform.parent.rotation;
        }
        yield return null;
        state.rotate = false;
        yield break;
    }

    private void RotateVertical()
    {
        state.rotate = true;
        foreach (var cube in state.cubes)
        {
            cube.transform.SetParent(null);
        }
        state.SetTransforms();
        foreach (var cube in state.cubes)
        {
            switch (state.GetOrientation())
            {
                case Orientation.RightX:
                    if (Mathf.Abs(cube.transform.position.x - transform.position.x) < 0.1f)
                    {
                        if (cube.transform.position.x > 0.5)
                            cube.transform.SetParent(state.rightVerticalTransform);
                        else if (cube.transform.position.x < -0.5)
                            cube.transform.SetParent(state.leftVerticalTransform);
                        else
                            cube.transform.SetParent(state.centralVerticalXTransform);
                    }
                    break;
                case Orientation.LeftX:
                    if (Mathf.Abs(cube.transform.position.x - transform.position.x) < 0.1f)
                    {
                        if (cube.transform.position.x > 0.5)
                            cube.transform.SetParent(state.leftVerticalTransform);
                        else if (cube.transform.position.x < -0.5)
                            cube.transform.SetParent(state.rightVerticalTransform);
                        else
                            cube.transform.SetParent(state.centralVerticalXTransform);
                    }
                    break;
                case Orientation.RightZ:
                    if (Mathf.Abs(cube.transform.position.z - transform.position.z) < 0.1f)
                    {
                        if (cube.transform.position.z > 0.5)
                            cube.transform.SetParent(state.rightVerticalTransform);
                        else if (cube.transform.position.z < -0.5)
                            cube.transform.SetParent(state.leftVerticalTransform);
                        else
                            cube.transform.SetParent(state.centralVerticalZTransform);
                    }
                    break;
                case Orientation.LeftZ:
                    if (Mathf.Abs(cube.transform.position.z - transform.position.z) < 0.1f)
                    {
                        if (cube.transform.position.z > 0.5)
                            cube.transform.SetParent(state.leftVerticalTransform);
                        else if (cube.transform.position.z < -0.5)
                            cube.transform.SetParent(state.rightVerticalTransform);
                        else
                            cube.transform.SetParent(state.centralVerticalZTransform);
                    }
                    break;
            }

        }
        StartCoroutine(RotateVerticalCoroutine());
    }

    public IEnumerator RotateVerticalCoroutine()
    {
        Quaternion rotation;
        Vector3 axis;
        Quaternion currentRotation;
        if (transform.parent == null)
        {
            rotation = transform.rotation;
            axis = forward;
        }
        else
        {
            rotation = transform.parent.rotation;
            var x = transform.parent.GetComponent<RotateBehaviour>();
            if (x != null)
                axis = x.forward;
            else
            {
                switch (state.GetOrientation())
                {
                    case Orientation.LeftX:
                    case Orientation.RightX:
                        axis = Vector3.left;
                        break;
                    case Orientation.LeftZ:
                    case Orientation.RightZ:
                        axis = Vector3.back;
                        break;
                    default:
                        axis = Vector3.forward;
                        Debug.Log("How?");
                        break;
                }
            }
        }
        currentRotation = rotation;
        rotation *= Quaternion.AngleAxis(90f, axis);

        float t = 0f;
        while (currentRotation != rotation)
        {
            Debug.LogFormat("Vertical t:{0}", t);
            if (transform.parent == null)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed);
                currentRotation = transform.rotation;
            }
            else
            {
                transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, rotation, rotateSpeed);
                currentRotation = transform.parent.rotation;
            }
            t = Mathf.Clamp01(t + rotateSpeed);
            yield return null;
        }
        Debug.LogFormat("Vertical t:{0}", t);
        if (transform.parent == null)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, t);
            currentRotation = transform.rotation;
        }
        else
        {
            transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, rotation, t);
            currentRotation = transform.parent.rotation;
        }
        yield return null;
        state.rotate = false;
        yield break;
    }
}
