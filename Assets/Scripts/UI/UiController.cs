using Didenko.BattleCity.Utils;
using Didenko.BattleCity.Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Didenko.BattleCity.Behaviors;

namespace Didenko.BattleCity.Ui
{
    public class UiController : MonoBehaviour
    {
        [SerializeField]
        private DialogWindow dialogWindow;
        [SerializeField]
        private GameObject pickupDialog;

        private PlayerController playerController;

        public void Init(PlayerController playerController)
        {
            dialogWindow.gameObject.SetActive(false);
            pickupDialog.SetActive(false);

            playerController.CanBeCollected += ShowPickUpDialog;
        }

        public void ShowPlayerWin()
        {
            GamePauser.GameState = GameState.Pause;
            dialogWindow.ShowDialog("Wow, such win", "It wasn't that difficult", Reload);
        }

        public void ShowPlayerLose()
        {
            GamePauser.GameState = GameState.Pause;
            dialogWindow.ShowDialog("You lose \n Good day sir", "Give me one more chance", Reload);
        }

        public void ShowPickUpDialog(bool isCollectable, DroppedModuleBehavior droppedModuleBehavior)
        {
            pickupDialog.SetActive(isCollectable);
        }

        private void Reload()
        {
            GamePauser.GameState = GameState.Play;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
