using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Behaviors;
using Didenko.BattleCity.Utils;
using UnityEngine.SceneManagement;
using System;

namespace Didenko.BattleCity.Controllers 
{
    public class PlayerTankController : MonoBehaviour
    {
        private MoveBehavior moveBehavior;
        private CannonBehavior cannonBehavior;
        private TankBehavior tankBehavior;

        private int 
            horizontal,
            vertical;
        private bool canBePicked;

        public void Init(Factory factory)
        {
            moveBehavior = GetComponent<MoveBehavior>();
            cannonBehavior = GetComponent<CannonBehavior>();
            tankBehavior = GetComponent<TankBehavior>();

            tankBehavior.CanBeCollected += UpdateDropState;

        }

        public void UpdateDropState(bool isCollectable)
        {
            canBePicked = isCollectable;
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

            if (Input.GetKeyDown(KeyCode.Z) && canBePicked)
                tankBehavior.PickUp();
            else if (Input.GetKeyDown(KeyCode.M) && canBePicked)
                tankBehavior.DestroyDrop();

            Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        }
    }
}

