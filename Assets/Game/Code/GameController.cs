using UnityEngine;
using UnityEngine.SceneManagement;
using JoyTeam.Core;

namespace JoyTeam.Game
{
    public class GameController : Singleton<GameController>
    { 
        [SerializeField] private SwipeController SwipeController;
        [SerializeField] private FieldController FieldController;
        [SerializeField] private Screen screen;
        [SerializeField] private Camera Camera;

        private static bool inputState = false;

        public static void SetInput(bool state) => inputState = state;

        public static void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public override void Init()
        {
            SwipeController.OnSwipe += OnSwipe;
            Camera.transform.position = new Vector3(
                Config.Level.Width * 0.5f * FieldController.Field.XStep,
                Config.Level.Width,
                Config.Level.Width * 0.5f * FieldController.Field.ZStep
            );

            inputState = true;
        }

        private void Update()
        {
            SwipeController.ProccesInput();
        }

        private void OnSwipe(SwipeData data)
        {
            if(!inputState) return;

            switch (data.Direction)
            {
                case SwipeDirection.Up:
                    StartCoroutine(FieldController.MoveUnitsUp());
                    break;
                case SwipeDirection.Down:
                    StartCoroutine(FieldController.MoveUnitsDown());
                    break;
                case SwipeDirection.Right:
                    StartCoroutine(FieldController.MoveUnitsRight());
                    break;
                case SwipeDirection.Left:
                    StartCoroutine(FieldController.MoveUnitsLeft());
                    break;
            }
        }
    }
}