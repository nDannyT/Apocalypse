﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioClip Music;
    public AudioSource MusicSource;
    // Start is called before the first frame update
    void Start()
    {
        MusicSource.clip = Music;
        MusicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    MusicSource.Play();
        //}
    }
}
