using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Framework
{
    public class UGUIBase : MonoBehaviour
    {
        private readonly Dictionary<string, List<UIBehaviour>> _uiDic = new();

        protected virtual void Awake()
        {
            FindChildrenUIControl<Image>();
            FindChildrenUIControl<Button>();
            //Find All Needed Control
        }

        private void FindChildrenUIControl<T>() where T : UIBehaviour
        {
            var controls = GetComponentsInChildren<T>();
            foreach (var control in controls)
            {
                var objName = control.gameObject.name;
                if (_uiDic.ContainsKey(objName))
                    _uiDic[objName].Add(control);
                else
                    _uiDic.Add(objName, new List<UIBehaviour>() { control });
            }
        }

        /// <summary>
        /// Get UIControl
        /// </summary>
        /// <param name="objName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns> UIBehaviour </returns>
        protected T GetUIControl<T>(string objName) where T : UIBehaviour
        {
            if (_uiDic.ContainsKey(objName))
            {
                var control = _uiDic[objName].Find(control => control is T);
                if (control != null)
                    return control as T;
                Debug.LogError($"Cannot Find {typeof(T)} in {objName}");
                return null;
            }
            Debug.LogError($"Cannot Find {objName} in UIDic");
            return null;
        }
        
        /// <summary>
        /// Trigger when the panel show
        /// </summary>
        public virtual void Show(){}
        
        /// Trigger when the panel hide
        public virtual void Hide(){}
    }
}