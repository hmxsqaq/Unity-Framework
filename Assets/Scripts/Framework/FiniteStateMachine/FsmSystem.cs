﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class FsmSystem<T> where T : Enum
    {
        private Dictionary<T, StateBase<T>> _stateDic = new();
        private T _currentStateType;
        private StateBase<T> _currentState;

        public T Type => _currentStateType;

        public void AddState(StateBase<T> state)
        {
            if(state == null) return;
            if(_stateDic.ContainsKey(state.Type)) return;
            
            _stateDic.Add(state.Type,state);
        }

        public void RemoveState(T type)
        {
            if(!_stateDic.ContainsKey(type)) return;

            _stateDic.Remove(type);
        }

        public void StartState(T type)
        {
            if (!_stateDic.ContainsKey(type)) return;

            _currentStateType = type;
            _currentState = _stateDic[type];
            _currentState.OnEnter();
        }

        public void Update()
        {
            if(_currentState.Transition(out T type))
                TransferState(type);
            _currentState.OnUpdate();
        }

        private void TransferState(T type)
        {
            if (!_stateDic.ContainsKey(type))
            { 
                Debug.LogError($"Can not find {type} in stateDic");
                return;
            }
            _currentState.OnExit();
            _currentState = _stateDic[type];
            _currentStateType = type;
            _currentState.OnEnter();
        }
    }
}