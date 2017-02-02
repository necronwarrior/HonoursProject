using UnityEngine;
using System.Collections;

public class IKRigJoint : MonoBehaviour
{
	//base Unity transform
	public Transform root;

	public Transform constraintdir;

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

	//constraints for leg movement
	public Vector4 constraintsVal;

	public float aperture =45; 

	public Vector4 constraints
	{
		get{ return constraintsVal;}
		set{ constraintsVal = value;}
	}
}