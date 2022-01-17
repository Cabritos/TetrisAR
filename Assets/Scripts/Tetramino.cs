using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Tetramino : MonoBehaviour
{
    [SerializeField] private GameObject _rotationPointIndicator;
    [SerializeField] private Vector3 _rotationPoint;
    [SerializeField] private float _fallTime = 0.8f;
    [SerializeField] private Vector3 _spawnOffset;
    private float _previousTime;
    private static int _gameAreaSize = 5;
    private static int _gameAreaHeight = 12;
    private static Transform[,,] _grid = new Transform[_gameAreaSize + 1, _gameAreaHeight + 2, _gameAreaSize +1];

    public bool _moveUp = false;
    public bool _moveDown = false;
    public bool _moveLeft = false;
    public bool _moveRight = false;

    public bool _rotateUp = false;
    public bool _rotateDown = false;
    public bool _rotateLeft = false;
    public bool _rotateRight = false;

    private Spawner _spawner;

    void Start()
    {
        _spawner = FindObjectOfType<Spawner>();
    }

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
        if (Input.GetKeyDown(KeyCode.LeftArrow) || _moveLeft)
        {
            MoveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || _moveRight)
        {
            MoveRight();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || _moveUp)
        {
            MoveUp();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || _moveDown)
        {
            MoveDown();
        }
    }

    public void UpdateCubesPositionsDisplay()
    {
        if (transform.childCount == 0) return;

        foreach (Transform children in transform)
        {
            var cube = children.GetComponent<Cube>();

            if (cube != null) cube.UpdatePositionText();
        }
    }

    private void MoveUp()
    {
        _moveUp = false;
        transform.localPosition += new Vector3(0, 0, 1);
        if (!ValidMove())
            transform.localPosition -= new Vector3(0, 0, 1);
    }

    private void MoveDown()
    {
        _moveDown = false;
        transform.localPosition += new Vector3(0, 0, -1);
        if (!ValidMove())
            transform.localPosition -= new Vector3(0, 0, -1);
    }

    private void MoveRight()
    {
        _moveRight = false;
        transform.localPosition += new Vector3(1, 0, 0);
        if (!ValidMove())
            transform.localPosition -= new Vector3(1, 0, 0);
    }

    private void MoveLeft()
    {
        _moveLeft = false;
        transform.localPosition += new Vector3(-1, 0, 0);
        if (!ValidMove())
            transform.localPosition -= new Vector3(-1, 0, 0);
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
        _rotateLeft = false;
        transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(0, 0, 1), 90);

        if (!ValidMove())
            transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(0, 0, 1), -90);
    }

    private void RotateRight()
    {
        _rotateRight = false;
        transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(0, 0, 1), -90);

        if (!ValidMove())
            transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(0, 0, 1), 90);
    }

    private void RotateUp()
    {
        _rotateUp = false;
        transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(1, 0, 0), 90);

        if (!ValidMove())
            transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(1, 0, 0), -90);
    }

    private void RotateDown()
    {
        _rotateDown = false;
        transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(1, 0, 0), -90);

        if (!ValidMove())
            transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(1, 0, 0), 90);
    }

    public void Fall()
    {
        if (Time.time - _previousTime > (Input.GetKey(KeyCode.RightControl) ? _fallTime / 10 : _fallTime))
        {
            transform.localPosition += new Vector3(0, -1, 0);
            UpdateCubesPositionsDisplay();

            if (!ValidMove())
            {
                transform.localPosition += new Vector3(0, 1, 0);
                Destroy(_rotationPointIndicator.gameObject);
                
                if (ExcedesHeight()) return;

                AddToGrid();
                UpdateCubesPositionsDisplay();
                CheckForLines();
                _spawner.NewTetramino();
                CallSocialServices();
                Destroy(gameObject);
            }

            _previousTime = Time.time;
        }
    }
    
    private bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            var cube = children.GetComponent<Cube>();

            if (cube == null) continue;

            var gridPos = cube.GetGridPosition();
            
            if (gridPos.x < 0 || gridPos.x >= _gameAreaSize ||
                gridPos.z < 0 || gridPos.z >= _gameAreaSize ||
                gridPos.y < 0) 
            {
                return false;
            }

            if (_grid[gridPos.x, gridPos.y, gridPos.z] != null) return false;
        }

        return true;
    }

    private bool ExcedesHeight()
    {
        foreach (Transform children in transform)
        {
            var cube = children.GetComponent<Cube>();

            if (cube == null) continue;
            
            if (cube.GetGridPosition().y < _gameAreaHeight) continue;
            
            Debug.LogError("This is endgame");
            Destroy(_spawner.gameObject);
            enabled = false;
            return true;
        }

        return false;
    }

    private void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            var cube = children.GetComponent<Cube>();

            if (cube == null) continue;

            var gridPos = cube.GetGridPosition();
            _grid[gridPos.x, gridPos.y, gridPos.z] = children;
        }
    }

    private void CheckForLines()
    {
        for (int y = _gameAreaHeight - 1; y >= 0; y--)
        {
            if (HasLine(y))
            {
                DeleteLine(y);
                RowDown(y);
            }
        }
    }

    private bool HasLine(int y)
    {
        for (int x = 0; x < _gameAreaSize; x++)
        {
            for (int z = 0; z < _gameAreaSize; z++)
            {
                if (_grid[x, y, z] == null)
                    return false;
            }
        }

        return true;
    }

    private void DeleteLine(int y)
    {
        for (int x = 0; x < _gameAreaSize; x++)
        {
            for (int z = 0; z < _gameAreaSize; z++)
            {
                Destroy(_grid[x, y, z].gameObject);
                _grid[x, y, z] = null;
            }
        }
    }

    private void RowDown(int startingRow)
    {
        for (int y = startingRow + 1; y < _gameAreaHeight; y++)
        {
            for (int x = 0; x < _gameAreaSize; x++)
            {
                for (int z = 0; z < _gameAreaSize; z++)
                {
                    if (_grid[x, y, z] == null) continue;

                    _grid[x, y, z].gameObject.GetComponent<Cube>().MoveDown();
                    _grid[x, y - 1, z] = _grid[x, y, z];
                    _grid[x, y, z] = null;
                }
            }
        }
    }

    private void CallSocialServices()
    {
        transform.DetachChildren();
        return;

        //this is the real intention, buts its buggy as it is
        foreach (Transform child in transform)
        {
            child.SetParent(_spawner.Cubes, true);
        }
    }
}
