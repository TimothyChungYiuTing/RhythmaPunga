using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Assets")]
    public AudioSource levelMusic;
    public NoteScroller NSScript;

    [Header("Flags")]
    public bool playMusic;

    [Header("Detector")]
    public static GameManager instance;

    [Header("Score")]
    public int currentScore;
    public int normalNote = 100;
    public int goodNote = 150;
    public int perfectNote = 200;


    [Header("Multiplier")]
    public int currentMiltiplier;
    public int multiplierTracker;
    public int[] multiplierThreshold;

    [Header("Text")]
    public TMP_Text scoreText;
    public TMP_Text multiplierText;

    [Header("Results")]
    public float totalNotes;
    public float normalHits;
    public float goodHits;
    public float perfectHits;
    public float missedHits;

    public GameObject resultsScreen;
    public TMP_Text percentHitText;

    public TMP_Text normalNotesText;
    public TMP_Text goodNotesText;
    public TMP_Text perfectNotesText;
    public TMP_Text missedNotesText;

    public TMP_Text rankText;
    public TMP_Text finalScoreText;

    void Start() {
        //ensure that the GameManager is initialised
        if (instance == null) {
            instance = this; }
        else {
            Debug.LogWarning("Duplicate GameManager instance detected. Destroying this one.");
            Destroy(gameObject); }

        //display the score as 0 from the beginning
        scoreText.text = "Score: 0";
        
        //start multiplier at 1 and tracker at 0
        currentMiltiplier = 1;
        multiplierTracker = 0;
        
        //count the total number of notes in the level
        totalNotes = FindObjectsOfType <NoteObject>().Length; }

    //only begin the music once the game has started, only start the game when a putton is pressed
    void Update() {
        //start the game
        if (!playMusic) {
            if (Input.anyKeyDown) {
                playMusic = true;
                NSScript.songStarted = true;
            
                levelMusic.Play(); } }
        //end of game, show results screen
        else if (!levelMusic.isPlaying && !resultsScreen.activeInHierarchy) {
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
            if (percentHit > 30) { rankText.text = "E";
                if (percentHit > 45) { rankText.text = "D";
                    if (percentHit > 60) { rankText.text = "C";
                        if (percentHit > 75) { rankText.text = "B";
                            if (percentHit > 90) { rankText.text = "A"; } } } } }
        
            finalScoreText.text = currentScore.ToString(); } }

    //when the player hits a note, they recieve points, if they miss they recieve nothing
    public void NoteHit() { 
        Debug.Log("Note Hit");

        //track multiplier
        if (currentMiltiplier - 1 < multiplierThreshold.Length) { 
            multiplierTracker ++;
            if (multiplierThreshold[currentMiltiplier - 1] <= multiplierTracker) {
                multiplierTracker = 0;
                currentMiltiplier ++; } }

        //update multiplier UI text
        multiplierText.text = "Multiplier: x" + currentMiltiplier;
        //update score UI text
        scoreText.text = "Score: " + currentScore; }

    public void NormalHit() {
        currentScore += normalNote * currentMiltiplier;
        normalHits++;
        NoteHit(); }
    public void GoodHit() {
        currentScore += goodNote * currentMiltiplier;
        goodHits++;
        NoteHit(); }
    public void PerfectHit() {
        currentScore += perfectNote * currentMiltiplier;
        perfectHits++;
        NoteHit(); }

    public void NoteMissed() {
        Debug.Log("Note Missed");
        missedHits++;
    
        //multiplier nonsense
        currentMiltiplier = 1;
        multiplierTracker = 0;

        //update multiplier UI text
        multiplierText.text = "Multiplier: x" + currentMiltiplier; }
}