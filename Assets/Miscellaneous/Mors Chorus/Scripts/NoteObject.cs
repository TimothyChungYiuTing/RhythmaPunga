using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public enum NoteType {Normal, Shuriken, Heal, Shield, Fire, Zap, Poison, Sloth, Greed, GluttonyFast, GluttonySlow, Wrath}
    public enum NoteDirection {W, A, S, D}

    [Header("NoteCustomization")]
    public NoteType noteType;
    public NoteDirection noteDirection;
    public float hitTime;
    private Quaternion moveDirectionRotation;
    private Vector3 targetPositon; //Note must arrive targetPosition when hitTime is arrived
    private float moveSpeed;

    [Header("Flags")]
    public bool canBePressed;
    public bool obtained = false;

    [Header("Misc")]
    public KeyCode keyToPress;

    [Header("Effects")]
    public GameObject missEffect;
    public GameObject normalEffect;
    public GameObject goodEffect;
    public GameObject perfectEffect;

    public GameObject effectsHolder;

    [Header("Calibration Sync")]
    private float startTime; //Reference to the start time of the scene, when the song starts playing
    private Transform collidedActivator = null;

    void Start()
    {
        startTime = FindObjectOfType<InputRecorder>().startTime;

        effectsHolder = FindObjectOfType<EffectsHolder>().gameObject;

        if (noteDirection == NoteDirection.W) {
            keyToPress = KeyCode.W;
            targetPositon = new Vector3(-9f, 3.5f, 6f);
            moveDirectionRotation = Quaternion.Euler(0f, 0f, 90f);
        }
        else if (noteDirection == NoteDirection.A) {
            keyToPress = KeyCode.A;
            targetPositon = new Vector3(-10f, 2.5f, 6f);
            moveDirectionRotation = Quaternion.Euler(0f, 0f, 180f);
        }
        else if (noteDirection == NoteDirection.S) {
            keyToPress = KeyCode.S;
            targetPositon = new Vector3(-9f, 1.5f, 6f);
            moveDirectionRotation = Quaternion.Euler(0f, 0f, -90f);
        }
        else if (noteDirection == NoteDirection.D) {
            keyToPress = KeyCode.D;
            targetPositon = new Vector3(-8f, 2.5f, 6f);
            moveDirectionRotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    void Update()
    {
        //TODO: Change position according to NoteDirection and NoteType (arrive targetPositon at dedicated time)
        Positioning();

        if (Input.GetKeyDown (keyToPress)) {
            if (canBePressed) {
                obtained = true;
                gameObject.SetActive (false);

                //TODO: Change to Distance check
                //TODO: Prioritize closest note and only remove that note
                if (Vector2.Distance(transform.position, collidedActivator.position) > 0.5) {
                    Debug.Log("Normal Hit");
                    Instantiate(normalEffect, transform.position, Quaternion.Euler(new Vector3(0, 0, Random.Range(-10f, 10f))), effectsHolder.transform);
                    ScoreSystem.Instance.NormalHit();
                    Destroy(gameObject);
                }
                else if (Vector2.Distance(transform.position, collidedActivator.position) > 0.35) {
                    Debug.Log("Good Hit!");
                    Instantiate(goodEffect, transform.position, Quaternion.Euler(new Vector3(0, 0, Random.Range(-10f, 10f))), effectsHolder.transform);
                    ScoreSystem.Instance.GoodHit();
                    Destroy(gameObject);
                }
                else {
                    Debug.Log("Perfect Hit!!");
                    Instantiate(perfectEffect, transform.position, Quaternion.Euler(new Vector3(0, 0, Random.Range(-10f, 10f))), effectsHolder.transform);
                    ScoreSystem.Instance.PerfectHit();
                    Destroy(gameObject);
                }
            }
        }
    }

    private void Positioning()
    {
        switch (noteType) {
            case NoteType.Normal:
                moveSpeed = 7f;
                if (hitTime - (Time.time-startTime) <  2f) {
                    transform.position = targetPositon - moveDirectionRotation * ((hitTime - (Time.time-startTime)) * Vector3.left * moveSpeed);
                }
                break;
            case NoteType.Shuriken:
                break;
            case NoteType.Heal:
                break;
            case NoteType.Shield:
                break;
            case NoteType.Fire:
                break;
            case NoteType.Zap:
                break;
            case NoteType.Poison:
                break;
            case NoteType.Sloth:
                break;
            case NoteType.Greed:
                break;
            case NoteType.GluttonyFast:
                break;
            case NoteType.GluttonySlow:
                break;
            case NoteType.Wrath:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Triggered");
        if (other.tag == "Activator") {
            collidedActivator = other.transform;
            Debug.Log("Active");
            canBePressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Activator") {
            canBePressed = false;

            if (!obtained) {
                Debug.Log ("Note Missed");
                Instantiate(missEffect, transform.position, Quaternion.Euler(new Vector3(0, 0, Random.Range(-10, 10))), effectsHolder.transform);
                ScoreSystem.Instance.NoteMissed();
            }

            Destroy(gameObject);

        }
    }
}
