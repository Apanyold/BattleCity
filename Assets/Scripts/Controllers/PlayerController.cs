using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Behaviors;
using Didenko.BattleCity.Utils;
using UnityEngine.SceneManagement;
using System;

namespace Didenko.BattleCity.Controllers 
{
    public class PlayerController : MonoBehaviour
    {
        public Action OnPlayerTankDestroyed;

        private MoveBehavior moveBehavior;
        private CannonBehavior cannonBehavior;
        public TankBehavior tankBehavior;

        private int 
            horizontal,
            vertical;
        private bool canBePicked;

        public void SetTank(TankBehavior tankBehavior)
        {
            tankBehavior.isControllerAttached = true;

            moveBehavior = tankBehavior.GetComponent<MoveBehavior>();
            cannonBehavior = tankBehavior.GetComponent<CannonBehavior>();
            this.tankBehavior = tankBehavior;

            this.tankBehavior.CanBeCollected += UpdateDropState;
            this.tankBehavior.OnPullReturned += OnTankPoolReturned;
        }

        public void OnTankPoolReturned(TankBehavior tankBehavior)
        {
            tankBehavior.CanBeCollected -= UpdateDropState;
            tankBehavior.OnPullReturned -= OnTankPoolReturned;

            OnPlayerTankDestroyed?.Invoke();
        }

        public void UpdateDropState(bool isCollectable, DroppedModuleBehavior droppedModuleBehavior)
        {
            canBePicked = isCollectable;
        }

        private void FixedUpdate()
        {
            if (!tankBehavior)
                return;

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

            if (!tankBehavior)
                return;

            if (Input.GetKeyDown(KeyCode.Space))
                cannonBehavior.Fire();

            if(Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            if (Input.GetKeyDown(KeyCode.Z) && canBePicked)
                tankBehavior.PickUp();
            else if (Input.GetKeyDown(KeyCode.M) && canBePicked)
                tankBehavior.DestroyDrop();

            Camera.main.transform.position = new Vector3(tankBehavior.transform.position.x, tankBehavior.transform.position.y, Camera.main.transform.position.z);
        }
    }
}

