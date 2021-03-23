using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterProjectile : MonoBehaviour
{
    public Transform shadow;
    private PlayerController pjCont;

    private void Start()
    {
        pjCont = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == shadow.gameObject)
        {
            shadow.gameObject.SetActive(false);
            Destroy(shadow.gameObject, 0.1f);
        }
        else if (collision.gameObject.CompareTag("HitBox"))
        {
            pjCont.Vidas--;
            AudioManager.Instance.PlaySFX(pjCont.hitPJ_clip);
            shadow.gameObject.SetActive(false);
            Destroy(shadow.gameObject, 0.1f);
        }
    }
}
