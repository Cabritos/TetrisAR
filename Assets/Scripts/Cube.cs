using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cube : MonoBehaviour
{
    static Game Game;
    private Text _text;

    static float Size;

    void Awake()
    {
        Game = FindObjectOfType<Game>();
        _text = GetComponentInChildren<Text>();

        Size = transform.lossyScale.x;
    }

    public Vector3Int GetGridPosition()
    {
        var result = new Vector3Int(
            Mathf.FloorToInt((transform.position.x - Game.GetCeroCoord().position.x) / Game.Length),
            Mathf.FloorToInt((transform.position.y - Game.GetCeroCoord().position.y) / Game.Height),
            Mathf.FloorToInt((transform.position.z - Game.GetCeroCoord().position.z) / Game.Depth)
        );

        Debug.Log(result);
        return result;
    }

    void OnGUI()
    {
        print($"g: {GetGridPosition()}, t: {transform.position}");
    }

    public void UpdatePositionText()
    {
        _text.text = $"{GetGridPosition()}";
    }

    public void MoveDown()
    { 
        transform.position -= new Vector3(0, Size, 0); 
        UpdatePositionText();
    }
}
