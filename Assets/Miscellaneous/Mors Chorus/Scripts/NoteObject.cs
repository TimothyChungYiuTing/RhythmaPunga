using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NoteType {Normal, Shuriken, Heal, Shield, Fire, Zap, Poison, Sloth, Greed, GluttonyFast, GluttonySlow, Wrath}
public class NoteObject : MonoBehaviour
{
    public enum NoteDirection {D, W, A, S}

    [Header("NoteCustomization")]
    private SpriteRenderer spriteRenderer;
    public List<Sprite> noteSprites;
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
    public float startTime; //Reference to the start time of the scene, when the song starts playing
    private Transform collidedActivator = null;

    void Start()
    {
        startTime = FindObjectOfType<InputRecorder>().startTime;
        spriteRenderer = GetComponent<SpriteRenderer>();
        Setup();

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
                if (Vector2.Distance(transform.position, collidedActivator.position) > 0.6) {
                    Debug.Log("Normal Hit");
                    Instantiate(normalEffect, transform.position, Quaternion.Euler(new Vector3(0, 0, Random.Range(-10f, 10f))), effectsHolder.transform);
                    ScoreSystem.Instance.NormalHit();
                    Destroy(gameObject);
                }
                else if (Vector2.Distance(transform.position, collidedActivator.position) > 0.4) {
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

    private void Setup()
    {
        spriteRenderer.sprite = noteSprites[(int)noteType];
        transform.localRotation = Quaternion.Euler(0f, 0f, (int)noteDirection * 90f);

        if ((int)noteType > 6) {
            spriteRenderer.color = Color.red;
            if (noteType == NoteType.GluttonyFast) {
                spriteRenderer.color = Color.cyan;
            }
            else if (noteType == NoteType.Wrath) {
                spriteRenderer.color = Color.yellow;
            }
        }
    }

    private void Positioning()
    {
        switch (noteType) {
            case NoteType.Normal:
                moveSpeed = 8f;
                if (hitTime - (Time.time-startTime) <  2f) {
                    transform.position = targetPositon - moveDirectionRotation * ((hitTime - (Time.time-startTime)) * Vector3.left * moveSpeed);
                }
                break;
            case NoteType.Shuriken:
                moveSpeed = 15f;
                if (hitTime - (Time.time-startTime) <  2f) {
                    transform.position = targetPositon - moveDirectionRotation * ((hitTime - (Time.time-startTime)) * Vector3.left * moveSpeed);
                    transform.localRotation = Quaternion.Euler(0f, 0f, (hitTime - (Time.time-startTime)) * (int)noteDirection * 90f);
                }
                break;
            case NoteType.Heal:
                moveSpeed = 8f;
                if (hitTime - (Time.time-startTime) <  2f) {
                    transform.position = targetPositon - moveDirectionRotation * ((hitTime - (Time.time-startTime)) * Vector3.left * moveSpeed);
                }
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.Clamp01(Mathf.Sin(Time.time * 8f) * 0.7f + 0.5f));
                
                break;
            case NoteType.Shield:
                moveSpeed = 8f;
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);
                if (hitTime - (Time.time-startTime) <  0.5f) {
                    spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, (0.5f - (hitTime - (Time.time-startTime))) * 10f);
                    transform.position = targetPositon - moveDirectionRotation * ((hitTime - (Time.time-startTime)) * Vector3.left * moveSpeed);
                }
                break;
            case NoteType.Fire:
                moveSpeed = 5f;
                if (hitTime - (Time.time-startTime) <  2f) {
                    transform.position = targetPositon - moveDirectionRotation * ((hitTime - (Time.time-startTime)) * Mathf.Lerp(2.4f, 1.25f, hitTime - (Time.time-startTime)) * Vector3.left * moveSpeed);
                }
                break;
            case NoteType.Zap:
                moveSpeed = 5f;
                if (hitTime - (Time.time-startTime) <  2f) {
                    float posMult;
                    if (hitTime - (Time.time-startTime) < 0.4f) {
                        posMult = (hitTime - (Time.time-startTime))*2.5f;
                    } else if (hitTime - (Time.time-startTime) < 1f) {
                        posMult = 1f;
                    } else {
                        posMult = hitTime - (Time.time-startTime);
                    }
                    transform.position = targetPositon - moveDirectionRotation * (posMult * Vector3.left * moveSpeed);
                }
                break;
            case NoteType.Poison:
                moveSpeed = 8f;
                if (hitTime - (Time.time-startTime) <  2f) {
                    transform.position = targetPositon - moveDirectionRotation * ((hitTime - (Time.time-startTime)) * Vector3.left * moveSpeed) + moveDirectionRotation * (Vector3.up * Mathf.Sin((hitTime - (Time.time-startTime)) * 6f) * 0.5f);
                }
                break;
            case NoteType.Sloth:
                moveSpeed = 5f;
                if (hitTime - (Time.time-startTime) <  2f) {
                    transform.position = targetPositon - moveDirectionRotation * ((hitTime - (Time.time-startTime)) * Vector3.left * moveSpeed);
                }
                break;
            case NoteType.Greed:
                moveSpeed = 5.5f;
                if (hitTime - (Time.time-startTime) <  2f) {
                    transform.position = targetPositon - moveDirectionRotation * ((hitTime - (Time.time-startTime)) * Mathf.Lerp(2.3f, 1.4f, hitTime - (Time.time-startTime)) * Vector3.left * moveSpeed);
                    transform.position += moveDirectionRotation * ((hitTime - (Time.time-startTime)) * Vector3.up * moveSpeed);
                }
                break;
            case NoteType.GluttonyFast:
                moveSpeed = 12f;
                if (hitTime - (Time.time-startTime) <  2f) {
                    transform.position = targetPositon - moveDirectionRotation * ((hitTime - (Time.time-startTime)) * Vector3.left * moveSpeed);
                }
                break;
            case NoteType.GluttonySlow:
                moveSpeed = 5f;
                if (hitTime - (Time.time-startTime) <  2f) {
                    transform.position = targetPositon - moveDirectionRotation * ((hitTime - (Time.time-startTime)) * Vector3.left * moveSpeed);
                }
                break;
            case NoteType.Wrath:
                moveSpeed = 5f + (Time.time-startTime) * 0.11f;
                if (hitTime - (Time.time-startTime) <  8f) {
                    transform.position = targetPositon - moveDirectionRotation * ((hitTime - (Time.time-startTime)) * Vector3.left * moveSpeed);
                }
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
