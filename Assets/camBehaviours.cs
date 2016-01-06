using UnityEngine;
using System.Collections;

public class camBehaviours : MonoBehaviour
{
	public GameController gc;
	public Transform father;
	Vector3 defaultPosition;
	Quaternion defaultRotation;
	bool lastGameState = false;
	float timer = 0;
	// Use this for initialization
	void Start ()
	{
		defaultPosition = transform.localPosition;
		defaultRotation = transform.localRotation;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!gc.gaming) {
			transform.LookAt (father);
			transform.RotateAround (father.position, Vector3.up, 30 * Time.deltaTime);
		} else {
			if (lastGameState != gc.gaming) {
				timer = Time.time;
			}
			transform.localPosition = Vector3.Slerp (transform.localPosition, defaultPosition, (Time.time - timer)/2);
			transform.localRotation = Quaternion.Slerp (transform.localRotation, defaultRotation, (Time.time - timer)/2);
		}
		lastGameState = gc.gaming;
	}
}
