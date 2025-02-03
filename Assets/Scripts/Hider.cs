using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hider : MonoBehaviour
{
    public HidingSpot mySpot;
    public float initSize;
    public void Hide()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        HidingSpot[] spots = FindObjectsOfType<HidingSpot>();
        if (spots.Length > 0)
        {
            foreach (HidingSpot spot in spots)
            {
                spot.InitSpot();
            }
            int randInd = Random.Range(0, spots.Length - 1);
            mySpot = spots[randInd];
            transform.position = mySpot.transform.position;
        }
    }
}
