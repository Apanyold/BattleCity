using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Didenko.BattleCity.Utils
{
    public abstract class SpriteLoader : MonoBehaviour
    {
        [SerializeField]
        protected SpriteRenderer spriteRenderer;
        protected void LoadSpriteForTeam(string spriteName, Team team)
        {
            string path = team.ToString() + "/" + spriteName;
            spriteRenderer.sprite =  LoadSprite(path);

            Debug.Log(spriteName + " for team: " + team);
        }

        protected Sprite LoadSprite(string spriteName)
        {
            string path = "Images/" + spriteName;
            var sprite = Resources.Load<Sprite>(path);
            return sprite;
        }
    }
}
