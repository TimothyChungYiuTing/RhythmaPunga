using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [HideInInspector] public AudioSource audioSource;
    private bool isGameSongPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameSongPlaying && Input.GetKeyDown(KeyCode.Space)) {
            isGameSongPlaying = true;
            audioSource.Play();
        }
        if (!audioSource.isPlaying) {
            isGameSongPlaying = false;
        }
    }
}
