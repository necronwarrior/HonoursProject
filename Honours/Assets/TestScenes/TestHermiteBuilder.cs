using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestHermiteBuilder : MonoBehaviour {
	
	Transform MainBody;
	Vector3 AvPoint;
	bool noop,once; 

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
			ResetMove (i);
		}
		noop= true;
		once = true;
	}

	void ResetMove(int i)
	{
		TargetStruct [i].OldTargets = TargetStruct [i].Maintransform.position; 
	}

	// Update is called once per frame
	void FixedUpdate () {
		AvPoint = new Vector3(0.0f,640.0f,0.0f);
		if(noop == true&&
			once == true)
		{
			for (int i=0;i<4;i++) {	
				AvPoint += TargetStruct[i].Maintransform.transform.position;
				AvPoint += TargetStruct[i+4].Maintransform.transform.position;
				CreateChild (i, new Vector3 (0, 0, 100));

				TargetStruct [i].Maintransform.GetComponent<SplineController> ().UpdateTransforms ();
				TargetStruct [i].Maintransform.GetComponent<SplineController> ().FollowSpline ();
				once = false;
				if (TargetStruct [i].Maintransform.position == TargetStruct [i].HermiteTransform.GetChild(TargetStruct [i].HermiteTransform.childCount-1).position) {
					noop = false;
					once = true;
				}
			}

		}
		else{
			if (once == true) {
				for (int i = 4; i < 8; i++) {		
					AvPoint += TargetStruct [i].Maintransform.transform.position;
					AvPoint += TargetStruct [i - 4].Maintransform.transform.position;
					CreateChild (i, new Vector3 (0, 0, 100));

					TargetStruct [i].Maintransform.GetComponent<SplineController> ().UpdateTransforms ();
					TargetStruct [i].Maintransform.GetComponent<SplineController> ().FollowSpline ();

					once = false;
					if (TargetStruct [i].Maintransform.position == TargetStruct [i].HermiteTransform.GetChild (TargetStruct [i].HermiteTransform.childCount - 1).position) {
						noop = true;
						once = true;
					}
				}

				if (noop == true) {
					for (int i = 0; i < 8; i++) {
						ResetMove (i);
					}
				}
			}
		}



		AvPoint /= 8;
		MainBody.position = AvPoint;
	}

	void CreateChild(int i, Vector3 nextPoint)
	{
		Instantiate(empty,nextPoint,TargetStruct[i].Maintransform.rotation,TargetStruct[i].HermiteTransform);
	}
}
