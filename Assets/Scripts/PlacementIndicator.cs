using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
public class PlacementIndicator : MonoBehaviour
{
    ARRaycastManager rayManager;
    ARPlaneManager planes;
    GameObject visual;
    public GameObject spawnObj;

    [SerializeField] Slider height;
    [SerializeField] Slider area;
    private void Start()
    {
        rayManager = FindObjectOfType<ARRaycastManager>();
        planes = FindObjectOfType<ARPlaneManager>();

        visual = transform.GetChild(0).gameObject;
        visual.SetActive(false);
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];

            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    rayManager.Raycast(touch.position, hits, TrackableType.Planes);

                    if (hits.Count > 0)
                    {
                        transform.position = hits[0].pose.position;
                        transform.rotation = hits[0].pose.rotation;

                        if (!visual.activeInHierarchy)
                            visual.SetActive(true);
                    }
                    break;
                case TouchPhase.Moved:
                    rayManager.Raycast(touch.position, hits, TrackableType.Planes);

                    if (hits.Count > 0)
                    {
                        transform.position = hits[0].pose.position;
                        transform.rotation = hits[0].pose.rotation;
                    }
                    break;
                case TouchPhase.Ended:
                    FindObjectOfType<SFXPlayer>().Warp();
                    planes.enabled = false;
                    rayManager.enabled = false;
                    foreach (var plane in planes.trackables)
                        plane.gameObject.SetActive(false);
                    visual.SetActive(false);
                    GameObject temp = Instantiate(spawnObj, transform.position, transform.rotation);
                    Field field = temp.GetComponent<Field>();
                    field.height = height;
                    field.area = area;
                    field.InitField();

                    FindObjectOfType<PlayerControl>().StartGame();
                    Hider hider = FindObjectOfType<Hider>();
                    hider.transform.SetParent(field.house);
                    hider.initSize = hider.transform.localScale.x;
                    Destroy(gameObject);
                    break;
            }
        }
    }

}
