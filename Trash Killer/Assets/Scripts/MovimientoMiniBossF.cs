using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovimientoMiniBossF : MonoBehaviour
{
    // Start is called before the first frame update
    public HealthBar healthBar;
    private float speed;
    public bool moveRight;
    public bool muerto;
    public AudioClip muerte_clip;
    public GameObject enemyGFX;
    public GameObject target;
    private PlayerController PJCont;

    private int vidas;
    public int Vidas
    {
        get { return vidas; }
        set { vidas = value; }
    }

    Animator anim;

    void Awake()
    {
        anim = enemyGFX.GetComponent<Animator>();
        
    }

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        PJCont = FindObjectOfType<PlayerController>();
        vidas = 100;
        healthBar.SetMaxHealth(vidas);
        speed = 3f;
         
    }

    // Update is called once per frame
    void Update()
    {
        if (moveRight){
            transform.Translate(2 * Time.deltaTime * speed, 0,0);
            
        }else {
            transform.Translate(-2 * Time.deltaTime * speed, 0,0);

        }

        if (vidas <= 0)
        {
            if (!muerto)
                Muerte();
        }
    }


    private void Muerte()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        muerto = true;
        anim.SetTrigger("muerte");
        Invoke("DeadSound", .8f);
    }
    private void DeadSound() { AudioManager.Instance.PlaySFX(muerte_clip, .2f); }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            vidas -= 2;
            healthBar.SetHealth(vidas);
            PJCont.Puntuacion += 20;
        }
        else if (collision.gameObject.CompareTag("Bullet Organic"))
        {
            vidas -= 2;
            healthBar.SetHealth(vidas);
            PJCont.Puntuacion += 20;
        }
        else if (collision.gameObject.CompareTag("Bullet Plastic"))
        {
            vidas -= 2;
            healthBar.SetHealth(vidas);
            PJCont.Puntuacion += 20;
        }
        else if (collision.gameObject.CompareTag("Bullet Vidrio"))
        {
            vidas -= 2;
            healthBar.SetHealth(vidas);
            PJCont.Puntuacion += 20;
        }
        else if (collision.gameObject.CompareTag("Bullet Paper"))
        {
            vidas -= 2;
            healthBar.SetHealth(vidas);
            PJCont.Puntuacion += 20;
        }
    }
    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "turn"){
            if(moveRight){
                moveRight = false;
            }else {
                moveRight = true;
            } 
        }else
        if (col.gameObject.tag == "HitBox"){
            //AudioManager.Instance.PlaySFX(hitPJ_clip, .5f);
            target.GetComponentInChildren<Animator>().SetTrigger("Daño");
            PJCont.Vidas--;
            Debug.Log("Un punto de vida menos.");
        }
    }
}
