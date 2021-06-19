using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Didenko.BattleCity.Behaviors
{
    public class TeamBehavior : MonoBehaviour
    {
        public Team Team => team;
        private Team team;

        public void SetTeam(Team team)
        {
            this.team = team;
        }
    }
}
