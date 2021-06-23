using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Utils;

namespace Didenko.BattleCity.Behaviors
{
    public class AttackableBehavior: MonoBehaviour, IOnPoolReturn
    {
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
                Debug.Log(health);
            }
        }
        private int health;

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
