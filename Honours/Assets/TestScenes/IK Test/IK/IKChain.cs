using UnityEngine;
using System.Collections;

[System.Serializable]
public class IKChain
{

	//list of joints to be populated
	public IKRigJoint[] joints;

	//target point for the solution to aim for (foot)
	public IKRigJoint target;

	//these variables will not show in the inspector, however we want them to be accessed publicly
	[System.NonSerialized]
	public float[] segmentLengths;
	
	[System.NonSerialized]
	public float length;
	
	//use simple transforms for making the basics of the IK solver
	public IKChain(IKRigJoint[] Joints, IKRigJoint Target)
	{
		joints = Joints;
		target = Target;
	}

	//populate lists
	public void Init()
	{
		segmentLengths = new float[joints.Length];
		
		for (int i = 0; i < joints.Length - 1; i++)
		{
			float dist = (joints[i].position - joints[i+1].position).magnitude;
			
			segmentLengths[i] = dist;
			length += dist;
		}
	}

	//for visual aid
	public void DebugDraw(Color c)
	{
		for (int i = 0; i < joints.Length - 1; i++)
		{
			Debug.DrawLine(joints[i].position, joints[i+1].position, c);
		}
	}
}