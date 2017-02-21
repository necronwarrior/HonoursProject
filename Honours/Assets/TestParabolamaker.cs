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
		public Vector3 ParabolaHeight;
		public float LerpTimer;
	}

	TargetMovement[] TargetStruct;

	// Use this for initialization
	void Start () {
		TargetStruct = new TargetMovement[8];
		MainBody = transform.parent.GetChild (0);
		for (int i=0;i<8;i++) {
			TargetStruct[i].Maintransform = transform.GetChild (i);
			ResetMove (i);
		}
		noop= true;
	}

	void ResetMove(int i)
	{
		TargetStruct[i].NewTargets = new Vector3 (TargetStruct[i].Maintransform.position.x, TargetStruct[i].Maintransform.position.y, TargetStruct[i].Maintransform.position.z + 100);
		TargetStruct[i].OldTargets = TargetStruct[i].Maintransform.position;
		TargetStruct [i].LerpTimer = 0.0f;
	}

	void ParabolaLerp(int index)
	{
		//TargetStruct[index].ParabolaHeight = (TargetStruct[index].OldTargets +(TargetStruct[index].OldTargets-TargetStruct[index].NewTargets))*0.5f;
		//TargetStruct [index].ParabolaHeight += TargetStruct [index].Maintransform.up * 3.0f;

		if (TargetStruct [index].LerpTimer < 1.0f) {
			float height = Mathf.Sin (Mathf.PI * TargetStruct [index].LerpTimer) * 30.0f;

			TargetStruct [index].Maintransform.position = Vector3.Lerp (TargetStruct [index].OldTargets, TargetStruct [index].NewTargets, TargetStruct [index].LerpTimer) + TargetStruct [index].Maintransform.up * height;
		} else {
			if (TargetStruct [index].LerpTimer > 1.0f) {
				TargetStruct [index].Maintransform.position = TargetStruct [index].NewTargets;
			}
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		AvPoint = new Vector3(0.0f,640.0f,0.0f);
		if(noop == true)
		{
			for (int i=0;i<4;i++) {
				ParabolaLerp (i);		
				AvPoint += TargetStruct[i].Maintransform.transform.position;
				AvPoint += TargetStruct[i+4].Maintransform.transform.position;
				TargetStruct [i].LerpTimer += Time.deltaTime*2.0f;

				if (TargetStruct [i].Maintransform.position == TargetStruct [i].NewTargets) {
					noop = false;
				}
			}

		}
		else{
			for (int i=4;i<8;i++) {
				ParabolaLerp (i);			
				AvPoint += TargetStruct[i].Maintransform.transform.position;
				AvPoint += TargetStruct[i-4].Maintransform.transform.position;
				TargetStruct [i].LerpTimer += Time.deltaTime*2.0f;

				if (TargetStruct [i].Maintransform.position == TargetStruct [i].NewTargets) {
					noop = true;
				}
			}

			if (noop ==true) {
				for (int i=0;i<8;i++) {
					ResetMove (i);
				}
			}

		}

		AvPoint /= 8;
		MainBody.position = AvPoint;
	}
}
