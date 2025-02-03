using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Field : MonoBehaviour
{
    public Transform baseMesh;
    public Transform house;
    public Slider height;
    public Slider area;

    float initHeight = 0.4f;
    float initArea = 0.05131504f;
    Vector3 initBaseScale;
    bool initDone = false;

    HidingSpot[] spots;
    private void Start()
    {
        InitField();
    }
    public void InitField()
    {
        height.value = initHeight;
        area.value = initArea * 10;
        initBaseScale = baseMesh.localScale;

        initDone = true;

        oldArea = area.value;
        oldHeight = height.value;

        spots = FindObjectsOfType<HidingSpot>();
    }

    float oldHeight;
    float oldArea;
    private void Update()
    {
        if (!initDone) return;

        if (oldArea == area.value && oldHeight == height.value) return;

        Vector3 newHeight = baseMesh.localScale;
        newHeight.y = height.value;
        baseMesh.localScale = newHeight;

        Vector3 newHeightPos = baseMesh.localPosition;
        newHeightPos.y = newHeight.y / 2;
        baseMesh.localPosition = newHeightPos;

        newHeightPos.y = newHeight.y;
        house.localPosition = newHeightPos;

        Vector3 newArea = area.value / 10 * Vector3.one;
        house.localScale = newArea;

        newArea = initBaseScale;
        
        newArea.x = initBaseScale.x * area.value / 10 / initArea;
        newArea.y = newHeight.y;
        newArea.z = initBaseScale.z * area.value / 10 / initArea;
        baseMesh.localScale = newArea;

        foreach (HidingSpot spot in spots)
        {
            spot.particles.localScale = Vector3.one * area.value / 10;
        }
    }


}
