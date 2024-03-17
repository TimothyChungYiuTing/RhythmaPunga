using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameCanvas : MonoBehaviour
{
    public List<Image> ItemFrames;
    public List<Image> ItemContents;

    public int selected = -1;
    public GameObject ChoosePopup;
    public TextMeshProUGUI Text_ChooseOne;
    public List<Image> ChooseItemFrames;
    public List<Image> ChooseItemContents;

    public List<Color> FrameColors;
    public List<Sprite> ItemSprites;

    public bool choosingItem = false;

    // Start is called before the first frame update
    void Start()
    {
        ReloadItems();
        //LoadChooseItems();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReloadItems()
    {
        for (int i=0; i < 4; i++) {
            ItemFrames[i].color = FrameColors[(int)GameManager.Instance.noteTypes[i]];
            ItemContents[i].sprite = ItemSprites[(int)GameManager.Instance.noteTypes[i]];
        }
    }

    public void LoadChooseItems()
    {
        selected = -1;
        Text_ChooseOne.text = "Choose One";
        ChoosePopup.SetActive(true);
        choosingItem = true;
        ChooseItemFrames[0].color = FrameColors[(int)GameManager.Instance.chooseNoteTypes[0]];
        ChooseItemContents[0].sprite = ItemSprites[(int)GameManager.Instance.chooseNoteTypes[0]];
        ChooseItemFrames[1].color = FrameColors[(int)GameManager.Instance.chooseNoteTypes[1]];
        ChooseItemContents[1].sprite = ItemSprites[(int)GameManager.Instance.chooseNoteTypes[1]];
    }

    public void SelectItem(int selectedID)
    {
        selected = selectedID;
        Text_ChooseOne.text = "Select Item to Swap";

        ChooseItemFrames[0].color = FrameColors[(int)GameManager.Instance.chooseNoteTypes[0]];
        ChooseItemFrames[1].color = FrameColors[(int)GameManager.Instance.chooseNoteTypes[1]];
        
        ChooseItemFrames[selectedID].color = Color.white;
    }

    public void CloseChooseItems()
    {
        ChoosePopup.SetActive(false);
        choosingItem = false;
    }

    public void SwapItem(int swapID)
    {
        if (selected != -1) {
            if (selected < 2) {
                GameManager.Instance.noteTypes[swapID] = GameManager.Instance.chooseNoteTypes[selected];
                
                selected = -1;
                ReloadItems();
                
                ChoosePopup.SetActive(false);
                choosingItem = false;
            } else {
                //Swap Notes
                NoteType tempNoteType = GameManager.Instance.noteTypes[selected-2];
                GameManager.Instance.noteTypes[selected-2] = GameManager.Instance.noteTypes[swapID];
                GameManager.Instance.noteTypes[swapID] = tempNoteType;
                
                selected = -1;
                ReloadItems();
            }
        } else {
            selected = swapID+2;
            ItemFrames[swapID].color = Color.white;
        }
    }
}
