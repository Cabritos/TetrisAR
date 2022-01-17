using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Spawner _spawner;

    void Awake()
    {
        _spawner = FindObjectOfType<Spawner>();
    }

    public void MoveUp()
    {
        FindObjectOfType<Spawner>().ActiveTetramino._moveUp = true;
    }

    public void MoveDown()
    {
        FindObjectOfType<Spawner>().ActiveTetramino._moveDown = true;
    }

    public void MoveLeft()
    {
        FindObjectOfType<Spawner>().ActiveTetramino._moveLeft = true;
    }

    public void MoveRight()
    {
        FindObjectOfType<Spawner>().ActiveTetramino._moveRight = true;
    }
}
