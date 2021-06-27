using Didenko.BattleCity.Behaviors;
using Didenko.BattleCity.MapScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Didenko.BattleCity.Ai
{

    public class DestroyerAiBehavior : AiController
    {
        public override AiType AiType => AiType.Destroyer;

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

            UpdateIgnoreList(true, false);

            tankBehavior.OnPullReturned += _ => { if (!isDestroy) Destroy(gameObject); };
        }

        private void SelectNewShootTarget()
        {
            if (isDestroy)
                return;

            SetTargetToShoot(aiManager.GetEnemyBase(Team));
        }

        private void SelectNewMoveTarget()
        {
            if (isDestroy)
                return;

            SetTargetToMove(aiManager.GetEnemyBase(Team));
        }

        private void OnDestroy()
        {
            TargetShootDestroyed = null;
            TargetMoveDestroyed = null;
            isDestroy = true;
        }
    }
}
