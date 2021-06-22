using Didenko.BattleCity.Utils;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigSetter : MonoBehaviour
{
    public TanksData TanksData;

    public void DownloadDatas(TextAsset textAsset)
    {
        TanksData = JsonConvert.DeserializeObject<TanksData>(textAsset.text);
    }

    public void SetData(GameObject gameObject)
    {
        var configuravle = gameObject.GetComponents<IConfigurable>();
        foreach (var item in configuravle)
        {
            item.SetConfings(TanksData.tankData[item.DataType].ToString());
        }
    }
}
