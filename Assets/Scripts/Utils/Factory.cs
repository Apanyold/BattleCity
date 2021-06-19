using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Behaviors;

namespace Didenko.BattleCity.Utils
{
    public class Factory : MonoBehaviour
    {
        private ObjectPooler objectPooler;
        public Factory(ObjectPooler objectPooler)
        {
            this.objectPooler = objectPooler;
        }

        public GameObject CreateObject(PoolObject poolObject, Vector3 position, Team team)
        {
            GameObject gameObject = null;
            gameObject = objectPooler.GetFromPool(poolObject);

            if (gameObject.TryGetComponent(out TeamBehavior teamBehavior))
                teamBehavior.SetTeam(team);
            else
                throw new System.Exception("Object dosent have TeamBehavior");

            gameObject.transform.position = position;

            return gameObject;
        }

        public GameObject CreateObject(PoolObject poolObject, Vector3 position, Team team, Quaternion rotation)
        {
            GameObject gameObject = null;

            gameObject = CreateObject(poolObject, position, team);
            gameObject.transform.rotation = rotation;

            return gameObject;
        }
    }
}
