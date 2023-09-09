using UnityEngine;
using DG.Tweening;

namespace JoyTeam.Game
{
    public class MergeEffect : MonoBehaviour
    {
        [SerializeField] private Transform transparentTransform;
        [SerializeField] private MeshRenderer transparentMaterial;
        [SerializeField] private ParticleSystemStopped[] particlesSystemsStopped;

        private void Awake()
        {            
            for (var i = 0; i < particlesSystemsStopped.Length - 1; i++)
            {
                var particleSystemStopped = particlesSystemsStopped[i];
                particleSystemStopped.OnStop += () => {
                    particlesSystemsStopped[i] = null;
                };
            }
        }

        private void Start()
        {
            transparentTransform.DOMoveY(0.585f, 0.5f);
            transparentTransform.DOScale(new Vector3(1.15f, 1.15f, 1.15f), 0.5f).OnComplete(() => {
                transparentMaterial.material.DOColor(new Color(1, 1, 1, 0), 0.5f);
            });
        }

        private void Update()
        {
            var isEmpty = true;
            foreach (var particleSystemStopped in particlesSystemsStopped)
            {
                if (particleSystemStopped != null)
                {
                    isEmpty = false;
                    break;
                }
            }

            if (isEmpty) Destroy(this.gameObject);
        }
    }
}