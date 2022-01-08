using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private Vector3 _rotationPoint;
    [SerializeField] private float _fallTime = 0.8f;
    [SerializeField] private Vector3 _spawnOffset;
    private float _previousTime;
    private static int _gameAreaSize = 7;
    private static int _gameAreaHeight = 12;
    private static Transform[,,] _grid = new Transform[_gameAreaSize, _gameAreaHeight, _gameAreaSize];


    void Update()
    {
        Move();
        Rotate();
        Fall();
    }

    public Vector3 GetSpawnOffset()
    {
        return _spawnOffset;
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
        if (!ValidMove())
            transform.position -= new Vector3(0, 0, -1);
    }

    private void MoveDown()
    {
        transform.position += new Vector3(0, 0, 1);
        if (!ValidMove())
            transform.position -= new Vector3(0, 0, 1);
    }

    private void MoveRight()
    {
        transform.position += new Vector3(1, 0, 0);
        if (!ValidMove())
            transform.position -= new Vector3(1, 0, 0);
    }

    private void MoveLeft()
    {
        transform.position += new Vector3(-1, 0, 0);

        if (!ValidMove())
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

        if (!ValidMove())
            transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(0, 0, 1), -90);
    }

    private void RotateRight()
    {
        transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(0, 0, 1), -90);

        if (!ValidMove())
            transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(0, 0, 1), 90);
    }

    private void RotateUp()
    {
        transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(1, 0, 0), 90);

        if (!ValidMove())
            transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(1, 0, 0), -90);
    }

    private void RotateDown()
    {
        transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(1, 0, 0), -90);

        if (!ValidMove())
            transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(1, 0, 0), 90);
    }

    private void Fall()
    {
        if (Time.time - _previousTime > (Input.GetKey(KeyCode.RightControl) ? _fallTime / 10 : _fallTime))
        {
            transform.position += new Vector3(0, -1, 0);

            if (!ValidMove())
            {
                transform.position -= new Vector3(0, -1, 0);
                FindObjectOfType<Spawner>().NewTetramino();
                AddToGrid();
                enabled = false;
                return;
            }

            _previousTime = Time.time;
        }
    }
    
    private bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int x = Mathf.FloorToInt(children.transform.position.x);
            int y = Mathf.FloorToInt(children.transform.position.y);
            int z = Mathf.FloorToInt(children.transform.position.z);


            if (x  < 0 || x >= _gameAreaSize ||
                z  < 0 || z >= _gameAreaSize ||
                y < 0) 
            {
                return false;
            }

            if (_grid[x, y, z] != null) return false;
        }
        
        return true;
    }

    private void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int x = Mathf.FloorToInt(children.transform.position.x);
            int y = Mathf.FloorToInt(children.transform.position.y);
            int z = Mathf.FloorToInt(children.transform.position.z);

            _grid[x, y, z] = children;
        }
    }
}
