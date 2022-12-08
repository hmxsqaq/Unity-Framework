﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public enum AudioType
    {
        BGM,
        UI,
        Effect
    }

    /// <summary>
    /// 封装被播放音频的参数
    /// </summary>
    public class Audio
    {
        public AudioType Type;
        public string ClipName;
        public bool IsLoop;
        public Action Callback;

        public bool Is3D;
        public Vector3 Position = Vector3.zero;

        public Audio(AudioType type, string clipName, bool isLoop = false, Action callback = null)
        {
            Type = type;
            ClipName = clipName;
            IsLoop = isLoop;
            Callback = callback;
        }
        
        public Audio(AudioType type, string clipName, bool is3D, Vector3 position, bool isLoop = false, Action callback = null)
        {
            Type = type;
            ClipName = clipName;
            Is3D = is3D;
            Position = position;
            IsLoop = isLoop;
            Callback = callback;
        }
    }

    [Serializable]
    public class AudioPoolData
    {
        [SerializeField]private AudioType type;
        [SerializeField]private AudioSource audioSource;
        [SerializeField]private bool isUsing;// 可用状态
        [SerializeField]private bool isInPause;// 暂停中

        public AudioType Type { get=> type; set => type = value; }

        public AudioSource AudioSource { get=> audioSource; set => audioSource = value; }
        public bool IsUsing { get=> isUsing; set => isUsing = value; }
        public bool IsInPause { get=> isInPause; set => isInPause = value; }

        public AudioPoolData(AudioType pType, AudioSource pAudioSource, bool pIsUsing, bool pIsInPause)
        {
            Type = pType;
            AudioSource = pAudioSource;
            IsUsing = pIsUsing;
            IsInPause = pIsInPause;
        }
    }
    
    public class AudioCenter : SingletonMono<AudioCenter>
    {
        private static readonly Dictionary<AudioType, string> AudioPathDic = new();
        private static readonly Dictionary<AudioType, float> AudioVolumeDic = new();
        private static readonly Dictionary<AudioType, bool> AudioMuteStateDic = new();
        private static readonly Dictionary<string, AudioClip> AudioClipDic = new();

        private AudioSource _bgAudioSource;

        public int poolMaxCount = 10;
        public List<AudioPoolData> audioPoolList = new();

        private AudioListener _audioListener;
        public AudioListener AudioListener
        {
            get
            {
                if (_audioListener != null)
                    return _audioListener;
                
                _audioListener = GetComponent<AudioListener>();
                if (_audioListener != null)
                    return _audioListener;

                _audioListener = gameObject.AddComponent<AudioListener>();
                return _audioListener;
            }
        }

        private void Awake()
        {
            Init();
        }

        /// <summary>
        /// Init Path/MuteState/Volume Dict and BgSource
        /// </summary>
        private void Init()
        {
            foreach (AudioType type in Enum.GetValues(typeof(AudioType)))
            {
                AudioPathDic.Add(type,$"Audios/{type}/");
                AudioVolumeDic.Add(type,1);
                AudioMuteStateDic.Add(type,false);
            }

            _bgAudioSource = gameObject.AddComponent<AudioSource>();
            
            LoadPlayerSet();

            if (_bgAudioSource != null)
            {
                _bgAudioSource.mute = AudioMuteStateDic[AudioType.BGM];
                _bgAudioSource.volume = AudioVolumeDic[AudioType.BGM];
            }
            foreach (var data in audioPoolList)
            {
                data.AudioSource.mute = AudioMuteStateDic[data.Type];
                data.AudioSource.volume = AudioVolumeDic[data.Type];
            }
        }

        /// <summary>
        /// Load PlayerSet
        /// </summary>
        private void LoadPlayerSet()
        {
            foreach (AudioType type in Enum.GetValues(typeof(AudioType)))
            {
                if (PlayerPrefs.HasKey($"{type}MuteState"))
                    AudioMuteStateDic[type] = PlayerPrefs.GetInt($"{type}MuteState") == 1;
                if (PlayerPrefs.HasKey($"{type}Volume"))
                    AudioVolumeDic[type] = PlayerPrefs.GetFloat($"{type}Volume");
            }
        }

        /// <summary>
        /// Sync Load or Get Clip
        /// </summary>
        /// <param name="type"> AudioType </param>
        /// <param name="clipName"> FileName </param>
        /// <returns> Clip </returns>
        private AudioClip GetClipSync(AudioType type, string clipName)
        {
            AudioPathDic.TryGetValue(type, out string path);
            if (path == null)
            {
                Debug.LogError($"Cannot Find Path:{type}");
                return null;
            }
            if (!AudioClipDic.ContainsKey(path+clipName))
            {
                AudioClip clip = ResourcesManager.Instance.LoadSync<AudioClip>(path+clipName);
                if (clip == null)
                {
                    Debug.LogError($"Cannot Find Clip:{clipName}");
                    return null;
                }
                AudioClipDic.Add(path+clipName,clip);
            }
            return AudioClipDic[path+clipName];
        }

        /// <summary>
        /// Add or Get Source
        /// </summary>
        /// <param name="type"> AudioType </param>
        /// <param name="data"> AudioPoolData </param>
        /// <returns> Source </returns>
        private AudioSource GetSource(AudioType type, out AudioPoolData data)
        {
            if (type == AudioType.BGM)
            {
                data = null;
                return _bgAudioSource;
            }

            data = audioPoolList.Find(data => !data.IsUsing && data.Type == type);
            if (data == null)
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                data = new AudioPoolData(type, source, true, false);
                audioPoolList.Add(data);
                return source;
            }

            if (data.AudioSource == null)
            {
                Debug.LogError($"Cannot Find Source In AudioData:{data.Type}");
                return null;
            }
            data.IsUsing = true;
            return data.AudioSource;
        }

        /// <summary>
        /// Coroutine that check the state of source that is playing
        /// </summary>
        /// <param name="data"> AudioPoolData </param>
        /// <param name="callback"> CallbackFunction that would be invoked after the clip done(can be null) </param>
        /// <returns></returns>
        private IEnumerator WaitToPush(AudioPoolData data, Action callback = null)
        {
            if (data == null) yield break;
            yield return new WaitUntil(() => !data.IsUsing || (!data.AudioSource.isPlaying && !data.IsInPause));

            if (audioPoolList.Contains(data))
                data.IsUsing = false;
            if (audioPoolList.Count > poolMaxCount)
            {
                audioPoolList.Remove(data);
                Destroy(data.AudioSource);
            }
            
            callback?.Invoke();
        }

        /// <summary>
        /// Start Audio
        /// </summary>
        /// <param name="pAudio"> Audio Class </param>
        public void AudioPlay(Audio pAudio)
        {
            AudioClip clip = GetClipSync(pAudio.Type, pAudio.ClipName);
            AudioSource source = GetSource(pAudio.Type, out AudioPoolData data);
            if (clip == null || source == null) return;
            AudioMuteStateDic.TryGetValue(pAudio.Type, out bool muteState);
            AudioVolumeDic.TryGetValue(pAudio.Type, out float volume);
            
            source.clip = clip;
            source.clip.LoadAudioData();
            source.loop = pAudio.IsLoop;
            source.mute = muteState;
            source.volume = volume;
            source.Play();

            if (pAudio.Type != AudioType.BGM)
                StartCoroutine(WaitToPush(data, pAudio.Callback));

            if (pAudio.Is3D)
                AudioSource.PlayClipAtPoint(clip,pAudio.Position);
        }

        /// <summary>
        /// Stop Audio
        /// </summary>
        /// <param name="type"> AudioType </param>
        /// <param name="clipName"> Clip File Name </param>
        public void AudioStop(AudioType type, string clipName = "")
        {
            if (type == AudioType.BGM)
            {
                if (!_bgAudioSource.isPlaying) return;
                _bgAudioSource.Stop();
                return;
            }
            foreach (var data in audioPoolList)
            {
                if (data.Type == type && data.IsUsing && data.AudioSource.clip.name == clipName)
                {
                    data.AudioSource.Stop();
                    if (audioPoolList.Contains(data))
                        data.IsUsing = false;
                }
            }
        }
        
        /// <summary>
        /// Pause Audio
        /// </summary>
        /// <param name="type"> AudioType </param>
        /// <param name="clipName"> Clip File Name </param>
        public void AudioPause(AudioType type, string clipName = "")
        {
            if (type == AudioType.BGM)
            {
                if (!_bgAudioSource.isPlaying) return;
                _bgAudioSource.Pause();
                return;
            }
            foreach (var data in audioPoolList)
            {
                if (data.Type == type && data.IsUsing && data.AudioSource.clip.name == clipName)
                {
                    data.AudioSource.Pause();
                    if (audioPoolList.Contains(data))
                        data.IsInPause = true;
                }
            }
        }
        
        /// <summary>
        /// UnPause Audio
        /// </summary>
        /// <param name="type"> AudioType </param>
        /// <param name="clipName"> Clip File Name </param>
        public void AudioUnPause(AudioType type, string clipName = "")
        {
            if (type == AudioType.BGM)
            {
                if (_bgAudioSource.isPlaying) return;
                _bgAudioSource.UnPause();
                return;
            }
            foreach (var data in audioPoolList)
            {
                if (data.Type == type && data.IsUsing && data.AudioSource.clip.name == clipName)
                {
                    data.AudioSource.UnPause();
                    if (audioPoolList.Contains(data))
                        data.IsInPause = false;
                }
            }
        }

        /// <summary>
        /// Set Mute State
        /// </summary>
        /// <param name="type"> AudioType </param>
        /// <param name="muteState"> bool state </param>
        public void SetMuteState(AudioType type,bool muteState)
        {
            int value = muteState ? 1 : 0;
            if (type == AudioType.BGM)
                _bgAudioSource.mute = muteState;
            else
            {
                foreach (var data in audioPoolList)
                {
                    if (data.Type == type)
                    {
                        data.AudioSource.mute = muteState;
                    }
                }
            }
            AudioMuteStateDic[type] = muteState;
            PlayerPrefs.SetInt($"{type}MuteState",value);
        }

        /// <summary>
        /// Switch Mute State
        /// </summary>
        /// <param name="type"> AudioType </param>
        public void SwitchMuteState(AudioType type)
        {
            bool muteState = !AudioMuteStateDic[type];
            int value = muteState ? 1 : 0;
            if (type == AudioType.BGM)
                _bgAudioSource.mute = muteState;
            else
            {
                foreach (var data in audioPoolList)
                {
                    if (data.Type == type)
                    {
                        data.AudioSource.mute = muteState;
                    }
                }
            }
            AudioMuteStateDic[type] = muteState;
            PlayerPrefs.SetInt($"{type}MuteState",value);
        }

        /// <summary>
        /// Set Volume
        /// </summary>
        /// <param name="type"> AudioType </param>
        /// <param name="volume"> float </param>
        public void SetVolume(AudioType type, float volume)
        {
            if (type == AudioType.BGM)
                _bgAudioSource.volume = volume;
            else
            {
                foreach (var data in audioPoolList)
                {
                    if (data.Type == type)
                    {
                        data.AudioSource.volume = volume;
                    }
                }
            }
            AudioVolumeDic[type] = volume;
            PlayerPrefs.SetFloat($"{type}Volume",volume);
        }
    }
}