using UnityEngine;
using System.Collections;
using LitJson;
using System.IO;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
	//	public GameObject cubes;
	public GameObject playersCar;
	Transform playersCarTs;
	public float basicSize;
	public CarsCubes ccs;
	public string fileName;
	JsonData json;
	ISSCDBlocksList cubeList;
	GameObject[] allKindOfCubes;
	Vector3 originPos;
	public bool gaming = false;
	public CubeBehaviours supported;
	public bool endedGame = false;



	void Start ()
	{
		playersCarTs = playersCar.transform;
		cubeList = ISSCDBlocksList.LoadList ();
		allKindOfCubes = new GameObject[cubeList.blocks.Length];
		for (int i = 0; i < allKindOfCubes.Length; i++) {
			allKindOfCubes [i] = cubeList.blocks [i].gameObject;
		}


		json = ISSCDGridFileUtilities.LoadFileAsJson (Application.dataPath + "/Resources/", fileName);
		ccs = new CarsCubes (int.Parse (json ["size"] ["x"].ToString ()), int.Parse (json ["size"] ["y"].ToString ()), int.Parse (json ["size"] ["z"].ToString ()));
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space) && !gaming && !endedGame) {
			gaming = true;
			spawnCubes ();
		}

		if(Input.GetKeyDown(KeyCode.Space)&& endedGame){
			Application.LoadLevel(0);
		}
	}

	void spawnCubes ()
	{
		playersCar.transform.parent.position += Vector3.up * (basicSize * (float)ccs.sizeY / 2 + 1);
//		playersCar.transform.parent.GetComponent<Collider>().isTrigger = true;
		originPos = getOriginPos ();
		for (int i = 0; i < ccs.sizeX; i++) {
			for (int ii = 0; ii < ccs.sizeY; ii++) {
				for (int iii = 0; iii < ccs.sizeZ; iii++) {
//					Debug.Log(i * ccs.sizeX + ii * ccs.sizeY + iii);
					int id = int.Parse (json ["blocks"] [(i * ccs.sizeX * ccs.sizeY + ii * ccs.sizeY + iii).ToString ()].ToString ());
					if (id >= 2) {
						ccs.cubes [i, ii, iii] = Instantiate (setCube (id), getPosByIds (i, ii, iii), Quaternion.identity) as GameObject;
						ccs.cubes [i, ii, iii].transform.parent = playersCarTs;
						ccs.cubes [i, ii, iii].transform.localScale = Vector3.one * basicSize;
						Destroy (ccs.cubes [i, ii, iii].GetComponent<ISSCBlock> ());
						CubeBehaviours cb = ccs.cubes [i, ii, iii].AddComponent<CubeBehaviours> ();
						cb.runtimeCore = GetComponent<GameController> ();
						cb.massCore = playersCar.transform.parent.gameObject;
						cb.droped = false;
						cb.catchRadiusParam = 0.0625f;
						cb.forceParam = 0.06f;
					}
				}
			}
		}
		ccs.getAllExist();
//		playersCarTs.localScale = Vector3.one*basicSize;
	}

	GameObject setCube (int id)
	{
		return allKindOfCubes [id];
	}

	Vector3 getPosByIds (int x, int y, int z)
	{
		return new Vector3 (
			originPos.x - (float)x * basicSize,
			originPos.y + (float)y * basicSize,
			originPos.z + (float)z * basicSize
		);
	}

	Vector3 getOriginPos ()
	{
		Vector3 centerPos = playersCarTs.position;
		float offsetX = (float)ccs.sizeX * basicSize;
		float offsetY = (float)ccs.sizeY * basicSize;
		float offsetZ = (float)ccs.sizeZ * basicSize;
		float x, y, z;
		x = centerPos.x + offsetX / 2;
		y = centerPos.y;
		z = centerPos.z - offsetZ / 2;
		return new Vector3 (x, y, z);
//		return centerPos;
	}

}

public class CarsCubes
{
	public int sizeX;

	public int sizeY;

	public int sizeZ;

	public GameObject[,,] cubes;

	public GameObject[] allExist;

	public CarsCubes (int x, int y, int z)
	{
		sizeX = x;
		sizeY = y;
		sizeZ = z;
		cubes = new GameObject[sizeX, sizeY, sizeZ];
	}

	public Collider[] getSphere (GameObject go, float radius)
	{
		radius /= 0.5f;
		List<Collider> cols = new List<Collider> ();
		int x = 0, y = 0, z = 0;
		for (int i = 0; i < sizeX; i++) {
			for (int ii = 0; ii < sizeY; ii++) {
				for (int iii = 0; iii < sizeZ; iii++) {
					if (cubes [i, ii, iii] == null)
						continue;
					if (cubes [i, ii, iii].Equals (go)) {
						x = i;
						y = ii;
						z = iii;
					}
				}
			}
		}



		for (int i = 0; i < sizeX; i++) {
			for (int ii = 0; ii < sizeY; ii++) {
				for (int iii = 0; iii < sizeZ; iii++) {
					if (
						Mathf.Pow (Mathf.Pow (Mathf.Abs ((float)(i - x)), 2) + Mathf.Pow (Mathf.Abs ((float)(ii - y)), 2) + Mathf.Pow (Mathf.Abs ((float)(iii - z)), 2), 0.5f) < radius) {
						if (cubes [i, ii, iii] != null) {
							cols.Add (cubes [i, ii, iii].GetComponent<Collider> ());
						}
					}
				}
			}
		}

		return cols.ToArray ();

	}

	public GameObject[] getAllExist ()
	{
		List<GameObject> gos = new List<GameObject> ();
		for (int i = 0; i < sizeX; i++) {
			for (int ii = 0; ii < sizeY; ii++) {
				for (int iii = 0; iii < sizeZ; iii++) {
					if (cubes [i, ii, iii] == null)
						continue;
					gos.Add (cubes [i, ii, iii]);
				}
			}
		}
		allExist = gos.ToArray();
		return allExist;
	}
}
