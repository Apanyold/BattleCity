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

        [SerializeField]
        private AttackableBehavior attackableBehavior;
        [SerializeField]
        private TeamBehavior teamBehavior;
        [SerializeField]
        private MoveBehavior moveBehavior;

        private List<HullData> hullData = new List<HullData>();
        private HullData currentData;


        public SetupData DropModule()
        {
            throw new System.NotImplementedException();
        }

        public void InitSetup()
        {
            int lvl = UnityEngine.Random.Range(0, hullData.Count - 1);
            Setup(new SetupData(lvl, SetupType.Hull, CannonType.None, ""));
        }

        public void SetConfings(string data)
        {
           hullData = JsonConvert.DeserializeObject<List<HullData>>(data);
        }

        public void Setup(SetupData setupData)
        {
            if (setupData.setupType != SetupType.Hull)
                return;
            currentData = hullData.Find(x => x.lvl == setupData.lvl);

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
