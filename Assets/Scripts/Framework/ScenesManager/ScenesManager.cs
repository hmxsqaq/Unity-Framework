using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework
{
    public class ScenesManager : Singleton<ScenesManager>
    {
        /// <summary>
        /// SyncLoad
        /// </summary>
        /// <param name="type"> SceneName </param>
        /// <param name="callback"> CallbackFunction </param>
        public void LoadSceneSync(SceneType type, Action callback = null)
        {
            SceneManager.LoadScene(type.ToString());
            callback?.Invoke();
        }

        /// <summary>
        /// AsyncLoad
        /// </summary>
        /// <param name="type"> SceneName </param>
        /// <param name="callback"> CallbackFunction </param>
        public void LoadSceneAsync(SceneType type, Action callback = null)
        {
            MonoManager.Instance.StartCoroutine(AsyncLoad(type.ToString()));
            callback?.Invoke();
        }

        private IEnumerator AsyncLoad(string name)
        {
            AsyncOperation async = SceneManager.LoadSceneAsync(name);
            while (!async.isDone)
            {
                //LoadingEvent
                yield return null;
            }
        }
    }
}