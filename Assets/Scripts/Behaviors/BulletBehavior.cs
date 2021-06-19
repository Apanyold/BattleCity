using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Utils;

namespace Didenko.BattleCity.Behaviors
{
    public class BulletBehavior : MonoBehaviour, IOnPoolReturn
    {
        public int Damage => damage;
        public float BulletSpeed => bulletSpeed;

        public bool isPenetrated;

        [SerializeField]
        private MoveBehavior moveBehavior;

        [SerializeField]
        private float bulletSpeed = 5;
        private int damage = 1;

        private float
            timeToDestroy = 1f,
            timeTicker;

        private GameObject owner;
        public void Init(int damage, GameObject owner)
        {
            timeTicker = Time.time;
            this.damage = damage;
            this.owner = owner;

            StartCoroutine(StartMove());
        }

        public void OnReturnToPool()
        {
            owner = null;
            damage = 0;
            timeTicker = 0f;

            StopCoroutine(StartMove());
        }

        private IEnumerator StartMove()
        {
            while (true)
            {
                yield return null;

                moveBehavior.MoveForward(bulletSpeed);

                if (Time.time - timeTicker >= timeToDestroy)
                    gameObject.GetComponent<PoolObjBehavior>().ReturnToPool();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var go = collision.gameObject;
            if (go != owner)
            {
                if (isPenetrated && go.TryGetComponent(out AttackableBehavior attackableBehavior))
                    attackableBehavior.Health -= damage;

                gameObject.GetComponent<PoolObjBehavior>().ReturnToPool();
            }
        }
    }
}
