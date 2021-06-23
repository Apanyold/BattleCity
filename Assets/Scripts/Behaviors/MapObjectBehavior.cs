using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Didenko.BattleCity.Behaviors
{
    public class MapObjectBehavior : MonoBehaviour
    {
        public Vector2Int[] SizeInfo => sizeInfo;
        public Vector2Int[] PositionsOccupied { get; set; }

        [SerializeField]
        private Vector2Int[] sizeInfo;
    }
}

