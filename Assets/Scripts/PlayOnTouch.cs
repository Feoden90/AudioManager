using UnityEngine;
using System.Collections;
using CustomManager;

public class PlayOnTouch : MonoBehaviour
{

	public AudioClip sound;
	public float fadetime;
	public bool loop;

	void OnMouseDown ()
	{
		PlaySound (Input.mousePosition);
	}
	
	private void PlaySound (Vector3 pos)
	{
		if (loop) {
			CustomManager.AudioManager.GetInstance ().FadeInBackGroundMusic (sound, fadetime, 1.0f, 1.0f);
		} else {
			CustomManager.AudioManager.GetInstance ().PlaySoundEffect (sound, 1.0f, 1.0f);
		}
	}
}