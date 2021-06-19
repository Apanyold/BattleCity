using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Didenko.BattleCity.Utils
{
    public class ObjectPooler : MonoBehaviour
    {
        [SerializeField]
        private Transform gameZone;
        private Dictionary<PoolObject, Queue<GameObject>> PoolDictionary = new Dictionary<PoolObject, Queue<GameObject>>();

        public GameObject GetFromPool(PoolObject poolObject)
        {
            string prefabPath = "PoolableObjects/" + poolObject.ToString();
            GameObject gameObject = null;

            if (PoolDictionary[poolObject].Count == 0)
            {
                gameObject = Instantiate(Resources.Load<GameObject>(prefabPath));
                gameObject.name = $"{poolObject}_id{ Guid.NewGuid()}";
            }
            else
            {
                gameObject = PoolDictionary[poolObject].Dequeue();
                gameObject.SetActive(true);
                gameObject.name = $"{poolObject}_id{ Guid.NewGuid()}";
            }
            gameObject.transform.parent = gameZone;

            return gameObject;
        }

        public void ReturnToPool(GameObject gameObject)
        {
            gameObject.SetActive(false);
            gameObject.transform.parent = this.transform;

            var onRetun = gameObject.GetComponents<IOnPoolReturn>();
            foreach (var item in onRetun)
                item.OnReturnToPool();

            var index = gameObject.name.IndexOf("_id");
            var poolName = (PoolObject)Enum.Parse(typeof(PoolObject), gameObject.name.Remove(index));

            PoolDictionary[poolName].Enqueue(gameObject);
        }
    }

    public enum PoolObject
    {
        BlueTank,
        RedTank,
        Bullet,
    }
}
