using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsController : MonoBehaviour
{
    [Header("Button Image Utilities")]
    private SpriteRenderer buttonRenderer;
    public Sprite defaultButtonImage;
    public Sprite pressedButtonImage;

    [Header("Button Function Utilities")]
    public KeyCode keyToPress;

    void Start() {
        buttonRenderer = GetComponent<SpriteRenderer>(); }

    void Update() {
        if (Input.GetKeyDown (keyToPress) ) {
            buttonRenderer.sprite = pressedButtonImage; }
        if (Input.GetKeyUp (keyToPress) ) {
            buttonRenderer.sprite = defaultButtonImage; } }
}
