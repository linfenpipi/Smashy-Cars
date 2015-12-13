using UnityEngine;
using System.Collections;

public class CamRenders : MonoBehaviour
{

	//MouseCasterRender
	public bool selecting;
	public Material lineMat;
	public Vector3 selectStartPoint;
	public Vector3 selectEndPoint;

	void Start ()
	{
		selecting = false;
	}

	void OnPostRender ()
	{
		if (selecting) {
			GL.PushMatrix ();
			lineMat.SetPass (0);
			GL.LoadOrtho ();
			GL.Begin (GL.LINES);
			GL.Color (Color.cyan);
			GL.Vertex (selectStartPoint);
			GL.Vertex (selectEndPoint);
			GL.End ();
			GL.PopMatrix ();
		}
	}
}
