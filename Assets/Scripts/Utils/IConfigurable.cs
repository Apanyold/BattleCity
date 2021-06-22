using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;
using Didenko.BattleCity.Behaviors;
using System.Collections.Generic;

namespace Didenko.BattleCity.Utils
{
    public interface IConfigurable
    {
        void SetConfings(string data);
        DataType DataType { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum DataType
    {
        [Description("cannondatas")] cannondatas
    }

    public struct TanksData
    {
        public Dictionary<DataType, object> tankData;
    }
}
