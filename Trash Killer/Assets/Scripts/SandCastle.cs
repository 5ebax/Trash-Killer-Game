using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandCastle : MonoBehaviour
{
    public Sprite castle;
    public AudioClip sand;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("HitBox"))
        {
            AudioManager.Instance.PlaySFX(sand, 0.5f);
            sr.sprite = castle;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
