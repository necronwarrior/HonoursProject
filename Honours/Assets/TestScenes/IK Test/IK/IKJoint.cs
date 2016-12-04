using UnityEngine;
using System.Collections;

[System.Serializable]
public class IKRigJoint
{
	public Transform root;
	
	public Vector3 constraintsMax;
	public Vector3 constraintsMin;
	
	public Vector3 position
	{
		get{ return root.position; }
		set{ root.position = value; }
	}
}