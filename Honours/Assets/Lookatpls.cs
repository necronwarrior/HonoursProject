using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lookatpls : MonoBehaviour {

	public Transform target;
	public Transform attached;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.position = attached.position;
		transform.LookAt (target);
	}
}
