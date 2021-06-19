using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Behaviors;
using Didenko.BattleCity.Utils;

namespace Didenko.BattleCity.Controllers 
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private MoveBehavior moveBehavior;
        [SerializeField]
        private CannonBehavior cannonBehavior;

        private float 
            horizontal,
            vertical;

        public void Init(Factory factory)
        {
            cannonBehavior.Init(factory);
        }

        private void FixedUpdate()
        {
            if (moveBehavior.IsMoving)
                return;

            if (horizontal != 0)
                moveBehavior.MoveHorizontal(horizontal);
            else if (vertical != 0 )
                moveBehavior.MoveVertical(vertical);
        }

        private void Update()
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(KeyCode.Space))
                cannonBehavior.Fire();
        }
    }
}

