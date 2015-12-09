using UnityEngine;
using System.Collections;

public class ISSCBGridController : MonoBehaviour
{

	public ISSCBGridDescriber grid;
	public bool updateDataEachFrame;

	ISSCBGrid gridData;
	int currentVersion = 0;
	int[] versionDataCache;

	ISSCDBlocksList blockList;
	GameObject[] blockObjects;

	void Start ()
	{
		blockList = ISSCDBlocksList.LoadList ();

		for (int i = 0; i < blockList.blocks.Length; i++) ISObjectPoolManager.New (blockList.blocks [i].gameObject,1);

		gridData = new ISSCBGrid (grid);
		int length = gridData.gridSize.Length ();
		blockObjects = new GameObject[length];
		versionDataCache = new int[length];
	}

	void Update(){
		if(updateDataEachFrame) UpdateEntireScene ();
	}

	public void SwitchDataSet(ISSCBGrid newDataSet){
		gridData = newDataSet;
		currentVersion = -1;
		ClearNCreateNewInSceneCaches (gridData.gridSize.Length ());
		UpdateEntireScene ();

		Debug.Log("data switched...");
	}

	void UpdateEntireScene ()
	{
		int versionCheckResult = gridData.IsLastestVersion (currentVersion);
		if(versionCheckResult == -1) return;

		Debug.Log("New version detected, updating " + (versionCheckResult - currentVersion).ToString() + " changes...");

		int[] data = gridData.GetRawData ();

		for (int i = 0; i < data.Length; i++) {
			if(data[i] == versionDataCache[i]) continue;

			//Once a block is changed(otherwise, loop is continued), check the surrounding blocks and see if some of them also need to be updated.
			UpdateBlocksCacheIgroned(gridData.SurroundingBlocks(gridData.DecodeIndex(i)));

			versionDataCache[i] = data[i];

			ISSCBlockVector b = gridData.DecodeIndex(i);

			if (blockObjects [i]) ISObjectPoolManager.Unspawn (blockObjects [i]);
			if(data[i] <= 1) continue;
			if(!gridData.IsBlockVisiable(b)) continue;

			Vector3 position = ISSCBGrid.GridPositionToWorldPosition (b, transform.position);
			blockObjects [i] = ISObjectPoolManager.Spawn (blockList.blocks [data [i]].gameObject, position, Quaternion.identity) as GameObject;
		}

		currentVersion = versionCheckResult;
	}

	public void UpdateBlocksCacheIgroned(ISSCBlockVector[] blocks){
		for (int i = 0; i < blocks.Length; i++) {
			UpdateBlockCacheIgroned (blocks [i]);
		}
	}

	public void UpdateBlockCacheIgroned(ISSCBlockVector block){
		int id = gridData.EncodeIndex (block);

		if (blockObjects [id]) ISObjectPoolManager.Unspawn (blockObjects [id]);

		if(!gridData.IsBlockVisiable(block)) return;
		int[] data = gridData.GetRawData ();

		if(data[id] <= 1) return;

		Vector3 position = ISSCBGrid.GridPositionToWorldPosition (block, transform.position);
		blockObjects [id] = ISObjectPoolManager.Spawn (blockList.blocks [data [id]].gameObject, position, Quaternion.identity) as GameObject;
	}

	void ClearNCreateNewInSceneCaches(int newLength){
		versionDataCache = new int[newLength];

		for (int i = 0; i < blockObjects.Length; i++) {
			if(blockObjects[i])ISObjectPoolManager.Unspawn (blockObjects [i]);
		}

		blockObjects = new GameObject[newLength];
	}
}
