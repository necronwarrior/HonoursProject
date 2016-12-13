using UnityEngine;
using System.Collections;

[System.Serializable]
public class IKRigJoint
{
	//base Unity transform
	public Transform root;

	public Vector3 position
	{
		get{ return root.position; }
		set{ root.position = value; }
	}

	//constraints for leg movement
	public Vector3 constraintsMax
	{
		get{ return constraintsMax;}
		set{ constraintsMax = value;}
	}

	public Vector3 constraintsMin
	{
		get{ return constraintsMin;}
		set{ constraintsMin = value;}
	}
}