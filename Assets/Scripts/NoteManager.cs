using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class NoteManager : MonoBehaviour
{
    public List<InputRecord> notes;
    public InputRecorder inputRecorder;

    [Header("NotePrefabs")]
    public GameObject NotePrefab;

    private ScoreSystem scoreSystem;

    // Start is called before the first frame update
    void Start()
    {
        scoreSystem = FindObjectOfType<ScoreSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InstantiateNotes()
    {
        StartCoroutine(Instantiation());
    }

    private IEnumerator Instantiation()
    {
        foreach (InputRecord inputRecord in notes) {
            GameObject noteInstantiated = Instantiate(NotePrefab, new Vector3(-50f, 20f, 6f), Quaternion.identity, transform);
            NoteObject noteObject_inInstantiated = noteInstantiated.GetComponent<NoteObject>();

            noteObject_inInstantiated.hitTime = inputRecord.time + Offset.Instance.offset;  //Add offset to hitTIme

            if (inputRecord.note == "W") {
                noteObject_inInstantiated.noteDirection = NoteObject.NoteDirection.W;
                if ((int)(noteObject_inInstantiated.hitTime+0.02f) / 12 % 2 == 0)
                    noteObject_inInstantiated.noteType = GameManager.Instance.noteTypes[1];
                else {
                    noteObject_inInstantiated.noteType = inputRecorder.inputFileIndex switch
                    {
                        1 => NoteType.Sloth,
                        2 => NoteType.Greed,
                        3 => NoteType.GluttonySlow,
                        4 => NoteType.Wrath,
                        _ => NoteType.Sloth,
                    };
                }
            }
            else if (inputRecord.note == "A") {
                noteObject_inInstantiated.noteDirection = NoteObject.NoteDirection.A;
                if ((int)(noteObject_inInstantiated.hitTime+0.02f) / 12 % 2 == 0)
                    noteObject_inInstantiated.noteType = GameManager.Instance.noteTypes[2];
                else {
                    noteObject_inInstantiated.noteType = inputRecorder.inputFileIndex switch
                    {
                        1 => NoteType.Sloth,
                        2 => NoteType.Greed,
                        3 => NoteType.GluttonyFast,
                        4 => NoteType.Wrath,
                        _ => NoteType.Sloth,
                    };
                }
            }
            else if (inputRecord.note == "S") {
                noteObject_inInstantiated.noteDirection = NoteObject.NoteDirection.S;
                if ((int)(noteObject_inInstantiated.hitTime+0.02f) / 12 % 2 == 0)
                    noteObject_inInstantiated.noteType = GameManager.Instance.noteTypes[3];
                else {
                    noteObject_inInstantiated.noteType = inputRecorder.inputFileIndex switch
                    {
                        1 => NoteType.Sloth,
                        2 => NoteType.Greed,
                        3 => NoteType.GluttonySlow,
                        4 => NoteType.Wrath,
                        _ => NoteType.Sloth,
                    };
                }
            }
            else if (inputRecord.note == "D") {
                noteObject_inInstantiated.noteDirection = NoteObject.NoteDirection.D;
                if ((int)(noteObject_inInstantiated.hitTime+0.02f) / 12 % 2 == 0)
                    noteObject_inInstantiated.noteType = GameManager.Instance.noteTypes[0];
                else {
                    noteObject_inInstantiated.noteType = inputRecorder.inputFileIndex switch
                    {
                        1 => NoteType.Sloth,
                        2 => NoteType.Greed,
                        3 => NoteType.GluttonyFast,
                        4 => NoteType.Wrath,
                        _ => NoteType.Sloth,
                    };
                }
            }
            yield return null;
        }

        scoreSystem.StartSong();
    }
}
