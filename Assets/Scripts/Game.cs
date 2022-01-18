using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject _gameArea;
    [SerializeField] private Transform CeroCoord;
    [SerializeField] private Transform EndCoord;
    private Spawner _spawner;

    public float Length { get; private set; }
    public float Height { get; private set; }
    public float Depth { get; private set; }
    public bool HasStarted { get; private set; }

    private int _gameAreaX;
    private int _gameAreaY;
    private int _gameAreaZ;

    void Awake()
    { 
        _spawner = FindObjectOfType<Spawner>();
        _gameAreaX = (int)_gameArea.transform.localScale.x;
        _gameAreaY = (int)_gameArea.transform.localScale.y;
        _gameAreaZ = (int)_gameArea.transform.localScale.z;
    }

    void Update()
    {
        Length = (EndCoord.position.x - CeroCoord.position.x) / _gameAreaX;
        Height = (EndCoord.position.y - CeroCoord.position.y) / _gameAreaY;
        Depth = (EndCoord.position.z - CeroCoord.position.z) / _gameAreaZ;
    }

    public void StartGame()
    {
        if (HasStarted == false)
        {
            HasStarted = true;
            _spawner.NewTetramino();
        }
    }

    public Vector3Int GetGameArea()
    {
        return new Vector3Int(_gameAreaX, _gameAreaY, _gameAreaZ);
    }

    public Transform GetCeroCoord()
    {
        return CeroCoord;
    }

    public Transform GetEndCoord()
    {
        return EndCoord;
    }
}
