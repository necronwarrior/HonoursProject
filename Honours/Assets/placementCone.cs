using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placementCone : MonoBehaviour {

	Vector3 Direction, currSection, right;
	Vector2 ConeDirection;
	public Transform target;
	public Transform FootTarget;

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

	int RowMin, RowMax;

	float sections ,negsection;

	// Use this for initialization
	void Start () {
		RowMin = 8;
		RowMax = 20;
		sections = (float)(1.0f / RowMax);
		negsection = -1 * (float)(1.0f / RowMax);
		Pathlist = new List<pathpoint>();
		Direction = GetComponent<IKRigJoint> ().pos;

		if (bloop) {
			ConeDirection = new Vector2 (transform.position.x - Direction.x, transform.position.z - Direction.z);
			ConeDirection.Normalize ();
			ConeDirection *= 2.0f; //distance



			for (int Rows = RowMin; Rows < RowMax; Rows++) {
				for (int Columns = 0; Columns <= Rows-2; Columns++) {
					currSection = new Vector3 (transform.position.x + (ConeDirection.x * negsection * Rows), transform.position.y+15.0f, transform.position.z + (ConeDirection.y * negsection * Rows));
					right = Vector3.Cross (((new Vector3 (ConeDirection.x, 0.0f, ConeDirection.y)) * -1), Vector3.up.normalized);

					Debug.DrawRay (currSection, 
						(right * sections * Columns), Color.yellow, 100.0f);
					Debug.DrawRay (currSection + (right * sections * Columns), Vector3.down*100.0f, Color.black, 100.0f);

					Debug.DrawRay (currSection, 
						(right * negsection * Columns), Color.red, 100.0f);
					Debug.DrawRay (currSection + (right * negsection * Columns), Vector3.down*100.0f, Color.black, 100.0f);

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
			ConeDirection *= 2.0f;

		for (int Rows = RowMin; Rows < RowMax; Rows++) {
			for (int Columns = 0; Columns <= Rows-2; Columns++) {
				currSection = new Vector3 (transform.position.x + (ConeDirection.x * negsection * Rows), transform.position.y, transform.position.z + (ConeDirection.y * negsection * Rows));
					right = Vector3.Cross (((new Vector3 (ConeDirection.x, 0.0f, ConeDirection.y)) * -1), Vector3.up.normalized);

					RaycastHit pathinfo;
				if(Physics.Raycast(currSection + (right * sections * Columns),Vector3.down,out pathinfo,50.0f))
					{
					if (pathinfo.collider.tag != "Self") {
							Pathlist.Add (new pathpoint (pathinfo.point, target.position));
						}
					}

				if(Physics.Raycast(currSection + (right * negsection * Columns),Vector3.down,out pathinfo,50.0f))
					{
					if (pathinfo.collider.tag !=  "Self") {
						Pathlist.Add (new pathpoint (pathinfo.point, target.position));
						}
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

		Vector3 midpoint = new Vector3 ((FootTarget.position.x+Pathlist[lowest].point.x)/2,((FootTarget.position.y+Pathlist[lowest].point.y)/2)+2.0f,(FootTarget.position.z+Pathlist[lowest].point.z)/2);
		PointPasser.Add (midpoint);
		//PointPasser.Add (transform.position);
		PointPasser.Add (Pathlist[lowest].point);

		for (int h = 0; h < PointPasser.Count; ++h) 
		{
			PointPasser [h].Set(PointPasser [h].x,PointPasser [h].y+1.0f,PointPasser [h].z);
		}
		return PointPasser;
	}
}
