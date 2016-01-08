using UnityEngine;
using System.Collections;

public class V_UISizeChange : MonoBehaviour
{

	public GameObject[] UIs;
	float width, height;
	Vector3 scaleTimes;
	Vector3 tmpV3;

	// Use this for initialization
	void Start ()
	{
		width = Screen.width / 764;
		height = Screen.height / 423;
		scaleTimes = new Vector3 (width, height, 1);
		for (int i = 0; i < UIs.Length; i++) {
			tmpV3 = UIs [i].transform.localScale;
			UIs [i].transform.localScale = new Vector3 (tmpV3.x * scaleTimes.x, tmpV3.y * scaleTimes.y, tmpV3.z * scaleTimes.z);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
