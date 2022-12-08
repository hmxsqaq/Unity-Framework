using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework
{
    public enum SceneName
    {
        Main1,
        Main2
    }
    
    public class ScenesManager : Singleton<ScenesManager>
    {
        /// <summary>
        /// SyncLoad
        /// </summary>
        /// <param name="name"> SceneName </param>
        /// <param name="callback"> CallbackFunction </param>
        public void LoadSceneSync(SceneName name, Action callback = null)
        {
            SceneManager.LoadScene(name.ToString());
            callback?.Invoke();
        }

        /// <summary>
        /// AsyncLoad
        /// </summary>
        /// <param name="name"> SceneName </param>
        /// <param name="callback"> CallbackFunction </param>
        public void LoadSceneAsync(SceneName name, Action callback = null)
        {
            MonoManager.Instance.StartCoroutine(AsyncLoad(name.ToString()));
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