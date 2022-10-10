using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGrowth : MonoBehaviour
{
    [Header("Config")]
    public float MinSaplingSize = 0.45f;
    public float MaxSaplingSize = 0.7f;
    public float MinTreeSize = 0.8f;
    public float MaxTreeSize = 1.5f;
    [Space(10)]
    public float SaplingGrowthDuration = 10f;
    public float TreeGrowthDuration = 20f;

    [Header("Objects")]
    public GameObject Sapling;
    public GameObject Tree;

    [Header("Status")]
    public bool IsGrowing = false;
    public bool IsSapling = true;
    [Range(0f, 1f)] public float GrowthAmount = 0f;

    public void Awake()
    {
        //Start as a sapling with minimum size
        Sapling.gameObject.SetActive(true);
        Tree.gameObject.SetActive(false);
        Sapling.transform.localScale = Vector3.one * MinSaplingSize;
    }

    public void Update()
    {
        if (!IsGrowing) return;
        
        //Grow!
        if (IsSapling) {
            GrowthAmount += Time.deltaTime / SaplingGrowthDuration;
        } else {
            GrowthAmount += Time.deltaTime / TreeGrowthDuration;
        }

        //If we've finished the sapling stage, become a tree. Otherwise just hold here
        if (GrowthAmount >= 1f) {
            if (IsSapling) {
                TurnIntoTree();
            } else {
                GrowthAmount = 1f;
            }
        }

        //Set scales as necessary
        if (IsSapling) {
            Sapling.transform.localScale = Vector3.one * Mathf.Lerp(MinSaplingSize, MaxSaplingSize, GrowthAmount);
        } else {
            Tree.transform.localScale = Vector3.one * Mathf.Lerp(MinTreeSize, MaxTreeSize, GrowthAmount);
        }
    }
    
    public void TurnIntoTree()
    {
        //Disable the sapling object and enable the tree object in its minimum size
        Sapling.SetActive(false);
        Tree.SetActive(true);
        Tree.transform.localScale = Vector3.one * MinTreeSize;
        IsSapling = false;
        
        //Also reset growth amount
        GrowthAmount = 0f;
    }
    
}
