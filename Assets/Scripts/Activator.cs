using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public KeyCode keyCode;
    public Sprite DefenseSprite;
    public Sprite OffenseSprite;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyCode)) {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
        }
        if (Input.GetKeyUp(keyCode)) {
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
