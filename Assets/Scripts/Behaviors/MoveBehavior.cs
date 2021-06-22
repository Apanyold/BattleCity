using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Didenko.BattleCity.Behaviors 
{
    public class MoveBehavior : MonoBehaviour
    {
        public float Speed { get => speed; set => speed = value; }
        public bool IsMoving => isMoving;

        private float speed = 3f;
        private bool isMoving = false;

        public void MoveHorizontal(int moveHorizontal)
        {
            //for some reason can't round the 90 and -90 degrees rotation
            float rotation;
            if (moveHorizontal > 0)
                rotation = -90f;
            else
                rotation = 90f;

            transform.eulerAngles = new Vector3Int(0, 0, 0);
            transform.localRotation = Quaternion.Euler(0, 0, rotation);

            MoveForward(speed);
            //Vector3 moveVector = Vector3.right * moveHorizontal * speed * Time.deltaTime;
            //transform.position += moveVector;
        }

        public void MoveVertical(int moveVertical)
        {
            Quaternion rotation;
            if (moveVertical < 0)
                rotation = Quaternion.Euler(0, 0, -180);
            else
                rotation = Quaternion.Euler(0, 0, 0);

            transform.rotation = rotation;

            MoveForward(speed);
            //Vector3 moveVector = Vector3.up * moveVertical * speed * Time.deltaTime;
            //transform.position += moveVector;
        }

        public void MoveForward(float speed)
        {
            Vector3 moveVector = transform.TransformDirection(Vector3.up) * speed * Time.deltaTime;
            
            var vec = transform.eulerAngles;
            vec.x = Mathf.Round(vec.x / 90) * 90;
            vec.y = Mathf.Round(vec.y / 90) * 90;
            vec.z = Mathf.Round(vec.z / 90) * 90;
            transform.eulerAngles = vec;

            transform.position += moveVector;
        }
    }
}
