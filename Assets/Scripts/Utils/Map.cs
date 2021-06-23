using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private Vector2Int[,] mapCells;
    private Transform mapHolder;

    private GameObject 
        groundGo,
        stoneGo;
    public Map(Vector2Int size, Transform mapHolder)
    {
        this.mapHolder = mapHolder;
        mapCells = new Vector2Int[size.x, size.y];

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                mapCells[i, j] = new Vector2Int(i, j);
            }
        }

        groundGo = Resources.Load<GameObject>("Prefabs/Ground");
        stoneGo = Resources.Load<GameObject>("Prefabs/Stone");
        GenerateMap();
    }

    private void GenerateMap()
    {
        foreach (var item in mapCells)
        {
            if(item.x == 0 || item.y == 0 || item.x == mapCells.GetLength(1)-1 || item.y == mapCells.GetLength(0)-1)
                Instantiate(stoneGo, new Vector3(item.x *2, item.y * 2, 0), new Quaternion(0,0,0,0), mapHolder);
            else
                Instantiate(groundGo, new Vector3(item.x * 2, item.y * 2, 0), new Quaternion(0, 0, 0, 0), mapHolder);
        }
    }
}
