using UnityEngine;
using System.Collections;

public class TestParabolamaker : MonoBehaviour {
	
	Transform MainBody;
	Vector3 AvPoint;
	bool noop; 

	struct TargetMovement{
		public Transform Maintransform;
		public Vector3 NewTargets;
		public Vector3 OldTargets;
		//public Vector3 ParabolaHeight;
		public float LerpTimer;
	}

	TargetMovement[] TargetStruct;

	// Use this for initialization
	void Start () {
		MainBody = transform.parent;
		TargetStruct = new TargetMovement[8];
		for (int i=0;i<8;i++) {
			TargetStruct[i].Maintransform = transform.GetChild (i);
			TargetStruct [i].OldTargets = TargetStruct[i].Maintransform.localPosition -(MainBody.forward*50.0f) ;
			ResetMove (i);
		}
		noop= true;
	}

	void ResetMove(int i)
	{
		TargetStruct [i].Maintransform.localPosition = TargetStruct [i].OldTargets;
		TargetStruct [i].NewTargets = TargetStruct [i].Maintransform.localPosition + (MainBody.right*-100.0f);
		//TargetStruct[i].OldTargets = TargetStruct[i].Maintransform.localPosition;
		TargetStruct [i].LerpTimer = 0.0f;
	}

	void ParabolaLerp(int index)
	{
		//TargetStruct[index].ParabolaHeight = (TargetStruct[index].OldTargets +(TargetStruct[index].OldTargets-TargetStruct[index].NewTargets))*0.5f;
		//TargetStruct [index].ParabolaHeight += TargetStruct [index].Maintransform.up * 3.0f;

		if (TargetStruct [index].LerpTimer < 1.0f) {
			float height = Mathf.Sin (Mathf.PI * TargetStruct [index].LerpTimer) * 30.0f;

			TargetStruct [index].Maintransform.localPosition = Vector3.Lerp (TargetStruct [index].OldTargets, TargetStruct [index].NewTargets, TargetStruct [index].LerpTimer) + TargetStruct [index].Maintransform.up * height;
		} else {
			if (TargetStruct [index].LerpTimer > 1.0f) {
				TargetStruct [index].Maintransform.localPosition = TargetStruct [index].NewTargets;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if(noop == true)
		{
			for (int i=0;i<4;i++) {
				ParabolaLerp (i);		
				AvPoint += TargetStruct[i].Maintransform.transform.localPosition;
				AvPoint += TargetStruct[i+4].Maintransform.transform.localPosition;
				TargetStruct [i].LerpTimer += Time.deltaTime*1.0f;

				if (TargetStruct [i].Maintransform.localPosition == TargetStruct [i].NewTargets) {
					noop = false;
				}
			}

		}
		else{
			for (int i=4;i<8;i++) {
				ParabolaLerp (i);			
				AvPoint += TargetStruct[i].Maintransform.transform.localPosition;
				AvPoint += TargetStruct[i-4].Maintransform.transform.localPosition;
				TargetStruct [i].LerpTimer += Time.deltaTime*1.0f;

				if (TargetStruct [i].Maintransform.localPosition == TargetStruct [i].NewTargets) {
					noop = true;
				}
			}

			if (noop ==true) {
				for (int i=0;i<8;i++) {
					ResetMove (i);
				}
			}

		}
	}
}
