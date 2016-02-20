using UnityEngine;
using System.Collections;

public class PianoKey : MonoBehaviour {
	
	public AudioClip sound;

	public int pianoKey;
	public bool bgm;
	public float fadetime;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void OnMouseDown () {
		PlaySound (Input.mousePosition);
	}

	private void PlaySound(Vector3 pos){
		if (bgm) {
			CustomManager.AudioManager.GetInstance ().FadeInBackGroundMusic (sound,fadetime, 1.0f, Mathf.Pow (2, pianoKey / 12f));
			//CustomManager.AudioManager.GetInstance ().PlayBackgroundMusic (sound, 1.0f, Mathf.Pow (2, pianoKey / 12f));
		} else {
			CustomManager.AudioManager.GetInstance().PlaySoundEffect(sound, 1.0f, Mathf.Pow (2, pianoKey / 12f));
		}
	}
}
