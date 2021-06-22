﻿using System.Collections;
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
        [SerializeField]
        private TextAsset gameData;

        private Factory factory;
        private ConfigSetter configSetter;

        private void Awake()
        {
            configSetter = new ConfigSetter();
            configSetter.DownloadDatas(gameData);

            factory = new Factory(objectPooler, configSetter);
            factory.CreateObject(PoolObject.Tank, transform.position, Team.Red);
        }

        private void Start()
        {
            var player = FindObjectOfType<PlayerController>();
            player.Init(factory);
        }
    }
}
