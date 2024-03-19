using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Rendering.Universal;

public enum Mode {Defense, Offense}
public class ScoreSystem : MonoBehaviour
{
    public Mode mode;
    public Light2D globalLight;
    public Light2D heartLight;

    public bool inGame;

    [Header("Score")]
    public int currentScore;
    public int normalNote = 5;
    public int goodNote = 7;
    public int perfectNote = 10;

    
    public int normalHurt = 2;
    public int goodHurt = 1;
    public int missHurt = 10;


    [Header("Multiplier")]
    public float currentMult;
    public int combo;


    [Header("Results")]
    public float totalNotes;
    public float normalHits;
    public float goodHits;
    public float perfectHits;
    public float missedHits;
    
    /*
    public GameObject resultsScreen;
    public TMP_Text percentHitText;

    public TMP_Text normalNotesText;
    public TMP_Text goodNotesText;
    public TMP_Text perfectNotesText;
    public TMP_Text missedNotesText;

    public TMP_Text rankText;
    public TMP_Text finalScoreText;
    */

    public TextMeshProUGUI Text_Combo;

    public InputRecorder inputRecorder;

    [Header("AudioPlayer")]
    public AudioPlayer audioPlayer;

    [Header("Song")]
    public bool shopping = false;
    public bool songStarted = false;
    public InGameCanvas inGameCanvas;

    [Header("Health")]
    public int playerHealth;
    public int bossHealth;
    public int playerMaxHealth;
    public int bossMaxHealth;
    private List<float> bossDamage = new() { 2f, 1.5f, 1.5f, 1.5f };
    private List<int> bossMaxHealths = new() { 500, 650, 900, 1200 };

    [Header("Effects")]
    public int comboProtection = 0;
    public int poisonLevel = 0;
    public float poisonTime = -999f; //Start of current poison effect
    public float fireTime = -999f; //Start of current fire effect
    public float lastPoisonTime = -999f; //last poison damage taken's time
    public float lastFireTime = -999f; //last fire damage taken's time

    public GameObject projectile;

    public bool lost = false;

    void Start() {       
        bossHealth = bossMaxHealths[0];
        bossMaxHealth = bossMaxHealths[0];

        inGameCanvas = FindObjectOfType<InGameCanvas>();
        // inGameCanvas.UpdateHealth(); - - - - - - - add back in TODO

        //display the score as 0 from the beginning
            //scoreText.text = "Score: 0";
        
        //start multiplier at 1 and tracker at 0
        currentMult = 1f;
        combo = 0;
        
        //count the total number of notes in the level
            //totalNotes = FindObjectsOfType <NoteObject>().Length;
    }

    //only begin the music once the game has started, only start the game when a putton is pressed
    void Update()
    {
        if (!lost && playerHealth <= 0) {
            lost = true;
            //TODO: Lost
        }

        if (!shopping && !songStarted && audioPlayer.currentClip == 9 && Input.GetKeyDown(KeyCode.Space)) {
            //Press Space to Load and Start Song
            songStarted = true;

            audioPlayer.currentClip = inputRecorder.inputFileIndex;

            if (!inputRecorder.recording) {
                inputRecorder.LoadInputRecords(); //Load all Note data and Instantiate objects, then StartSong
            }
            else {
                StartSong(); //Start recording
            }
        }
        if (songStarted && audioPlayer.currentClip != 9 && !audioPlayer.audioSource.isPlaying) {
            //Song Ended

            if (!lost && bossHealth > 0) {
                lost = true;
                //TODO: Lost
            }

            songStarted = false;
            FindObjectOfType<NoteManager>().StopAllCoroutines();
            
            audioPlayer.currentClip = 9;
            audioPlayer.audioSource.clip = audioPlayer.songClips[9];
            audioPlayer.audioSource.loop = true;
            audioPlayer.audioSource.Play();
            
            comboProtection = 0;
            poisonLevel = 0;
            poisonTime = -999f;
            fireTime = -999f;
            lastPoisonTime = -999f;
            lastFireTime = -999f;
            inGameCanvas.ShieldEffect.color = new Color(1f, 1f, 1f, 0.2f);
            inGameCanvas.Text_ShieldNum.text = "";
            inGameCanvas.PoisonEffect.color = new Color(1f, 1f, 1f, 0.2f);
            inGameCanvas.Text_PoisonNum.text = "";
            inGameCanvas.FireEffect.color = new Color(1f, 1f, 1f, 0.2f);

            if (inputRecorder.inputFileIndex < 4) {
                //Heal 10%
                playerHealth = (int)Mathf.Clamp(playerHealth + playerMaxHealth * 0.1f, 0f, playerMaxHealth);

                GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity); 
                proj.GetComponent<Projectile>().noteType = NoteType.Heal;

                bossHealth = bossMaxHealths[inputRecorder.inputFileIndex];
                bossMaxHealth = bossMaxHealths[inputRecorder.inputFileIndex];
                inGameCanvas.UpdateHealth();

                inGameCanvas.StartPopup.SetActive(true);
                inGameCanvas.Text_BossName.text = inGameCanvas.bossNames[inputRecorder.inputFileIndex];
                inGameCanvas.Text_Boss.text = inGameCanvas.bossNames[inputRecorder.inputFileIndex];
                inGameCanvas.boss_SR.sprite = inGameCanvas.bossSprites[inputRecorder.inputFileIndex];
                GameManager.Instance.NewChooseItems();
                inputRecorder.Increment();
            }
        }
        if (songStarted) {
            if (Time.time - inputRecorder.startTime <= 12f || (int)(Time.time - inputRecorder.startTime) / 12 % 2 == 1)
                DefenseMode();
            else
                OffenseMode();
        }
        
        //end of game, show results screen

        //Deactivated by Timothy for now
        /*
        if (!audioPlayer.audioSource.isPlaying && !resultsScreen.activeInHierarchy) {
            resultsScreen.SetActive (true);
            //calculate hits
            normalNotesText.text = "" + normalHits;
            goodNotesText.text = goodHits.ToString();
            perfectNotesText.text = perfectHits.ToString(); 
            missedNotesText.text = "" + missedHits;
            float totalHit = normalHits + goodHits + perfectHits;
            float percentHit = (totalHit / totalNotes) * 100f;
            //percent hit
            percentHitText.text = percentHit.ToString ("F2") + "%";
            //rank
            rankText.text = "F";
            if (percentHit > 30)
                rankText.text = "E";
            if (percentHit > 45)
                rankText.text = "D";
            if (percentHit > 60)
                rankText.text = "C";
            if (percentHit > 75)
                rankText.text = "B";
            if (percentHit > 90)
                rankText.text = "A";
        
            finalScoreText.text = currentScore.ToString();
        }
        */
        
        //Effect management
        if (Time.time - poisonTime > 1.5f) {
            poisonLevel = 0;
        }

        if (Time.time - fireTime < 1.5f) {
            Debug.LogError("FireTime");
            if (Time.time - lastFireTime > 0.3f) {
                Debug.LogError("LastFireTime");
                lastFireTime = Time.time;
                FindObjectOfType<VFXManager>().ShakeBoss();
                bossHealth -= 2;
                inGameCanvas.UpdateHealth();
            }
        } else if (inGame) {
            inGameCanvas.FireEffect.color = new Color(1f, 1f, 1f, 0.2f);
        }

        if (Time.time - poisonTime < 1.5f) {
            Debug.LogError("PoisonTime");
            if (Time.time - lastPoisonTime > 0.3f) {
                Debug.LogError("LastPoisonTime");
                lastPoisonTime = Time.time;
                FindObjectOfType<VFXManager>().ShakeBoss();
                bossHealth -= poisonLevel;
                inGameCanvas.UpdateHealth();
            }
        } else if (inGame) {
            inGameCanvas.PoisonEffect.color = new Color(1f, 1f, 1f, 0.2f);
            inGameCanvas.Text_PoisonNum.text = "";
        }
    }

    private void OffenseMode()
    {
        mode = Mode.Offense;
        globalLight.color = new Color(1f, 1f, 1f, 1f);
        globalLight.intensity = Mathf.Sin((Time.time - inputRecorder.startTime) * Mathf.PI / 2f)*0.1f + 0.8f;
        heartLight.intensity = 0f;
    }

    private void DefenseMode()
    {
        mode = Mode.Defense;
        globalLight.color = new Color(1f, 0.5f, 0.5f, 1f);
        globalLight.intensity = 1f;
        heartLight.intensity = Mathf.Sin((Time.time - inputRecorder.startTime) * Mathf.PI / 2f)*0.1f + 0.5f;
    }

    public void StartSong()
    {
        audioPlayer.audioSource.clip = audioPlayer.songClips[inputRecorder.inputFileIndex];
        audioPlayer.audioSource.loop = false;
        audioPlayer.audioSource.Play();
        inputRecorder.startTime = Time.time;

        inGameCanvas.StartPopup.SetActive(false);

        mode = Mode.Defense;

        //Reset Song Data
        combo = 0;
        totalNotes = 0;
        normalHits = 0;
        goodHits = 0;
        perfectHits = 0;
        missedHits = 0;

        //Reset Effects
        comboProtection = 0;
        poisonLevel = 0;
        poisonTime = -999f;
        fireTime = -999f;
        lastPoisonTime = -999f;
        lastFireTime = -999f;
        inGameCanvas.ShieldEffect.color = new Color(1f, 1f, 1f, 0.2f);
        inGameCanvas.Text_ShieldNum.text = "";
        inGameCanvas.PoisonEffect.color = new Color(1f, 1f, 1f, 0.2f);
        inGameCanvas.Text_PoisonNum.text = "";
        inGameCanvas.FireEffect.color = new Color(1f, 1f, 1f, 0.2f);
        
        foreach (NoteObject noteObject in FindObjectsOfType<NoteObject>()) {
            noteObject.startTime = inputRecorder.startTime;
            totalNotes++;
        }
    }

    //when the player hits a note, they recieve points, if they miss they recieve nothing
    public void NoteHit()
    { 
        Debug.Log("Note Hit");

        //track multiplier
        combo++;
        currentMult += 0.1f;
        if (currentMult > 1.5f) {
            currentMult = 1f;
        }

        //update combo UI text
        if (combo >= 5) {
            Text_Combo.text = combo.ToString();
            Text_Combo.transform.localScale = Vector3.one;
            Text_Combo.color = Color.gray;
        }
        if (combo >= 20) {
            Text_Combo.transform.localScale = Vector3.one * 1.2f;
            Text_Combo.color = Color.yellow;
        }
        if (combo >= 50) {
            Text_Combo.transform.localScale = Vector3.one * 1.3f;
            Text_Combo.color = Color.cyan;
        }
    }

    public void NormalHit(NoteType noteType)
    {
        currentScore += (int)(normalNote * currentMult);
        normalHits++;
        NoteHit();

        if ((int)noteType < 7) {
            switch (noteType) {
                case NoteType.Normal:
                    bossHealth -= (int)(normalNote * currentMult);
                    break;
                case NoteType.Shuriken:
                    bossHealth -= (int)(normalNote * currentMult * 2f);
                    break;
                case NoteType.Heal:
                    playerHealth += 5;
                    if (playerHealth > 1000)
                        playerHealth = 1000;
                    break;
                case NoteType.Shield:
                    break;
                case NoteType.Fire:
                    bossHealth -= (int)(normalNote * currentMult); //Burn 5 times, every 0.3 second -2 health
                    inGameCanvas.FireEffect.color = Color.white;
                    fireTime = Time.time;
                    break;
                case NoteType.Zap:
                    if (combo >= 5) {
                        FindObjectOfType<VFXManager>().ShakeBoss();
                        bossHealth -= (int)(normalNote * currentMult * 3f); //If Combo > 5, x3 damage
                    }
                    break;
                case NoteType.Poison:
                    bossHealth -= (int)(normalNote * currentMult); //Poison up to 5 stacks, every 0.3 second -1 health, poison 5 times
                    if (poisonLevel < 5)
                        poisonLevel++;
                    inGameCanvas.PoisonEffect.color = Color.white;
                    inGameCanvas.Text_PoisonNum.text = poisonLevel.ToString();
                    poisonTime = Time.time;
                    break;
            }
        }
        else {
            playerHealth -= (int)(bossDamage[inputRecorder.inputFileIndex-1] * normalHurt);
        }
        
        inGameCanvas.UpdateHealth();
    }
    public void GoodHit(NoteType noteType)
    {
        currentScore += (int)(goodNote * currentMult);
        goodHits++;
        NoteHit();

        if ((int)noteType < 7) {
            switch (noteType) {
                case NoteType.Normal:
                    bossHealth -= (int)(goodNote * currentMult);
                    break;
                case NoteType.Shuriken:
                    bossHealth -= (int)(goodNote * currentMult * 2f);
                    break;
                case NoteType.Heal:
                    playerHealth += 5;
                    if (playerHealth > 1000)
                        playerHealth = 1000;
                    break;
                case NoteType.Shield:
                    comboProtection++;
                    inGameCanvas.ShieldEffect.color = Color.white;
                    inGameCanvas.Text_ShieldNum.text = comboProtection.ToString();
                    break;
                case NoteType.Fire:
                    bossHealth -= (int)(goodNote * currentMult);
                    inGameCanvas.FireEffect.color = Color.white;
                    fireTime = Time.time;
                    break;
                case NoteType.Zap:
                    if (combo >= 5) {
                        FindObjectOfType<VFXManager>().ShakeBoss();
                        bossHealth -= (int)(goodNote * currentMult * 3f); //If Combo > 5, x3 damage
                    }
                    break;
                case NoteType.Poison:
                    bossHealth -= (int)(goodNote * currentMult);
                    if (poisonLevel < 5)
                        poisonLevel++;
                    inGameCanvas.PoisonEffect.color = Color.white;
                    inGameCanvas.Text_PoisonNum.text = poisonLevel.ToString();
                    poisonTime = Time.time;
                    break;
            }
        }
        else {
            playerHealth -= (int)(bossDamage[inputRecorder.inputFileIndex-1] * goodHurt);
        }
        
        inGameCanvas.UpdateHealth();
    }
    public void PerfectHit(NoteType noteType)
    {
        currentScore += (int)(perfectNote * currentMult);
        perfectHits++;
        NoteHit();

        if ((int)noteType < 7) {
            switch (noteType) {
                case NoteType.Normal:
                    bossHealth -= (int)(perfectNote * currentMult);
                    break;
                case NoteType.Shuriken:
                    bossHealth -= (int)(perfectNote * currentMult * 2f);
                    break;
                case NoteType.Heal:
                    playerHealth += 5;
                    if (playerHealth > 1000)
                        playerHealth = 1000;
                    break;
                case NoteType.Shield:
                    comboProtection++;
                    inGameCanvas.ShieldEffect.color = Color.white;
                    inGameCanvas.Text_ShieldNum.text = comboProtection.ToString();
                    break;
                case NoteType.Fire:
                    bossHealth -= (int)(perfectNote * currentMult);
                    inGameCanvas.FireEffect.color = Color.white;
                    fireTime = Time.time;
                    break;
                case NoteType.Zap:
                    if (combo >= 5) {
                        FindObjectOfType<VFXManager>().ShakeBoss();
                        bossHealth -= (int)(perfectNote * currentMult * 3f); //If Combo > 5, x3 damage
                    }
                    break;
                case NoteType.Poison:
                    bossHealth -= (int)(perfectNote * currentMult);
                    if (poisonLevel < 5)
                        poisonLevel++;
                    inGameCanvas.PoisonEffect.color = Color.white;
                    inGameCanvas.Text_PoisonNum.text = poisonLevel.ToString();
                    poisonTime = Time.time;
                    break;
            }
        }

        inGameCanvas.UpdateHealth();
    }

    public void NoteMissed(NoteType noteType)
    {
        missedHits++;
    
        //Clear multiplier
        currentMult = 1;
        if (comboProtection > 0) {
            comboProtection--;
            inGameCanvas.Text_ShieldNum.text = comboProtection.ToString();
        }
        else {
            combo = 0;
            //update combo UI text
            Text_Combo.text = "";
        }

        if (comboProtection <= 0) {
            inGameCanvas.ShieldEffect.color = new Color(1f, 1f, 1f, 0.2f);
            inGameCanvas.Text_ShieldNum.text = "";
        }

        if ((int)noteType > 6) {
            playerHealth -= (int)(bossDamage[inputRecorder.inputFileIndex-1] * missHurt);
        }
        
        if (noteType == NoteType.Poison) {
            poisonTime = -999f;
            poisonLevel = 0;
        }

        inGameCanvas.UpdateHealth();
    }
}
