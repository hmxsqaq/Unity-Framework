using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace Framework
{
    public enum UILayer
    {
        System,
        Top,
        Mid,
        Bot
    }

    public enum PanelName
    {
        PanelAudio
    }
    public class UGUIManager : Singleton<UGUIManager>
    {
        private readonly Dictionary<PanelName, UGUIBase> _panelDic = new();

        private Transform _system;
        private Transform _top;
        private Transform _mid;
        private Transform _bot;

        public Canvas Canvas;
        public RectTransform CanvasTransform;

        protected override void OnInstanceCreate()
        {
            var canvasObj = ResourcesManager.Instance.LoadSync<GameObject>("Prefabs/UI/Canvas");
            var eventSystemObj = ResourcesManager.Instance.LoadSync<GameObject>("Prefabs/UI/EventSystem");
            if (canvasObj is null || eventSystemObj is null)
            {
                Debug.LogError("Cannot Find Canvas or EventSystem Object");
                return;
            }
            Object.DontDestroyOnLoad(canvasObj);
            Object.DontDestroyOnLoad(eventSystemObj);

            Canvas = canvasObj.GetComponent<Canvas>();
            CanvasTransform = canvasObj.transform as RectTransform;
            if (CanvasTransform != null)
            {
                _bot = CanvasTransform.GetChild(0);
                _mid = CanvasTransform.GetChild(1);
                _top = CanvasTransform.GetChild(2);
                _system = CanvasTransform.GetChild(3);
            }
        }

        /// <summary>
        /// Create Panel
        /// </summary>
        /// <param name="panelName"> enum PanelName </param>
        /// <param name="layer"> enum Layer </param>
        /// <param name="callback"> return the script of Panel </param>
        /// <typeparam name="T"> ScriptName of Panel </typeparam>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void ShowPanel<T>(PanelName panelName,UILayer layer = UILayer.Mid,Action<T> callback = null) where T : UGUIBase
        {
            if (_panelDic.ContainsKey(panelName))
            {
                Debug.LogError($"Panel:{panelName} has been created");
                return;
            }
            ResourcesManager.Instance.LoadAsync<GameObject>($"Prefabs/UI/Panels/{panelName}",obj =>
            {
                Transform parent;
                switch (layer)
                {
                    case UILayer.System:
                        parent = _system;
                        break;
                    case UILayer.Top:
                        parent = _top;
                        break;
                    case UILayer.Mid:
                        parent = _mid;
                        break;
                    case UILayer.Bot:
                        parent = _bot;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(layer), layer, null);
                }

                obj.transform.SetParent(parent);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                ((RectTransform)obj.transform).offsetMax = Vector2.zero;
                ((RectTransform)obj.transform).offsetMin = Vector2.zero;

                T panel = obj.GetComponent<T>();
                callback?.Invoke(panel);
                panel.Show();
                
                _panelDic.Add(panelName,panel);
            });
        }

        /// <summary>
        /// Delete Panel
        /// </summary>
        /// <param name="panelName"></param>
        public void HidePanel(PanelName panelName)
        {
            if (_panelDic.ContainsKey(panelName))
            {
                _panelDic[panelName].Hide();
                Object.Destroy(_panelDic[panelName].gameObject);
                _panelDic.Remove(panelName);
            }
            else
            {
                Debug.LogError($"Cannot Find Panel:{panelName} in PanelDic");
            }
        }

        /// <summary>
        /// Get opened Panel
        /// </summary>
        /// <param name="panelName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetPanel<T>(PanelName panelName) where T : UGUIBase
        {
            if (_panelDic.ContainsKey(panelName))
                return _panelDic[panelName] as T;
            Debug.LogError("Cannot Find Panel:{panelName} in PanelDic");
            return null;
        }

        /// <summary>
        /// Create Custom Event
        /// </summary>
        /// <param name="control"></param>
        /// <param name="type"></param>
        /// <param name="callback"></param>
        public static void AddCustomEventListener(UIBehaviour control, EventTriggerType type,
            UnityAction<BaseEventData> callback)
        {
            EventTrigger trigger = control.GetComponent<EventTrigger>();
            if (trigger == null) trigger = control.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = type;
            entry.callback.AddListener(callback);
            trigger.triggers.Add(entry);
        }
    }
}