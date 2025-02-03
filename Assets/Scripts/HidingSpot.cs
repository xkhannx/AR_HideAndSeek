using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    [SerializeField] Transform particlesSpot;
    [SerializeField] Transform particlesPrefabs;

    public Transform particles;
    Transform furniture;
    BoxCollider box;

    public Transform room;
    private void OnEnable()
    {
        furniture = transform.parent;
        foreach (Transform child in transform)
        {
            child.GetComponent<MeshRenderer>().enabled = false;
        }
        particles = Instantiate(particlesPrefabs, particlesSpot.position, particlesSpot.rotation, transform);
        particles.gameObject.SetActive(false);

        box = GetComponentInChildren<BoxCollider>();
        box.enabled = false;

        room = furniture.parent;
    }

    public AnimationCurve boinkCurve;
    public float boinkDur = 0.5f;
    float boinkTimer;
    public IEnumerator Boink()
    {
        box.enabled = false;
        particles.gameObject.SetActive(false);

        Vector3 initScale = furniture.localScale;
        boinkTimer = 0;
        while (boinkTimer < boinkDur)
        {
            boinkTimer += Time.deltaTime;
            
            float horScale = boinkCurve.Evaluate(boinkTimer / boinkDur);
            float verScale = (1 - horScale) + 1;

            Vector3 boinkScale = initScale;
            boinkScale.x *= horScale; 
            boinkScale.y *= horScale;
            boinkScale.z *= verScale;

            furniture.localScale = boinkScale;
            yield return null;
        }
        
    }

    public void InitSpot()
    {
        box.enabled = true;
        particles.gameObject.SetActive(true);
    }
}
