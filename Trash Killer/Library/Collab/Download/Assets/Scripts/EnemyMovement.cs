using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

public class EnemyMovement : MonoBehaviour
{
    //public float speed;
      private bool verPJ = false;
      private Transform target;
    private CharacterController2D controller;
    public int MoveSpeed;
      public float MaxDist;
      public float MinDist;
    

    // Start is called before the first frame update
    void Start()
    {
        //controller = GetComponent<CharacterController2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float distancia = Vector3.Distance(transform.position, target.position);

    if(distancia <= MinDist)
    {
       verPJ = true;
    }
        else if(distancia >= MaxDist)
    {
       verPJ = false;
    }
    if (verPJ == true)
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, MoveSpeed * Time.deltaTime);
    }

    //controller.onControllerCollidedEvent()
 }


    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (gameObject.tag)
        {
            case "Plastic":
                if (collision.gameObject.tag == "Bullet Plastic")
                {
                    gameObject.SetActive(false);
                    Destroy(gameObject, 0.5f);
                    //Puntuacion +100
                }
                else
                {
                    gameObject.SetActive(false);
                    Destroy(gameObject, 0.5f);
                    //Puntuacion -50
                }
                break;
            case "Vidrio":
                if (collision.gameObject.tag == "Bullet Vidrio")
                {
                    gameObject.SetActive(false);
                    Destroy(gameObject, 0.5f);
                    //Puntuacion +100
                }
                else
                {
                    gameObject.SetActive(false);
                    Destroy(gameObject, 0.5f);
                    //Puntuacion -50
                }
                break;
            case "Paper":
                if (collision.gameObject.tag == "Bullet Paper")
                {
                    gameObject.SetActive(false);
                    Destroy(gameObject, 0.5f);
                    //Puntuacion +100
                }
                else
                {
                    gameObject.SetActive(false);
                    Destroy(gameObject, 0.5f);
                    //Puntuacion -50
                }
                break;
            case "Organic":
                if (collision.gameObject.tag == "Bullet Organic")
                {
                    gameObject.SetActive(false);
                    Destroy(gameObject, 0.5f);
                    //Puntuacion +100
                }
                else
                {
                    gameObject.SetActive(false);
                    Destroy(gameObject, 0.5f);
                    //Puntuacion -50
                }
                break;
            default:
                if (collision.gameObject.tag == "Bullet")
                {
                    gameObject.SetActive(false);
                    Destroy(gameObject, 0.5f);
                    //Puntuacion +10
                    //Ya vemos si permitimos matar cualquiera con este pero to pocos puntos.
                    //O el PJ de metal con este y +100 y -50, pero solo hay un PJ de Metal asi que nc.
                }
                break;
        }
        Debug.Log("Muerto el enemigo.");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HitBox")
        {
            gameObject.SetActive(false);
            Destroy(gameObject, 0.5f);
            Debug.Log("Un punto de vida menos.");
            //Pierde vida.
        }
    }
}
