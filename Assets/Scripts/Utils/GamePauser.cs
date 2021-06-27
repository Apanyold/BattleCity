using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Didenko.BattleCity.Utils
{
    public static class GamePauser
    {
       public static GameState GameState
        {
            get => gameState;
            set
            {
                gameState = value;
                if (gameState == GameState.Play)
                    Time.timeScale = 1;
                else if(gameState == GameState.Pause)
                    Time.timeScale = 0;
            }        
        }

        private static GameState gameState;
    }
    public enum GameState 
    {
        Play, 
        Pause
    }

}
