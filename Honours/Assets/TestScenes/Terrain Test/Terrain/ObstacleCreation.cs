using UnityEngine;
using System.Collections;
using System.Net;

public class ObstacleCreation : MonoBehaviour {

	public int NumberOfObstacles;
	public float speed = 1;
	public bool Rand = false;
	// Use this for initialization
	void Start () {

		if (Rand) {
			for (int i = 0; i < NumberOfObstacles; i++) {
				GameObject Obstacle = (GameObject)Instantiate (Resources.Load ("Obstacle1"));
				Obstacle.GetComponent<ObstacleMovement> ().RandInit (GetComponent<Terrain> ().GetPosition (),
					GetComponent<Terrain> ().GetPosition () + GetComponent<Terrain> ().terrainData.size, speed);

			}
		} else {
			ReadBMP ();
		}
	}

	void ReadBMP()
	{
		Texture2D ObjectBitmap = Resources.Load( "ObjectBitmap" ) as Texture2D;
		for(int i = 0; i<512; i++)
		{
			for(int j = 0; j<512; j++)
			{
				if (ObjectBitmap.GetPixel (i, j).linear == Color.black) {
					GameObject Obstacle = (GameObject)Instantiate (Resources.Load ("Obstacle1"));
					Obstacle.GetComponent<ObstacleMovement> ().ControlledInit (new Vector3(GetComponent<Terrain> ().GetPosition ().x + i,10.0f, GetComponent<Terrain> ().GetPosition ().z+j),1.0f, false);
				}
			}
		}
	}
}
