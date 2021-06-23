using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Utils;

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

        public void Init(Factory factory, Team team)
        {
            this.factory = factory;
            teamBehavior.SetTeam(team);

            LoadSpriteForTeam("Flag" , teamBehavior.Team);

            attackableBehavior.Init(10);

            TryCreateTanks();
        }

        private void TryCreateTanks()
        {
            if (activeTanks.Count >= maxAliveTanks || tankCreationLimit <= 0) 
                return;

            var tank = factory.CreateObject(PoolObject.Tank, transform.position + transform.up*2, teamBehavior.Team).GetComponent<TankBehavior>();

            tank.OnPullReturned += OnTankCollectionChanged;
            activeTanks.Add(tank);

            tankCreationLimit--;

            Debug.Log("tankCreationLimit: " + tankCreationLimit);

            if (activeTanks.Count < maxAliveTanks)
                TryCreateTanks();
        }

        private void OnTankCollectionChanged(TankBehavior tankBehavior)
        {
            activeTanks.Remove(tankBehavior);
            TryCreateTanks();
        }
    }
}
