using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Game _game;
    private InputManager _inputManager;
    [SerializeField] private GameObject[] _tetraminos;
    [SerializeField] public Transform Cubes;

    public Tetramino ActiveTetramino { get; private set; }

    void Awake()
    {
        _game = FindObjectOfType<Game>();
        _inputManager = FindObjectOfType<InputManager>();
    }

    public void NewTetramino()
    {
        var tetraminoPrefab = _tetraminos[Random.Range(0, _tetraminos.Length)];

        var newTetramino = Instantiate(tetraminoPrefab,
            transform.position,
            transform.rotation,
            _game.gameObject.transform);

        ActiveTetramino = newTetramino.GetComponent<Tetramino>();
        ActiveTetramino.gameObject.transform.localPosition += ActiveTetramino.GetSpawnOffset();
        ActiveTetramino.UpdateCubesPositionsDisplay();
        _inputManager.SetActiveTetramino(ActiveTetramino);
    }
}
