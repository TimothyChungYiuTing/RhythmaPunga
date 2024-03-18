using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrationNotes : MonoBehaviour
{
    private Vector3 startPos;
    public float speed;

    private void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position += Time.deltaTime * Vector3.left * speed;
    }

    public void ResetPosition()
    {
        transform.position = startPos;
        transform.position += (ScoreSystem.Instance.offset * Vector3.left) * speed;
    }
}