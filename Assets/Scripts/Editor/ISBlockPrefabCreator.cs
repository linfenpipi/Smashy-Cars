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

		int shouldFill = EditorUtility.DisplayDialogComplex ("Fill ?", "Do you want to fill the model ?", "Yep", "NO !", "Nevermind !");
		int shouldConnectWithJoint = EditorUtility.DisplayDialogComplex ("Apply Physic Connections ?", "Do you want to apply physic connections to blocks ?", "Yep", "NO !", "Nevermind !");

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
		List<GameObject> childrens = new List<GameObject> ();
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

			if (shouldFill != 0 && !dataSet.IsBlockVisiable (b)) continue;

			Vector3 position = ISSCBGrid.GridPositionToWorldPosition (b, rootTrans.position) - centerPosition;
			GameObject obj = ISObjectPoolManager.Spawn (blockList.blocks [rawData [i]].gameObject, position, Quaternion.identity) as GameObject;
			obj.GetComponent<ISSCBlock> ().blockID = i;
			obj.name = "Block " + i.ToString ();
			obj.transform.parent = rootObj.transform;
			childrens.Add (obj);

			EditorUtility.DisplayProgressBar ("Load from SCB", "Creating GameObjects...", (float)i/(float)rawData.Length);
		}

		Selection.activeObject = rootObj;

		EditorUtility.ClearProgressBar ();

		//Now, if we need to apply physic connects, continue !
		if (shouldConnectWithJoint != 0) return;
		EditorUtility.DisplayProgressBar ("Applying Physic Connections", "Processing...", 0);

		//Since we're using data set ID to create blocks, ID in list would be its ID of data set.
		for (int i = 0; i < childrens.Count; i++) {

			GameObject operatingObj = childrens [i];
			int objectID = operatingObj.GetComponent<ISSCBlock> ().blockID;
			ISSCBlockVector bv = dataSet.DecodeIndex (objectID);
			ISSCBlockVector[] surrounds = dataSet.SurroundingBlocks (bv);

			for (int j = 0; j < surrounds.Length; j++) {
				int toConnectObjectID = dataSet.EncodeIndex (surrounds [j]);
				if (rawData [toConnectObjectID] <= 1) continue;
				GameObject toConnect = null;

				foreach(GameObject obj in childrens){
					if(obj.GetComponent<ISSCBlock>().blockID == toConnectObjectID){
						toConnect = obj;
						break;
					}
				}

				if (!toConnect) continue;

				FixedJoint joint = operatingObj.AddComponent<FixedJoint> ();
				joint.connectedBody = toConnect.GetComponent<Rigidbody> () ? toConnect.GetComponent<Rigidbody> () : toConnect.AddComponent<Rigidbody> ();
				
			}

			EditorUtility.DisplayProgressBar ("Applying Physic Connections", "Processing " + i.ToString() + " of " + childrens.Count + " objects..", (float)i/(float)childrens.Count);
		}


		//Start validing, in another word, this will make me programs easier :)
		for (int i = 0; i < childrens.Count; i++) {

			Rigidbody r = childrens [i].GetComponent<Rigidbody> ();
			if (r) {
				r.isKinematic = false;
			} else {
				Debug.LogError ("Failed to vaild : Unknown error");
			}

			FixedJoint[] joints = childrens [i].GetComponents<FixedJoint> ();
			for (int j = 0; j < joints.Length; j++) {
				if (!joints [j].connectedBody)
					DestroyImmediate (joints [j]);

				joints [j].breakForce = 1000;
				joints [j].breakTorque = 1000;
			}

			EditorUtility.DisplayProgressBar ("Applying Physic Connections", "Vailding " + i.ToString() + " of " + childrens.Count + " objects..", (float)i/(float)childrens.Count);
		}

		EditorUtility.ClearProgressBar ();
	}
		
}
