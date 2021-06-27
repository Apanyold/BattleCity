using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Didenko.BattleCity.Ai
{

    public interface IAiBehavior
    {
        AiType AiType { get; }
    }

    public enum AiType
    {
        Stormtrooper,
        Destroyer,
        Assistant
    }
}
