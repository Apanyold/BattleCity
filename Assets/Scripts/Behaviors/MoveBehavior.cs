using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Didenko.BattleCity.Behaviors 
{
    public class MoveBehavior : MonoBehaviour
    {
        public float Speed { get => speed; set => speed = value; }
        public bool IsMoving => isMoving;

        [SerializeField]
        private Rigidbody2D rigidbody;

        private float speed = 3f;
        private bool isMoving = false;

        public void MoveHorizontal(float moveHorizontal)
        {
            var  rotation = Quaternion.Euler(0, 0, -moveHorizontal * 90f);
            transform.rotation = rotation;

            Vector3 moveVector = Vector3.right * moveHorizontal * speed * Time.deltaTime;
            transform.position += moveVector;
        }

        public void MoveVertical(float moveVertical)
        {
            Quaternion rotation;
            if (moveVertical < 0)
                rotation = Quaternion.Euler(0, 0, moveVertical * 180f);
            else
                rotation = Quaternion.Euler(0, 0, 0);

            transform.rotation = rotation;

            Vector3 moveVector = Vector3.up * moveVertical * speed * Time.deltaTime;
            transform.position += moveVector;
        }

        public void MoveForward(float speed)
        {
            Vector3 moveVector = rigidbody.transform.up * speed * Time.deltaTime;

            Debug.DrawLine(transform.position, rigidbody.transform.position + moveVector, Color.red, 0.5f);

            transform.position += moveVector;
        }
    }
}
