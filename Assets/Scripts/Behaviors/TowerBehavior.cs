using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Utils;

namespace Didenko.BattleCity.Behaviors
{
    public class TowerBehavior : MonoBehaviour, IOnPoolReturn
    {
        [SerializeField]
        private CannonBehavior cannonBehavior;
        [SerializeField]
        private TeamBehavior teamBehavior;
        [SerializeField]
        private SpriteRenderer towerSprite;

        private float penetrationChance = 75;

        public void Init(float penetrationChance)
        {
            this.penetrationChance = penetrationChance;
            cannonBehavior.OnFired += CalculateChance;
        }

        public void CalculateChance(BulletBehavior bulletBehavior)
        {
            var chance = Random.Range(0, 100);
            bool isPenetrated = penetrationChance >= chance;
            Debug.Log("isPenetrated" + isPenetrated);
            bulletBehavior.isPenetrated = isPenetrated;
        }

        public void OnReturnToPool()
        {
            cannonBehavior.OnFired -= CalculateChance;
        }
    }

    public struct TowerData
    {
        public float penetrationChance;
        public string spriteName;
        public int lvl;
    }
}
