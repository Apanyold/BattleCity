using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Behaviors;

namespace Didenko.BattleCity.MapScripts
{
    public class Map : MonoBehaviour
    {
        private Vector2Int[,] mapCells;

        [SerializeField]
        private Vector2Int mapSize;
        [SerializeField]
        private Transform mapHolder;

        [SerializeField]
        private GameObject groundGo;

        [SerializeField]
        private MapObjectBehavior[] mapObjectBehaviors;

        private Dictionary<Vector2Int, MapCell> mapCellDic = new Dictionary<Vector2Int, MapCell>();

        private int 
            wallCreationChance,
            wallsMaxCount;

        public void InitMap()
        {
            mapCells = new Vector2Int[mapSize.x, mapSize.y];

            wallsMaxCount = mapSize.x * mapSize.y / 2;
            wallCreationChance = (int)(wallsMaxCount * 0.1f);

            for (int i = 0; i < mapSize.x; i++)
            {
                for (int j = 0; j < mapSize.y; j++)
                {
                    mapCells[i, j] = new Vector2Int(i, j);
                }
            }

            GenerateMap();
            GenerateWalls();

            mapHolder.position = new Vector3(-mapSize.x, -mapSize.y, 0);
        }

        public void CreateMapObj(MapObjectBehavior mapObj, Vector2Int position)
        {
            var newPositions = TryPlaceMapObject(mapObj, position);
            if(newPositions.Length != mapObj.SizeInfo.Length)
                return;

            var obj = Instantiate(mapObj, new Vector3(position.x * 2, position.y * 2, 0), new Quaternion(0, 0, 0, 0), mapHolder);
            obj.PositionsOccupied = newPositions;
        }

        public Vector2Int[] TryPlaceMapObject(MapObjectBehavior mapObj, Vector2Int position)
        {
            if (!CheckPosition(mapObj, position))
                return new Vector2Int[0];

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
                    mapCellDic.Add(pos, new MapCell(mapObj.gameObject, pos));

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

        public bool CheckPosition(MapObjectBehavior mapObj, Vector2Int position)
        {
            foreach (var item in mapObj.SizeInfo)
            {
                if (mapCellDic.ContainsKey(position + item))
                {
                    var cell = mapCellDic[position + item];
                    if (cell.cellObject != null)
                        return false;
                }
            }
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

        private void GenerateMap()
        {
            foreach (var item in mapCells)
            {
                Instantiate(groundGo, new Vector3(item.x * 2, item.y * 2, 0), new Quaternion(0, 0, 0, 0), mapHolder);

                if (item.x == mapCells.GetLength(0) - 1 || item.x == 0)
                    CreateMapObj(mapObjectBehaviors[0], item);
                else if (item.y == mapCells.GetLength(0) - 1 || item.y == 0)
                    CreateMapObj(mapObjectBehaviors[1], item);
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

                    CreateMapObj(selectedWall, new Vector2Int(i, j));
                }
            }
        }
    }
    public class MapCell
    {
        public GameObject cellObject;
        public Vector2Int cellPosition;

        public MapCell(GameObject cellObject, Vector2Int cellPosition)
        {
            this.cellObject = cellObject;
            this.cellPosition = cellPosition;
        }
    }

    public enum CellType
    {
        Walkable,
        NonWalkable
    }
}

