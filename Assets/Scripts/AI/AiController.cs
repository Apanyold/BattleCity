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
        private AttackableBehavior target;
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

            SetAiTarget(null);
        }

        public void SetAiTarget(AttackableBehavior attackableBehavior)
        {
            //target = attackableBehavior;

            var pos = map.RealPosToCell(tankBehavior.transform.position);
            pathfinder.FidnPathAsync(pos, new Vector2Int(Random.Range(0,40), Random.Range(0, 40)), BluildRoute);
            //pathfinder.FidnPathAsync(pos, new Vector2Int(19, 21), BluildRoute);
        }

        public void BluildRoute(Stack<Vector2Int> path)
        {
            movePath = path;
            Debug.Log(path.Count);
            var currentPos = map.RealPosToCell(tankBehavior.transform.position);

            //if (currentPos == path.Pop())
            //{
            //    //BluildRoute(path);
            //    //return;
            //}

            foreach (var item in path)
            {
                var mar = Instantiate(marker, new Vector3(item.x * 2, item.y * 2, 0), new Quaternion(0, 0, 0, 0), mapHolder.transform);
                //Destroy(mar, 1f);
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
        public IEnumerator MoveToPosition(Vector2Int position)
        {
            currentPos = map.RealPosToCell(tankBehavior.transform.position);
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
            var mar = Instantiate(markerRed, new Vector3(position.x * 2, position.y * 2, 0), new Quaternion(0, 0, 0, 0), mapHolder.transform);
            movePath.Pop();
            MoveNext();
        }

        private void ChoseDirection(Vector2Int position)
        {
            currentPos = map.RealPosToCell(tankBehavior.transform.position);

            if(position.x < currentPos.x)
            {
                moveDirection = MoveDirection.Left;
            }
            else if(position.x > currentPos.x)
            {
                moveDirection = MoveDirection.Right;
            }
            else if(position.y < currentPos.y)
            {
                moveDirection = MoveDirection.Down;
            }
            else if (position.y > currentPos.y)
            {
                moveDirection = MoveDirection.Up;
            }
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
