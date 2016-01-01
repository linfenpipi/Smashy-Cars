using UnityEngine;
using System.Collections;
using LitJson;
using System.IO;

public class GameController : MonoBehaviour
{
//	public GameObject cubes;
	public GameObject playersCar;
	Transform playersCarTs;
	public float basicSize;
	CarsCubes ccs;
	public string fileName;
	JsonData json;
	ISSCDBlocksList cubeList;
	GameObject[] allKindOfCubes;
	Vector3 originPos;
	bool gaming = false;

	void Start ()
	{
		playersCarTs = playersCar.transform;
		cubeList = ISSCDBlocksList.LoadList ();
		allKindOfCubes = new GameObject[cubeList.blocks.Length];
		for (int i = 0; i < allKindOfCubes.Length; i++) {
			allKindOfCubes [i] = cubeList.blocks [i].gameObject;
		}


		json = ISSCDGridFileUtilities.LoadFileAsJson (Application.dataPath + "/Resources/SavedDatas/", fileName);
		ccs = new CarsCubes (int.Parse (json ["size"] ["x"].ToString ()), int.Parse (json ["size"] ["y"].ToString ()), int.Parse (json ["size"] ["z"].ToString ()));
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Space)&&!gaming){
		gaming = true ;
		spawnCubes();
		}
	}

	void spawnCubes(){
		playersCar.transform.parent.position += Vector3.up*(basicSize*(float)ccs.sizeY/2+1);
//		playersCar.transform.parent.GetComponent<Collider>().isTrigger = true;
		originPos = getOriginPos();
		for (int i = 0; i < ccs.sizeX; i++) {
			for (int ii = 0; ii < ccs.sizeY; ii++) {
				for (int iii = 0; iii < ccs.sizeZ; iii++) {
//					Debug.Log(i * ccs.sizeX + ii * ccs.sizeY + iii);
					int id = int.Parse (json ["blocks"] [(i * ccs.sizeX * ccs.sizeY + ii * ccs.sizeY + iii).ToString ()].ToString ());
					if (id >= 2) {
						ccs.cubes [i, ii, iii] = Instantiate (setCube (id),getPosByIds(i,ii,iii),Quaternion.identity) as GameObject;
						ccs.cubes [i,ii,iii].transform.parent = playersCarTs;
						ccs.cubes [i,ii,iii].transform.localScale = Vector3.one*basicSize;
					}
				}
			}
		}
//		playersCarTs.localScale = Vector3.one*basicSize;
	}

	GameObject setCube (int id)
	{
		return allKindOfCubes [id];
	}

	Vector3 getPosByIds(int x,int y,int z){
		return new Vector3 (
		originPos.x-(float)x*basicSize,
		originPos.y+(float)y*basicSize,
		originPos.z+(float)z*basicSize
		);
	} 

	Vector3 getOriginPos (){
		Vector3 centerPos = playersCarTs.position;
		float offsetX = (float)ccs.sizeX*basicSize;
		float offsetY = (float)ccs.sizeY*basicSize;
		float offsetZ = (float)ccs.sizeZ*basicSize;
		float x,y,z;
		x = centerPos.x + offsetX/2 ;
		y = centerPos.y ;
		z = centerPos.z - offsetZ/2;
		return new Vector3 (x,y,z);
//		return centerPos;
	}

}

public class CarsCubes
{
	public int sizeX;

	public int sizeY;

	public int sizeZ;

	public GameObject[,,] cubes;

	public CarsCubes (int x, int y, int z)
	{
		sizeX = x;
		sizeY = y;
		sizeZ = z;
		cubes = new GameObject[sizeX, sizeY, sizeZ];
	}
}
