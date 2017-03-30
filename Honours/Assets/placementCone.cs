using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placementCone : MonoBehaviour {

	Vector3 Direction, currSection, right;
	Vector2 ConeDirection;
	public Transform target;

	public struct pathpoint
	{
		public Vector3 point;
		public float value;
		float height;
		bool passable;
		public pathpoint (Vector3 realpoint, Vector3 tar )
		{
			point = realpoint;
			value = Vector3.Distance(point, tar);
			height = 0.0f;
			passable = true;
		}
	}

	public List<pathpoint> Pathlist;

	public bool bloop = false;
	public bool boop = false;

	// Use this for initialization
	void Start () {
		Pathlist = new List<pathpoint>();
		Direction = GetComponent<IKRigJoint> ().pos;

		if (bloop) {
			ConeDirection = new Vector2 (transform.position.x - Direction.x, transform.position.z - Direction.z);
			ConeDirection.Normalize ();
			ConeDirection *= 3.0f;

			for (int i = 4; i < 10; i++) {
				for (int j = 0; j <= i-2; j++) {
					currSection = new Vector3 (transform.position.x + (ConeDirection.x * -0.1f * i), transform.position.y+15.0f, transform.position.z + (ConeDirection.y * -0.1f * i));
					right = Vector3.Cross (((new Vector3 (ConeDirection.x, 0.0f, ConeDirection.y)) * -1), Vector3.up.normalized);

					Debug.DrawRay (currSection, 
						(right * 0.1f * j), Color.yellow, 100.0f);
					Debug.DrawRay (currSection + (right * 0.1f * j), Vector3.down*100.0f, Color.black, 100.0f);

					Debug.DrawRay (currSection, 
						(right * -0.1f * j), Color.red, 100.0f);
					Debug.DrawRay (currSection + (right * -0.1f * j), Vector3.down*100.0f, Color.black, 100.0f);

				}
			
			}
			Debug.DrawRay (transform.position, (new Vector3 (ConeDirection.x, 0.0f, ConeDirection.y)) * -1, Color.cyan, 100.0f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (boop) {
			GameObject GO = GameObject.CreatePrimitive (PrimitiveType.Cube);
			//GO.transform.position = Checkpoint ();
			Pathlist.Clear ();
			boop = false;
		}
	}

	public List<Vector3> Checkpoint()
	{
		List<Vector3> PointPasser;
		PointPasser = new List<Vector3>(0);
		Direction = GetComponent<IKRigJoint> ().pos;

			ConeDirection = new Vector2 (transform.position.x - Direction.x, transform.position.z - Direction.z);
			ConeDirection.Normalize ();
			ConeDirection *= 3.0f;

			for (int i = 0; i < 10; i++) {
				for (int j = 4; j <= i-2; j++) {
					currSection = new Vector3 (transform.position.x + (ConeDirection.x * -0.1f * i), transform.position.y, transform.position.z + (ConeDirection.y * -0.1f * i));
					right = Vector3.Cross (((new Vector3 (ConeDirection.x, 0.0f, ConeDirection.y)) * -1), Vector3.up.normalized);

					RaycastHit pathinfo;
					if(Physics.Raycast(currSection + (right * 0.1f * j),Vector3.down,out pathinfo,50.0f))
					{
					Pathlist.Add (new pathpoint(pathinfo.point, target.position));
					}

					if(Physics.Raycast(currSection + (right * -0.1f * j),Vector3.down,out pathinfo,50.0f))
					{
					Pathlist.Add (new pathpoint(pathinfo.point, target.position));
					}
				}

			}
		int lowest = 0;
		for (int p = 0; p < Pathlist.Count; ++p) 
			{
			if (Pathlist[p].value < Pathlist[lowest].value)
				{
					lowest = p;
				}
			}
		PointPasser.Add (transform.position);
		PointPasser.Add (Pathlist[lowest].point);
		return PointPasser;
	}
}
