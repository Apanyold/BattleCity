using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Utils;

namespace Didenko.BattleCity.Behaviors
{
    public class TowerBehavior : SpriteLoader, IConfigurable, ISetupable, IModuleDrop
    {
        [SerializeField]
        private CannonBehavior cannonBehavior;
        [SerializeField]
        private TeamBehavior teamBehavior;
        [SerializeField]
        private SpriteRenderer towerSprite;

        private float penetrationChance = 75;

        public DataType DataType => throw new System.NotImplementedException();

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

        public void SetConfings(string data)
        {
            throw new System.NotImplementedException();
        }

        public void Setup(SetupData setupData)
        {
            throw new System.NotImplementedException();
        }

        public void InitSetup()
        {
            throw new System.NotImplementedException();
        }

        public SetupData DropModule()
        {
            throw new System.NotImplementedException();
        }
    }

    public struct TowerData
    {
        public float penetrationChance;
        public string spriteName;
        public int lvl;
    }
}
