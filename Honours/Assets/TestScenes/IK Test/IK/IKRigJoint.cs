using UnityEngine;
using System.Collections;

public class IKRigJoint : MonoBehaviour
{
	//base Unity transform
	public Transform root;

	public Transform constraintdir ;

	public Vector3 RotOff;
	public Vector3 localrot;

	public Quaternion Quat;

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

	public float aperture =360; 

	void Awake()
	{
		if (constraintdir.GetComponent<MeshRenderer> ()) {
			constraintdir.GetComponent<MeshRenderer> ().enabled = false;
		}
		RotOff = new Vector3 (0,0,0);
		Quat = new Quaternion();

	}

	public void Straighten(Vector3 Prev) {


		transform.LookAt (Prev);

	}
}