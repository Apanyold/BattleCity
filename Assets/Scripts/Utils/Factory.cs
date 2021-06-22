using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Behaviors;

namespace Didenko.BattleCity.Utils
{
    public class Factory
    {
        private ObjectPooler objectPooler;
        private ConfigSetter configSetter;

        public Factory(ObjectPooler objectPooler, ConfigSetter configSetter)
        {
            this.objectPooler = objectPooler;
            this.configSetter = configSetter;
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

            configSetter.SetData(gameObject);

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
