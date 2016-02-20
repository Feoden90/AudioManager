using UnityEngine;
using System.Collections;
using UnityEditor;

//custom inspector class for Keyboard Generator;
[CustomEditor(typeof(GenerateKeyboard))]
public class GenerateKeyboardEditor : Editor {

	//private variables to draw rectangles inside inspector;
	private static Texture2D rectTexture;

	private bool autoGen;

	//method to draw rectangles inside inspector (didn't find a better way);
	private void DrawInspectorRect(Rect position, Color color){
		//instantiate only once the textures;
		if (rectTexture == null) {
			rectTexture = new Texture2D(1,1);
		}
		//set the color for the next rectangle;
		rectTexture.SetPixel (0, 0, color);
		rectTexture.Apply ();
		//draw the rectangle;
		GUI.DrawTexture (position, rectTexture);
	}

	public override void OnInspectorGUI(){

		//Drawing key object fields;
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("Keys:");
		GUILayout.Label ("W");
		((GenerateKeyboard)target).WhiteKey = EditorGUILayout.ObjectField (((GenerateKeyboard)target).WhiteKey, typeof(GameObject), false) as GameObject;
		GUILayout.Label ("B");
		((GenerateKeyboard)target).BlackKey = EditorGUILayout.ObjectField (((GenerateKeyboard)target).BlackKey, typeof(GameObject), false) as GameObject;
		EditorGUILayout.EndHorizontal ();

		//drawing audioclip object field;
		((GenerateKeyboard)target).KeyTone = EditorGUILayout.ObjectField ("Audio Clip",((GenerateKeyboard)target).KeyTone, typeof(AudioClip), false) as AudioClip;
		
		//Drawing number sliders;
		((GenerateKeyboard)target).NumberOfKeys = EditorGUILayout.IntSlider ("Number of Keys:", ((GenerateKeyboard)target).NumberOfKeys, 13, 52);
		((GenerateKeyboard)target).StartingKey = EditorGUILayout.IntSlider("Starting Key: " + GenerateKeyboard.GetKeyName(((GenerateKeyboard)target).StartingKey),((GenerateKeyboard)target).StartingKey,-24,0);

		//drawing keyboard preview;
		DrawInspectorKeyboard (27);

		//drawing buttons;
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("AutoEdit",GUILayout.MaxWidth(55));
		autoGen = GUILayout.Toggle (autoGen,GUIContent.none,GUILayout.MaxWidth(20));

		if(GUILayout.Button("Create")){
			((GenerateKeyboard)target).Generate();
		}
		if(GUILayout.Button("Clear")){
			((GenerateKeyboard)target).ClearKeyboard();
		}
		EditorGUILayout.EndHorizontal ();

		if (autoGen) {
			((GenerateKeyboard)target).Generate();
		}
	}

	//private method to draw the preview keyboard in the inspector;
	private void DrawInspectorKeyboard(float height){
		//initializing position variables for the drawing;
		Vector2 indent = new Vector2 (2, 2);
		Rect last = GUILayoutUtility.GetLastRect ();
		Vector2 pos = new Vector2 (0, last.yMax) + indent;
		Vector2 size = new Vector2 (EditorGUIUtility.currentViewWidth, height);
		Rect area = new Rect (pos, size);
		//allocating space for the keyboard;
		GUILayoutUtility.GetRect (size.x, size.y);

		//getting the keyboard parameters;
		int nkeys = ((GenerateKeyboard)target).NumberOfKeys;
		int startkey = ((GenerateKeyboard)target).StartingKey;

		//evaluating the number of white keys (= the keyboard size);
		int whitekeys = 0;
		for (int i = 0; i < nkeys; i++) {
			if (GenerateKeyboard.IsKeyWhite(startkey + i)){
				whitekeys += 1;
			}
		}

		//setting the size of the keys;
		Vector2 whiteSize = new Vector2 ((int)(size.x / whitekeys),area.height);
		Vector2 blackSize = new Vector2 (whiteSize.x * 2/3, whiteSize.y * 2 / 3);

		DrawInspectorRect(new Rect(0,pos.y,size.x,size.y),new Color(0.5f,0.5f,0.9f));

		//cycle to draw the white keys;
		Vector2 startpos = pos;
		for (int i = 0; i < nkeys; i++){
			
			if (GenerateKeyboard.IsKeyWhite(startkey + i)){
				DrawInspectorRect(new Rect(startpos,whiteSize),Color.black);
				DrawInspectorRect(new Rect(startpos + new Vector2(1,1),whiteSize - new Vector2(1,2)),Color.white);//,GenerateKeyboard.GetKeyName(startkey + i));
				startpos += (new Vector2(whiteSize.x,0));
			}
		}
		//cycle to draw the black keys (need to be drawn above the white keys);
		startpos = pos;
		for (int i = 0; i < nkeys; i++){
			if (!GenerateKeyboard.IsKeyWhite(startkey + i)){
				DrawInspectorRect(new Rect(startpos - new Vector2(blackSize.x/2,0),blackSize),Color.black);
				} else {
				startpos +=(new Vector2(whiteSize.x,0));
			}
		}
		//just draw the last vertical bar to end the keyboard;
		DrawInspectorRect (new Rect(startpos, new Vector2 (1, whiteSize.y)), Color.black);

	}
	
}
