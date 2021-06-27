using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Utils;
using System;

namespace Didenko.BattleCity.Behaviors
{
    public class AttackableBehavior: MonoBehaviour, IOnPoolReturn
    {
        public Action<GameObject> DamageReceived;
        public int Health
        {
            get => health;
            set
            {
                if (value <= 0)
                {
                    if (gameObject.TryGetComponent(out PoolObjBehavior poolObjBehavior))
                        poolObjBehavior.ReturnToPool();
                    else
                        Destroy(gameObject);
                }
                else
                    health = value;
            }
        }
        private int health;

        public void ReceiveDamage(int damage, GameObject sender)
        {
            Health -= damage;
            DamageReceived?.Invoke(sender);
        }

        public void Init(int health)
        {
            this.health = health;
        }

        public void OnReturnToPool()
        {
            health = 0;
        }
    }
}
