using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Utils;
using System;

namespace Didenko.BattleCity.Behaviors
{
    public class CannonBehavior : MonoBehaviour
    {
        public event Action<BulletBehavior> OnFired;
        public int Damage => damage;

        [SerializeField]
        private TeamBehavior teamBehavior;
        [SerializeField]
        private List<Transform> fireTransfom;

        private int barrelsCount = 1;
        private int damage = 1;
        private Factory factory;

        public void Init(Factory factory)
        {
            this.factory = factory;
        }

        public void Setup(int barrelsCount)
        {
            this.barrelsCount = barrelsCount;
        }

        public void Fire()
        {
            for (int i = 0; i < barrelsCount; i++)
            {
                var go = factory.CreateObject(PoolObject.Bullet, fireTransfom[i].position, teamBehavior.Team, transform.rotation);
                var bullet = go.GetComponent<BulletBehavior>();

                OnFired?.Invoke(bullet);
                bullet.Init(damage, gameObject);
            }
        }
    }
}
