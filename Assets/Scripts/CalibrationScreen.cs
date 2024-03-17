using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CalibrationScreen : MonoBehaviour
{
    [Header("Offset Shenanigans")]
    public TMP_InputField offsetInputField;
    public Scrollbar scrollbar;

    // Start is called before the first frame update
    void Start()
    {
        // Set the initial value of the scrollbar to correspond to offset 0
        scrollbar.value = 0.5f;

        // Subscribe to input field value changes
        offsetInputField.onValueChanged.AddListener(OnInputFieldValueChanged);


        // Subscribe the input field value to match the offset
        scrollbar.onValueChanged.AddListener(OffsetDisplayUpdate);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OffsetDisplayUpdate(float value)
    {
        offsetInputField.text = ScoreSystem.Instance.offset.ToString();


        // Update the offset based on the scrollbar value
        ScoreSystem.Instance.offset = (value - 0.5f) * 2f;

        // Update the input field value accordingly
        offsetInputField.text = ScoreSystem.Instance.offset.ToString();
    }

    // Callback when the input field value changes
    void OnInputFieldValueChanged(string value)
    {
        float offsetValue;
        if (float.TryParse(value, out offsetValue))
        {
            // Clamp the offset value between -1 and 1
            offsetValue = Mathf.Clamp(offsetValue, -1f, 1f);

            // Calculate the corresponding scrollbar value
            float scrollbarValue = (offsetValue / 2f) + 0.5f;

            // Update the scrollbar value
            scrollbar.value = scrollbarValue;

            // Update the offset in the ScoreSystem
            ScoreSystem.Instance.offset = offsetValue;
        }
    }
}