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
        /// <param name="name"> SceneName </param>
        /// <param name="callback"> CallbackFunction </param>
        public void LoadSceneSync(string name, Action callback)
        {
            SceneManager.LoadScene(name);
            callback?.Invoke();
        }

        /// <summary>
        /// AsyncLoad
        /// </summary>
        /// <param name="name"> SceneName </param>
        /// <param name="callback"> CallbackFunction </param>
        public void LoadSceneAsync(string name, Action callback)
        {
            MonoManager.Instance.StartCoroutine(AsyncLoad(name));
            callback?.Invoke();
        }

        private IEnumerator AsyncLoad(string name)
        {
            AsyncOperation async = SceneManager.LoadSceneAsync(name);
            while (!async.isDone)
            {
                //EventCenter.Instance.Trigger("Loading",async.progress);
                yield return null;
            }
        }
    }
}