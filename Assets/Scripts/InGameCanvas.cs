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
    public RectTransform playerHealthBar; 
    public RectTransform bossHealthBar; 
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

    private float hoverTime = 99999f;
    private bool hovering = false;
    private NoteType hoveringNoteType = NoteType.Normal;

    [Header("Item Info")]
    public GameObject ItemInfo;
    public TextMeshProUGUI Text_ItemName;
    public TextMeshProUGUI Text_ItemDesc;
    private List<String> ItemNames = new()
    {
        "Dagger",
        "Shuriken",
        "Heal",
        "Shield",
        "Fireball",
        "Zap",
        "Poison"
    };
    private List<String> ItemDescs = new()
    {
        "The Safest Choice\nDeals 1x Damage",
        "2x Speed\n2x Damage",
        "+5 health per hit",
        "Good / Perfect hits\ngives Combo Protection (Can stack)",
        "Burns after dealing damage\n(Cannot stack)",
        "Deals 3x Damage ONLY if\n Combo > 5",
        "Poisons after dealing damage\n(Max Stack 5 times, Miss = Reset)"
    };

    [Header("Effects")]
    public Image ShieldEffect;
    public Image PoisonEffect;
    public Image FireEffect;
    public TextMeshProUGUI Text_ShieldNum;
    public TextMeshProUGUI Text_PoisonNum;

    // Start is called before the first frame update
    void Start()
    {
        ReloadItems();
        
        GameManager.Instance.NewChooseItems();
    }

    // Update is called once per frame
    void Update()
    {
        if (hovering && Time.time - hoverTime > 0.3f) {
            ShowAndUpdateItemInfo();
        } else {
            DisableItemInfo();
        }
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
        ScoreSystem.Instance.playerHealth = (int)Mathf.Clamp(ScoreSystem.Instance.playerHealth + ScoreSystem.Instance.playerMaxHealth * 0.3f, 0f, ScoreSystem.Instance.playerMaxHealth);
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
        playerHealthBar.offsetMax = new Vector2(Mathf.Lerp(-297f, -3f, (float)ScoreSystem.Instance.playerHealth/ScoreSystem.Instance.playerMaxHealth), playerHealthBar.offsetMax.y);
        bossHealthBar.offsetMin = new Vector2(Mathf.Lerp(297f, 3f, (float)ScoreSystem.Instance.bossHealth/ScoreSystem.Instance.bossMaxHealth), bossHealthBar.offsetMin.y);
    }

    public void HoverChooseItem(int hoverID)
    {
        hoverTime = Time.time;
        hovering = true;
        hoveringNoteType = GameManager.Instance.chooseNoteTypes[hoverID];
    }

    public void HoverItem(int hoverID)
    {
        hoverTime = Time.time;
        hovering = true;
        hoveringNoteType = GameManager.Instance.noteTypes[hoverID];
    }

    public void ExitHover()
    {
        hovering = false;
    }

    private void ShowAndUpdateItemInfo()
    {
        ItemInfo.SetActive(true);
        Text_ItemName.text = ItemNames[(int)hoveringNoteType];
        Text_ItemDesc.text = ItemDescs[(int)hoveringNoteType];
        ItemInfo.GetComponent<RectTransform>().anchoredPosition = new Vector2(Input.mousePosition.x, Mathf.Clamp(Input.mousePosition.y, 175f, 999999f));
    }

    private void DisableItemInfo()
    {
        ItemInfo.SetActive(false);
    }
}