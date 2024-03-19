using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Projectile : MonoBehaviour
{
    public NoteType noteType;
    public List<Sprite> projSprites;
    public List<Color> ProjectileColors;
    private SpriteRenderer spriteRenderer;
    private TrailRenderer trailRenderer;
    public Light2D light2D;
    public bool friendly = false;
    public int id = 0;

    private Vector3 HoodieGuyPos = new Vector3(2.58f, -0.03f, 0f);
    private Vector3 BossPos = new Vector3(15f, 0f, -0.1f);

    private Vector3 RandomYShootOffset;
    private Vector3 startPos;

    public GameObject particleSystem;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
        spriteRenderer.sprite = projSprites[(int)noteType];
        trailRenderer.endColor = ProjectileColors[(int)noteType];
        friendly = (int)noteType < 7;

        transform.position = (friendly ? HoodieGuyPos : BossPos) + Vector3.up * Random.Range(-0.3f, 0.3f);
        startPos = transform.position;
        //spriteRenderer.flipX = friendly ? false : true;
        transform.localScale = friendly ? Vector3.one : Vector3.one * 1.7f;

        RandomYShootOffset = Vector3.up * Random.Range(-1.3f, 1.3f);

        switch (noteType) {
            case NoteType.Normal:
                break;
            case NoteType.Shuriken:
                break;
            case NoteType.Heal:
                transform.position += Vector3.right * Random.Range(-0.6f, 0.6f);
                particleSystem.SetActive(true);
                FadeOut();
                break;
            case NoteType.Shield:
                transform.position = new Vector3(HoodieGuyPos.x, -0.6f, HoodieGuyPos.z);
                FadeOut();
                break;
            case NoteType.Fire:
                break;
            case NoteType.Zap:
                spriteRenderer.flipY = Random.Range(0,2) == 0;
                transform.position += Vector3.right * 7f;
                light2D.intensity = 0;
                FadeOut();
                break;
            case NoteType.Poison:
                break;
            case NoteType.Sloth:
                break;
            case NoteType.Greed:
                break;
            case NoteType.GluttonyFast:
                break;
            case NoteType.GluttonySlow:
                break;
            case NoteType.Wrath:
                break;
        }
    }

    private void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }
    
    private IEnumerator FadeOutCoroutine()
    {
        float timer = 0f;
        while (timer < 0.5f) {
            spriteRenderer.color = new Color(1f, 1f, 1f, 1.25f - timer * 2.5f);

            timer += Time.deltaTime;
            yield return null;
        }
        
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        switch (noteType) {
            case NoteType.Normal:
                transform.position += (friendly ? Vector3.right : Vector3.left) * Time.deltaTime * 20f + RandomYShootOffset * Time.deltaTime;
                break;
            case NoteType.Shuriken:
                transform.position += (friendly ? Vector3.right : Vector3.left) * Time.deltaTime * 40f + RandomYShootOffset * Time.deltaTime * 3f;
                break;
            case NoteType.Heal:
                transform.position += Vector3.up * Time.deltaTime * 3f;
                break;
            case NoteType.Shield:
                break;
            case NoteType.Fire:
                transform.position += (friendly ? Vector3.right : Vector3.left) * Time.deltaTime * 30f;
                transform.position = new Vector3(transform.position.x, startPos.y + Mathf.Sin(Mathf.Abs(transform.position.x - startPos.x) * Mathf.PI / 15f) * 3f, transform.position.z);
                break;
            case NoteType.Zap:
                break;
            case NoteType.Poison:
                transform.position += (friendly ? Vector3.right : Vector3.left) * Time.deltaTime * 25f;
                transform.position = new Vector3(transform.position.x, startPos.y + Mathf.Sin(Mathf.Abs(transform.position.x - startPos.x) * Mathf.PI / 2f) * 0.5f, transform.position.z);
                break;
            case NoteType.Sloth:
                transform.position += (friendly ? Vector3.right : Vector3.left) * Time.deltaTime * 20f + RandomYShootOffset * Time.deltaTime;
                break;
            case NoteType.Greed:
                transform.position += (friendly ? Vector3.right : Vector3.left) * Time.deltaTime * 20f + RandomYShootOffset * Time.deltaTime;
                break;
            case NoteType.GluttonyFast:
                transform.position += (friendly ? Vector3.right : Vector3.left) * Time.deltaTime * 20f + RandomYShootOffset * Time.deltaTime;
                break;
            case NoteType.GluttonySlow:
                transform.position += (friendly ? Vector3.right : Vector3.left) * Time.deltaTime * 20f + RandomYShootOffset * Time.deltaTime;
                break;
            case NoteType.Wrath:
                transform.position += (friendly ? Vector3.right : Vector3.left) * Time.deltaTime * 20f + RandomYShootOffset * Time.deltaTime;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Triggered");
        if (other.gameObject.layer == LayerMask.NameToLayer("HoodieGuy")) {
            if (!friendly) {
                FindObjectOfType<VFXManager>().ShakePlayer();
                Destroy(gameObject);
            }
        } else if (other.gameObject.layer == LayerMask.NameToLayer("Boss")) {
            if (friendly) {
                FindObjectOfType<VFXManager>().ShakeBoss();
                Destroy(gameObject);
            }
        }
    }
}
