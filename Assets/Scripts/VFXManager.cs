using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public InputRecorder inputRecorder;
    public GameObject PlayBG;
    public Vector3 PlayBG_Original_Size;
    public Vector3 PlayBG_Original_Pos;

    public GameObject CombatBG;
    public Vector3 CombatBG_Original_Size;
    public Vector3 CombatBG_Original_Pos;

    public GameObject Heart;
    public Vector3 Heart_Original_Size;
    public Vector3 Heart_Original_Pos;

    public GameObject Player;
    public Vector3 Player_Original_Size;
    public Vector3 Player_Original_Pos;

    public GameObject Boss;
    public Vector3 Boss_Original_Size;
    public Vector3 Boss_Original_Pos;
    
    public SpriteRenderer Barrier;
    
    public float modeCycle;

    private Coroutine blockCoroutine = null;

    // Start is called before the first frame update
    void Start()
    {
        PlayBG_Original_Size = PlayBG.transform.localScale;
        PlayBG_Original_Pos = PlayBG.transform.position;
        
        CombatBG_Original_Size = CombatBG.transform.localScale;
        CombatBG_Original_Pos = CombatBG.transform.position;
        
        Heart_Original_Size = Heart.transform.localScale;
        Heart_Original_Pos = Heart.transform.position;
        
        Player_Original_Size = Player.transform.localScale;
        Player_Original_Pos = Player.transform.position;
        
        Boss_Original_Size = Boss.transform.localScale;
        Boss_Original_Pos = Boss.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (inputRecorder.startTime > 0f) {
            Heart.transform.localScale = Vector3.one * (Mathf.Clamp(Mathf.Sin(((Time.time - inputRecorder.startTime) * Mathf.PI + Mathf.PI/2f) * 32f/modeCycle), -0.1f, 0.1f) * 0.4f + 1f);
        }
        else {
            Heart.transform.localScale = Vector3.one;
        }
    }

    public void Lit_PlayBG()
    {
        StartCoroutine(Lit_PlayBG_Coroutine(0.2f));
    }

    private IEnumerator Lit_PlayBG_Coroutine(float shakeSize)
    {
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);
        Vector2 randomDirection = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));

        float timer = 0f;
        PlayBG.transform.GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.6f, 0.6f, 1f);

        while (timer < 0.1f) {
            //PlayBG.transform.position = PlayBG_Original_Pos + (Vector3)(randomDirection * Mathf.Sin(timer*10 * Mathf.PI / 2f)) * shakeSize;
            timer += Time.deltaTime;
            yield return null;
        }
        PlayBG.transform.position = PlayBG_Original_Pos;
        PlayBG.transform.GetComponent<SpriteRenderer>().color = Color.gray;
    }

    public void Block()
    {
        if (blockCoroutine != null)
            StopCoroutine(blockCoroutine);
        blockCoroutine = StartCoroutine(BlockCoroutine());
    }

    private IEnumerator BlockCoroutine()
    {
        Barrier.enabled = true;
        Barrier.color = new Color(1f, 1f, 1f, 0.5f);
        
        float timer = 0f;
        while (timer < 0.5f) {
            Barrier.color = new Color(1f, 1f, 1f, 0.5f - timer);
            timer += Time.deltaTime;
            yield return null;
        }
        
        Barrier.enabled = false;
    }

    public void ShakePlayer()
    {
        StartCoroutine(ShakePlayer_Coroutine(0.2f));
    }

    private IEnumerator ShakePlayer_Coroutine(float shakeSize)
    {
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);
        Vector2 randomDirection = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));

        float timer = 0f;
        Player.transform.GetComponent<SpriteRenderer>().color = new Color(1f, 0.8f, 0.8f, 1f);

        while (timer < 0.1f) {
            Player.transform.position = Player_Original_Pos + (Vector3)(randomDirection * Mathf.Sin(timer*10 * Mathf.PI / 2f)) * shakeSize;
            timer += Time.deltaTime;
            yield return null;
        }
        Player.transform.position = Player_Original_Pos;
        Player.transform.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void ShakeBoss()
    {
        StartCoroutine(ShakeBoss_Coroutine(0.2f));
    }

    private IEnumerator ShakeBoss_Coroutine(float shakeSize)
    {
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);
        Vector2 randomDirection = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));

        float timer = 0f;
        Boss.transform.GetComponent<SpriteRenderer>().color = new Color(1f, 0.8f, 0.8f, 1f);

        while (timer < 0.1f) {
            Boss.transform.position = Boss_Original_Pos + (Vector3)(randomDirection * Mathf.Sin(timer*10 * Mathf.PI / 2f)) * shakeSize;
            timer += Time.deltaTime;
            yield return null;
        }
        Boss.transform.position = Boss_Original_Pos;
        Boss.transform.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
