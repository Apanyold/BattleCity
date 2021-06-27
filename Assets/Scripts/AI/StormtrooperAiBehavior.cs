using Didenko.BattleCity.Behaviors;
using Didenko.BattleCity.MapScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Didenko.BattleCity.Ai
{
    public class StormtrooperAiBehavior : AiController
    {
        public override AiType AiType => AiType.Stormtrooper;
        private bool isDestroy = false;

        private void Start()
        {
            TargetShootDestroyed = SelectNewShootTarget;
            TargetMoveDestroyed = SelectNewMoveTarget;
        }

        public override void Init(TankBehavior tankBehavior, Pathfinder pathfinder, Map map, AiManager aiManager)
        {
            isDestroy = false;
            base.Init(tankBehavior, pathfinder, map, aiManager);

            SelectNewShootTarget();
            SelectNewMoveTarget();

            UpdateIgnoreList(false, true);

            tankBehavior.OnPullReturned += _ => { if (!isDestroy) Destroy(gameObject); };
        }

        private void SelectNewShootTarget()
        {
            if (isDestroy)
                return;

            SetTargetToShoot(aiManager.GetEnemyTank(Team, gameObject));
        }

        private void SelectNewMoveTarget()
        {
            if (isDestroy)
                return;

            SetTargetToMove(aiManager.GetEnemyTank(Team, gameObject));
        }

        private void OnDestroy()
        {
            TargetShootDestroyed = null;
            TargetMoveDestroyed = null;
            isDestroy = true;
        }
    }
}
