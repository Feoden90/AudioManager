using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CustomManager
{
	public class AudioManager2 : PoolManager
	{
		//static instance of the manager;
		private static AudioManager2 audioManager;
		//private variables to keep state of the manager;
		private bool pausedManager;
		private string bgmInstanceKey;
		private float bgmVolume;
		private float seVolume;

		//method used only for renaming the container;
		private Dictionary<string,GameObject> GetAudioChannels ()
		{
			return myobjectList;
		}

		protected override void Start ()
		{
			//pool manager initialization;
			base.Start ();
			//instantiating the static manager instance;
			audioManager = this;
			//audiomanager initialization;
			pausedManager = false;
			bgmInstanceKey = "";
		}

		//method invoked by base manager to setup object properties;
		protected override GameObject CreateObjectProperties ()
		{
			//giving a name to the object;
			int index = GetAudioChannels ().Count;
			GameObject tmp = new GameObject ("Channel n°" + index.ToString ());
			//adding components to the object;
			tmp.AddComponent<AudioSource> ();
			tmp.AddComponent<AudioObject> ();
			//returning the customized object to the manager;
			return tmp;
		}

		//static method to actually access the manager;
		public static AudioManager2 GetInstance ()
		{
			//this is the last instantiated manager, not a singleton;
			return audioManager;
		}

		//method to play a sound with no parameters;
		public void PlayAudioClip (AudioClip clip)
		{
			this.PlayAudioClip (clip, Vector3.zero, 1.0f, 1.0f, false);
		}

		//method to play a sound with only position parameter;
		public void PlayAudioClip (AudioClip clip, Vector3 position)
		{
			this.PlayAudioClip (clip, position, 1.0f, 1.0f, false);
		}

		//method to play a sound with position, volume and pitch;
		public void PlayAudioClip (AudioClip clip, Vector3 position, float volume, float pitch)
		{
			this.PlayAudioClip (clip, position, volume, pitch, false);
		}

		//most generic method to play a sound, invoked by all other methods;
		private GameObject PlayAudioClip (AudioClip clip, Vector3 position, float volume, float pitch, bool loop)
		{
			//if manager is paused for whatever reason we should not instantiate new sounds;
			if (pausedManager)
				return null;
			//fetch the next free object, if none create a new one;
			GameObject channel = GetNextFreeObjectInstance ();

			//invoke play method in audioobject;
			channel.GetComponent<AudioObject> ().PlayClip (clip, position, volume, pitch, loop);
			return channel;
		}

		//method to play bg music (if a new is played, the old is stopped);
		public void PlayBackgroundMusic (AudioClip bgm, float volume, float pitch)
		{
			//command unified with the fadeIn command (fadetime = 0);
			FadeInBackGroundMusic (bgm, 0.0f, volume, pitch);
		}

		//method to play a generic 2D sound effect with volume and pitch;
		public void PlaySoundEffect (AudioClip se, float volume, float pitch)
		{
			this.PlayAudioClip (se, Vector3.zero, volume, pitch, false);
		}

		//method to make smooth transition between different bg musics;
		public void FadeInBackGroundMusic (AudioClip bgm, float fadetime, float volume, float pitch){
			//asking the manager to queue the new audioclip (volume = 0);
			GameObject newchannel = this.PlayAudioClip (bgm, Vector3.zero, 0, pitch, true);
			//if it couldn't start, then stop here (paused manager);
			if (newchannel == null)
				return;
			//if bg music was already running, then fade it Out;
			if (bgmInstanceKey != "") {
				StartCoroutine(GetAudioChannels()[bgmInstanceKey].GetComponent<AudioObject>().FadeOut(fadetime));
			}
			//Fade in the new music;
			StartCoroutine(newchannel.GetComponent<AudioObject>().FadeSound(fadetime,volume));
			bgmInstanceKey = newchannel.GetInstanceID ().ToString();

		}

		//method to pause the manager. If paused, no sound can be instantiated;
		public void PauseSoundManager (bool paused)
		{
			pausedManager = paused;
			foreach (GameObject channel in GetAudioChannels().Values) {
				channel.GetComponent<AudioObject> ().PauseClip (paused);
			}
		}

		//method to stop the manager; it will free all objects, including bgm;
		public void StopAllSounds ()
		{
			bgmInstanceKey = "";
			foreach (GameObject channel in GetAudioChannels().Values) {
				channel.GetComponent<AudioObject> ().StopPlaying ();
			}
		}

	}
}
