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

            marker = Resources.Load<Behaviors.MapObjectBehavior>("Prefabs/MarkerBlue");

            map.InitMap();
            pathfinder = new Pathfinder(map);

            configSetter = new ConfigSetter();
            configSetter.DownloadDatas(gameData);

            factory = new Factory(objectPooler, configSetter);

            objectPooler.Init(factory);

            var player = factory.CreateObject(PoolObject.Tank, transform.position + new Vector3(40,40,0), Team.Red);
            player.AddComponent<PlayerTankController>().Init(factory);

            var redBase = Instantiate(basePrefab, transform.position + new Vector3(42, 42, 0), new Quaternion(0, 0, 90f, 0), objectPooler.GameZone);
            redBase.Init(factory, Team.Red, map);

            //var blueTank = factory.CreateObject(PoolObject.Tank, transform.position + new Vector3(-2,2,0), Team.Blue);

            OnPathBuilded += OnPathEnded;
            PlaceBases();

            //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            //stopwatch.Start();
            //var path = pathfinder.FidnPath(new Vector2Int(2, 2), new Vector2Int(38,38));
            //stopwatch.Stop();
            //Debug.Log(stopwatch.ElapsedMilliseconds);

            //if(path != null)
            //    foreach (var item in path)
            //    {
            //        Instantiate(marker, new Vector3(item.x * 2, item.y * 2, 0), new Quaternion(0, 0, 0, 0), mapHolder);
            //    }
        }

        private async void PlaceBases()
        {
            var vector2Ints = map.GetBasePositions();

            selectedRed = vector2Ints[0][UnityEngine.Random.Range(0, vector2Ints[0].Count)];
            selectedBlue = vector2Ints[1][UnityEngine.Random.Range(0, vector2Ints[1].Count)];

            stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            await Task.Run(() => pathfinder.FidnPath(selectedRed, selectedBlue)).ContinueWith(value =>
            {
                OnPathBuilded?.Invoke(value.Result);
            }, unityTaskScheduler);
        }

        private void OnPathEnded(Stack<Vector2Int> path)
        {
            stopwatch.Stop();
            Debug.Log(stopwatch.ElapsedMilliseconds);

            if (path == null)
            {
                Debug.Log("Reload");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            var redBase = Instantiate(basePrefab, new Vector3(selectedRed.x * 2, selectedRed.y * 2, objectPooler.GameZone.position.z), new Quaternion(0, 0, 90f, 0), objectPooler.GameZone);
            redBase.Init(factory, Team.Red, map);

            var blueBase = Instantiate(basePrefab, new Vector3(selectedBlue.x * 2, selectedBlue.y * 2, objectPooler.GameZone.position.z), transform.rotation, objectPooler.GameZone);
            blueBase.Init(factory, Team.Blue, map);
        }

        private void Start()
        {

        }
    }
}
