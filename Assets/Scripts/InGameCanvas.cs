using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InGameCanvas : MonoBehaviour
{
    public List<Image> ItemFrames;
    public List<Image> ItemContents;

    public int selected = -1;
    public GameObject StartPopup;
    public TextMeshProUGUI Text_BossName;
    public TextMeshProUGUI Text_Boss;
    public TextMeshProUGUI Text_PlayerHealth;
    public TextMeshProUGUI Text_BossHealth;
    public SpriteRenderer boss_SR;
    public List<String> bossNames;
    public List<Sprite> bossSprites;

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
        
        GameManager.Instance.NewChooseItems();
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
        ScoreSystem.Instance.shopping = true;
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
        //FindObjectOfType<InputRecorder>().TryLoadInputRecords();
        ScoreSystem.Instance.shopping = false;
        ChoosePopup.SetActive(false);
        choosingItem = false;
        ScoreSystem.Instance.playerHealth = (int)Mathf.Clamp(ScoreSystem.Instance.playerHealth + ScoreSystem.Instance.playerMaxHealth * 0.5f, 0f, ScoreSystem.Instance.playerMaxHealth);
        UpdateHealth();
    }

    public void SwapItem(int swapID)
    {
        if (!ScoreSystem.Instance.songStarted) {
            if (selected != -1) {
                if (selected < 2) {
                    //Swap chosen Note with Key
                    GameManager.Instance.noteTypes[swapID] = GameManager.Instance.chooseNoteTypes[selected];

                    selected = -1;
                    ReloadItems();
                    
                    //FindObjectOfType<InputRecorder>().TryLoadInputRecords();
                    ScoreSystem.Instance.shopping = false;
                    ChoosePopup.SetActive(false);
                    choosingItem = false;
                } else {
                    //Swap Notes between 2 Keys
                    if (swapID != selected-2) {
                        NoteType tempNoteType = GameManager.Instance.noteTypes[selected-2];
                        GameManager.Instance.noteTypes[selected-2] = GameManager.Instance.noteTypes[swapID];
                        GameManager.Instance.noteTypes[swapID] = tempNoteType;
                        
                        selected = -1;
                        ReloadItems();
                        
                        //FindObjectOfType<InputRecorder>().TryLoadInputRecords();
                    }
                }
            } else {
                selected = swapID+2;
                ItemFrames[swapID].color = Color.white;
            }
        }
    }

    public void UpdateHealth()
    {
        Text_PlayerHealth.text = ScoreSystem.Instance.playerHealth.ToString() + " / " + ScoreSystem.Instance.playerMaxHealth;
        Text_BossHealth.text = ScoreSystem.Instance.bossHealth.ToString() + " / " + ScoreSystem.Instance.bossMaxHealth;
    }
}