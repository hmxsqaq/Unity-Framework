using UnityEngine;
using UnityEngine.UI;

namespace Framework
{
    public class SceneFade : MonoBehaviour
    {
        [SerializeField]private float fadeSpeed;
        [SerializeField]private Color colorFade;
        
        private RawImage _switchImg;

        private void Awake()
        {
            EventCenter.Instance.AddEventListener(EventType.SceneStart,SceneStart);
            EventCenter.Instance.AddEventListener(EventType.SceneEnd,SceneEnd);
        }

        private void OnDestroy()
        {
            EventCenter.Instance.RemoveEventListener(EventType.SceneStart,SceneStart);
            EventCenter.Instance.RemoveEventListener(EventType.SceneEnd,SceneEnd);
        }

        private void Start()
        {
            GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
            _switchImg = GetComponent<RawImage>();
            _switchImg.color = colorFade;
        }
        
        private void FadeToClear()
        {
            _switchImg.color = Color.Lerp(_switchImg.color, colorFade, fadeSpeed*Time.deltaTime);
        }
        
        private void FadeToBlack()
        {
            _switchImg.color = Color.Lerp(_switchImg.color, Color.black, fadeSpeed * Time.deltaTime);
        }

        private void SceneStarting()
        {
            _switchImg.enabled = true;
            FadeToClear();
            if (_switchImg.color.a <= 0.05f)
            {
                _switchImg.color = Color.clear;
                _switchImg.enabled = false;
                MonoManager.Instance.RemoveUpdateEvent(SceneStarting);
            }
        }

        private void SceneEnding()
        {
            _switchImg.enabled = true;
            FadeToBlack();
            if (_switchImg.color.a >= 0.95f)
            {
                _switchImg.color = colorFade;
                _switchImg.enabled = false;
                MonoManager.Instance.RemoveUpdateEvent(SceneEnding);
            }
        }

        private void SceneStart()
        {
            MonoManager.Instance.AddUpdateEvent(SceneStarting);
        }

        private void SceneEnd()
        {
            MonoManager.Instance.AddUpdateEvent(SceneEnding);
        }
    }
}