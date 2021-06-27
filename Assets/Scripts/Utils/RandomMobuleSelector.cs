using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Behaviors;

namespace Didenko.BattleCity.Utils
{
    public static class RandomMobuleSelector
    {
        private static Dictionary<DataType, int> lvlDic = new Dictionary<DataType, int>();

        public static int ChooseRandomly(int maxLvl, DataType dataType)
        {
            if (!lvlDic.ContainsKey(dataType))
                lvlDic.Add(dataType, 0);
            else
                ++lvlDic[dataType];

            if (lvlDic[dataType] > maxLvl)
            {
                lvlDic[dataType] = 0;
                return maxLvl;
            }
            var randomInt = Random.Range(1, maxLvl + 1);

            if (randomInt == maxLvl)
                if (lvlDic[dataType] > 0)
                    --lvlDic[dataType];

            return randomInt;
        }
    }

}
