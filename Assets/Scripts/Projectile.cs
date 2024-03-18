using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public NoteType noteType;
    public List<Sprite> projSprites;
    public List<Color> ProjectileColors;
    private SpriteRenderer spriteRenderer;
    private TrailRenderer trailRenderer;
    public bool friendly = false;

    private Vector3 HoodieGuyPos = new Vector3(2.58f, -0.03f, 0f);
    private Vector3 BossPos = new Vector3(15f, 0f, -0.1f);
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
        spriteRenderer.sprite = projSprites[(int)noteType];
        trailRenderer.endColor = ProjectileColors[(int)noteType];
        friendly = (int)noteType < 7;

        transform.position = (friendly ? HoodieGuyPos : BossPos) + Vector3.up * Random.Range(-0.3f, 0.3f);
        //spriteRenderer.flipX = friendly ? false : true;
        transform.localScale = friendly ? Vector3.one : Vector3.one * 1.7f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (friendly ? Vector3.right : Vector3.left) * Time.deltaTime * 20f;
        switch (noteType) {
            case NoteType.Normal:
                break;
            case NoteType.Shuriken:
                break;
            case NoteType.Heal:
                break;
            case NoteType.Shield:
                break;
            case NoteType.Fire:
                break;
            case NoteType.Zap:
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

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Triggered");
        if (other.gameObject.layer == LayerMask.NameToLayer("HoodieGuy")) {
            if (!friendly) {
                Destroy(gameObject);
            }
        } else if (other.gameObject.layer == LayerMask.NameToLayer("Boss")) {
            if (friendly) {
                Destroy(gameObject);
            }
        }
    }
}
