using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemappingObjectPlacer : MonoBehaviour
{
	public Vector2 Start = Vector2.zero;
	public Vector2 End = new Vector2(30f, 30f);
	public int CubeCount = 1000;
	public float CubeSize = 1f;
	public int SphereCount = 20;
	public float SphereSize = 2f;

	[ContextMenu("Spawn Cubes")]
	public void SpawnCubes()
	{
		if (transform.Find("Cubes")) {
			if (Application.isPlaying) Destroy(transform.Find("Cubes").gameObject);
			else DestroyImmediate(transform.Find("Cubes").gameObject);
		}
		var cubeParent = new GameObject("Cubes");
		cubeParent.transform.parent = transform;

		//We use the square root here just so we can provide an absolute number of cubes and have each side
		//have the correct number of cubes to make the total count match this value.
		//i.e. 100 cubes means we need 10 per side - sqrt(100) = 10
		var count = Mathf.RoundToInt(Mathf.Sqrt(CubeCount));
		for (var i = 0; i < count; i++) {
			for (var j = 0; j < count; j++) {
				var newCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				newCube.transform.parent = cubeParent.transform;
				
				//Here's an easier way of doing what we did earlier in the course
				//Rather than having to worry about the actual maths, we just treat our position in the loop as an
				//input signal, and treat the output X and Z positions as output signals
				var rayPos = new Vector3(
					LinearRemap(i + 0.5f, 0, count, Start.x, End.x),
					100f,
					LinearRemap(j + 0.5f, 0, count, Start.y, End.y)
				);
				
				//Like last week, we do a raycast so that we always land on the terrain
				if (!Physics.Raycast(rayPos, Vector3.down, out var hit, 100f)) continue;
				
				//Place the cube at the resulting position, and make it face away from the surface
				newCube.transform.position = hit.point;
				newCube.transform.rotation = Quaternion.LookRotation(hit.normal);
				newCube.transform.localScale = Vector3.one * CubeSize;
			}
		}
	}

	[ContextMenu("Spawn Spheres")]
	public void SpawnSpheres()
	{
		if (transform.Find("Spheres")) {
			if (Application.isPlaying) Destroy(transform.Find("Spheres").gameObject);
			else DestroyImmediate(transform.Find("Spheres").gameObject);
		}
		var sphereParent = new GameObject("Spheres");
		sphereParent.transform.parent = transform;
		
		for (var j = 0; j < SphereCount; j++) {
			var newSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			newSphere.transform.parent = sphereParent.transform;
				
			//Since we don't need to worry about any input signal (this is just random)
			//We can use a simple lerp
			var rayPos = new Vector3(
				Mathf.Lerp(Start.x, End.x, Random.value),
				100f,
				Mathf.Lerp(Start.x, End.x, Random.value)
			);
				
			//Like last week, we do a raycast so that we always land on the terrain
			if (!Physics.Raycast(rayPos, Vector3.down, out var hit, 100f)) continue;
			newSphere.transform.position = hit.point;
			newSphere.transform.localScale = Vector3.one * SphereSize;
		}
	}

	public float LinearRemap(float value, float fromMin, float fromMax, float toMin, float toMax)
	{
		//Where are we within the input range?
		var t = Mathf.InverseLerp(fromMin, fromMax, value);
		
		//Return the corresponding position in the output range
		return Mathf.Lerp(toMin, toMax, t);
	}
	
	[ContextMenu("Clear Objects")]
	public void ClearObjects()
	{
		//We iterate backwards through children, since if we go forwards we end up changing child indices
		for (var i = transform.childCount - 1; i >= 0; i--) {
			//DestroyImmediate is for edit-mode, Destroy is for play-mode (including in the built application)
			if (Application.isPlaying) Destroy(transform.GetChild(i).gameObject);
			else DestroyImmediate(transform.GetChild(i).gameObject);
		}
	}
}
