using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum Mode {Defense, Offense}
public class ScoreSystem : Singleton<ScoreSystem>
{
    public Mode mode;

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
    private AudioPlayer audioPlayer;

    [Header("Song")]
    public bool shopping = false;
    public bool songStarted = false;
    public InGameCanvas inGameCanvas;

    [Header("Calibration Sync")]
    public float offset = 0f;  //Calibration syncing, added onto hitTime

    [Header("Health")]
    public int playerHealth;
    public int bossHealth;
    public int playerMaxHealth;
    public int bossMaxHealth;
    public List<float> bossDamage;
    public List<int> bossMaxHealths;

    void Start() {
        audioPlayer = FindObjectOfType<AudioPlayer>();
        
        bossHealth = bossMaxHealths[0];
        bossMaxHealth = bossMaxHealths[0];
        inGameCanvas.UpdateHealth();

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
            songStarted = false;
            FindObjectOfType<NoteManager>().StopAllCoroutines();
            
            audioPlayer.currentClip = 9;
            audioPlayer.audioSource.clip = audioPlayer.songClips[9];
            audioPlayer.audioSource.loop = true;
            audioPlayer.audioSource.Play();

            if (inputRecorder.inputFileIndex < 4) {
                playerHealth = (int)Mathf.Clamp(playerHealth + playerMaxHealth * 0.1f, 0f, playerMaxHealth);
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
    }

    private void OffenseMode()
    {
        mode = Mode.Offense;
    }

    private void DefenseMode()
    {
        mode = Mode.Defense;
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
                    break;
                case NoteType.Shield:
                    break;
                case NoteType.Fire:
                    bossHealth -= (int)(normalNote * currentMult); //Burn 5 times, every 0.3 second -2 health
                    break;
                case NoteType.Zap:
                    if (combo >= 20)
                        bossHealth -= (int)(normalNote * currentMult * 3f); //If Combo > 20, x3 damage
                    break;
                case NoteType.Poison:
                    bossHealth -= (int)(normalNote * currentMult); //Poison up to 5 stacks, every 0.3 second -1 health, poison 5 times
                    break;
            }
        }
        else
            playerHealth -= (int)(bossDamage[inputRecorder.inputFileIndex-1] * normalHurt);
        
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
                    break;
                case NoteType.Shield:
                    break;
                case NoteType.Fire:
                    bossHealth -= (int)(goodNote * currentMult);
                    break;
                case NoteType.Zap:
                    if (combo >= 20)
                        bossHealth -= (int)(goodNote * currentMult * 3f); //If Combo > 20, x3 damage
                    break;
                case NoteType.Poison:
                    bossHealth -= (int)(goodNote * currentMult);
                    break;
            }
        }
        else
            playerHealth -= (int)(bossDamage[inputRecorder.inputFileIndex-1] * goodHurt);
        
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
                    break;
                case NoteType.Shield:
                    break;
                case NoteType.Fire:
                    bossHealth -= (int)(perfectNote * currentMult);
                    break;
                case NoteType.Zap:
                    if (combo >= 20)
                        bossHealth -= (int)(perfectNote * currentMult * 3f); //If Combo > 20, x3 damage
                    break;
                case NoteType.Poison:
                    bossHealth -= (int)(perfectNote * currentMult);
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
        combo = 0;

        //update combo UI text
        Text_Combo.text = "";

        if ((int)noteType > 6)
            playerHealth -= (int)(bossDamage[inputRecorder.inputFileIndex-1] * missHurt);

        inGameCanvas.UpdateHealth();
    }
}
