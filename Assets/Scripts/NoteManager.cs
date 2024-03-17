using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public List<InputRecord> notes;

    [Header("NotePrefabs")]
    public GameObject NotePrefab;

    [Header("Calibration Sync")]
    public float offset = 0f;  //Calibration syncing, added onto hitTime


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InstantiateNotes()
    {
        foreach (InputRecord inputRecord in notes) {
            GameObject noteInstantiated = Instantiate(NotePrefab, new Vector3(-50f, 20f, 6f), Quaternion.identity);
            NoteObject noteObject_inInstantiated = noteInstantiated.GetComponent<NoteObject>();
            if (inputRecord.note == "W") {
                noteObject_inInstantiated.noteDirection = NoteObject.NoteDirection.W;
                noteObject_inInstantiated.noteType = GameManager.Instance.noteTypes[1];
            }
            else if (inputRecord.note == "A") {
                noteObject_inInstantiated.noteDirection = NoteObject.NoteDirection.A;
                noteObject_inInstantiated.noteType = GameManager.Instance.noteTypes[2];
            }
            else if (inputRecord.note == "S") {
                noteObject_inInstantiated.noteDirection = NoteObject.NoteDirection.S;
                noteObject_inInstantiated.noteType = GameManager.Instance.noteTypes[3];
            }
            else if (inputRecord.note == "D") {
                noteObject_inInstantiated.noteDirection = NoteObject.NoteDirection.D;
                noteObject_inInstantiated.noteType = GameManager.Instance.noteTypes[0];
            }

            noteObject_inInstantiated.hitTime = inputRecord.time + offset;  //Add offset to hitTime
        }
    }
}
