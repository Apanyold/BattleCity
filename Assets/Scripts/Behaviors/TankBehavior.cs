using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Utils;
using System;

namespace Didenko.BattleCity.Behaviors
{
    public class TankBehavior : MonoBehaviour, IOnPoolReturn
    {
        public Action<TankBehavior> OnPullReturned;
        public Action<DroppedModuleBehavior> DropExited;
        public Action<DroppedModuleBehavior> DropEntered;
        public Action<bool> CanBeCollected;

        [SerializeField]
        private CannonBehavior cannonBehavior;
        [SerializeField]
        private DropBehavior dropBehavior;
        [SerializeField]
        private MoveBehavior moveBehavior;
        [SerializeField]
        private TowerBehavior towerBehavior;

        private DroppedModuleBehavior droppedModuleBehavior;

        public void Init(Factory factory)
        {
            cannonBehavior.Init(factory);
            dropBehavior.Init(factory);
            DropExited += OnDropEntered;

            cannonBehavior.OnFired += towerBehavior.CalculateChance;
        }

        public void SetupTank()
        {
            var componets = gameObject.GetComponents<ISetupable>();
            foreach (var item in componets)
                item.InitSetup();
        }

        public void SetupPickedModule(DropData dropData)
        {
            var componets = gameObject.GetComponents<ISetupable>();
            foreach (var item in componets)
            {
                item.Setup(new SetupData(dropData.lvl, dropData.setupType, dropData.cannonType));
            }
        }

        public void DropAction(bool isPickUp)
        {
            if (isPickUp)
                PickUp();
            else
                DestroyDrop();
        }

        public void PickUp()
        {
            SetupPickedModule(droppedModuleBehavior.PickUp());
            //droppedModuleBehavior.Destroy();
        }

        public void DestroyDrop()
        {
            droppedModuleBehavior.Destroy();
        }

        public void OnDropEntered(DroppedModuleBehavior droppedModuleBehavior)
        {
            this.droppedModuleBehavior = droppedModuleBehavior;
            CanBeCollected?.Invoke(true);
        }

        public void OnDropExited(DroppedModuleBehavior droppedModuleBehavior)
        {
            this.droppedModuleBehavior = null;
            CanBeCollected?.Invoke(false);
        }

        public void OnReturnToPool()
        {
            OnPullReturned?.Invoke(this);
        }
    }
}