using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScroller : MonoBehaviour
{
    [Header("Settings")]
    public float beatTempo;

    [Header("Flags")]
    public bool songStarted;

    void Start()
    {
        beatTempo = beatTempo / 60f;
    }

    void Update()
    {
        if (!songStarted) { /*
            if (Input.anyKeyDown) {
                songStarted = true; } */
        }
        else {
            transform.position -= new Vector3 (0f, beatTempo * Time.deltaTime, 0f);
        }
    }
}
