using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Didenko.BattleCity.Behaviors
{
    public class HullBehavior : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer hullSprite;
        [SerializeField]
        private AttackableBehavior attackableBehavior;
        [SerializeField]
        private TeamBehavior teamBehavior;
        [SerializeField]
        private MoveBehavior moveBehavior;

        private void Start()
        {

        }
    }

    public struct HullData
    {
        public int health;
        public float moveSpeed;
        public string spriteName;
        public int lvl;
    }
}
