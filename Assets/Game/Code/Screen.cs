using UnityEngine;
using UnityEngine.UI;
using JoyTeam.Core;

namespace JoyTeam.Game
{
    public class Screen : MonoBehaviour
    {
        [SerializeField] private Text level;
        [SerializeField] private Transition transition;
        
        public void OnClickRestartButton()
        {
            GameController.SetInput(false);
            transition.Hide();
            this.ActionWithDelay(GameController.Restart, transition.Length);
        }
    }
}
