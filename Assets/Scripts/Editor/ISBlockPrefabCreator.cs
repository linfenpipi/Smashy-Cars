using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ISBlockPrefabCreator : Editor {

	[MenuItem("SPINACH/Load From SCB")]
	static void LoadFromSCB(){
		//First, we ask user to select a .scb file to load.w

		EditorUtility.DisplayProgressBar ("Load from SCB", "Waiting for user action...", 0);

		string path = EditorUtility.OpenFilePanel ("Choose SCB", Application.dataPath, "scb");
		if (string.IsNullOrEmpty (path)) {
			EditorUtility.ClearProgressBar ();
			return;
		}

		EditorUtility.DisplayProgressBar ("Load from SCB", "Reading file...", 0);

		//Once we have the file path, we parse its name, and use FileUtilities to load it.
		string fileName = path.Substring (path.LastIndexOf ("/"));
		ISSCBGrid dataSet = ISSCDGridFileUtilities.LoadFromFile (path);
		/*
		if (!dataSet) {
			Debug.Log ("Failed to create : could not open data set...");
			EditorUtility.ClearProgressBar ();
			return;
		}
		*/

		EditorUtility.DisplayProgressBar ("Load from SCB", "Loading files...", 0.5f);

		//FileUntilities return the data set means the file is vaild, so now we can start the creation process,
		//First, create the root object, and load all blocks

		ISSCDBlocksList blockList = ISSCDBlocksList.LoadList ();
		GameObject rootObj = new GameObject (string.IsNullOrEmpty(dataSet.name) ? fileName : dataSet.name);
		List<GameObject> children = new List<GameObject> ();
		Transform rootTrans = rootObj.transform;

		//It should be zero, just make sure.
		rootTrans.position = Vector3.zero;

		EditorUtility.DisplayProgressBar ("Load from SCB", "Loading files...", 1);

		int[] rawData = dataSet.GetRawData ();
		Vector3 centerPosition = ISSCBGrid.GridPositionToWorldPosition (dataSet.GetCenterBlock (),rootTrans.position);

		EditorUtility.DisplayProgressBar ("Load from SCB", "Creating GameObjects...", 0);

		for (int i = 0; i < rawData.Length; i++) {
			if (rawData [i] <= 1) continue;

			ISSCBlockVector b = dataSet.DecodeIndex (i);

			Vector3 position = ISSCBGrid.GridPositionToWorldPosition (b, rootTrans.position) - centerPosition;
			GameObject obj = ISObjectPoolManager.Spawn (blockList.blocks [rawData [i]].gameObject, position, Quaternion.identity) as GameObject;
			obj.transform.parent = rootObj.transform;
			children.Add (obj);

			EditorUtility.DisplayProgressBar ("Load from SCB", "Creating GameObjects...", (float)i/(float)rawData.Length);
		}

		Selection.activeObject = rootObj;

		EditorUtility.ClearProgressBar ();
	}
		
}
