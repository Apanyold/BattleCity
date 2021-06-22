using Didenko.BattleCity.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Didenko.BattleCity.Behaviors
{
    public class DroppedModuleBehavior : SpriteLoader, IOnPoolReturn
    {
        public SetupData SetupData => setupData;

        [SerializeField]
        private PoolObjBehavior poolObj;
        private SetupData setupData;

        private void Awake()
        {
            //Init(Team.Blue, new SetupData(3, SetupType.Cannon, CannonType.PC, "PC3"));
        }

        public void Init(Team team, SetupData setupData)
        {
            this.setupData = setupData;
            LoadSpriteForTeam(setupData.spriteName, team);
            StartCoroutine(Rotate());
        }

        public SetupData PickUp()
        {
            var data = new SetupData(setupData);
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
            setupData = new SetupData();
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

