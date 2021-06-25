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

            StartCoroutine(CheckFireDistance());
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
            CanBeCollected?.Invoke(true, droppedModuleBehavior);
        }

        public void OnDropExited(DroppedModuleBehavior droppedModuleBehavior)
        {
            this.droppedModuleBehavior = null;
            CanBeCollected?.Invoke(false, droppedModuleBehavior);
        }

        public void OnReturnToPool()
        {
            StopCoroutine(CheckFireDistance());
            OnPullReturned?.Invoke(this);
        }

        private IEnumerator CheckFireDistance()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);

                RaycastHit2D[] result = new RaycastHit2D[5];
                ContactFilter2D filter = new ContactFilter2D();

                int count = Physics2D.Raycast(transform.position, transform.up, filter, result, cannonBehavior.BulletFlyDistance * 3f);

                //for (int i = 0; i < count; i++)
                if(result.Length > 1)
                {
                    var hit = result[1];

                    if (!hit || hit.collider == null || hit.collider.gameObject == this.gameObject || hit.collider.gameObject == null)
                        continue;
                    if (!hit.collider.gameObject.TryGetComponent(out AttackableBehavior attackableBehavior))
                        continue;

                    Debug.DrawLine(transform.position, hit.transform.position, Color.red, 0.1f);
                }
            }
        }
    }
}