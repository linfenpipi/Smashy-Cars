using System;
using UnityEditor;
using UnityEngine;
using System.Collections;

public class ISEditorInputDialog : EditorWindow {

	static EditorWindow self;
	static string ok;
	static Action<string> callbackAction;
	string curValue = "values here";

	static public void Display(string title, string okButton, Action<string> callback){
		self = EditorWindow.GetWindow<ISEditorInputDialog> ();
		self.Show ();

		GUIContent t = new GUIContent ();
		t.text = title;
		self.titleContent = t;
		ok = okButton;

		callbackAction = callback;
	}

	void OnGUI(){
		curValue = GUILayout.TextField (curValue);

		bool pressed = GUILayout.Button (ok);
		if (pressed) {
			callbackAction (curValue);
			self.Close ();
			self = null;
		}
	}

	void OnLostFocus(){
		self.Focus ();
	}
}
