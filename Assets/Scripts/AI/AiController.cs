using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Behaviors;
using Didenko.BattleCity.MapScripts;

namespace Didenko.BattleCity.Ai
{
    public class AiController : MonoBehaviour
    {
        [SerializeField]
        GameObject 
            mapHolder,
            marker,
            markerRed;

        private MoveBehavior moveBehavior;
        private CannonBehavior cannonBehavior;
        private TankBehavior tankBehavior;
        private GameObject target;
        private Pathfinder pathfinder;
        private Map map;

        private Stack<Vector2Int> movePath = new Stack<Vector2Int>();
        private Vector2Int currentPos;

        private MoveDirection moveDirection;

        public void SetTank(TankBehavior tankBehavior, Pathfinder pathfinder, Map map)
        {
            moveBehavior = tankBehavior.GetComponent<MoveBehavior>();
            cannonBehavior = tankBehavior.GetComponent<CannonBehavior>();
            this.tankBehavior = tankBehavior;
            this.pathfinder = pathfinder;
            this.map = map;
        }

        public void SetTarget(GameObject target)
        {
            this.target = target;
            StartCoroutine(FindPath());
        }

        public void BluildRoute(Stack<Vector2Int> path)
        {
            StopCoroutine(MoveToPosition(Vector2Int.zero));
            movePath = path;

            var currentPos = map.RealPosToCell(tankBehavior.transform.position);

            foreach (var item in path)
            {
                var mar = Instantiate(marker, new Vector3(item.x, item.y, 0), new Quaternion(0, 0, 0, 0), mapHolder.transform);
                Destroy(mar, 1f);
            }

            MoveNext();
        }

        private void MoveNext()
        {
            if (movePath.Count == 0)
                return;

            var pop = movePath.Peek();
            StartCoroutine(MoveToPosition(pop));
        }

        public IEnumerator FindPath()
        {
            while (target != null && tankBehavior != null)
            {
                var pos = map.RealPosToCell(tankBehavior.transform.position);
                var targetPos = map.RealPosToCell(target.transform.position);

                pathfinder.FidnPathAsync(pos, targetPos, BluildRoute);
                yield return new WaitForSeconds(5);
            }
        }

        public IEnumerator MoveToPosition(Vector2Int position)
        {
            while (currentPos != position)
            {
                ChoseDirection(position);

                if (moveDirection == MoveDirection.Left || moveDirection == MoveDirection.Right)
                {
                    moveBehavior.MoveHorizontal((int)moveDirection);
                }
                else if(moveDirection == MoveDirection.Up || moveDirection == MoveDirection.Down)
                {
                    moveBehavior.MoveVertical((int)moveDirection);
                }
                yield return new WaitForEndOfFrame();
            }
            var mar = Instantiate(markerRed, new Vector3(position.x, position.y, 0), new Quaternion(0, 0, 0, 0), mapHolder.transform);
            Destroy(mar, 1f);

            if (movePath != null && movePath.Count >0)
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
