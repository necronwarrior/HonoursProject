using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TestHermiteBuilder : MonoBehaviour {
	
	Transform MainBody;
	Vector3 AvPoint;
	public bool noop,once, Parabolas, start; 
	float gaitTime;
	public Slider slider;
	public GameObject TimerHolder;

	float Tmin, Tmax;

	struct TargetMovement{
		public Transform Maintransform, HermiteTransform;
		public Slider GaitVal;

		public bool EnterGait, LeaveGait;
	}
		
	GameObject empty;
	TargetMovement[] TargetStruct;

	// Use this for initialization
	void Start () {
		start = false;
		empty = new GameObject ();
		TargetStruct = new TargetMovement[8];
		MainBody = transform.parent.GetChild (0);
		for (int i=0;i<8;i++) {
			TargetStruct [i].HermiteTransform = transform.GetChild (transform.childCount - 1).GetChild (i);
			TargetStruct[i].Maintransform = transform.GetChild (i);
			if (Parabolas == false) {
				TargetStruct [i].Maintransform.GetComponent<SplineController> ().AutoClose = false;
			}
			TargetStruct [i].Maintransform.GetComponent<SplineController> ().StartSpline ();
			TargetStruct [i].GaitVal = slider.transform.GetChild (i + 2).transform.GetComponent<Slider>();
			TargetStruct [i].LeaveGait = false;
			TargetStruct [i].EnterGait = true;
		
			ResetMove (i);
		}
		noop= true;
		once = true;
		gaitTime = 0.0f;
		if (Parabolas != true) {
		}
	}

	void ResetMove(int i)
	{
		
	}

	// Update is called once per frame
	void Update () {
		
		if (start == true) {
			Tmin = gaitTime - (Time.deltaTime / 2);
			Tmax = gaitTime + (Time.deltaTime / 2);
			for (int i = 0; i < 8; i++) {
				if (TargetStruct [i].GaitVal.value - 0.5f > Tmin &&
					TargetStruct [i].GaitVal.value - 0.5f < Tmax ) {
					if (TargetStruct [i].EnterGait == true) {
						TargetStruct [i].Maintransform.GetComponent<SplineController> ().RestartSpline ();
						TargetStruct [i].EnterGait = false;
						TargetStruct [i].LeaveGait = true;
					}
				}
				if (TargetStruct [i].GaitVal.value + 0.5f > Tmin &&
					TargetStruct [i].GaitVal.value + 0.5f < Tmax) {
					if (TargetStruct [i].LeaveGait == true) {
						if (Parabolas==false) {
							for(int j = 0;j<TargetStruct[i].HermiteTransform.childCount-1;j++)
							{
								GameObject.Destroy(TargetStruct[i].HermiteTransform.GetChild(j).gameObject);
								//insert new point finding

							}
						}
						//TargetStruct [i].Maintransform.GetComponent<SplineController> ().RestartSpline ();
						TargetStruct [i].LeaveGait = false;
						TargetStruct [i].EnterGait = true;
					}
				}
			}
			gaitTime += Time.deltaTime;
			//noop = false;
			//}
			if (gaitTime > 2.2f) {
				gaitTime = -0.2f;
			}
			slider.value = gaitTime;
		}
	}

	void CreateChild(int i, Vector3 nextPoint)
	{
		Instantiate(empty,nextPoint,TargetStruct[i].Maintransform.rotation,TargetStruct[i].HermiteTransform);
	}

	void PointPass(int Limb, Vector3 Targetpoint)
	{
		
	}

	public void StartWalk()
	{
		start = true;
	}

}