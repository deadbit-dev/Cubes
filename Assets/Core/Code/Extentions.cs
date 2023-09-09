using System;
using System.Collections;
using UnityEngine;

namespace JoyTeam.Core
{
    public static class Extentions
    {
        private static IEnumerator ActionWithDelay(Action callback, float time)
        {
            yield return new WaitForSeconds(time);
            callback();
        }

        public static Coroutine ActionWithDelay(this MonoBehaviour script, Action callback, float time)
        {
            return script.StartCoroutine(ActionWithDelay(callback, time));
        }
    }
}