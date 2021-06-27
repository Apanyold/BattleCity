using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.MapScripts;
using Didenko.BattleCity.Behaviors;

namespace Didenko.BattleCity.Ai
{

    public class AiManager : MonoBehaviour
    {
        [SerializeField]
        private AiController[] aiControllers;

        private Map map;

        private List<BaseBehavior> baseBehaviors = new List<BaseBehavior>();
        private Dictionary<AiController, TankBehavior> aiDic = new Dictionary<AiController, TankBehavior>();
        public void Init(Map map, List<BaseBehavior> baseBehaviors)
        {
            this.map = map;
            this.baseBehaviors = baseBehaviors;

            SetAiTanks();
        }

        public void SetAiTanks()
        {
            foreach(var baseBeh in baseBehaviors)
            {
                baseBeh.TankCreated += SetAiToTank;

                foreach (var tank in baseBeh.ActiveTanks)
                {
                    if (!tank.isControllerAttached)
                    {
                        SetAiToTank(tank);
                    }
                }
            }
        }

        public void SetAiToTank(TankBehavior tankBehavior)
        {
            AiController aiController;

            var controller = aiControllers[Random.Range(0, aiControllers.Length)];

            aiController = Instantiate(controller, tankBehavior.transform);

            var isIgnoreLastStep = false;
            if (controller.AiType == AiType.Assistant)
                isIgnoreLastStep = true;

            aiController.Init(tankBehavior, new Pathfinder(map, isIgnoreLastStep), map, this);

            tankBehavior.OnPullReturned += OnAiPoolRetured;
        }

        public GameObject GetEnemyBase(Team team)
        {
            var go = baseBehaviors.Find(x => x.Team != team).gameObject;
            return go;
        }

        public GameObject GetEnemyTank(Team team, GameObject selfTank)
        {
            //TODO return closest tank
            var enemyBase = baseBehaviors.Find(x => x.Team != team);

            GameObject go = null;

            var targetDistance = int.MaxValue;
            foreach (var item in enemyBase.ActiveTanks)
            {
                var x = (int)Vector3.Distance(item.transform.position, selfTank.transform.position);
                if (targetDistance > x)
                {
                    go = item.gameObject;
                    targetDistance = x;
                }
            }

            if (!go.activeInHierarchy)
                GetEnemyTank(team, selfTank);

            return go;
        }

        public GameObject GetFriendlyTank(Team team, TankBehavior selfTank)
        {
            var friendlyBase = baseBehaviors.Find(x => x.Team == team);

            var tankList = friendlyBase.ActiveTanks.FindAll(x => x != selfTank);

            GameObject go = null;
            var targetDistance = int.MaxValue;
            foreach (var item in tankList)
            {
                var x = (int)Vector3.Distance(item.transform.position, selfTank.transform.position);
                if (targetDistance > x)
                {
                    go = item.gameObject;
                    targetDistance = x;
                }
            }

            if (!go.activeInHierarchy)
                GetFriendlyTank(team, selfTank);

            return go;
        }

        private void OnAiPoolRetured(TankBehavior tankBehavior)
        {
            tankBehavior.OnPullReturned -= OnAiPoolRetured;
        }
    }
}
