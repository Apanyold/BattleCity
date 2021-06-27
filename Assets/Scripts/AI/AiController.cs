using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Behaviors;
using Didenko.BattleCity.MapScripts;
using System;
using Didenko.BattleCity.Utils;

namespace Didenko.BattleCity.Ai
{
    public abstract class AiController : MonoBehaviour, IAiBehavior
    {
        public abstract AiType AiType { get; }
        public Team Team => team;

        public Action TargetShootDestroyed;
        public Action TargetMoveDestroyed;

        [SerializeField]
        private GameObject targetToMove;
        [SerializeField]
        private GameObject targetToShoot;
        [SerializeField]
        protected TankBehavior tankBehavior { get; private set; }

        protected AiManager aiManager;

        private MoveBehavior moveBehavior;
        private CannonBehavior cannonBehavior;
        private Pathfinder pathfinder;
        private Map map;

        private Stack<Vector2Int> movePath = new Stack<Vector2Int>();
        private Vector2Int currentPos;
        private Vector2Int pathPosition;
        private GameObject 
            prevShootTarget,
            prevMoveTarget;

        private Team team;
        private MoveDirection moveDirection;
        private bool canFire;

        private RaycastHit2D[] result;
        private ContactFilter2D filter = new ContactFilter2D();

        public virtual void Init(TankBehavior tankBehavior, Pathfinder pathfinder, Map map, AiManager aiManager)
        {
            tankBehavior.isControllerAttached = true;

            filter.useTriggers = true;

            canFire = false;
            moveBehavior = tankBehavior.GetComponent<MoveBehavior>();
            cannonBehavior = tankBehavior.GetComponent<CannonBehavior>();

            this.tankBehavior = tankBehavior;
            this.pathfinder = pathfinder;
            this.map = map;
            this.aiManager = aiManager;

            team = tankBehavior.Team;

            this.tankBehavior.CanBeCollected += UpdateDropState;
            this.tankBehavior.OnPullReturned += UnsubTank;
        }

        public void SetTargetToMove(GameObject targetToMove)
        {
            if (!gameObject || !gameObject.activeInHierarchy)
                return;

            if(prevMoveTarget)
                if (prevMoveTarget.TryGetComponent(out TankBehavior Tank))
                    Tank.OnPullReturned -= MoveTargedDestroyed;


            this.targetToMove = targetToMove;

            if (targetToMove.TryGetComponent(out TankBehavior targetTank))
                targetTank.OnPullReturned += MoveTargedDestroyed;

            prevMoveTarget = targetToMove;

            StartCoroutine(FindPath());
        }

        public void SetTargetToShoot(GameObject targetToShoot)
        {
            if (!gameObject || !gameObject.activeInHierarchy)
                return;

            if (prevShootTarget)
                if (prevShootTarget.TryGetComponent(out TankBehavior Tank))
                    Tank.OnPullReturned -= ShootTargedDestroyed;

            this.targetToShoot = targetToShoot;

            if (targetToShoot.TryGetComponent(out TankBehavior targetTank))
                targetTank.OnPullReturned += ShootTargedDestroyed;

            prevShootTarget = targetToShoot;
            StartCoroutine(ShootWithDelay());
        }

        private void UpdateDropState(bool isEntered, DroppedModuleBehavior drop)
        {
            if (!isEntered)
                return;

            var componets = tankBehavior.GetComponents<IConfigurable>();

            foreach (var item in componets)
            {
                if (item.DataType == drop.DropData.dataType)
                {
                    if (item.CurrentLvl < drop.DropData.lvl)
                        tankBehavior.DropAction(true);
                    else
                        tankBehavior.DropAction(false);
                }
            }
        }

        private void ShootTargedDestroyed(TankBehavior tankBehavior)
        {
            targetToShoot = null;

            TargetShootDestroyed?.Invoke();
        }
        private void MoveTargedDestroyed(TankBehavior tankBehavior)
        {
            targetToMove = null;

            TargetMoveDestroyed?.Invoke();
        }

        private void UnsubTank(TankBehavior tankBehavior)
        {
            tankBehavior.OnPullReturned -= UnsubTank;
            tankBehavior = null;
            team = Team.None;
            StopAllCoroutines();
        }

        private void BluildRoute(Stack<Vector2Int> path)
        {
            if (path == null || path.Count == 0)
                return;

            movePath = path;

            MoveNext();
        }

        private IEnumerator ShootWithDelay()
        {
            while (targetToShoot != null && tankBehavior != null)
            {
                yield return new WaitForSeconds(0.5f);

                result = new RaycastHit2D[2];

                Physics2D.Raycast(tankBehavior.transform.position, tankBehavior.transform.up, filter, result, cannonBehavior.BulletFlyDistance * 3f);

                var hit = result[1];

                if (!hit || hit.collider == null || hit.collider.gameObject == gameObject || hit.collider.gameObject == null)
                    continue;

                if (!hit.collider.gameObject.TryGetComponent(out AttackableBehavior a))
                    continue;

                if (hit.collider.gameObject.TryGetComponent(out TeamBehavior teamBehavior) && teamBehavior.Team == team)
                    continue;

                cannonBehavior.Fire();
            }
        }

        private void MoveNext()
        {
            if (movePath.Count == 0)
                return;

            var pop = movePath.Peek();
            pathPosition = pop;
        }

        private IEnumerator FindPath()
        {
            while (targetToMove != null && tankBehavior != null && targetToMove.activeInHierarchy)
            {
                var pos = map.RealPosToCell(tankBehavior.transform.position);
                var targetPos = map.RealPosToCell(targetToMove.transform.position);

                pathfinder.FidnPathAsync(pos, targetPos, BluildRoute);
                yield return new WaitForSeconds(0.5f);
            }
        }

        private void FixedUpdate()
        {
            if (targetToMove == null || tankBehavior == null)
                return;

            if(canFire)
                StartCoroutine(ShootWithDelay());

            if (movePath == null || movePath.Count == 0)
                return;

            ChoseDirection(pathPosition);

            if (moveDirection == MoveDirection.Left || moveDirection == MoveDirection.Right)
            {
                moveBehavior.MoveHorizontal((int)moveDirection);
            }
            else if (moveDirection == MoveDirection.Up || moveDirection == MoveDirection.Down)
            {
                moveBehavior.MoveVertical((int)moveDirection);
            }

            if(currentPos == pathPosition)
            {
                movePath.Pop();
                MoveNext();
            }
        }
        private void ChoseDirection(Vector2Int position)
        {
            int x = 0, y = 0;
            float xC = tankBehavior.transform.position.x;
            float yC = tankBehavior.transform.position.y;

            x = (int)xC;
            y = (int)yC;

            if (position.x < currentPos.x)
            {
                moveDirection = MoveDirection.Left;
                x = Mathf.CeilToInt(xC);
            }
            else if(position.x > currentPos.x)
            {
                moveDirection = MoveDirection.Right;
                x = Mathf.FloorToInt(xC);
            }
            else if(position.y < currentPos.y)
            {
                moveDirection = MoveDirection.Down;
                y = Mathf.CeilToInt(yC);
            }
            else if (position.y > currentPos.y)
            {
                moveDirection = MoveDirection.Up;
                y = Mathf.FloorToInt(yC);
            }
            currentPos = new Vector2Int(x, y);
        }
    }

    public enum MoveDirection
    {
        Left = -1,
        Right = 1,
        Up = 2,
        Down = -2
    }
}
