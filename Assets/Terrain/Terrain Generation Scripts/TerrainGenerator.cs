using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerrainGenerator:MonoBehaviour {
	public GameObject terrainParent;
	public GameObject water;
	public GameObject sand;
	public GameObject grass;
	public GameObject snow;
	public GameObject tree3x3;
	[Range(0, 1)]
	public float waterHeight = 0.368f;
	[Range(0, 1)]
	public float sandHeight = 0.466f;
	[Range(0, 1)]
	public float grassHeight = 1.0f;

	public int depth = 2;
	public int size = 64;
	public float scale = 3.5f;


	[Range(1, 12)]
	public int octaves = 3;
	public float lacunarity = 3f;
	[Range(0, 1)]
	public float persistance = 0.4f;

	[Range(0, 1)]
	public float treeDensity = 0.03f;

	public Boolean shouldRandomSeed;

	float[,] terrainHeights;

	float seed = -1;

	void Start() {
		GenerateTerrain();
	}

	void Update() {
		if (Input.GetKeyUp("l")) {
			GenerateTerrain();
		}
	}

	public void GenerateTerrain()
	{
		foreach (Transform child in terrainParent.transform)
		{
			GameObject.Destroy(child.gameObject);
		}
		if (seed < 0 || shouldRandomSeed)
		{
			seed = Random.value;
		}
		GenerateCubes();
		GenerateTrees();
	}

	void GenerateTrees() {
		Random.InitState(Mathf.FloorToInt(seed*10000));
		for(int x = 0; x < size; x++) {
			for(int y = 0; y < size; y++) {
				if(Random.value < treeDensity) {
					if(terrainHeights[x, y] > sandHeight) {
						Vector3 position = new Vector3(x, terrainHeights[x, y]*depth, y);
						Quaternion rotation = Quaternion.Euler(0, 0, 0);
						int randomRot = Mathf.FloorToInt(Random.value*4);
						if(randomRot == 1) {
							rotation = Quaternion.Euler(0, 90, 0);
						} else if(randomRot == 2) {
							rotation = Quaternion.Euler(0, 180, 0);
						} else if(randomRot == 3) {
							rotation = Quaternion.Euler(0, 270, 0);
						}
						GameObject newObject = Instantiate(tree3x3, position, rotation);
						newObject.transform.parent = terrainParent.transform;
					}
				}
			}
		}
	}

	void GenerateCubes() {
		terrainHeights = GenerateHeights();
		for(int x = 0; x < size; x++) {
			for(int y = 0; y < size; y++) {
				if(terrainHeights[x, y] < waterHeight) {
					GenerateObject(water, new Vector3(x, 0, y), waterHeight*depth);
				} else if(terrainHeights[x, y] < sandHeight) {
					GenerateObject(sand, new Vector3(x, 0, y), terrainHeights[x, y]*depth);
				} else if(terrainHeights[x, y] < grassHeight) {
					GenerateObject(grass, new Vector3(x, 0, y), terrainHeights[x, y]*depth);
				} else {
					GenerateObject(snow, new Vector3(x, 0, y), terrainHeights[x, y]*depth);
				}
			}
		}
	}

	float[,] GenerateHeights() {
		float[,] heights = new float[size, size];

		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		for (int x = 0; x < size; x++) {
			for(int y = 0; y < size; y++) {
				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;
				for (int i = 0; i < octaves; i++) {
					float xCoord = (float)x/size*scale*frequency;
					float yCoord = (float)y/size*scale*frequency;
					float perlinValue = Mathf.PerlinNoise(xCoord + seed* 10000, yCoord + seed*10000);
					noiseHeight += perlinValue*amplitude;
					amplitude *= persistance;
					frequency *= lacunarity;
				}

				if(noiseHeight > maxNoiseHeight) {
					maxNoiseHeight = noiseHeight;
				}

				if(noiseHeight < minNoiseHeight) {
					minNoiseHeight = noiseHeight;
				}

				heights[x, y] = noiseHeight;
			}
		}

		for(int x = 0; x < size; x++) {
			for(int y = 0; y < size; y++) {
				heights[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, heights[x, y]);
			}
		}

		return heights;
	}

	void GenerateObject(GameObject terrainObject, Vector3 position, float height) {
		terrainObject.transform.localScale = new Vector3(1f, height, 1f);
		position = new Vector3(position.x, terrainObject.transform.localScale.y/2, position.z);
		GameObject newObject = Instantiate(terrainObject, position, Quaternion.identity);
		newObject.transform.parent = terrainParent.transform;
	}

	Vector3[] GetCornerVerticies(float[,] terrainHeights, int x, int y) {
		Vector3[] cornerVerticies = new Vector3[4];
		cornerVerticies[0] = new Vector3((float)x - 0.5f, terrainHeights[x, y]*depth, (float)y - 0.5f);
		cornerVerticies[1] = new Vector3((float)x - 0.5f, terrainHeights[x, y]*depth, (float)y + 0.5f);
		cornerVerticies[2] = new Vector3((float)x + 0.5f, terrainHeights[x, y]*depth, (float)y - 0.5f);
		cornerVerticies[3] = new Vector3((float)x + 0.5f, terrainHeights[x, y]*depth, (float)y + 0.5f);
		return cornerVerticies;
	}
}
