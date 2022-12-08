using System;
using Framework;
using UnityEngine.UI;

namespace Test
{
    public class UGUITest : UGUIBase
    {
        private void Start()
        {
            GetUIControl<Button>("BGM").onClick.AddListener(PlayBGM);
        }

        private void PlayBGM()
        {
            AudioCenter.Instance.AudioPlayAsync(new Audio(AudioType.BGM,"BGM"));
        }
    }
}