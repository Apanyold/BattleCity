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
        public static int MapCellSize = 2;

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
                    mapCells[i, j] = new Vector2Int(i* MapCellSize, j* MapCellSize);
                }
            }

            GenerateMap();
            GenerateWalls();
            marker = Resources.Load<Behaviors.MapObjectBehavior>("Prefabs/MarkerRed");
        }

        public void CreateMapObj(MapObjectBehavior mapObj, Vector2Int position, CellType cellType)
        {
            var newPositions = TryPlaceMapObject(mapObj, position, cellType);
            if (newPositions.Length != mapObj.SizeInfo.Length)
                return;

            var obj = Instantiate(mapObj, new Vector3(position.x, position.y, 0), new Quaternion(0, 0, 0, 0), mapHolder);
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
            int x;
            int y;

            if (Mathf.CeilToInt(vector3.x) % 2 == 0)
                x = Mathf.CeilToInt(vector3.x);
            else
                x = Mathf.FloorToInt(vector3.x);

            if (Mathf.CeilToInt(vector3.y) % 2 == 0)
                y = Mathf.CeilToInt(vector3.y);
            else
                y = Mathf.FloorToInt(vector3.y);

            return new Vector2Int(x, y);
        }

        public bool IsInsideMap(Vector2Int position)
        {
            if (position.x > mapSize.x * MapCellSize|| position.x < 0)
                return false;
            if (position.y > mapSize.y * MapCellSize|| position.y < 0)
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

            for (int i = Map.MapCellSize; i < mapSize.x - Map.MapCellSize; i++)
            {
                var pos = new Vector2Int(i * MapCellSize, MapCellSize* mapSize.y - MapCellSize - MapCellSize);
                if (CheckPosition(pos))
                    vector2Ints[0].Add(pos);
            }
            for (int i = Map.MapCellSize; i < mapSize.x - Map.MapCellSize; i++)
            {
                var pos = new Vector2Int(i* MapCellSize, MapCellSize);
                if (CheckPosition(pos))
                    vector2Ints[1].Add(pos);
            }

            return vector2Ints;
        }
        private void GenerateMap()
        {
            foreach (var item in mapCells)
            {
                Instantiate(groundGo, new Vector3(item.x, item.y, 0), new Quaternion(0, 0, 0, 0), mapHolder);

                if (item.x == mapCells.GetLength(0) * MapCellSize - MapCellSize || item.x == 0)
                {
                    CreateMapObj(mapObjectBehaviors[0], item, CellType.Wall);
                }
                else if (item.y == mapCells.GetLength(0) * MapCellSize - MapCellSize || item.y == 0)
                    CreateMapObj(mapObjectBehaviors[1], item, CellType.Wall);
            }
        }

        private void GenerateWalls()
        {
            for (int i = 0; i < mapCells.GetLength(0)*2-1; i+=2)
            {
                for (int j = 0; j < mapCells.GetLength(1)*2 - 1; j+=2)
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

