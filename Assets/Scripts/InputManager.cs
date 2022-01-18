using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Game _game;
    private Tetramino _activeTetramino;
    
    void Awake()
    {
        _game = GetComponent<Game>();
    }

    public void SetActiveTetramino(Tetramino newTetramino)
    {
        _activeTetramino = newTetramino;
    }

    public void MoveUp()
    {
        if (!_game.HasStarted) return;
        _activeTetramino.MoveUpFlag = true;
    }

    public void MoveDown()
    {
        if (!_game.HasStarted) return;
        _activeTetramino.MoveDownFlag = true;
    }

    public void MoveLeft()
    {
        if (!_game.HasStarted) return;
        _activeTetramino.MoveLeftFlag = true;
    }

    public void MoveRight()
    {
        if (!_game.HasStarted) return;
        _activeTetramino.MoveRightFlag = true;
    }


    public void RotateUp()
    {
        if (!_game.HasStarted) return;
        _activeTetramino.MoveUpFlag = true;
    }

    public void RotateDown()
    {
        if (!_game.HasStarted) return;
        _activeTetramino.RotateDownFlag = true;
    }

    public void RotateLeft()
    {
        if (!_game.HasStarted) return;
        _activeTetramino.RotateLeftFlag = true;
    }

    public void RotateRight()
    {
        if (!_game.HasStarted) return;
        _activeTetramino.RotateRightFlag = true;
    }
}
