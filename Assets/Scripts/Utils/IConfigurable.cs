using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.ComponentModel;

namespace Didenko.BattleCity.Utils
{
    public interface IConfigurable
    {
        void SetConfings(string data);
        DataType DataType { get; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum DataType
    {
        [Description("cannonDatas")] cannonDatas,
        [Description("hullDatas")] hullDatas,
        [Description("towerDatas")] towerDatas
    }

    public struct TanksData
    {
        public Dictionary<DataType, object> tankData;
    }
}
