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
    [SerializeField] private Material _cubeMaterial;

    private float _previousTime;
    private static int _gameAreaSize;
    private static int _gameAreaHeight;
    private static Transform[,,] _grid = new Transform[6, 14, 6];

    public bool MoveUpFlag;
    public bool MoveDownFlag;
    public bool MoveLeftFlag;
    public bool MoveRightFlag;

    public bool HardFallFlag;

    public bool RotateUpFlag;
    public bool RotateDownFlag;
    public bool RotateLeftFlag;
    public bool RotateRightFlag;

    private Game _game;
    private Spawner _spawner;

    void Awake()
    {
        _game = FindObjectOfType<Game>();
        _spawner = FindObjectOfType<Spawner>();

        _gameAreaSize = _game.GetGameArea().x;
        _gameAreaHeight = _game.GetGameArea().y;
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
        if (Input.GetKeyDown(KeyCode.LeftArrow) || MoveLeftFlag)
        {
            MoveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || MoveRightFlag)
        {
            MoveRight();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || MoveUpFlag)
        {
            MoveUp();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || MoveDownFlag)
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
        MoveUpFlag = false;
        transform.localPosition += new Vector3(0, 0, 1);
        if (!ValidMove())
            transform.localPosition -= new Vector3(0, 0, 1);
    }

    private void MoveDown()
    {
        MoveDownFlag = false;
        transform.localPosition += new Vector3(0, 0, -1);
        if (!ValidMove())
            transform.localPosition -= new Vector3(0, 0, -1);
    }

    private void MoveRight()
    {
        MoveRightFlag = false;
        transform.localPosition += new Vector3(1, 0, 0);
        if (!ValidMove())
            transform.localPosition -= new Vector3(1, 0, 0);
    }

    private void MoveLeft()
    {
        MoveLeftFlag = false;
        transform.localPosition += new Vector3(-1, 0, 0);
        if (!ValidMove())
            transform.localPosition -= new Vector3(-1, 0, 0);
    }

    private void Rotate()
    {
        if (Input.GetKeyDown(KeyCode.A) || RotateLeftFlag)
        {
            RotateLeft();
        }
        else if (Input.GetKeyDown(KeyCode.D) || RotateRightFlag)
        {
            RotateRight();
        }
        else if (Input.GetKeyDown(KeyCode.W) || RotateUpFlag)
        {
            RotateUp();
        }
        else if (Input.GetKeyDown(KeyCode.S) || RotateDownFlag)
        {
            RotateDown();
        }
    }

    private void RotateLeft()
    {
        RotateLeftFlag = false;
        transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(0, 0, 1), 90);

        if (!ValidMove())
            transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(0, 0, 1), -90);
    }

    private void RotateRight()
    {
        RotateRightFlag = false;
        transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(0, 0, 1), -90);

        if (!ValidMove())
            transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(0, 0, 1), 90);
    }

    private void RotateUp()
    {
        RotateUpFlag = false;
        transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(1, 0, 0), 90);

        if (!ValidMove())
            transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(1, 0, 0), -90);
    }

    private void RotateDown()
    {
        RotateDownFlag = false;
        transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(1, 0, 0), -90);

        if (!ValidMove())
            transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(1, 0, 0), 90);
    }

    public void Fall()
    {
        if (Time.time - _previousTime > ((Input.GetKey(KeyCode.RightControl) || HardFallFlag) ? _fallTime / 10 : _fallTime))
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

            HardFallFlag = false;
            _previousTime = Time.time;
        }
    }
    
    private bool ValidMove()
    {
        Debug.Log("Valid call");

        foreach (Transform children in transform)
        {
            var cube = children.GetComponent<Cube>();

            if (cube == null) continue;

            var gridPos = cube.GetGridPosition();

            Debug.Log(_gameAreaSize);
            Debug.Log(_gameAreaHeight);

            if (gridPos.x < 0 || gridPos.x >= _gameAreaSize ||
                gridPos.z < 0 || gridPos.z >= _gameAreaSize ||
                gridPos.y < 0) 
            {
                Debug.LogWarning("Fuera de grilla");
                return false;
            }

            if (_grid[gridPos.x, gridPos.y, gridPos.z] != null)
            {
                Debug.Log(_grid[gridPos.x, gridPos.y, gridPos.z]);
                Debug.LogWarning("Ocupado");
                return false;
            }
        }

        Debug.LogWarning("Válido");
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
        foreach (Transform child in transform)
        {
            Material[] materials = child.gameObject.GetComponent<MeshRenderer>().materials;
            materials[0] = _cubeMaterial;
            child.gameObject.GetComponent<MeshRenderer>().materials = materials;
        }

        transform.DetachChildren();
        return;

        //this is the real intention, buts its buggy as it is
        foreach (Transform child in transform)
        {
            child.SetParent(_spawner.Cubes, true);
        }
    }
}
