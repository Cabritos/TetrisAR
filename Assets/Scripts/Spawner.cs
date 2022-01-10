using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _gameCointainer;
    [SerializeField] private GameObject[] _tetraminos;
    private bool _hasStarted;

    public void NewTetramino()
    {
        var tetramino = _tetraminos[Random.Range(0, _tetraminos.Length)];

        Instantiate(tetramino,
            transform.position + tetramino.GetComponent<Tetramino>().GetSpawnOffset(),
            transform.rotation,
            _gameCointainer.transform);

        //tetramino.GetComponent<Tetramino>().SetScale(_gameCointainer.transform.localScale.x); 
    }

    public void StartGame()
    {
        if (_hasStarted) return;
        NewTetramino();
        _hasStarted = true;
    }
}
