using UnityEngine;
using System.Collections;

public class IKRigJoint : MonoBehaviour
{
	//base Unity transform
	public Transform root;

	public Transform constraintdir;

	public Vector3 RotOff;

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

	bool noot;

	void Awake()
	{
		//RotOff = new Vector3 (0,0,0);

	}

	public void Straighten() {
		transform.LookAt (pos);

		transform.Rotate (RotOff);
	}
}