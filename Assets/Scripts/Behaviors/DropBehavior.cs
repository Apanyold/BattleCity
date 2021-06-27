using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Utils;

namespace Didenko.BattleCity.Behaviors
{
    public class DropBehavior : MonoBehaviour, IOnPoolReturn
    {
        [SerializeField]
        private TeamBehavior teamBehavior;
        private Factory factory;

        public void Init(Factory factory)
        {
            this.factory = factory;
        }

        public void DropModule()
        {

            var data = this.gameObject.GetComponent<CannonBehavior>().DropModule();
            //var modules = this.gameObject.GetComponents<IModuleDrop>();
            //var i = Random.Range(0, modules.Length);
            //var moduleData = modules[i].DropModule();

            var gameObject = factory.CreateObject(PoolObject.DroppedModule, transform.position, teamBehavior.Team);
            //gameObject.GetComponent<DroppedModuleBehavior>().Init(teamBehavior.Team, moduleData);
            gameObject.GetComponent<DroppedModuleBehavior>().Init(teamBehavior.Team, data);
        }

        public void OnReturnToPool()
        {
            DropModule();
        }
    }
}
