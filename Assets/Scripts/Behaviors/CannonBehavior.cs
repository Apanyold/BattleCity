using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Didenko.BattleCity.Utils;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq;
using System.Runtime.Serialization;

namespace Didenko.BattleCity.Behaviors
{
    public class CannonBehavior : SpriteLoader, IConfigurable, ISetupable, IModuleDrop
    {
        public event Action<BulletBehavior> OnFired;
        public int Damage => damage;

        public DataType DataType => DataType.cannonDatas;

        [SerializeField]
        private TeamBehavior teamBehavior;
        [SerializeField]
        private List<Transform> fireTransfoms;

        private int 
            damage,
            flyDisnatce;

        private Factory factory;
        private List<CannonData> cannonDatas = new List<CannonData>();
        private CannonData currentData;

        public void Init(Factory factory)
        {
            this.factory = factory;
        }

        public void Setup(SetupData setupData)
        {
            if (setupData.setupType != SetupType.Cannon)
                return;
            currentData = cannonDatas.Find(x => x.cannonType == setupData.cannonType && x.lvl == setupData.lvl);

            damage = currentData.damage;
            flyDisnatce = currentData.flyDisnatce;

            Debug.Log($"Cannon {currentData.spriteName}, {teamBehavior.Team}");
            LoadSpriteForTeam(currentData.spriteName, teamBehavior.Team);
        }

        public void Fire()
        {
            for (int i = 0; i < (int)currentData.cannonType; i++)
            {
                var go = factory.CreateObject(PoolObject.Bullet, fireTransfoms[i].position, teamBehavior.Team, transform.rotation);
                var bullet = go.GetComponent<BulletBehavior>();

                OnFired?.Invoke(bullet);
                bullet.Init(damage, gameObject, flyDisnatce);
            }
        }

        public void SetConfings(string data)
        {
            cannonDatas = JsonConvert.DeserializeObject<List<CannonData>>(data);
        }

        public DropData DropModule()
        {
            var data = new DropData(currentData.spriteName, currentData.lvl, SetupType.Cannon, currentData.cannonType);
            return data;
        }

        public void InitSetup()
        {
            var array = Enum.GetValues(typeof(CannonType));
            var cannonType = (CannonType)array.GetValue(UnityEngine.Random.Range(0, array.Length - 1));

            var maxLvl = cannonDatas.FindAll(x => x.cannonType == cannonType).Count();
            int lvl = UnityEngine.Random.Range(1, maxLvl + 1);

            Setup(new SetupData(lvl, SetupType.Cannon, cannonType));
        }
    }

    public struct CannonData
    {
        public int damage;
        public CannonType cannonType;
        public string spriteName;
        public int flyDisnatce;
        public int lvl;
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum CannonType 
    {
        [EnumMember(Value = "FC")]
        FC = 2,
        [EnumMember(Value = "PC")]
        PC = 1,
        [EnumMember(Value = "None")]
        None
    }
}
