using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderCam : MonoBehaviour {

	public Transform Spider;
//	Vector3 somePos;
	float distance;


	// Use this for initialization
	void Start () {
		transform.LookAt (Spider);
	}
	
	// Update is called once per frame
	void Update () {
		
		distance = Vector3.Distance(this.transform.position, Spider.transform.position);
 
	if(distance !=10) {
    distance = 10;

			transform.position = (transform.position - Spider.transform.position).normalized * distance + Spider.transform.position;
  	}

		//somePos= Spider.transform.position;
		//transform.RotateAround (somePos, Vector3.up, 20 * Time.deltaTime);

		transform.LookAt (Spider);
	}
}
