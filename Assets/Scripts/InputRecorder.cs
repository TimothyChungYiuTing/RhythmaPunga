using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;


[System.Serializable]
public class InputRecord
{
    public string note;
    public float time;
}

[System.Serializable]
public class InputData
{
    public InputRecord[] songNotesArray;
}

public class InputRecorder : MonoBehaviour
{
    public float startTime;
    public List<InputRecord> inputRecords;
    private string filePath;

    public bool recording = false;
    public int inputFileIndex = 0;

    public NoteManager noteManager;

    private List<string> fileNames = new()
    {
        "/Resources/Song0Input.json", //Testing
        "/Resources/Song1Input.json",
        "/Resources/Song2Input.json",
        "/Resources/Song3Input.json",
        "/Resources/Song4Input.json",
        "/Resources/Song5Input.json",
        "/Resources/Song6Input.json",
        "/Resources/Song7Input.json",
        "/Resources/Song8Input.json", // Calibration
    };

    public List<TextAsset> SongJSONs;

    private void Start()
    {
        noteManager = FindObjectOfType<NoteManager>();
        filePath = Application.dataPath + fileNames[inputFileIndex];
        
        inputRecords = new();
        noteManager.notes = new();

        //TryLoadInputRecords();
    }

    private void Update()
    {
        if (recording) {
            if (Input.GetKeyDown(KeyCode.W))
                RecordInput("W");
            if (Input.GetKeyDown(KeyCode.A))
                RecordInput("A");
            if (Input.GetKeyDown(KeyCode.S))
                RecordInput("S");
            if (Input.GetKeyDown(KeyCode.D))
                RecordInput("D");
        }
    }

    private void RecordInput(string note)
    {
        InputRecord record = new InputRecord();
        record.note = note;
        record.time = Time.time - startTime;
        inputRecords.Add(record);
        SaveInputRecords();
    }

    private void SaveInputRecords()
    {
        InputData inputData = new() { songNotesArray = inputRecords.ToArray() };

        string json = JsonUtility.ToJson(inputData, true);

        File.WriteAllText(filePath, json);
    }

    public void LoadInputRecords()
    {
        inputRecords.Clear();
        noteManager.notes.Clear();
        foreach (Transform child in noteManager.transform) {
            Destroy(child.gameObject);
        }

        InputData inputData;
        if (File.Exists(filePath)) {
            string json = File.ReadAllText(filePath);
            inputData = JsonUtility.FromJson<InputData>(json);
        }
        else {
            inputData = JsonUtility.FromJson<InputData>(SongJSONs[inputFileIndex].text);
        }
        
        if (inputData != null && inputData.songNotesArray != null) {
            inputRecords.AddRange(inputData.songNotesArray);
            noteManager.notes.AddRange(inputData.songNotesArray);
        }
        noteManager.InstantiateNotes(); //This calls a coroutine to make sure all notes are instantiated before starting the song
    }
    
    public void Increment()
    {
        inputFileIndex++;
        filePath = Application.dataPath + fileNames[inputFileIndex];
    }
}