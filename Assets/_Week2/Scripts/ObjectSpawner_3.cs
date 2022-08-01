using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Note that we didn't do most of this in class - it's just a piece of code for you to look at
//to see a few more techniques that are possible, and to perhaps give you some ideas if your programming level is more advanced
public class ObjectSpawner_3 : MonoBehaviour
{
    public PrimitiveType ObjectType = PrimitiveType.Cube;
    [Range(1, 10000)] public int ObjectCount = 500;
    [Range(0f, 10f)] public float SpawnArea = 5f;
    [Range(0f, 2f)] public float ObjectSizeMultiplier = 1f;
    public Gradient ColorGradient; //Providing gradients to inspectors is a great way to experiment with color!

    [Header("Noise settings")]
    [Range(1, 5)] public int NoiseOctaves = 2;
    [Range(0f, 5f)] public float NoiseAmplitude = 1f;
    [Range(0f, 1f)] public float NoiseAmplitudeMultiplier = 0.5f;
    [Range(0f, 10f)] public float NoiseFrequency = 1f;
    [Range(0f, 10f)] public float NoiseFrequencyMultiplier = 3f;

    //If we pass this into Unity's random number generator, everything will be exactly the same if we use that seed again later
    [HideInInspector] public int Seed;

    [ContextMenu("Spawn Objects")]
    public void SpawnObjects()
    {
        //This will spawn objects with a random seed, so it's different every time
        Seed = Random.Range(0, 1000000);
        SpawnObjects(Seed);
    }
    
    public void SpawnObjects(int seed)
    {
        ClearObjects();
        
        Seed = seed;
        Random.InitState(seed);
        var noiseOffset = Random.value * 10000f;

        //get how many per side - since we will have the square of this number in total objects, we use the square root
        //e.g. if we want 100 objects, that's 10 per side (sqrt(100) = 10)
        var sideCount = Mathf.FloorToInt(Mathf.Sqrt(ObjectCount));
        
        //to make sure there's no gaps, we set the object size with code
        var objectSize = SpawnArea / sideCount;
        
        //Spawn a bunch of primitives in a grid (the XZ plane)
        for (var i = 0; i < sideCount; i++) {
            //the x position is constant for this whole row, so we calculate it here
            
            //we get a float from 0 to 1 out of i by dividing it by sideCount
            //then we remap it to -SpawnArea * 0.5 -> SpawnArea * 0.5
            var x = SpawnArea * ((float)i / sideCount) - SpawnArea * 0.5f;
            
            for (var j = 0; j < sideCount; j++) {
                var z = SpawnArea * ((float)j / sideCount) - SpawnArea * 0.5f;

                //OK this is a little complex - we determine the height at each of the four corners of this grid cell
                //By taking the cross product of the two directions, we get the vector that points out away from the surface
                //We can use this to rotate our object.
                //I'll provide some resources explaining the dot and cross products once I have access to Canvas :D
                var heightCorners = new[]
                {
                    new Vector3(x - 0.01f, GetHeight(x - 0.01f, z, noiseOffset), z),
                    new Vector3(x + 0.01f, GetHeight(x + 0.01f, z, noiseOffset), z),
                    new Vector3(x, GetHeight(x, z - 0.01f, noiseOffset), z - 0.01f),
                    new Vector3(x, GetHeight(x, z + 0.01f, noiseOffset), z + 0.01f),
                };
                var gradientVectorA = heightCorners[1] - heightCorners[0];
                var gradientVectorB = heightCorners[3] - heightCorners[2];
                var normalVector = Vector3.Cross(gradientVectorA, gradientVectorB);
                
                //Create a new object, make sure it's a child of this object, and give it a random position, rotation and scale
                var newObj = GameObject.CreatePrimitive(ObjectType);
                newObj.transform.parent = transform;
                newObj.transform.localPosition = new Vector3(x, GetHeight(x, z, noiseOffset), z);
                
                //Our object faces towards the normal vector we just calculated
                newObj.transform.localRotation = Quaternion.LookRotation(normalVector, Vector3.forward);
                
                //now, object size represents a multiplier if we want them to be smaller and larger than this procedural value
                newObj.transform.localScale = Vector3.one * ObjectSizeMultiplier * objectSize;
            }
        }
        
        ColorizeObjects();
    }
    
    public void Awake()
    {
        //We run all the coloring stuff again here for reasons that are a little complicated and not too important right now
        //Awake is called before the first frame update when the object is enabled
        ColorizeObjects();   
    }

    //Recolor the terrain based on remapping the noise values to a gradient
    [ContextMenu("Colorize Objects")]
    public void ColorizeObjects()
    {
        //Knowing the maximum noise value lets us map our color gradient onto the various heights - useful for terrains and such
        var heightBounds = GetHeightBounds();
        
        for (var i = 0; i < transform.childCount; i++) {
            var targetRenderer = transform.GetChild(i).GetComponent<Renderer>();
            //InverseLerp is a really useful way of turning a value into a 0-1 range, provided you know the min and max possible values
            var normalizedHeight = Mathf.InverseLerp(heightBounds.x, heightBounds.y, transform.GetChild(i).localPosition.y);
            var color = ColorGradient.Evaluate(normalizedHeight);
            SetColor(targetRenderer, color);
        }
    }

    //This is a way to set various material properties without instantiating new materials
    //Instantiating new materials should be avoided when possible - we'll explore why a little later in the course
    //If you want to research yourself, it's because more materials = more draw calls = more expensive
    //As for why this DOESN'T instantiate materials...it's a little complicated! :D
    //Note that MaterialPropertyBlocks are NOT saved in the scene, so you will lose this information when you enter
    //play mode, or when you reload the scene. To do this in a performant way 'properly' we would need to take a
    //different approach. For now though, it's fine!
    private void SetColor(Renderer target, Color color)
    {
        var propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetColor("_Color", color);
        target.SetPropertyBlock(propertyBlock);
    }

    //Get the height at the given position - we use an offset so that we can change the entire pattern to get different results
    //We made this a separate function so that we could change it easily and have the rest of the terrain generation 'just work'
    private float GetHeight(float x, float y, float offset = 0f)
    {
        return MultiOctaveNoise(x + offset, y + offset, NoiseFrequency, NoiseOctaves, NoiseFrequencyMultiplier, NoiseAmplitudeMultiplier) * NoiseAmplitude;
    }

    //This is how multi-octave noise is calculated
    //Each pass, we increase the frequency (making the details finer) and decrease the amplitude (decreasing the effect the finer details have)
    //Then we add that to the total - this produces pleasing results and is used EVERYWHERE in computer graphics!
    private float MultiOctaveNoise(float x, float y, float frequency, int octaves, float frequencyMultiplier, float amplitudeMultiplier)
    {
        var sum = 0f;
        var curFreq = frequency;
        var curAmp = 1f;
        for(var i = 0; i < octaves; i++)
        {
            sum += Mathf.PerlinNoise(x * curFreq, y * curFreq) * curAmp;
            curFreq *= frequencyMultiplier;
            curAmp *= amplitudeMultiplier;
        }

        return sum;
    }

    //We get the highest and lowest generated points - we can use these for recoloring and various other things
    //We put the min value in the x coordinate of our vector, and the max value in the y coordinate
    //If you know what a C# tuple is, this would be a good place to use one of those instead of a vector
    private Vector2 GetHeightBounds()
    {
        //This is a standard way to get max and min values
        //Note that we start max at -float.MaxValue NOT float.MinValue - MinValue is zero and we could have negative heights!
        var min = float.MaxValue;
        var max = -float.MaxValue;
        for (var i = 0; i < transform.childCount; i++) {
            var posY = transform.GetChild(i).localPosition.y;
            max = Mathf.Max(max, posY);
            min = Mathf.Min(min, posY);
        }
        return new Vector2(min, max);
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
