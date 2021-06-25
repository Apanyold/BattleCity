using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Behaviors;
using System;
using Random = UnityEngine.Random;

namespace Didenko.BattleCity.MapScripts
{
    public class Map : MonoBehaviour
    {
        public int Size => mapSize.x * mapSize.y;

        [SerializeField]
        private Vector2Int mapSize;
        [SerializeField]
        private Transform mapHolder;
        [SerializeField]
        private GameObject groundGo;
        [SerializeField]
        private MapObjectBehavior[] mapObjectBehaviors;

        private Vector2Int[,] mapCells;

        private Dictionary<Vector2Int, MapCell> mapCellDic = new Dictionary<Vector2Int, MapCell>();

        private int
            wallCreationChance,
            wallsMaxCount;

        private MapObjectBehavior marker;

        public void InitMap()
        {
            mapCells = new Vector2Int[mapSize.x, mapSize.y];

            wallsMaxCount = mapSize.x * mapSize.y / 2;
            wallCreationChance = (int)(wallsMaxCount * 0.2f);

            for (int i = 0; i < mapSize.x; i++)
            {
                for (int j = 0; j < mapSize.y; j++)
                {
                    mapCells[i, j] = new Vector2Int(i, j);
                }
            }

            GenerateMap();
            //GenerateWalls();
            marker = Resources.Load<Behaviors.MapObjectBehavior>("Prefabs/MarkerRed");
            //mapHolder.position = new Vector3(-mapSize.x, -mapSize.y, 0);
        }

        public void CreateMapObj(MapObjectBehavior mapObj, Vector2Int position, CellType cellType)
        {
            var newPositions = TryPlaceMapObject(mapObj, position, cellType);
            if (newPositions.Length != mapObj.SizeInfo.Length)
                return;

            var obj = Instantiate(mapObj, new Vector3(position.x * 2, position.y * 2, 0), new Quaternion(0, 0, 0, 0), mapHolder);
            obj.PositionsOccupied = newPositions;
        }

        public Vector2Int[] TryPlaceMapObject(MapObjectBehavior mapObj, Vector2Int position, CellType cellType)
        {
            foreach (var item in mapObj.SizeInfo)
            {
                var pos = item + position;
                if (!CheckPosition(pos))
                    return new Vector2Int[0];
            }

            Vector2Int[] newPositions = new Vector2Int[mapObj.SizeInfo.Length];

            int ticker = 0;
            foreach (var item in mapObj.SizeInfo)
            {
                var pos = position + item;
                if (mapCellDic.ContainsKey(pos))
                {
                    var cell = mapCellDic[position];
                    cell.cellObject = mapObj.gameObject;
                }
                else
                    mapCellDic.Add(pos, new MapCell(mapObj.gameObject, pos, cellType));

                newPositions[ticker] = pos;
                ticker++;
            }
            return newPositions;
        }

        public void RemoveMapObject(MapObjectBehavior mapObj, Vector2Int position)
        {
            foreach (var item in mapObj.PositionsOccupied)
                RemoveCellData(item);

            Destroy(mapObj.gameObject);
        }

        public void RemoveCellData(Vector2Int position)
        {
            if (mapCellDic.ContainsKey(position))
                mapCellDic.Remove(position);
        }

        public bool CheckPosition(Vector2Int position)
        {
            if (!IsInsideMap(position))
                return false;

            if (mapCellDic.ContainsKey(position))
            {
                var cell = mapCellDic[position];
                if (cell.cellType == CellType.Wall)
                    return false;
            }
            return true;
        }

        public Vector2Int RealPosToCell(Vector3 vector3)
        {
            int x = (int)vector3.x;
            int y = (int)vector3.y;

            Vector2Int vector2Int = new Vector2Int(0,0);

            if(x % 2 == 0)
            {

            }

            //var x = Math.Round(vector3.x/2,1);
            //var y = Math.Round(vector3.y/2,1);

            //int xI = (int)Math.Round(vector3.x / 2, 1);
            //int yI = (int)Math.Round(vector3.y / 2, 1);

            //var xF = x - xI;
            //if (xF > 0.0)
            //    xR = (int)(x - xF);
            //else
            //    xR = (int)xF;

            //var yF = y - yI;
            //if (yF > 0.0)
            //    yR = (int)(y - yF);
            //else
            //    yR = (int)yF;




            //float xF = vector3.x/2f;
            //float yF = vector3.y/2f;

            //x = Mathf.FloorToInt(xF);
            //y = Mathf.FloorToInt(yF);

            //if (xF - x > 0.5f)
            //    x = Mathf.FloorToInt(vector3.x / 2);
            //    x = Mathf.CeilToInt(xF);
            //else
            //    ;//x = (int)Math.Truncate(xF);

            //if (yF - y > 0.5f)
            //    y = Mathf.FloorToInt(vector3.y / 2);
            //    y = Mathf.CeilToInt(yF);
            //else
            //    ;//y = (int)Math.Truncate(yF);
            //Mathf.CeilToInt
            //Vector3Int.FloorToInt
            //Debug.Log($"Rounded ({x},{y}) ({vector3.x},{vector3.y}), ({vector3.x / 2},{vector3.y / 2})");
            //return new Vector2Int(Mathf.FloorToInt(vector3.x/2), Mathf.FloorToInt(vector3.y/2));
            return new Vector2Int(x, y);
        }

        public bool IsInsideMap(Vector2Int position)
        {
            if (position.x > mapSize.x|| position.x < 0)
                return false;
            if (position.y > mapSize.y|| position.y < 0)
                return false;

            return true;
        }

        public void ReGenWalls()
        {
            Debug.Log("RegenMap");
            foreach (var item in mapCellDic)
                Destroy(item.Value.cellObject);

            mapCellDic = new Dictionary<Vector2Int, MapCell>();
            GenerateMap();
            GenerateWalls();
        }

        public List<List<Vector2Int>> GetBasePositions()
        {
            List<List<Vector2Int>> vector2Ints = new List<List<Vector2Int>>();
            vector2Ints.Add(new List<Vector2Int>()); 
            vector2Ints.Add(new List<Vector2Int>());

            for (int i = 1; i < mapSize.x -1; i++)
            {
                var pos = new Vector2Int(i, mapSize.y - 2);
                if (CheckPosition(pos))
                    vector2Ints[0].Add(pos);
            }
            for (int i = 1; i < mapSize.x - 1; i++)
            {
                var pos = new Vector2Int(i, 1);
                if (CheckPosition(pos))
                    vector2Ints[1].Add(pos);
            }

            return vector2Ints;
        }
        private void GenerateMap()
        {
            foreach (var item in mapCells)
            {
                Instantiate(groundGo, new Vector3(item.x * 2, item.y * 2, 0), new Quaternion(0, 0, 0, 0), mapHolder);

                if (item.x == mapCells.GetLength(0) - 1 || item.x == 0)
                    CreateMapObj(mapObjectBehaviors[0], item, CellType.Wall);
                else if (item.y == mapCells.GetLength(0) - 1 || item.y == 0)
                    CreateMapObj(mapObjectBehaviors[1], item, CellType.Wall);
            }
        }

        private void GenerateWalls()
        {
            for (int i = 1; i < mapCells.GetLength(0)-1; i++)
            {
                for (int j = 1; j < mapCells.GetLength(1) - 1; j++)
                {
                    if (wallCreationChance < Random.Range(0, wallsMaxCount))
                            continue;

                    var selectedWall = mapObjectBehaviors[Random.Range(0, mapObjectBehaviors.Length)];

                    CreateMapObj(selectedWall, new Vector2Int(i, j), CellType.Wall);
                }
            }
        }
    }
    public class MapCell
    {
        public GameObject cellObject;
        public Vector2Int cellPosition;
        public CellType cellType;

        public MapCell(GameObject cellObject, Vector2Int cellPosition, CellType cellType)
        {
            this.cellObject = cellObject;
            this.cellPosition = cellPosition;
            this.cellType = cellType;
        }
    }

    public enum CellType
    {
        Wall,
        Other
    }
}

