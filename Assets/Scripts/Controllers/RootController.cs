using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Behaviors;
using Didenko.BattleCity.Utils;
using Didenko.BattleCity.MapScripts;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;
using System.Threading;
using Didenko.BattleCity.Ai;

namespace Didenko.BattleCity.Controllers
{
    public class RootController : MonoBehaviour
    {
        public event Action<Stack<Vector2Int>> OnPathBuilded;

        [SerializeField]
        private ObjectPooler objectPooler;
        [SerializeField]
        private TextAsset gameData;
        [SerializeField]
        private BaseBehavior basePrefab;
        [SerializeField]
        private Map map;
        [SerializeField]
        private Transform mapHolder;
        [SerializeField]
        private PlayerController playerController;
        [SerializeField]
        private AiController aiController;
        [SerializeField]
        private AiManager aiManager;

        private Factory factory;
        private ConfigSetter configSetter;
        private Pathfinder pathfinder;

        private MapObjectBehavior marker;
        private Vector2Int
            selectedRed,
            selectedBlue;

        private System.Diagnostics.Stopwatch stopwatch;

        public static int unityThread;
        public static TaskScheduler unityTaskScheduler;


        private void Awake()
        {
            unityThread = Thread.CurrentThread.ManagedThreadId;
            unityTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            marker = Resources.Load<MapObjectBehavior>("Prefabs/MarkerBlue");

            map.InitMap();
            pathfinder = new Pathfinder(map);

            configSetter = new ConfigSetter();
            configSetter.DownloadDatas(gameData);

            factory = new Factory(objectPooler, configSetter);

            objectPooler.Init(factory);

            var vector2Ints = map.GetBasePositions();
            selectedRed = vector2Ints[0][UnityEngine.Random.Range(0, vector2Ints[0].Count)];
            selectedBlue = vector2Ints[1][UnityEngine.Random.Range(0, vector2Ints[1].Count)];

            OnPathBuilded += OnPathEnded;
            PlaceBases();
        }

        private void Start()
        {

        }

        private void PlaceBases()
        {
            stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            pathfinder.FidnPathAsync(selectedBlue, selectedRed, OnPathEnded);
        }

        private void OnPathEnded(Stack<Vector2Int> path)
        {
            stopwatch.Stop();

            if (path == null)
            {
                Debug.Log("Path between bases is null, reloading scene");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                return;
            }

            var redBase = Instantiate(basePrefab, new Vector3(selectedRed.x, selectedRed.y, objectPooler.GameZone.position.z), new Quaternion(0, 0, 90f, 0), objectPooler.GameZone);
            redBase.Init(factory, Team.Red, map);

            //playerController.SetTank(redBase.ActiveTanks[0]);

            var blueBase = Instantiate(basePrefab, new Vector3(selectedBlue.x, selectedBlue.y, objectPooler.GameZone.position.z), transform.rotation, objectPooler.GameZone);
            blueBase.Init(factory, Team.Blue, map);

            var basesList = new List<BaseBehavior>();
            basesList.Add(redBase);
            basesList.Add(blueBase);

            aiManager.Init(map, basesList);
        }
    }
}
