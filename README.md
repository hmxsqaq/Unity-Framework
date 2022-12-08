# Unity-Framework
To record my unity framework learning process.

- [x] Singleton

  ```c#
  public class Singleton<T>
  public class SingletonMono<T>
  ```

  

- [x] ObjectPool 

  ```c#
  public class ObjectPool : Singleton<ObjectPool>
  {
  	public void Push(string poolName,GameObject gameObject)
  
  	public GameObject GetSync(string poolName,string path)
          
  	public void GetAsync(string poolName,string path,Action<GameObject> callback = 		null)
          
      public void Clear()
  }
  ```

  

- [x] EventCenter

  ```c#
  public enum EventName
  
  public class EventCenter : Singleton<EventCenter>
  {
      public void AddEventListener(EventName name,Action action)
      public void AddEventListener<T>(EventName name,Action<T> action)
          
      public void RemoveEventListener(EventName name,Action action)
      public void RemoveEventListener<T>(EventName name,Action<T> action)
          
      public void Trigger(EventName name)
      public void Trigger<T>(EventName name,T info)
          
      public void Clear()
  }
  ```

  

- [x] MonoManager

  ```c#
  public class MonoManager : SingletonMono<MonoManager>
  {
      public void AddUpdateEvent(Action action)
      
      public void RemoveUpdateEvent(Action action)
  }
  ```

  

- [x] ScenesManager

  ```c#
  public class ScenesManager : Singleton<ScenesManager>
  {
      public void LoadSceneSync(string name, Action callback = null)
      
      public void LoadSceneAsync(string name, Action callback = null)
  }
  ```

  

- [x] ResourcesManager

  ```c#
  public class ResourcesManager : Singleton<ResourcesManager>
  {
      public T LoadSync<T>(string path) where T : Object
          
      public void LoadAsync<T>(string path, Action<T> callback = null) where T : Object
  }
  ```

  

- [x] AudioManager

  ```c#
  public class AudioManager : SingletonMono<AudioManager>
  {
      public void LoadBgMusic(string path)
      public void PauseBgMusic()
      public void UnPauseBgMusic()
      public void StopBgMusic()
      public void ChangeBgMusicVolume(float volume)
      
      public void LoadSound(string path, bool isLoop, Action<AudioSource> callback = null)
      public void StopSound(AudioSource source)
      public void ChangeSoundVolume(float volume)
  }
  ```

  

- [x] AudioCenter

  ```c#
  public enum AudioType
  
  public class Audio
  {
      public Audio(AudioType type, string clipName, bool isLoop = false, Action callback = null)
      public Audio(AudioType type, string clipName, bool is3D, Vector3 position, bool isLoop = false, Action callback = null)
  }
  
  public class AudioPoolData
  {
      public AudioPoolData(AudioType pType, AudioSource pAudioSource, bool pIsUsing, bool pIsInPause)
  }
  
  public class AudioCenter : SingletonMono<AudioCenter>
  {
      public void AudioPlay(Audio pAudio)
      public void AudioStop(AudioType type, string clipName = "")
      public void AudioPause(AudioType type, string clipName = "")
      public void AudioUnPause(AudioType type, string clipName = "")
      public void SetMuteState(AudioType type,bool muteState)
      public void SwitchMuteState(AudioType type)
      public void SetVolume(AudioType type, float volume)
  }
  ```

  

- [ ] UIManager
