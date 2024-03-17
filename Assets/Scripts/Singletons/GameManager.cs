using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public enum State {
    TITLE, MENU, GAME, SETTINGS, END
}

public class GameManager : Singleton<GameManager>
{

    public List<NoteType> noteTypes;
    public List<NoteType> chooseNoteTypes;
    public List<int> otherItemsID = new(); //Not used for now


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug
        if (Input.GetKey(KeyCode.R)) {
            /*
            Time.timeScale = 1f;
            AudioManager.Instance.spatialBlend = 0f;
            */
            AudioManager.Instance.ChangeSong(0);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}