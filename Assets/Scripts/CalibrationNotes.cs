using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrationNotes : MonoBehaviour
{
    private float resetTime;
    private Vector3 startPos;
    public float speed;

    private void Start()
    {
        resetTime = Time.time;
        startPos = transform.position;
    }

    void Update()
    {
        transform.position = startPos + (Time.time - resetTime) * Vector3.left * speed + (Offset.Instance.offset * Vector3.left * speed);
    }

    public void ResetPosition()
    {
        resetTime = Time.time;
        transform.position = startPos;
        transform.position += (Offset.Instance.offset * Vector3.left * speed);
    }
}