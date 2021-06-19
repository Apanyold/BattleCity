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

        [SerializeField]
        private MoveBehavior moveBehavior;

        [SerializeField]
        private float bulletSpeed = 5;
        private int damage;

        private float
            timeToDestroy = 0.5f,
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
            if(collision.gameObject != owner)
            {
                Debug.Log("collision with: " + collision.gameObject.name);
                gameObject.GetComponent<PoolObjBehavior>().ReturnToPool();
            }
        }
    }
}
