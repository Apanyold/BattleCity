using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Didenko.BattleCity.Ui
{
    public class DialogWindow : MonoBehaviour
    {
        [SerializeField]
        private Button btnOk;
        [SerializeField]
        private Text txtButton;
        [SerializeField]
        private Text txtTitle;

        public delegate void callBackAction();

        public void ShowDialog(string titleText, string buttonText, callBackAction callBackAction)
        {
            gameObject.SetActive(true);
            txtTitle.text = titleText;
            txtButton.text = buttonText;

            btnOk.onClick.AddListener(() => 
            { 
                gameObject.SetActive(false); 
                callBackAction?.Invoke(); 
            });
        }
    }
}
