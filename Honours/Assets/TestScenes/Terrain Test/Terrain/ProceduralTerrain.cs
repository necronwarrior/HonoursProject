using UnityEngine;
using System.Collections;

public class ProceduralTerrain : MonoBehaviour {

	UnityEngine.Terrain ProcedTerrain;
	float[,] HeightList;
	float PerlinX, PerlinY;
	private float seed;

	// Use this for initialization
	void Start () {
		ProcedTerrain = GetComponent<UnityEngine.Terrain> ();
		HeightList = new float[512,512];

	}
	
	// Update is called once per frame
	void Update () {
		PerlinX = seed;
		PerlinY = seed;

		for(int i = 0;i<512;i++)
		{
			for(int j = 0;j<512;j++)
			{
				HeightList [i, j] = (Mathf.PerlinNoise ((float)(i/PerlinX),(float)(j/PerlinY)))/10f;
				//noop = Mathf.PerlinNoise (i,j);
			}
		}
		ProcedTerrain.terrainData.SetHeights (0, 0, HeightList);
	}

	public void seedChange(float newVal)
	{
		seed = 512f * newVal;
	}

}
