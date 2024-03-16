using UnityEngine;
using System.Collections.Generic;
using System.IO;


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
    };

    private void Start()
    {
        noteManager = FindObjectOfType<NoteManager>();
        startTime = Time.time;
        filePath = Application.dataPath + fileNames[inputFileIndex];

        inputRecords = new();
        noteManager.notes = new();
        if (!recording) {
            LoadInputRecords();
        }
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

    private void LoadInputRecords()
    {
        if (File.Exists(filePath)) {
            string json = File.ReadAllText(filePath);
            InputData inputData = JsonUtility.FromJson<InputData>(json);
            if (inputData != null && inputData.songNotesArray != null) {
                inputRecords.AddRange(inputData.songNotesArray);
                noteManager.notes.AddRange(inputData.songNotesArray);
            }
        }
        noteManager.InstantiateNotes();
    }
}