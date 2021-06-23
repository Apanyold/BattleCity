using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Behaviors;
using Didenko.BattleCity.Utils;
using Didenko.BattleCity.MapScripts;

namespace Didenko.BattleCity.Controllers
{
    public class RootController : MonoBehaviour
    {
        [SerializeField]
        private ObjectPooler objectPooler;
        [SerializeField]
        private TextAsset gameData;
        [SerializeField]
        private BaseBehavior basePrefab;
        [SerializeField]
        private Map map;

        private Factory factory;
        private ConfigSetter configSetter;


        private void Awake()
        {
            map.InitMap();

            configSetter = new ConfigSetter();
            configSetter.DownloadDatas(gameData);

            factory = new Factory(objectPooler, configSetter);

            objectPooler.Init(factory);

            var player = factory.CreateObject(PoolObject.Tank, transform.position, Team.Red);
            player.AddComponent<PlayerTankController>().Init(factory);

            var blueTank = factory.CreateObject(PoolObject.Tank, transform.position + new Vector3(-2,2,0), Team.Blue);

            var redBase = Instantiate(basePrefab, transform.position + new Vector3(2, 2, 0), transform.rotation, objectPooler.GameZone);
            //redBase.Init(factory, Team.Red);
        }

        private void Start()
        {

        }
    }
}
