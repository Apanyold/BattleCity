using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Utils;

namespace Didenko.BattleCity.Behaviors
{
    public class CannonBehavior : MonoBehaviour
    {
        public float Accuracity => accuracity;
        public int Damage => damage;

        [SerializeField]
        private TeamBehavior teamBehavior;
        [SerializeField]
        private Transform fireTransfom;

        private float accuracity;
        private int damage;
        private Factory factory;

        public void Init(Factory factory)
        {
            this.factory = factory;
        }

        public void Fire()
        {
            var go = factory.CreateObject(PoolObject.Bullet, fireTransfom.position, teamBehavior.Team, transform.rotation);
            var bullet = go.GetComponent<BulletBehavior>();
            bullet.Init(damage, gameObject);
        }
    }
}
