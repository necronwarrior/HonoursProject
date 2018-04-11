using UnityEngine;
using System.Collections;

public class ProceduralTerrain : MonoBehaviour {

	UnityEngine.Terrain ProcedTerrain;
	float[,] HeightList;
	float PerlinX, PerlinY;
	private float seed;
	enum TerrainType {Perlin, Bitmap,None};
	TerrainType Current;
	bool PerlinChange;

	// Use this for initialization
	void Start () {
		ProcedTerrain = GetComponent<UnityEngine.Terrain> ();
		HeightList = new float[512,512];
		seed = -1;
		Current = TerrainType.None;
		PerlinChange = false;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void heightmapLoad ()
	{
		Current = TerrainType.Bitmap;
		if(Current == TerrainType.Bitmap)
		{
			ReadBMP ();
		}

		ProcedTerrain.terrainData.SetHeights (0, 0, HeightList);
	}

	public void seedChange(float newVal)
	{
		if (Current == TerrainType.Perlin) {
			seed = 512f * newVal;
			PerlinX = seed;
			PerlinY = seed;

			for (int i = 0; i < 512; i++) {
				for (int j = 0; j < 512; j++) {
					HeightList [i, j] = (Mathf.PerlinNoise ((float)(i / PerlinX), (float)(j / PerlinY))) / 10f;
					//noop = Mathf.PerlinNoise (i,j);
				}
			}
		}
		ProcedTerrain.terrainData.SetHeights (0, 0, HeightList);
	}

	public void perlinChange()
	{
		PerlinChange = !PerlinChange;
		if (PerlinChange == true) {
			Current = TerrainType.Perlin;
		} else {
			Current = TerrainType.None;
		}
	}

	void ReadBMP()
	{
		Texture2D ObjectBitmap = Resources.Load( "HeightBitmap1" ) as Texture2D;
		for(int i = 0; i<512; i++)
		{
			for(int j = 0; j<512; j++)
			{

				HeightList [i, j] = (ObjectBitmap.GetPixel (i, j).grayscale/10.0f);
				
			}
		}
	}
}
