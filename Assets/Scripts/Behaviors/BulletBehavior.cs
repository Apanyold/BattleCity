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
        private int 
            damage,
            flyDistacne;

        private float
            timeToDestroy = 1f,
            timeTicker;
        private Vector3 startPosition;

        private GameObject owner;
        public void Init(int damage, GameObject owner, int flyDistacne)
        {
            timeTicker = Time.time;
            this.damage = damage;
            this.owner = owner;
            this.flyDistacne = flyDistacne;

            startPosition = transform.position;

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

                if(Vector3.Distance(startPosition, transform.position)/2 >= flyDistacne)
                    gameObject.GetComponent<PoolObjBehavior>().ReturnToPool();
            }
        }

        //private void OnCollisionEnter2D(Collision2D collision)
        //{
        //    var go = collision.gameObject;
        //    if (go != owner)
        //    {
        //        if (isPenetrated && go.TryGetComponent(out AttackableBehavior attackableBehavior))
        //            attackableBehavior.Health -= damage;

        //        gameObject.GetComponent<PoolObjBehavior>().ReturnToPool();
        //    }
        //}

        private void OnTriggerEnter2D(Collider2D collision)
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
