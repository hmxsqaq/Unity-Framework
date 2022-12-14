using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace Framework
{
    public class UGUIManager : Singleton<UGUIManager>
    {
        private readonly Dictionary<PanelType, UGUIBase> _panelDic = new();

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
        /// <param name="panelType"> enum PanelName </param>
        /// <param name="layerType"> enum Layer </param>
        /// <param name="callback"> return the script of Panel </param>
        /// <typeparam name="T"> ScriptName of Panel </typeparam>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void ShowPanel<T>(PanelType panelType,UILayerType layerType = UILayerType.Mid,Action<T> callback = null) where T : UGUIBase
        {
            if (_panelDic.ContainsKey(panelType))
            {
                Debug.LogError($"Panel:{panelType} has been created");
                return;
            }
            ResourcesManager.Instance.LoadAsync<GameObject>($"Prefabs/UI/Panels/{panelType}",obj =>
            {
                Transform parent;
                switch (layerType)
                {
                    case UILayerType.System:
                        parent = _system;
                        break;
                    case UILayerType.Top:
                        parent = _top;
                        break;
                    case UILayerType.Mid:
                        parent = _mid;
                        break;
                    case UILayerType.Bot:
                        parent = _bot;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(layerType), layerType, null);
                }

                obj.transform.SetParent(parent);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                ((RectTransform)obj.transform).offsetMax = Vector2.zero;
                ((RectTransform)obj.transform).offsetMin = Vector2.zero;

                T panel = obj.GetComponent<T>();
                callback?.Invoke(panel);
                panel.Show();
                
                _panelDic.Add(panelType,panel);
            });
        }

        /// <summary>
        /// Delete Panel
        /// </summary>
        /// <param name="panelType"></param>
        public void HidePanel(PanelType panelType)
        {
            if (_panelDic.ContainsKey(panelType))
            {
                _panelDic[panelType].Hide();
                Object.Destroy(_panelDic[panelType].gameObject);
                _panelDic.Remove(panelType);
            }
            else
            {
                Debug.LogError($"Cannot Find Panel:{panelType} in PanelDic");
            }
        }

        /// <summary>
        /// Get opened Panel
        /// </summary>
        /// <param name="panelType"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetPanel<T>(PanelType panelType) where T : UGUIBase
        {
            if (_panelDic.ContainsKey(panelType))
                return _panelDic[panelType] as T;
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