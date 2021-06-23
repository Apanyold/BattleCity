using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Didenko.BattleCity.MapScripts
{
    public class Pathfinder : MonoBehaviour
    {
        private Map map;
        public Pathfinder(Map map)
        {
            this.map = map;
        }

        public Stack<Node> FidnPath(Vector2Int start, Vector2Int end)
        {
            var openedList = new List<Node>();
            var closedList = new List<Node>();
            //var neighbors = new 


            return null;
        }
    }

    public class Node
    {
        public Vector2Int positon;
        public bool walkable;
        public Node prevNode;
        public int cost;
        public int parentCost => prevNode.cost;
    }
}

