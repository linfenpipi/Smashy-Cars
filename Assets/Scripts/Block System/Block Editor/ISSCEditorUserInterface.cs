using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ISSCEditorUserInterface : MonoBehaviour {

	public InputField blockSelector;
	public ISSCBEditorCore core;

	public void ChangeToPlacingState(){
		core.state = ISSCBEditorState.Placing;
	}

	public void ChangeToDeletingState(){
		core.state = ISSCBEditorState.Deleting;
	}

	public void Return(string v){
		string command = blockSelector.text;

		if (command.StartsWith ("select block")) {
			string argu = command.Substring (12);
			core.currentFillingBlock = int.Parse (argu);
		} else if (command.StartsWith ("sb")) {
			string argu = command.Substring (2);
			core.currentFillingBlock = int.Parse (argu);
		} else if (command.StartsWith ("save")) {
			string argu = command.Substring (command.IndexOf("\"") + 1,command.LastIndexOf("\"") - command.IndexOf("\"") - 1);
			core.SaveCurrentScene (Application.dataPath + "/Resources/SavedDatas/" + argu);
		} else if (command.StartsWith ("load")) {
			string argu = command.Substring (command.IndexOf("\"") + 1,command.LastIndexOf("\"") - command.IndexOf("\"") - 1);
			core.OpenScene (Application.dataPath + "/Resources/SavedDatas/" + argu);
		} else if (command.StartsWith ("new")) {
			string argu = command.Substring (command.IndexOf("\"") + 1,command.LastIndexOf("\"") - command.IndexOf("\"") - 1);
			core.NewScene (new ISSCBlockVector (21, 21, 21), argu);
		}

		Debug.Log ("Command Accepted... " + command);
		blockSelector.text = "";
	}

	public void NewScene(){
		core.NewScene (new ISSCBlockVector (21, 21, 21),"NewScene");
	}

	public void Save(){
		core.SaveCurrentScene (Application.dataPath + "/Resources/SavedDatas/slot1");
		Debug.Log (Application.dataPath + "Resources/SavedDatas/slot1");
	}

	public void Load(){
		core.OpenScene (Application.dataPath + "/Resources/SavedDatas/slot1");
	}
}
