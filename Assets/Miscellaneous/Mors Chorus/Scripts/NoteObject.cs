using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
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

    void Update() {
        if (Input.GetKeyDown (keyToPress)) {
            if (canBePressed) {
                obtained = true;
                gameObject.SetActive (false);

                if (Mathf.Abs (transform.position.y) > 0.25) {
                    Debug.Log ("Normal Hit");
                    Instantiate (normalEffect, transform.position, Quaternion.Euler(new Vector3(0, 0, Random.Range(-10, 10))), effectsHolder.transform);
                    GameManager2.instance.NormalHit(); }
                else if (Mathf.Abs (transform.position.y) > 0.05) {
                    Debug.Log ("Good Hit!");
                    Instantiate(goodEffect, transform.position, Quaternion.Euler(new Vector3(0, 0, Random.Range(-10, 10))), effectsHolder.transform);
                    GameManager2.instance.GoodHit(); }
                else {
                    Debug.Log ("Perfect Hit!!");
                    Instantiate(perfectEffect, transform.position, Quaternion.Euler(new Vector3(0, 0, Random.Range(-10, 10))), effectsHolder.transform);
                    GameManager2.instance.PerfectHit(); } } } }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Activator") {
            canBePressed = true; }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Activator") {
            canBePressed = false;

            if (!obtained) {
                Debug.Log ("Note Missed");
                Instantiate(missEffect, transform.position, Quaternion.Euler(new Vector3(0, 0, Random.Range(-10, 10))), effectsHolder.transform);
                GameManager2.instance.NoteMissed();} }
    }
}
