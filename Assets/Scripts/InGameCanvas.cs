using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameCanvas : MonoBehaviour
{
    public List<Image> ItemFrames;
    public List<Image> ItemContents;

    public GameObject ChoosePopup;
    public List<Image> ChooseItemFrames;
    public List<Image> ChooseItemContents;

    public List<Color> FrameColors;
    public List<Sprite> ItemSprites;

    public bool choosingItem = false;

    // Start is called before the first frame update
    void Start()
    {
        LoadItems();
        LoadChooseItems();
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

    public void LoadChooseItems()
    {
        ChoosePopup.SetActive(true);
        choosingItem = true;
        ChooseItemFrames[0].color = FrameColors[(int)GameManager.Instance.chooseNoteTypes[0]];
        ChooseItemContents[0].sprite = ItemSprites[(int)GameManager.Instance.chooseNoteTypes[0]];
        ChooseItemFrames[1].color = FrameColors[(int)GameManager.Instance.chooseNoteTypes[1]];
        ChooseItemContents[1].sprite = ItemSprites[(int)GameManager.Instance.chooseNoteTypes[1]];
    }

    public void CloseChooseItems()
    {
        ChoosePopup.SetActive(false);
        choosingItem = false;
    }
}
