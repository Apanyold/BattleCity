using Didenko.BattleCity.Behaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Didenko.BattleCity.Utils
{
    public interface IModuleDrop
    {
        DropData DropModule();
    }

    public struct DropData
    {
        public string spriteName;
        public int lvl;
        public SetupType setupType;
        public CannonType cannonType;

        public DropData(string spriteName, int lvl, SetupType setupType, CannonType cannonType)
        {
            this.spriteName = spriteName;
            this.lvl = lvl;
            this.setupType = setupType;
            this.cannonType = cannonType;
        }
    }
}