using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Utils;
using System;

namespace Didenko.BattleCity.Behaviors
{
    public class TankBehavior : MonoBehaviour
    {
        [SerializeField]
        private CannonBehavior cannonBehavior;
        [SerializeField]
        private DropBehavior dropBehavior;
        [SerializeField]
        private MoveBehavior moveBehavior;
        [SerializeField]
        private TowerBehavior towerBehavior;

        public void Init(Factory factory)
        {
            cannonBehavior.Init(factory);
            dropBehavior.Init(factory);

        }

        public void SetupTank()
        {
            var componets = gameObject.GetComponents<ISetupable>();
            foreach (var item in componets)
                item.InitSetup();
        }
    }
}
