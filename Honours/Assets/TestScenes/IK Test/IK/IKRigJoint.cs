using UnityEngine;
using System.Collections;

public class IKRigJoint : MonoBehaviour
{
	//base Unity transform
	public Transform root;

	public Vector3 position
	{
		get{ return root.position; }
		set{ root.position = value; }
	}

	//constraints for leg movement
	public Vector4 constraintsVal;


	public Vector4 constraints
	{
		get{ return constraintsVal;}
		set{ constraintsVal = value;}
	}
}