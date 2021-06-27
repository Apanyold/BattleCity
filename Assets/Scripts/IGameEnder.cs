using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Didenko.BattleCity
{
    public interface IGameEnder
    {
        Action<Team> EndGameForATeam { get; set; }
    }
}
