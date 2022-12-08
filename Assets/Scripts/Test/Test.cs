using System;
using Framework;
using UnityEngine;

namespace Test
{
    public class Test : MonoBehaviour
    {
        private void Start()
        {
            UGUIManager.Instance.ShowPanel<UGUITest>(PanelName.PanelAudio,UILayer.System);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                ScenesManager.Instance.LoadSceneAsync(SceneName.Main2);
            }
        }
    }
}