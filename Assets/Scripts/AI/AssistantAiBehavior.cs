using Didenko.BattleCity.Behaviors;
using Didenko.BattleCity.MapScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Didenko.BattleCity.Ai
{
    public class AssistantAiBehavior : AiController
    {
        public override AiType AiType => AiType.Assistant;
        private bool isDestroy = false;
        private GameObject friend;

        private void Start()
        {
            TargetShootDestroyed = SelectNewShootTarget;
            TargetMoveDestroyed = SelectFriendTarget;
        }

        public override void Init(TankBehavior tankBehavior, Pathfinder pathfinder, Map map, AiManager aiManager)
        {
            isDestroy = false;
            base.Init(tankBehavior, pathfinder, map, aiManager);

            SelectFriendTarget();

            tankBehavior.OnPullReturned += _ => { if (!isDestroy) Destroy(gameObject); };
        }

        private void SelectNewShootTarget()
        {
            if (isDestroy)
                return;

            SetTargetToShoot(aiManager.GetEnemyTank(Team));
        }

        private void SelectFriendTarget()
        {
            if (isDestroy)
                return;

            friend = aiManager.GetFriendlyTank(Team, tankBehavior);

            friend.GetComponent<AttackableBehavior>().DamageReceived += FriendlyAttacked;

            SetTargetToMove(friend);
        }

        private void FriendlyAttacked(GameObject enemy)
        {
            if (isDestroy)
                return;
            SetTargetToMove(enemy);
            SetTargetToShoot(enemy);
        }

        private void OnDestroy()
        {
            friend.GetComponent<AttackableBehavior>().DamageReceived -= FriendlyAttacked;
            TargetShootDestroyed = null;
            TargetMoveDestroyed = null;
            isDestroy = true;
        }
    }
}
