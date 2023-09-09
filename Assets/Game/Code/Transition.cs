using UnityEngine;

namespace JoyTeam.Game
{
    public class Transition : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        public float Length { get; private set; }

        private void Start()
        {
            var length = 0.0f;
            foreach (var clip in animator.runtimeAnimatorController.animationClips)
                length = (clip.length > length) ? clip.length : length;
            Length = length;
        }

        public void Show() => animator.SetTrigger("show");
        public void Hide() => animator.SetTrigger("hide");
    }
}