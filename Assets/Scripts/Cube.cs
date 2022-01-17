using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cube : MonoBehaviour
{
    static Spawner Spawner;
    private Text _text;

    static float Size;

    void Awake()
    {
        Spawner = FindObjectOfType<Spawner>();
        _text = GetComponentInChildren<Text>();

        Size = transform.lossyScale.x;
    }

    public Vector3Int GetGridPosition()
    {
        var result = new Vector3Int(
            Mathf.FloorToInt((transform.position.x - Spawner.CeroCoord.position.x) / Spawner.Length),
            Mathf.FloorToInt((transform.position.y - Spawner.CeroCoord.position.y) / Spawner.Height),
            Mathf.FloorToInt((transform.position.z - Spawner.CeroCoord.position.z) / Spawner.Depth)
        );

        //Debug.Log(result);
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
