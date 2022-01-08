using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private Vector3 _rotationPoint;
    [SerializeField] private float _fallTime = 0.8f;
    private float _previousTime;
    private static int _gameAreaSize = 7;

    
    void Update()
    {
        Move();
        Rotate();
        Fall();
    }

    private void Move()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveDown();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveUp();
        }
    }

    private void MoveUp()
    {
        transform.position += new Vector3(0, 0, -1);
        if (!InsideGrid())
            transform.position -= new Vector3(0, 0, -1);
    }

    private void MoveDown()
    {
        transform.position += new Vector3(0, 0, 1);
        if (!InsideGrid())
            transform.position -= new Vector3(0, 0, 1);
    }

    private void MoveRight()
    {
        transform.position += new Vector3(1, 0, 0);
        if (!InsideGrid())
            transform.position -= new Vector3(1, 0, 0);
    }

    private void MoveLeft()
    {
        transform.position += new Vector3(-1, 0, 0);

        if (!InsideGrid())
            transform.position -= new Vector3(-1, 0, 0);
    }

    private void Rotate()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            RotateLeft();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            RotateRight();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            RotateUp();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            RotateDown();
        }
    }

    private void RotateLeft()
    {
        transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(0, 0, 1), 90);

        if (!InsideGrid())
            transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(0, 0, 1), -90);
    }

    private void RotateRight()
    {
        transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(0, 0, 1), -90);

        if (!InsideGrid())
            transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(0, 0, 1), 90);
    }

    private void RotateUp()
    {
        transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(1, 0, 0), 90);

        if (!InsideGrid())
            transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(1, 0, 0), -90);
    }

    private void RotateDown()
    {
        transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(1, 0, 0), -90);

        if (!InsideGrid())
            transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(1, 0, 0), 90);
    }

    private void Fall()
    {
        if (Time.time - _previousTime > (Input.GetKey(KeyCode.RightControl) ? _fallTime / 10 : _fallTime))
        {
            transform.position += new Vector3(0, -1, 0);

            if (!InsideGrid())
            {
                transform.position -= new Vector3(0, -1, 0);

                return;
            }

            _previousTime = Time.time;
        }
    }
    
    private bool InsideGrid()
    {
        foreach (Transform children in transform)
        {
            var roundedX = children.transform.position.x;
            var roundedY = children.transform.position.y;
            var roundedZ = children.transform.position.z;

            if (roundedX < 0 || roundedX >= _gameAreaSize ||
                roundedZ < 0 || roundedZ >= _gameAreaSize ||
                roundedY < 0)
            {
                return false;
            }
        }

        return true;
    }
}
