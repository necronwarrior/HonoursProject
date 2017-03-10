using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestHermiteBuilder : MonoBehaviour {
	
	Transform MainBody;
	Vector3 AvPoint;
	public bool noop,once; 

	struct TargetMovement{
		public Transform Maintransform, HermiteTransform;
		public List<Vector3> Targetlist;
		public Vector3 OldTargets;
		public Vector3 ParabolaHeight;
		public float LerpTimer;
	}
		
	GameObject empty;
	TargetMovement[] TargetStruct;

	// Use this for initialization
	void Start () {
		empty = new GameObject ();
		TargetStruct = new TargetMovement[8];
		MainBody = transform.parent.GetChild (0);
		for (int i=0;i<8;i++) {
			TargetStruct [i].HermiteTransform = transform.GetChild (transform.childCount - 1).GetChild (i);
			TargetStruct[i].Maintransform = transform.GetChild (i);
			TargetStruct [i].Maintransform.GetComponent<SplineController> ().StartSpline ();
			ResetMove (i);
		}
		noop= true;
		once = true;

	}

	void ResetMove(int i)
	{
		
	}

	// Update is called once per frame
	void Update () {
		if (noop == true){
			for (int i = 0; i < 8; i++) {
				TargetStruct [i].Maintransform.GetComponent<SplineController> ().RestartSpline ();
			}
			noop = false;
		}
	}

	void CreateChild(int i, Vector3 nextPoint)
	{
		Instantiate(empty,nextPoint,TargetStruct[i].Maintransform.rotation,TargetStruct[i].HermiteTransform);
	}

	void PointPass(int Limb, Vector3 Targetpoint)
	{
		
	}

}