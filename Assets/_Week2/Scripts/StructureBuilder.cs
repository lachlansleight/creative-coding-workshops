using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Note that we didn't do most of this in class - it's just a piece of code for you to look at
//to see a few more techniques that are possible, and to perhaps give you some ideas if your programming level is more advanced
public class StructureBuilder : MonoBehaviour
{
    public Transform WaypointsParent;

    public bool AlwaysUp = false;
    public bool CloseLoop = false;
    public float PostSeparation = 0.5f;
    public float FenceHeight = 1f;
    public float CrossbeamHeight = 0.6f;
    public float PostWidth = 0.1f;
    [Range(0f, 1f)] public float RandomizeRotationAmount = 0.1f;
    [Range(0f, 1f)] public float RandomizeHeightAmount = 0.1f;

    [ContextMenu("Build Fence")]
    public void BuildFence()
    {
        ClearObjects();

        //We keep track of the last position so that we can use it for the next fence post segment's orientation
        var lastPos = Vector3.zero;
        for(var i = 0; i < WaypointsParent.childCount - 1; i++)
        {
            lastPos = BuildFenceSegment(WaypointsParent.GetChild(i).position, WaypointsParent.GetChild(i + 1).position, lastPos);
        }
        
        //One more segment to connect the last waypoint back to the first
        if(CloseLoop && WaypointsParent.childCount > 2)
        {
            BuildFenceSegment(WaypointsParent.GetChild(WaypointsParent.childCount - 1).position, WaypointsParent.GetChild(0).position, lastPos);
        }
    }

    public Vector3 BuildFenceSegment(Vector3 start, Vector3 end, Vector3 lastEnd)
    {
        //Determine how many posts we need based on the total distance and the desired post spacing
        var distance = (end - start).magnitude;
        var requiredPosts = Mathf.RoundToInt(distance / PostSeparation);
        var lastPos = lastEnd;
        var offset = (end - start);
        for(var i = 0; i < requiredPosts; i++)
        {
            //Place a post at the correct position and rotate it to face along the fence segment's direction
            var pos = Vector3.Lerp(start, end, Mathf.InverseLerp(0, requiredPosts, i));
            var post = GameObject.CreatePrimitive(PrimitiveType.Cube);
            post.transform.position = pos;
            var defaultRotation = Quaternion.LookRotation(AlwaysUp ? new Vector3(offset.x, 0f, offset.z) : offset);
            post.transform.rotation = Quaternion.Lerp(defaultRotation, Random.rotation, RandomizeRotationAmount);
            var height = FenceHeight * (1f - (Random.value * RandomizeHeightAmount)); //cubes are centered about y, so we need to move it up by half it's height
            post.transform.Translate(0f, height * 0.5f, 0f, Space.Self);
            post.transform.localScale = new Vector3(PostWidth, height, PostWidth);
            post.transform.parent = transform;

            //We don't place a beam on the first post, since we don't know where the next post is yet!
            //Instead, we place beams between the last post and the current post
            var beamPos = pos + post.transform.up * height * CrossbeamHeight;
            if (i == 0 && lastPos == Vector3.zero)
            {
                lastPos = beamPos;
                continue;
            }

            //Place beams on either side of the post
            var beam = GameObject.CreatePrimitive(PrimitiveType.Cube);
            beam.transform.position = Vector3.Lerp(lastPos, beamPos, 0.5f);
            beam.transform.rotation = Quaternion.LookRotation(beamPos - lastPos, Vector3.up);
            beam.transform.Translate(PostWidth * 0.5f, 0f, 0f, Space.Self);
            beam.transform.localScale = new Vector3(PostWidth * 0.3f, PostWidth, (lastPos - beamPos).magnitude);
            beam.transform.parent = transform;

            var beam2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            beam2.transform.rotation = Quaternion.LookRotation(beamPos - lastPos, Vector3.up);
            beam2.transform.position = Vector3.Lerp(lastPos, beamPos, 0.5f);
            beam2.transform.Translate(-PostWidth * 0.5f, 0f, 0f, Space.Self);
            beam2.transform.localScale = new Vector3(PostWidth * 0.3f, PostWidth, (lastPos - beamPos).magnitude);
            beam2.transform.parent = transform;
            lastPos = beamPos;
        }

        return lastPos;
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
