using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [HideInInspector] public AudioSource audioSource;
    public List<AudioClip> songClips;
    public int currentClip;

    public bool calibration;

    public List<CalibrationNotes> calibrationNotes;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (calibration && !audioSource.isPlaying)
        {
            audioSource.Play();
            foreach (CalibrationNotes calibrationNote in calibrationNotes)
            {
                calibrationNote.ResetPosition();
            }
        }
    }
}
