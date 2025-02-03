using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    AudioSource sfx;
    [SerializeField] AudioClip boink;
    [SerializeField] AudioClip yay;
    [SerializeField] AudioClip warp;
    private void OnEnable()
    {
        sfx = GetComponent<AudioSource>();
    }

    public void Boink()
    {
        sfx.PlayOneShot(boink);
    }

    public void Yay()
    {
        sfx.PlayOneShot(yay);
    }

    public void Warp()
    {
        sfx.PlayOneShot(warp);
    }
}
