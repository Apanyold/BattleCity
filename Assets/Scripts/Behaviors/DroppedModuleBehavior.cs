using Didenko.BattleCity.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Didenko.BattleCity.Behaviors
{
    public class DroppedModuleBehavior : SpriteLoader, IOnPoolReturn
    {
        public DropData DropData => dropData;

        [SerializeField]
        private PoolObjBehavior poolObj;
        private DropData dropData;

        private void Awake()
        {
            Init(Team.Blue, new DropData("PC3", 3));
        }

        public void Init(Team team, DropData data)
        {
            this.dropData = data;
            LoadSpriteForTeam(data.spriteName, team);
            StartCoroutine(Rotate());
        }

        public DropData PickUp()
        {
            var data = new DropData(dropData.spriteName, dropData.lvl);
            Destroy();
            return data;
        }

        public void Destroy()
        {
            StopCoroutine(Rotate());
            poolObj.ReturnToPool();
        }

        public void OnReturnToPool()
        {
            dropData = new DropData();
        }

        private IEnumerator Rotate()
        {
            while (true)
            {
                spriteRenderer.transform.Rotate(0f,0f,1);
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Entered trigger: " + collision.name);
            collision.gameObject.GetComponents<ISetupable>();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            
        }
    }
}

