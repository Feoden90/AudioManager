using UnityEngine;
using System.Collections;

//class for keyboard generation (sequence of keys with different pitch);
public class GenerateKeyboard : MonoBehaviour {

	//the keys gameobjects;
	public GameObject WhiteKey;
	public GameObject BlackKey;

	//the keys tone (will be applied to the keys);
	public AudioClip KeyTone;

	//the number of keys that will be instantiated;
	public int StartingKey;
	public int NumberOfKeys;

	//method to erase the keyboard;
	public void ClearKeyboard(){
		//for(int index = 0; index < transform.childCount; index++){
		while (transform.childCount > 0){
			DestroyImmediate(transform.GetChild(0).gameObject);
		}
	}

	//method to create the keyboard;
	public void Generate(){
		//first step: destroy the previous keyboard (bad, but no time to make a KeyPoolManager);
		ClearKeyboard ();

		Vector2 startpos = transform.position;
		GameObject newkey;

		//loop through the keys (instantiation, parameters, position);
		for (int iKey = StartingKey; iKey < NumberOfKeys + StartingKey; iKey++){
			//check if key is white; position is different for the two keys;
			if (IsKeyWhite(iKey)){
				newkey = UnityEditor.PrefabUtility.InstantiatePrefab(WhiteKey) as GameObject;
				newkey.transform.position = startpos;
				float halfsizex = newkey.GetComponent<Collider2D>().bounds.extents.x;
				float halfsizey = newkey.GetComponent<Collider2D>().bounds.extents.y;
				newkey.name = "White Key" + GetKeyName(iKey);
				newkey.transform.position += new Vector3(halfsizex,-halfsizey,0);
				startpos += new Vector2(halfsizex*2, 0);
			} else {
				newkey = UnityEditor.PrefabUtility.InstantiatePrefab(BlackKey) as GameObject;
				newkey.transform.position = startpos;
				//float halfsizex = newkey.GetComponent<Collider2D>().bounds.extents.x;
				float halfsizey = newkey.GetComponent<Collider2D>().bounds.extents.y;
				newkey.name = "Black Key" + GetKeyName(iKey);
				newkey.transform.position += new Vector3(0, -halfsizey, -1);
			}
			//applying parameters to the newly instantiated keys;
			newkey.transform.SetParent(transform);
			newkey.GetComponent<PianoKey>().pianoKey = iKey;
			newkey.GetComponent<PianoKey>().sound = KeyTone;
		}

	}

	//static method that checks if a given key is black(b,#) or white;
	public static bool IsKeyWhite(int key){
		int basekey = PositiveMod (key, 12);
		//black keys are C#,D#,F#,G#,A# and C is 0;
		int[] blackkeys = {1,3,6,8,10};
		foreach (int blackkey in blackkeys) {
			if (basekey == blackkey){
				return false;
			}
		}
		return true;
	}

	//static method to obtain the correct modulo for negative numbers;
	private static int PositiveMod(int number, int dividend){
		return (number%dividend + dividend) % dividend;
	}

	//static method to retrieve the key name (English notation);
	public static string GetKeyName(int key){
		string name = "";
		int basekey = PositiveMod (key, 12);
		int octave = (key - basekey) / 12 + 4;
		switch (basekey) {
		case 0: name = "C"; break;
		case 1: name = "C#"; break;
		case 2: name = "D"; break;
		case 3: name = "D#"; break;
		case 4: name = "E"; break;
		case 5: name = "F"; break;
		case 6: name = "F#"; break;
		case 7: name = "G"; break;
		case 8: name = "G#"; break;
		case 9: name = "A"; break;
		case 10: name = "A#"; break;
		case 11: name = "B"; break;
		default: break;
		}
		name += octave.ToString ();
		return name;
	}
}
