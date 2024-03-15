using System.Collections;
using System.Collections.Generic;using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class InputRecorder : MonoBehaviour
{
    private List<(KeyCode, float)> inputRecord = new List<(KeyCode, float)>();
    private bool isRecording = false;

    void Start()
    {
        StartRecording();
    }

    //Update is called once per frame
    void Update()
    {
        if (isRecording)
        {
            RecordInput();
        }
    }

    //Record input when W, A, S, D keys are pressed
    void RecordInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)) {
            float timestamp = Time.time;
            KeyCode key = Event.current.keyCode;
            inputRecord.Add((key, timestamp));
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            StopRecording();
        }
    }

    //Start recording input
    public void StartRecording()
    {
        isRecording = true;
    }

    //Stop recording input
    public void StopRecording()
    {
        isRecording = false;
        SaveInputRecordToJson();
    }

    //Save input record to JSON file
    void SaveInputRecordToJson()
    {
        //Load existing JSON file from Resources folder
        TextAsset existingJson = Resources.Load<TextAsset>("existingInputRecord");
        if (existingJson == null)
        {
            Debug.LogError("Existing JSON file not found in Resources folder.");
            return;
        }

        //Parse existing JSON data
        List<(KeyCode, float)> existingInputRecord = JsonUtility.FromJson<List<(KeyCode, float)>>(existingJson.text);

        //Append new data to existing record
        existingInputRecord.AddRange(inputRecord);

        //Convert to JSON string
        string json = JsonUtility.ToJson(existingInputRecord);

        //Save JSON string to a new file outside Resources folder
        string filePath = Application.persistentDataPath + "/Song0Input.json";
        File.WriteAllText(filePath, json);

        Debug.Log("Input record saved to: " + filePath);
    }
}