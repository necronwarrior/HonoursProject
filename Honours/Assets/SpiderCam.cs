using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpiderCam : MonoBehaviour {

	public Transform Spider;
//	Vector3 somePos;
	float distance;
	public Slider Distslide;

	// Use this for initialization
	void Start () {
		transform.LookAt (Spider);
		GetComponent<Camera> ().targetTexture = (RenderTexture)Resources.Load (gameObject.name);

	}
	
	// Update is called once per frame
	void Update () {
		distance = Distslide.value;

		transform.position = (transform.position - Spider.transform.position).normalized*distance + Spider.transform.position;


		//somePos= Spider.transform.position;
		//transform.RotateAround (somePos, Vector3.up, 20 * Time.deltaTime);

		transform.LookAt (Spider);
	}
}
