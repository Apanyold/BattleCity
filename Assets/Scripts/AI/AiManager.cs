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
            var aiController = Instantiate(aiControllers[Random.Range(0, aiControllers.Length)], tankBehavior.transform);

            aiController.Init(tankBehavior, new Pathfinder(map), map, this);

            tankBehavior.OnPullReturned += OnAiPoolRetured;
        }

        public GameObject GetEnemyBase(Team team)
        {
            var go = baseBehaviors.Find(x => x.Team != team).gameObject;
            return go;
        }

        public GameObject GetEnemyTank(Team team)
        {
            var enemyBase = baseBehaviors.Find(x => x.Team != team);

            var go = enemyBase.ActiveTanks[Random.Range(0, enemyBase.ActiveTanks.Count)].gameObject;

            if (!go.activeInHierarchy)
                GetEnemyTank(team);

            return go;
        }

        public GameObject GetFriendlyTank(Team team, TankBehavior selfTank)
        {
            var friendlyBase = baseBehaviors.Find(x => x.Team == team);

            var tankList = friendlyBase.ActiveTanks.FindAll(x => x != selfTank);
            var go = tankList[Random.Range(0, tankList.Count)].gameObject;

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
