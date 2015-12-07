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

	public void FillingBlockSelectorValueChange(string newValue){
		core.currentFillingBlock = int.Parse (blockSelector.text);
	}

	public void NewScene(){
		core.NewScene (new ISSCBlockVector (21, 21, 21));
	}
}
