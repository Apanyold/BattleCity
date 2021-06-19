using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Behaviors;
using Didenko.BattleCity.Utils;

namespace Didenko.BattleCity.Controllers
{
    public class RootController : MonoBehaviour
    {
        [SerializeField]
        private ObjectPooler objectPooler;

        private Factory factory;

        private void Awake()
        {
            factory = new Factory(objectPooler);

            var player = FindObjectOfType<PlayerController>();
            player.Init(factory);
        }
    }
}
