using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public KeyCode keyCode;
    public Sprite DefenseSprite;
    public Sprite OffenseSprite;

    private ScoreSystem scoreSystem;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        scoreSystem = FindObjectOfType<ScoreSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyCode)) {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
            transform.localScale = Vector3.one * 0.8f;
        }
        if (Input.GetKeyUp(keyCode)) {
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            transform.localScale = Vector3.one * 0.7f;
        }

        if (scoreSystem.mode == Mode.Defense) {
            spriteRenderer.sprite = DefenseSprite;
        } else {
            spriteRenderer.sprite = OffenseSprite;
        }
    }
}
