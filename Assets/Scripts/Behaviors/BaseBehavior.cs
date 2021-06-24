using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Utils;
using Didenko.BattleCity.MapScripts;

namespace Didenko.BattleCity.Behaviors
{
    public class BaseBehavior : SpriteLoader
    {
        //TODO let base find free positions

        [SerializeField]
        private AttackableBehavior attackableBehavior;
        [SerializeField]
        private TeamBehavior teamBehavior;

        [SerializeField]
        private int tankCreationLimit;
        [SerializeField]
        private int maxAliveTanks;

        private Factory factory;

        private List<TankBehavior> activeTanks = new List<TankBehavior>();

        private Vector2Int[] spawnPoints;
        private List<Vector2Int> availablePoints = new List<Vector2Int>();
        private int pointTicker;

        public void Init(Factory factory, Team team, Map map)
        {
            this.factory = factory;
            teamBehavior.SetTeam(team);

            LoadSpriteForTeam("Flag" , teamBehavior.Team);

            attackableBehavior.Init(10);

            spawnPoints = new Vector2Int[4];
            spawnPoints[0] = new Vector2Int(0, 1);
            spawnPoints[1] = new Vector2Int(0, -1);
            spawnPoints[2] = new Vector2Int(1, 0);
            spawnPoints[3] = new Vector2Int(-1, 0);

            var baseCell = map.RealPosToCell(transform.position);
            foreach (var item in spawnPoints)
            {
                if (map.CheckPosition(baseCell + item))
                    availablePoints.Add(item);
            }
            pointTicker = 0;
            TryCreateTanks();
        }

        private void TryCreateTanks()
        {
            if (activeTanks.Count >= maxAliveTanks || tankCreationLimit <= 0) 
                return;

            pointTicker = pointTicker < availablePoints.Count - 1 ? pointTicker += 1: 0;
            try
            {
                var pos = new Vector3(availablePoints[pointTicker].x * 2, availablePoints[pointTicker].y * 2, transform.position.z);
                var tank = factory.CreateObject(PoolObject.Tank, transform.position + pos, teamBehavior.Team).GetComponent<TankBehavior>();

                tank.OnPullReturned += OnTankCollectionChanged;
                activeTanks.Add(tank);

                tankCreationLimit--;

                if (activeTanks.Count < maxAliveTanks)
                    TryCreateTanks();
            }
            catch
            {

            }
            

        }

        private void OnTankCollectionChanged(TankBehavior tankBehavior)
        {
            activeTanks.Remove(tankBehavior);
            TryCreateTanks();
        }
    }
}
