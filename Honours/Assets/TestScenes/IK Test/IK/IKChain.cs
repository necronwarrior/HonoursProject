using UnityEngine;
using System.Collections;

[System.Serializable]
public class IKChain
{
	//list of joints to be populated
	public Transform[] joints;

	//target point for the solution to aim for (foot)
	public Transform target;

	//these variables will not show in the inspector
	[System.NonSerialized]
	public float[] segLengths;
	
	[System.NonSerialized]
	public float length;
	

	public IKChain(ref Transform[] Joints, ref Transform Target)
	{
		joints = Joints;
		target = Target;
	}
	
	public void Init()
	{
		segLengths = new float[joints.Length];
		
		for (int i = 0; i < joints.Length - 1; i++)
		{
			float dist = (joints[i].position - joints[i+1].position).magnitude;
			
			segLengths[i] = dist;
			length += dist;
		}
	}
	
	public void DebugDraw(Color c)
	{
		for (int i = 0; i < joints.Length - 1; i++)
		{
			Debug.DrawLine(joints[i].position, joints[i+1].position, c);
		}
	}
}