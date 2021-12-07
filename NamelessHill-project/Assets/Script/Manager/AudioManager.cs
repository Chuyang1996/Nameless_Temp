using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Nameless.Manager;
using Nameless.Data;
using System.IO;
using Newtonsoft.Json;
//声音对象池
namespace Nameless.Data
{
	public class AudioObjectPool
	{
		//要生成的对象池预设  
		private GameObject prefab;
		//对象池列表  
		public List<GameObject> soundPool;
		//构造函数  
		public AudioObjectPool(GameObject prefab, int initialSize)
		{
			this.prefab = prefab;
			this.soundPool = new List<GameObject>();
			for (int i = 0; i < initialSize; i++)
			{
				AlllocateInstance();

			}
		}
		// 获取实例    
		public GameObject GetInstance()
		{
			if (soundPool.Count == 0)
			{
				AlllocateInstance();
			}
			GameObject instance = soundPool[0];
			soundPool.RemoveAt(0);
			instance.SetActive(true);
			return instance;
		}
		// 释放实例  
		public void ReleaseInstance(GameObject instance)
		{
			instance.SetActive(false);
			soundPool.Add(instance);
			this.LimitInstance();
		}
		// 生成本地实例  
		private GameObject AlllocateInstance()
		{
			GameObject instance = (GameObject)GameObject.Instantiate(prefab);
			instance.transform.SetParent(AudioManager.Instance.gameSceneSound.transform);
			instance.SetActive(false);
			soundPool.Add(instance);

			return instance;
		}

		private void LimitInstance()
		{
			if (soundPool.Count >= AudioManager.Instance.initAudioPrefabCount)
			{
				for (int i = 0; i < soundPool.Count; i++)
				{
					if (soundPool.Count > AudioManager.Instance.initAudioPrefabCount)
					{
						GameObject sound = soundPool[i];
						soundPool.Remove(soundPool[i]);
						AudioManager.Instance.DestoryAudioObject(sound);
						i--;
					}
					else
					{
						break;
					}

				}
			}
		}
	}
}
namespace Nameless.Manager
{
	public class AudioManager : SingletonMono<AudioManager>
	{
		private const string audioPath = "Audio/";
		//audioClip列表
		private List<string> audioList;
		//初始声音预设数量
		public int initAudioPrefabCount = 5;

		public int limitAudioPrefabCount = 20;
		//记录静音前的音量大小
		[HideInInspector]
		public float tempBgmVolume = 0;
		[HideInInspector]
		public float tempSoundVolume = 0;     //是否BGM静音
		public bool isBgmMute = false;
		//是否Sound静音
		public bool isSoundMute = false;

		public float SoundVolume
        {
			set
			{
				soundVolume = value;
			}
			get
            {
				return soundVolume;

            }
            
        }
		private float soundVolume = 0.5f;


		public float MusicVolume
        {
			set
			{
				this.backGroundMusic.volume = value;
				musicVolume = value;
			}
			get
			{
				return musicVolume;

			}
		}
		private float musicVolume = 0.5f;
		[HideInInspector]
		public AudioSource backGroundMusic;
		[HideInInspector]
		public GameObject gameSceneSound;


		public bool IsBgmMute
		{
			set
			{
				isBgmMute = value;
				if (isBgmMute)
				{
					tempBgmVolume = backGroundMusic.volume;
					backGroundMusic.volume = 0;
				}
				else
				{
					backGroundMusic.volume = tempBgmVolume;
				}
			}
			get { return isBgmMute; }
		}
		public bool IsSoundMute
		{
			set
			{
				isSoundMute = value;
				if (isSoundMute)
				{

					foreach (Transform child in this.gameSceneSound.transform)
                    {
						tempSoundVolume = child.GetComponent<AudioSource>().volume;
						child.GetComponent<AudioSource>().volume = 0;
                    }
				}
				else
				{
					foreach (Transform child in this.gameSceneSound.transform)
					{
						child.GetComponent<AudioSource>().volume = tempSoundVolume;
					}
				}
			}
			get { return isSoundMute; }
		}

		//声音大小系数
		private float volumeScale = 1;
		public float VolumeScale
		{
			set
			{
				volumeScale = Mathf.Clamp01(value);
				if (!IsBgmMute)
				{
					AudioListener.volume = value;
				}
			}
			private get
			{
				return volumeScale;
			}
		}
		//audio字典
		private Dictionary<string, AudioClip> audioDic = new Dictionary<string, AudioClip>();
		//声音对象池
		private AudioObjectPool audioObjectPool;

		public void InitAudio()
		{
			//Debug.Log(Application.dataPath);
			string data;
			FileStream file = File.Open(Application.streamingAssetsPath + "/" + "AudiosResources.txt", FileMode.Open, FileAccess.Read);
			StreamReader reader = new StreamReader(file);
			data = reader.ReadLine();
			this.audioList = JsonConvert.DeserializeObject<List<string>>(data);
			//SceneManager.sceneUnloaded += scene =>
			//{
			//	//StopAllCoroutines();
			//	for (int i = 0; i < transform.childCount; i++)
			//	{
			//		if (transform.GetChild(i).hideFlags != HideFlags.HideInHierarchy)
			//		{
			//			InitAudioSource(transform.GetChild(i).GetComponent<AudioSource>());
			//			audioObjectPool.ReleaseInstance(transform.GetChild(i).gameObject);
			//		}
			//	}
			//};
			GameObject audioPrefab = new GameObject("AudioObjectPool");
			this.backGroundMusic = (new GameObject("Music")).AddComponent<AudioSource>();
			this.gameSceneSound = new GameObject("Sound");
			audioPrefab.AddComponent<AudioSource>();
			audioPrefab.GetComponent<AudioSource>().playOnAwake = false;
			audioObjectPool = new AudioObjectPool(audioPrefab, initAudioPrefabCount);
			audioPrefab.hideFlags = HideFlags.HideInHierarchy;
			audioPrefab.transform.SetParent(this.transform);
			this.backGroundMusic.transform.SetParent(this.transform);
			this.gameSceneSound.transform.SetParent(this.transform);
			this.backGroundMusic.transform.localPosition = new Vector3(0, 0, 0);
			this.gameSceneSound.transform.localPosition = new Vector3(0, 0, 0);
			foreach (string ac in this.audioList)
			{
				audioDic.Add(ac, (Resources.Load(audioPath + ac, typeof(AudioClip)) as AudioClip));
			}
		}
		//暂停播放
		public void PauseAudio(AudioSource audioSource)
		{
			audioSource.Pause();
		}
		//继续播放
		public void ResumeAudio(AudioSource audioSource)
		{
			audioSource.UnPause();
		}
		//停止播放
		public void StopSound(AudioSource audioSource)
		{
			audioSource.Stop();
			InitAudioSource(audioSource);
			audioObjectPool.ReleaseInstance(audioSource.gameObject);
		}
		//播放声音
		public void PlayAudio(AudioSource audioSource, string audioName, bool isLoop = false, float volume = 1)
		{
			if (IsBgmMute)
			{
				return;
			}
			AudioClip audioClip;
			if (audioDic.TryGetValue(audioName, out audioClip))
			{
				audioSource.loop = isLoop;
				audioSource.clip = audioClip;
				audioSource.volume = volume;
				if (audioSource.isPlaying)
				{
					audioSource.Stop();
				}
				audioSource.Play();
			}
		}
		//播放音效
		public AudioSource PlayAudio(Transform audioPos, string audioName, bool isLoop = false)
		{
			AudioSource audioSource = null;
			AudioClip audioClip;
			if (audioDic.TryGetValue(audioName, out audioClip))
			{
				GameObject audioGo = audioObjectPool.GetInstance();
				audioGo.transform.position = audioPos.position;
				audioGo.name = audioName;
				audioSource = audioGo.GetComponent<AudioSource>();
				audioSource.clip = audioClip;
				if(!IsSoundMute)
				    audioSource.volume = this.soundVolume;
				else
					audioSource.volume = 0;
				audioSource.loop = isLoop;
				audioSource.Play();

				if (!isLoop)
				{
					StartCoroutine(DestroyAudioGo(audioSource, audioClip.length));
				}

            }
            else
            {

				Debug.LogError("不存在该音效资源请检查 表中数据是否正确，或者函数填入的参数是否正确");
            }
		    
			return audioSource;

		}
		//播放BGM
		public void PlayMusic(string audioName)
		{

			AudioClip audioClip;
			if (audioDic.TryGetValue(audioName, out audioClip))
			{
				this.backGroundMusic.Stop();
				this.backGroundMusic.clip = audioClip;
				this.backGroundMusic.volume = this.musicVolume;
				this.backGroundMusic.loop = true;
				this.backGroundMusic.Play();
			}
			else
			{

				Debug.LogError("不存在 " + audioName +" 该音效资源请检查 表中数据是否正确，或者函数填入的参数是否正确");
			}

		}
		//初始化AudioSource
		private void InitAudioSource(AudioSource audioSource)
		{
			if (audioSource == null)
			{
				return;
			}
			if (audioSource.isPlaying)
			{
				audioSource.Stop();
			}
			audioSource.playOnAwake = false;
			audioSource.loop = false;
			audioSource.volume = 1;
			audioSource.clip = null;
			audioSource.name = "AudioObjectPool";
		
		}
		//销毁声音
		IEnumerator DestroyAudioGo(AudioSource audioSource, float delayTime)
		{
			yield return new WaitForSeconds(delayTime);
			InitAudioSource(audioSource);
			audioObjectPool.ReleaseInstance(audioSource.gameObject);
		}
		public void DestoryAudioObject(GameObject sound)
        {
			DestroyImmediate(sound);
        }
		void Destroy()
		{
			StopAllCoroutines();
		}
	}
}