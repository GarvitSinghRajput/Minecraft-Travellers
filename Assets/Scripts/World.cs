using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public float furnaceTimer = 1f;

    private Block[] blockArray;
    public static GameObject[] blocks;
    private void Start()
    {
        blocks = GameObject.FindGameObjectsWithTag("Block");
        blockArray = Resources.FindObjectsOfTypeAll<Block>();

        foreach (var item in blockArray)
        {
            if (item.gameObject.activeInHierarchy)
            {
                item.Init();
            }
        }
    }

    /// <summary>
    /// Call this method to animate all the Furnace blocks in the game scene.
    /// </summary>
    public void StartAnimation()
    {
        foreach (var item in blockArray)
        {
            if (item.gameObject.activeInHierarchy && item.isFurnace)
                item.AnimateFrunace(furnaceTimer);
        }
    }
}
