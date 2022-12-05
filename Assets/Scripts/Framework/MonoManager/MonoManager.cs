using System;

namespace Framework
{
    public class MonoManager : SingletonMono<MonoManager>
    {
        private event Action UpdateEvent;

        private void Update()
        {
            if (UpdateEvent != null)
                UpdateEvent();
        }

        public void AddUpdateEvent(Action action)
        {
            UpdateEvent += action;
        }

        public void RemoveUpdateEvent(Action action)
        {
            UpdateEvent -= action;
        }
    }
}