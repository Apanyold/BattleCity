using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Behaviors;
using Didenko.BattleCity.Utils;
using UnityEngine.SceneManagement;

namespace Didenko.BattleCity.Controllers 
{
    public class PlayerController : MonoBehaviour
    {
        private MoveBehavior moveBehavior;
        private CannonBehavior cannonBehavior;
        private TowerBehavior towerBehavior;

        private int 
            horizontal,
            vertical;

        public void Init(Factory factory)
        {
            moveBehavior = GetComponent<MoveBehavior>();
            cannonBehavior = GetComponent<CannonBehavior>();
            towerBehavior = GetComponent<TowerBehavior>();

            //cannonBehavior.Setup(new SetupData(1, SetupType.Cannon, CannonType.FC));
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
            horizontal = (int)Input.GetAxisRaw("Horizontal");
            vertical = (int)Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(KeyCode.Space))
                cannonBehavior.Fire();
            if(Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

