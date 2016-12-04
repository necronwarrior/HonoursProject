using UnityEngine;
using System.Collections;

public class BasicMovement : MonoBehaviour {


	float Timer;

	bool R_Gait, L_Gait;

	// Use this for initialization
	void Start () {
		Timer = 0.0f ;
		R_Gait = true;
		L_Gait = false;
	}
	
	// Update is called once per frame
	void Update () {
		Timer += Time.deltaTime;

		if (Timer >= 0.2f) {
			foreach (Transform child in transform) {
				if (child.tag == "R_Upper") {
					if (R_Gait == true &&
						(child.name == "RF1" || child.name ==  "RB1"))
					{
						child.Rotate (new Vector3 (0.0f, 80.0f, 0.0f));
					}

					if (R_Gait == false &&
						child.name == ("RM1")) {
						child.Rotate (new Vector3 (0.0f, 80.0f, 0.0f));
					}

					R_Gait = !R_Gait;
				}

				if (child.tag == "L_Upper") {

					if (L_Gait == false &&
						(child.name == "LF1" || child.name == "LB1")) {
						child.Rotate (new Vector3 (0.0f, -80.0f, 0.0f));
					}

					if (L_Gait == true &&
					    child.name == ("LM1")) {
						child.Rotate (new Vector3 (0.0f, -80.0f, 0.0f));
					}
					L_Gait = !L_Gait;
				}
			}
			Timer = 0.0f;
		}
	}
}
