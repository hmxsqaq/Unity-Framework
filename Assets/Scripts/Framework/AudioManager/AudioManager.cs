using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class AudioManager : SingletonMono<AudioManager>
    {
        private AudioSource _bgSource;
        private float _bgVolume = 1f;

        private GameObject _soundObj;
        private readonly List<AudioSource> _soundList = new();
        private float _soundVolume = 1f;
        
        #region BgMusic
        
        /// <summary>
        /// Load and Play BGM
        /// </summary>
        /// <param name="path"> BGM File Path </param>
        public void LoadBgMusic(string path)
        {
            if (_bgSource == null)
            {
                GameObject bgObj = new GameObject
                {
                    name = "BgMusicSourceObject",
                    transform =
                    {
                        parent = transform
                    }
                };
                _bgSource = bgObj.AddComponent<AudioSource>();
            }
            
            ResourcesManager.Instance.LoadAsync<AudioClip>(path, clip =>
            {
                _bgSource.clip = clip;
                _bgSource.loop = true;
                _bgSource.volume = _bgVolume;
                _bgSource.Play();
            });
        }

        /// <summary>
        /// Pause BGM
        /// </summary>
        public void PauseBgMusic()
        {
            if (_bgSource == null)
                return;
            _bgSource.Pause();
        }
        
        /// <summary>
        /// Pause BGM
        /// </summary>
        public void UnPauseBgMusic()
        {
            if (_bgSource == null)
                return;
            _bgSource.UnPause();
        }
        
        /// <summary>
        /// Stop BGM
        /// </summary>
        public void StopBgMusic()
        {
            if (_bgSource == null)
                return;
            _bgSource.Stop();
        }
        
        /// <summary>
        /// Stop BGM
        /// </summary>
        public void ChangeBgMusicVolume(float volume)
        {
            if (_bgSource == null)
                return;
            _bgVolume = volume;
            _bgSource.volume = _bgVolume;
        }
        
        #endregion

        #region Sound

        /// <summary>
        /// Load and Play Sound
        /// </summary>
        /// <param name="path"> Sound File Path </param>
        /// <param name="isLoop"> Loop or not </param>
        /// <param name="callback"> CallbackFunction </param>
        public void LoadSound(string path, bool isLoop, Action<AudioSource> callback = null)
        {
            if (_soundObj == null)
            {
                _soundObj = new GameObject
                {
                    name = "SoundSourceObject",
                    transform =
                    {
                        parent = transform
                    }
                };
            }
            
            ResourcesManager.Instance.LoadAsync<AudioClip>(path, clip =>
            {
                AudioSource source = _soundObj.AddComponent<AudioSource>();
                source.clip = clip;
                source.loop = isLoop;
                source.volume = _soundVolume;
                source.Play();
                _soundList.Add(source);
                callback?.Invoke(source);
            });
        }

        /// <summary>
        /// Stop and Delete Source
        /// </summary>
        /// <param name="source"></param>
        public void StopSound(AudioSource source)
        {
            if (_soundList.Contains(source))
            {
                _soundList.Remove(source);
                source.Stop();
                Destroy(source);
            }
        }

        /// <summary>
        /// Change All Sound Volume
        /// </summary>
        /// <param name="volume"></param>
        public void ChangeSoundVolume(float volume)
        {
            if (_soundList.Count <= 0)
                return;
            _soundVolume = volume;
            
            foreach (var t in _soundList)
            {
                t.volume = _soundVolume;
            }
        }

        private void Update()
        {
            
            for (int i = _soundList.Count - 1; i >= 0; i--)
            {
                if (!_soundList[i].isPlaying)
                {
                    Destroy(_soundList[i]);
                    _soundList.RemoveAt(i);
                }
            }
            
        }

        #endregion
    }
}