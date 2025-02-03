using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerControl : MonoBehaviour
{
    Camera cam;
    Hider hider;
    SFXPlayer sfx;

    [SerializeField] GameObject winPanel;
    [SerializeField] Text hintText;

    public GameObject scaleButton;
    public GameObject scaleSettings;

    IEnumerator coroutine;
    void Start()
    {
        cam = Camera.main;
        hider = FindObjectOfType<Hider>();
        sfx = FindObjectOfType<SFXPlayer>();

        hintText.text = "";
    }

    bool gameStarted = false;
    void Update()
    {
        if (!gameStarted) return;
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Touch touch = Input.touches[0];
            
            RaycastHit hit;
            if (Physics.Raycast(cam.ScreenPointToRay(touch.position), out hit))
            {
                HidingSpot spot = hit.collider.transform.GetComponentInParent<HidingSpot>();

                if (spot != null)
                {
                    sfx.Boink();
                    StartCoroutine(spot.Boink());
                    if (coroutine != null) StopCoroutine(coroutine);

                    if (spot == hider.mySpot)
                    {
                        StartCoroutine(FlipOut(spot.transform.parent.parent));
                    }
                    else
                    {
                        coroutine = FlashHint(spot.transform.parent.parent);
                        StartCoroutine(coroutine);
                    }
                }
            }
        }
    }
    
    public void StartGame()
    {
        hintText.text = "";

        if (winPanel.activeInHierarchy)
            winPanel.SetActive(false);

        scaleButton.SetActive(true);

        hider.Hide();
        gameStarted = true;
    }

    IEnumerator FlashHint(Transform room)
    {
        if (room == hider.mySpot.room)
        {
            hintText.text = "Тепло...";
        } else
        {
            hintText.text = "Холодно...";
        }
        yield return new WaitForSeconds(3);
        hintText.text = "";
    }

    float flipDur = 1;
    float flipScaleDur = 0.3f;
    IEnumerator FlipOut(Transform room)
    {
        hider.transform.GetChild(0).gameObject.SetActive(true);

        sfx.Yay();
        hider.GetComponentInChildren<Animator>().SetTrigger("flip");

        gameStarted = false;
        hintText.text = "Горячо!";

        float flipTimer = 0;
        Vector3 orig = hider.transform.position;

        hider.transform.LookAt(room, Vector3.up);
        Vector3 ang = hider.transform.eulerAngles;
        ang.x = 0;
        hider.transform.eulerAngles = ang;

        while (flipTimer < flipDur)
        {
            flipTimer += Time.deltaTime;

            hider.transform.localScale = hider.initSize * Vector3.one * Mathf.Clamp01(flipTimer/flipScaleDur);
            hider.transform.position = orig + (room.position - orig) * flipTimer / flipDur;

            yield return null;
        }
        hider.transform.LookAt(cam.transform, Vector3.up);
        ang = hider.transform.eulerAngles;
        ang.x = 0;
        hider.transform.eulerAngles = ang; 
        winPanel.SetActive(true);
    }

    public void ToggleScaleSettings()
    {
        scaleSettings.SetActive(!scaleSettings.activeInHierarchy);
    }
}
