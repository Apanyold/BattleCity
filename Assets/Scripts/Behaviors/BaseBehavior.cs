using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Utils;
using Didenko.BattleCity.MapScripts;
using System;

namespace Didenko.BattleCity.Behaviors
{
    public class BaseBehavior : SpriteLoader
    {
        public Action<TankBehavior> TankCreated;
        public Team Team => team;
        public List<TankBehavior> ActiveTanks => activeTanks;

        [SerializeField]
        private AttackableBehavior attackableBehavior;
        [SerializeField]
        private TeamBehavior teamBehavior;

        [SerializeField]
        private int tankCreationLimit;
        [SerializeField]
        private int maxAliveTanks;

        private Factory factory;

        [SerializeField]
        private List<TankBehavior> activeTanks = new List<TankBehavior>();

        private Vector2Int[] spawnPoints;
        private List<Vector2Int> availablePoints = new List<Vector2Int>();
        private int pointTicker;
        private Team team;

        public void Init(Factory factory, Team team, Map map)
        {
            this.team = team;
            this.factory = factory;
            teamBehavior.SetTeam(team);

            LoadSpriteForTeam("Flag" , teamBehavior.Team);

            attackableBehavior.Init(20);

            spawnPoints = new Vector2Int[4];
            spawnPoints[0] = new Vector2Int(0, Map.MapCellSize);
            spawnPoints[1] = new Vector2Int(0, -Map.MapCellSize);
            spawnPoints[2] = new Vector2Int(Map.MapCellSize, 0);
            spawnPoints[3] = new Vector2Int(-Map.MapCellSize, 0);

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
            if (availablePoints.Count == 0)
                return;

            pointTicker = pointTicker < availablePoints.Count - 1 ? pointTicker += 1: 0;

            var pos = new Vector3(availablePoints[pointTicker].x, availablePoints[pointTicker].y, transform.position.z);
            var tank = factory.CreateObject(PoolObject.Tank, transform.position + pos, teamBehavior.Team).GetComponent<TankBehavior>();

            tank.OnPullReturned += OnTankCollectionChanged;
            activeTanks.Add(tank);

            tankCreationLimit--;

            TankCreated?.Invoke(tank);

            Physics2D.IgnoreCollision(tank.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);

            if (activeTanks.Count < maxAliveTanks)
                TryCreateTanks();
        }

        private void OnTankCollectionChanged(TankBehavior tankBehavior)
        {
            Physics2D.IgnoreCollision(tankBehavior.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);

            activeTanks.Remove(tankBehavior);
            TryCreateTanks();
        }
    }
}
