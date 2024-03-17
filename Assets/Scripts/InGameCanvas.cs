using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameCanvas : MonoBehaviour
{
    public List<Image> ItemFrames;
    public List<Image> ItemContents;

    public List<Color> FrameColors;
    public List<Sprite> ItemSprites;

    // Start is called before the first frame update
    void Start()
    {
        LoadItems();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadItems()
    {
        for (int i=0; i < 4; i++) {
            ItemFrames[i].color = FrameColors[(int)GameManager.Instance.noteTypes[i]];
            ItemContents[i].sprite = ItemSprites[(int)GameManager.Instance.noteTypes[i]];
        }
    }
}
