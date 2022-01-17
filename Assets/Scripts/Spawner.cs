using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _gameCointainer;
    [SerializeField] private GameObject[] _tetraminos;

    [SerializeField] public Transform CeroCoord;
    [SerializeField] public Transform EndCoord;
    [SerializeField] public Transform Cubes;

    public Tetramino ActiveTetramino { get; private set; }
    public float Length { get; private set; }
    public float Height { get; private set; }
    public float Depth { get; private set; }

    private bool _hasStarted;

    void Update()
    {
        Length = (EndCoord.position.x - CeroCoord.position.x) / 5;
        Height = (EndCoord.position.y - CeroCoord.position.y) / 12;
        Depth = (EndCoord.position.z - CeroCoord.position.z) / 5;
    }

    public void NewTetramino()
    {
        var tetraminoPrefab = _tetraminos[Random.Range(0, _tetraminos.Length)];

        var newTetramino = Instantiate(tetraminoPrefab,
            transform.position,
            transform.rotation,
            _gameCointainer.transform);

        ActiveTetramino = newTetramino.GetComponent<Tetramino>();
        ActiveTetramino.transform.localPosition += ActiveTetramino.GetSpawnOffset();
        ActiveTetramino.UpdateCubesPositionsDisplay();
    }

    public void StartGame()
    {
        if (_hasStarted == false)
        {
            _hasStarted = true;
            NewTetramino();
        }
    }
}
