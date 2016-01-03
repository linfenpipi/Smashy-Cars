using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ISBlockPrefabCreator : Editor {

	[MenuItem("SPINACH/Load From SCB")]
	static void LoadFromSCB(){
		//First, we ask user to select a .scb file to load.

		string path = EditorUtility.OpenFilePanel ("Choose SCB", Application.dataPath, "scb");
		if (string.IsNullOrEmpty (path)) return;

		//Once we have the file path, we parse its name, and use FileUtilities to load it.
		string fileName = path.Substring (path.LastIndexOf ("/"));

		ISSCBGrid dataSet = ISSCDGridFileUtilities.LoadFromFile (path);
		if (!dataSet) {
			Debug.Log ("Failed to create : could not open data set...");
			return;
		}

		//FileUntilities return the data set means the file is vaild, so now we can start the creation process,
		//First, create the root object.
		GameObject rootObj = new GameObject (string.IsNullOrEmpty(dataSet.name) ? fileName : dataSet.name);
		List<GameObject> children = new List<GameObject> ();
		Transform rootTrans = rootObj.transform;

		//It should be error, just make sure.
		rootTrans.position = Vector3.zero;

		int[] rawData = dataSet.GetRawData ();

		for (int i = 0; i < rawData.Length; i++) {
			
		}

	}
		
}
