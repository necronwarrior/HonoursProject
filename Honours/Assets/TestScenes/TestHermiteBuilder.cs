using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class TestHermiteBuilder : MonoBehaviour {
	
	Transform MainBody;
	Vector3 AvPoint, finalpoint;
	public bool noop,once, Parabolas, start; 
	public float startspeed = 0.0f;
	float gaitTime;
	public Slider slider;
	public GameObject TimerHolder, spliner;
	int tooter;
	float Tmin, Tmax;

	struct TargetMovement{
		public Transform Maintransform, HermiteTransform;
		public Slider GaitVal;
		public GameObject Hip;
		public bool EnterGait, LeaveGait;
		public Vector3 lastpoint;
		public List<Vector3>  splinelist;
	}
		
	GameObject empty;
	TargetMovement[] TargetStruct;

	// Use this for initialization
	void Start () {

		tooter = 0;
		start = false;
		empty = new GameObject ();
		TargetStruct = new TargetMovement[8];
		MainBody = transform.parent.GetChild (0);
		for (int i=0;i<8;i++) {
			TargetStruct [i].Hip = transform.parent.transform.GetChild (0).GetChild (transform.parent.transform.GetChild (0).childCount - 2).GetChild (i).gameObject;
			TargetStruct [i].HermiteTransform = spliner.transform.GetChild (i);
			TargetStruct[i].Maintransform = transform.GetChild (i);
			if (Parabolas == false) {
				TargetStruct [i].Maintransform.GetComponent<SplineController> ().AutoClose = false;
			}
			TargetStruct [i].Maintransform.GetComponent<SplineController> ().StartSpline ();
			TargetStruct [i].GaitVal = slider.transform.GetChild (i + 2).transform.GetComponent<Slider>();
			TargetStruct [i].LeaveGait = false;
			TargetStruct [i].EnterGait = true;

			TargetStruct [i].splinelist = new List<Vector3> (0);
			TargetStruct [i].splinelist.Add(TargetStruct [i].HermiteTransform.position);
			if (Parabolas == false) {

				finalpoint = TargetStruct [i].Maintransform.position;
				for (int j = 0; j < TargetStruct [i].HermiteTransform.childCount ; j++) {
					GameObject.Destroy (TargetStruct [i].HermiteTransform.GetChild (j).gameObject);
					//insert new point finding

				}
				CreateChild (i, TargetStruct [i].splinelist[0]);
			}
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
		
		if (start == true) {
			Tmin = gaitTime - (Time.deltaTime / 2);
			Tmax = gaitTime + (Time.deltaTime / 2);
			for (int i = 0; i < 8; i++) {
				if (TargetStruct [i].GaitVal.value - 0.5f > Tmin &&
				    TargetStruct [i].GaitVal.value - 0.5f < Tmax) {
					if (TargetStruct [i].EnterGait == true) {
						if (Parabolas == false) {
							//GameObject.Destroy (TargetStruct [i].HermiteTransform.GetChild (0).gameObject);

							TargetStruct [i].splinelist = TargetStruct [i].Hip.GetComponent<placementCone> ().Checkpoint ();

							/*CreateChild (i, new Vector3((TargetStruct [i].lastpoint.x+TargetStruct[i].Maintransform.position.x)/2,
								((TargetStruct [i].lastpoint.y+TargetStruct[i].Maintransform.position.y)/2)+1.0f,
								(TargetStruct [i].lastpoint.z+TargetStruct[i].Maintransform.position.z)/2));*/
							for (int p = 0; p < TargetStruct [i].splinelist.Count; p++) {
								CreateChild (i, TargetStruct [i].splinelist [p]);
								
							}
							TargetStruct [i].Maintransform.GetComponent<SplineController> ().RestartSpline (1.0f);

						} else {
							TargetStruct [i].Maintransform.GetComponent<SplineController> ().RestartSpline (1.0f);
						}
						TargetStruct [i].EnterGait = false;
						TargetStruct [i].LeaveGait = true;
					}
				}
				if (TargetStruct [i].GaitVal.value + 0.5f > Tmin &&
				    TargetStruct [i].GaitVal.value + 0.5f < Tmax) {
					if (TargetStruct [i].LeaveGait == true) {
						if (Parabolas == false) {
							
							for (int j = 0; j < TargetStruct [i].HermiteTransform.childCount - 1;) {
								GameObject.DestroyImmediate (TargetStruct [i].HermiteTransform.GetChild (j).gameObject);
								//insert new point finding

							}
							//TargetStruct [i].Maintransform.position = TargetStruct [i].HermiteTransform.GetChild (0).position;
						} else {
							//TargetStruct [i].Maintransform.GetComponent<SplineController> ().RestartSpline (1.0f);
						}
						//TargetStruct [i].Maintransform = TargetStruct [i].HermiteTransform.GetChild (i);

						TargetStruct [i].LeaveGait = false;
						TargetStruct [i].EnterGait = true;
					}
				}
				if (TargetStruct [i].EnterGait == true) {
					TargetStruct [i].Maintransform.position = TargetStruct [i].HermiteTransform.GetChild (0).position;
				}
			}
			gaitTime = slider.value;
		} 					
	}

	void CreateChild(int i, Vector3 nextPoint)
	{
		tooter++;
		empty.name = "L" + tooter;
		Instantiate(empty,nextPoint,TargetStruct[i].HermiteTransform.rotation,TargetStruct[i].HermiteTransform);
	}

	public void StartWalk()
	{
		start = true;
		transform.parent.GetComponent<NavMeshAgent> ().speed = startspeed;
	}

}