using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Utils;
using System;
using Didenko.BattleCity.MapScripts;

namespace Didenko.BattleCity.Behaviors
{
    public class TankBehavior : MonoBehaviour, IOnPoolReturn
    {
        public bool isControllerAttached;
        public Team Team => teamBehavior.Team;

        public Action<TankBehavior> OnPullReturned;
        public Action<DroppedModuleBehavior> DropExited;
        public Action<DroppedModuleBehavior> DropEntered;
        public Action<bool, DroppedModuleBehavior> CanBeCollected;

        [SerializeField]
        private CannonBehavior cannonBehavior;
        [SerializeField]
        private DropBehavior dropBehavior;
        [SerializeField]
        private MoveBehavior moveBehavior;
        [SerializeField]
        private TowerBehavior towerBehavior;
        [SerializeField]
        private TeamBehavior teamBehavior;

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
                item.Setup(new SetupData(dropData.lvl, dropData.dataType, dropData.cannonType));
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
        }

        public void DestroyDrop()
        {
            if(droppedModuleBehavior)
                droppedModuleBehavior.Destroy();
        }

        public void OnDropEntered(DroppedModuleBehavior droppedModuleBehavior)
        {
            this.droppedModuleBehavior = droppedModuleBehavior;
            CanBeCollected?.Invoke(true, droppedModuleBehavior);
        }

        public void OnDropExited(DroppedModuleBehavior droppedModuleBehavior)
        {
            this.droppedModuleBehavior = null;
            CanBeCollected?.Invoke(false, droppedModuleBehavior);
        }

        public void OnReturnToPool()
        {
            OnPullReturned?.Invoke(this);
            isControllerAttached = false;
        }
    }
}