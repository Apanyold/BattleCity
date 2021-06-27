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
        public DataType dataType;
        public CannonType cannonType;

        public DropData(string spriteName, int lvl, DataType dataType, CannonType cannonType)
        {
            this.spriteName = spriteName;
            this.lvl = lvl;
            this.dataType = dataType;
            this.cannonType = cannonType;
        }
    }
}