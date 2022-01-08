using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _tetraminos;

    void Start()
    {
        NewTetramino();
    }

    public void NewTetramino()
    {
        var tetramino = _tetraminos[Random.Range(0, _tetraminos.Length)];

        Instantiate(tetramino,
            transform.position + tetramino.GetComponent<Block>().GetSpawnOffset(),
            transform.rotation);
    }
}
