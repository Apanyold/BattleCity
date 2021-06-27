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
using Didenko.BattleCity.Ui;

namespace Didenko.BattleCity.Controllers
{
    public class RootController : MonoBehaviour
    {
        public static int unityThread;
        public static TaskScheduler unityTaskScheduler;
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
        private AiManager aiManager;
        [SerializeField]
        private UiController uiController;

        private Factory factory;
        private ConfigSetter configSetter;
        private Pathfinder pathfinder;

        private Vector2Int
            selectedRed,
            selectedBlue;

        private List<IGameEnder> gameEnders = new List<IGameEnder>();

        private void Awake()
        {
            GamePauser.GameState = GameState.Play;

            unityThread = Thread.CurrentThread.ManagedThreadId;
            unityTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            map.InitMap();
            pathfinder = new Pathfinder(map);

            configSetter = new ConfigSetter();
            configSetter.DownloadDatas(gameData);

            factory = new Factory(objectPooler, configSetter);

            objectPooler.Init(factory);

            OnPathBuilded += OnPathEnded;
            PlaceBases();

            uiController.Init(playerController);
        }

        private void EndGameForTeam(Team team)
        {
            if(playerController.Team != team)
            {
                uiController.ShowPlayerWin();
            }
            else
            {
                uiController.ShowPlayerLose();
            }
        }

        private void PlaceBases()
        {
            var vector2Ints = map.GetBasePositions();
            selectedRed = vector2Ints[0][UnityEngine.Random.Range(0, vector2Ints[0].Count)];
            selectedBlue = vector2Ints[1][UnityEngine.Random.Range(0, vector2Ints[1].Count)];

            pathfinder.FidnPathAsync(selectedBlue, selectedRed, OnPathEnded);
        }

        private void OnPathEnded(Stack<Vector2Int> path)
        {
            if (path == null)
            {
                Debug.Log("Path between bases is null, reloading scene");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                return;
            }
            
            var redBase = Instantiate(basePrefab, new Vector3(selectedRed.x, selectedRed.y, objectPooler.GameZone.position.z), new Quaternion(0, 0, 90f, 0), objectPooler.GameZone);
            redBase.Init(factory, Team.Red, map);

            playerController.SetTank(redBase.ActiveTanks[0]);

            var blueBase = Instantiate(basePrefab, new Vector3(selectedBlue.x, selectedBlue.y, objectPooler.GameZone.position.z), transform.rotation, objectPooler.GameZone);
            blueBase.Init(factory, Team.Blue, map);

            var basesList = new List<BaseBehavior>();
            basesList.Add(redBase);
            basesList.Add(blueBase);

            aiManager.Init(map, basesList);

            gameEnders.Add(blueBase);
            gameEnders.Add(redBase);
            gameEnders.Add(playerController);

            foreach (var item in gameEnders)
            {
                item.EndGameForATeam += EndGameForTeam;
            }

        }
    }
}
