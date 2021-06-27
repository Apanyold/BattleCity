using Didenko.BattleCity.Utils;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Didenko.BattleCity.Behaviors
{
    public class HullBehavior : SpriteLoader, IConfigurable, ISetupable, IModuleDrop
    {
        public DataType DataType => DataType.hullDatas;
        public int CurrentLvl { get => currentData.lvl; }

        [SerializeField]
        private AttackableBehavior attackableBehavior;
        [SerializeField]
        private TeamBehavior teamBehavior;
        [SerializeField]
        private MoveBehavior moveBehavior;

        private List<HullData> hullDatas = new List<HullData>();
        private HullData currentData;

        public DropData DropModule()
        {
            return new DropData(currentData.spriteName, currentData.lvl, DataType, CannonType.None);
        }

        public void InitSetup()
        {
            int lvl = UnityEngine.Random.Range(1, hullDatas.Count + 1);
            Setup(new SetupData(lvl, DataType, CannonType.None));
        }

        public void SetConfings(string data)
        {
           hullDatas = JsonConvert.DeserializeObject<List<HullData>>(data);
        }

        public void Setup(SetupData setupData)
        {
            if (setupData.dataType != DataType)
                return;

            currentData = hullDatas.Find(x => x.lvl == setupData.lvl);

            attackableBehavior.Init(currentData.health);
            moveBehavior.Speed = currentData.moveSpeed;

            LoadSpriteForTeam(currentData.spriteName, teamBehavior.Team);
        }
    }

    public struct HullData
    {
        public int health;
        public float moveSpeed;
        public string spriteName;
        public int lvl;
    }
}
