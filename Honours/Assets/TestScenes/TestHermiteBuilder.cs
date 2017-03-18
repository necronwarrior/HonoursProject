using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TestHermiteBuilder : MonoBehaviour {
	
	Transform MainBody;
	Vector3 AvPoint;
	public bool noop,once, Parabolas; 
	float gaitTime;
	public Slider slider;
	public GameObject TimerHolder;

	struct TargetMovement{
		public Transform Maintransform, HermiteTransform;
		public Slider GaitVal;

		public bool EnterGait, LeaveGait;
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
			TargetStruct [i].GaitVal = slider.transform.GetChild (i + 2).transform.GetComponent<Slider>();
			TargetStruct [i].LeaveGait = false;
			TargetStruct [i].EnterGait = true;
			ResetMove (i);
		}
		noop= true;
		once = true;
		gaitTime = 0.0f;
	}

	void ResetMove(int i)
	{
		
	}

	// Update is called once per frame
	void Update () {
		
		//if (noop == true){
		for (int i = 0; i < 8; i++) {
			if(TargetStruct[i].GaitVal.value-0.5f>=gaitTime)
			{
				if (TargetStruct [i].EnterGait == true) {
					TargetStruct [i].Maintransform.GetComponent<SplineController> ().RestartSpline ();
					TargetStruct [i].EnterGait = false;
					TargetStruct [i].LeaveGait = true;
				}
			}
			if(TargetStruct[i].GaitVal.value+0.5f<gaitTime)
			{
				if (TargetStruct [i].LeaveGait == true) {
					TargetStruct [i].Maintransform.GetComponent<SplineController> ().RestartSpline ();
					TargetStruct [i].LeaveGait = false;
					TargetStruct [i].EnterGait = true;
				}
			}
		}
		gaitTime += Time.deltaTime;
			//noop = false;
		//}
		if(gaitTime>2.0f){
			gaitTime = 0.0f;
		}
		slider.value = gaitTime;
	}

	void CreateChild(int i, Vector3 nextPoint)
	{
		Instantiate(empty,nextPoint,TargetStruct[i].Maintransform.rotation,TargetStruct[i].HermiteTransform);
	}

	void PointPass(int Limb, Vector3 Targetpoint)
	{
		
	}

}