using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Utils;
using System;

namespace Didenko.BattleCity.Behaviors
{
    public class PoolObjBehavior : MonoBehaviour
    {
        public event Action<GameObject> returnToPoolCallback;
        public void ReturnToPool()
        {
            returnToPoolCallback?.Invoke(gameObject);
        }
    }
}
