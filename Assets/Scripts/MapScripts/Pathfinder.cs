using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Didenko.BattleCity.Controllers;

namespace Didenko.BattleCity.MapScripts
{
    public class Pathfinder : MonoBehaviour
    {
        private Map map;
        private int iterationLimit;

        public Pathfinder(Map map)
        {
            this.map = map;
        }

        public Stack<Vector2Int> FidnPath(Vector2Int startPosition, Vector2Int endposition)
        {
            var openedList = new List<Node>();
            var closedList = new List<Node>();
            var neighborsList = new List<Node>();

            if (!map.CheckPosition(startPosition))
            {
                Debug.Log("Start position occupied");
                return null;
            }

            if (!map.CheckPosition(endposition))
            {
                Debug.Log("End position occupied");
                return null;
            }

            Node startNode = new Node()
            {
                positon = startPosition,
                prevNode = null,

                pathLenght = 0,
                heuristicPathLength = GetHeuristicPathLength(startPosition, endposition),
            };

            iterationLimit = GetHeuristicPathLength(startPosition, endposition) * map.Size;

            openedList.Add(startNode);
            //TODO iterationLimit;
            while (openedList.Count > 0 && iterationLimit > 0)
            {
                if (iterationLimit <= 2)
                    Debug.Log("Iteration limit reached ");

                iterationLimit--;

                openedList.Sort();
                var currentNode = openedList[0];

                if (currentNode.positon == endposition)
                    return GetPath(currentNode);

                openedList.Remove(currentNode);
                closedList.Add(currentNode);

                SetNeighbors(out neighborsList, currentNode, endposition);
                foreach (var neighbourNode in neighborsList)
                {
                    if (closedList.Find(x => x.positon == neighbourNode.positon) != null)
                        continue;
                    Node openNode = null;

                    if (openedList.Count > 0 && openedList.Exists(x => x.positon == neighbourNode.positon))
                        openNode = openedList.Find(x => x.positon == neighbourNode.positon);

                    if (openNode == null)
                        openedList.Add(neighbourNode);
                    //TODO check distance
                    else if(openNode.pathLenght > neighbourNode.pathLenght)
                    {
                        openNode.prevNode = currentNode;
                        openNode.pathLenght = currentNode.pathLenght;
                    }
                }
            }

            return null;
        }

        public async void FidnPathAsync(Vector2Int startPosition, Vector2Int endposition, Action<Stack<Vector2Int>> callback)
        {
            await Task.Run(() => FidnPath(startPosition, endposition)).ContinueWith(value =>
            {
                callback?.Invoke(value.Result);
            },  RootController.unityTaskScheduler);
        }

        public Stack<Vector2Int> GetPath(Node node)
        {
            Stack<Vector2Int> vector2Ints = new Stack<Vector2Int>();
            var currentNode = node;
            while (currentNode != null)
            {
                vector2Ints.Push(currentNode.positon);
                currentNode = currentNode.prevNode;
            }
            return vector2Ints;
        }

        public void SetNeighbors(out List<Node> neighbors, Node currentNode, Vector2Int endPosition)
        {
            neighbors = new List<Node>();

            Vector2Int[] neighborsArray = new Vector2Int[4];
            neighborsArray[0] = new Vector2Int(currentNode.positon.x + 1, currentNode.positon.y);
            neighborsArray[1] = new Vector2Int(currentNode.positon.x -1, currentNode.positon.y);
            neighborsArray[2] = new Vector2Int(currentNode.positon.x, currentNode.positon.y + 1);
            neighborsArray[3] = new Vector2Int(currentNode.positon.x, currentNode.positon.y - 1);

            foreach (var item in neighborsArray)
            {
                if (!map.CheckPosition(item))
                    continue;

                var node = new Node()
                {
                    pathLenght = currentNode.pathLenght + 1,
                    heuristicPathLength = GetHeuristicPathLength(item, endPosition),
                    positon = item,
                    prevNode = currentNode
                };
                neighbors.Add(node);
            }
        }

        private int GetHeuristicPathLength(Vector2Int from, Vector2Int to)
        {
            return (int)Vector2Int.Distance(from, to);
        }
    }

    public class Node : IComparable<Node>
    {
        public int EstimateFullPathLength => pathLenght + heuristicPathLength;

        public int pathLenght;
        public int heuristicPathLength;

        public Vector2Int positon;
        public Node prevNode;

        public int CompareTo(Node other)
        {
            if (other == null)
                return 0;
            else
                return EstimateFullPathLength.CompareTo(other.EstimateFullPathLength);
        }
    }
}

