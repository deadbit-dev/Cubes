using UnityEngine;
using UnityEngine.SceneManagement;
using JoyTeam.Core;

namespace JoyTeam.Game
{
    public class EntryController : Singleton<EntryController>
    {
        [SerializeField] private Transition transition;

        private void Start()
        {
            Instance.ActionWithDelay(transition.Hide, Config.TimeForEntry);
            Instance.ActionWithDelay(LoadGame, Config.TimeForEntry + transition.Length);
        }

        private void LoadGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}