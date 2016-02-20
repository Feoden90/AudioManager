using UnityEngine;
using System.Collections;
using CustomManager;

public class DebugGui : MonoBehaviour {

	private bool paused = false;

	void OnGUI(){
		
		if (GUI.Button (new Rect (0, 0, 100, 40), "Pause sounds")) {
			paused = !paused;
			AudioManager.GetInstance().PauseSoundManager(paused);
		}

		if (GUI.Button (new Rect (0, 40, 100, 40), "Stop sounds")) {
			AudioManager.GetInstance().StopAllSounds();
		}

	}
}
