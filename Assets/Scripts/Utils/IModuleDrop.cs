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

        public DropData(string spriteName, int lvl)
        {
            this.spriteName = spriteName;
            this.lvl = lvl;
        }
    }
}