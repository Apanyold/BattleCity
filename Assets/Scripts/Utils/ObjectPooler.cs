using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Behaviors;

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

            if (!PoolDictionary.ContainsKey(poolObject))
                PoolDictionary[poolObject] = new Queue<GameObject>();

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

            if (gameObject.TryGetComponent(out PoolObjBehavior poolObjBehavior))
                poolObjBehavior.returnToPoolCallback += ReturnToPool;
            else
            {
                var beh = gameObject.AddComponent<PoolObjBehavior>();
                beh.returnToPoolCallback += ReturnToPool;
            }

            gameObject.transform.parent = gameZone;

            return gameObject;
        }

        public void ReturnToPool(GameObject gameObject)
        {
            gameObject.SetActive(false);
            gameObject.transform.parent = transform;
            gameObject.GetComponent<PoolObjBehavior>().returnToPoolCallback -= ReturnToPool;

            var onRetun = gameObject.GetComponents<IOnPoolReturn>();
            foreach (var item in onRetun)
                item.OnReturnToPool();

            var index = gameObject.name.IndexOf("_id");
            var name = gameObject.name.Remove(index);
            var poolName = (PoolObject)Enum.Parse(typeof(PoolObject), name);

            PoolDictionary[poolName].Enqueue(gameObject);
        }
    }

    public enum PoolObject
    {
        Tank,
        Bullet,
        DroppedModule,
    }
}
