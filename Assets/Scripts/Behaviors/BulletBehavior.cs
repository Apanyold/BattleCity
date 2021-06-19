using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Utils;

namespace Didenko.BattleCity.Behaviors
{
    public class BulletBehavior : MonoBehaviour, IOnPoolReturn
    {
        public int Damage => damage;
        public int Speed => speed;

        [SerializeField]
        private MoveBehavior moveBehavior;
        private Collider2D collider2D;

        private int damage;
        private int speed = 5;

        private void Start()
        {
            moveBehavior.Speed = speed;

            StartCoroutine(StartMove());
        }
        public void Init(int damage, int speed)
        {
            StartCoroutine(StartMove());
        }

        public void OnReturnToPool()
        {
            StopCoroutine(StartMove());
        }

        private IEnumerator StartMove()
        {
            while (true)
            {
                yield return null;

                moveBehavior.MoveForward();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnReturnToPool();
        }
    }
}
