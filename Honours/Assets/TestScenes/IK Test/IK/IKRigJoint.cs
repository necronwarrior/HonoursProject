using UnityEngine;
using System.Collections;

public class IKRigJoint : MonoBehaviour
{
	//base Unity transform
	public Transform root;

	public Transform constraintdir ;

	GameObject realdir;

	public Vector3 RotOff;
	public Vector3 localrot;

	public Quaternion Quat;

	bool once;

	public Vector3 position
	{
		get{ return root.position; }
		set{ root.position = value; }
	}

	public Vector3 pos
	{
		get{ return constraintdir.position; }
		set{ constraintdir.position = value; }
	}

	public float aperture =45; 

	void Awake()
	{
		RotOff = new Vector3 (0,0,0);
		once = true;
		Quat = new Quaternion();
		realdir = new GameObject ();

	}

	public void Straighten(Vector3 Prev) {



	
		if(once==true)
		{
			realdir.transform.forward = transform.position - Prev;

			realdir.transform.up = Vector3.RotateTowards (realdir.transform.forward, transform.position + Vector3.up, 90.0f, 1.0f);
			once = false;
		}
		realdir.transform.LookAt (Prev);
		localrot = transform.localRotation.eulerAngles ;
		transform.localRotation = realdir.transform.rotation;

		//transform.forward = transform.position - Prev;
		Debug.DrawLine( transform.position, Prev, Color.blue);

	
		//transform.rotation = ToPrev;

		//transform.Rotate (-RotOff);
	}
}