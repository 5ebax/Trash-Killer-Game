using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // Variables para gestionar el radio de visión, el de ataque y la velocidad
    public float visionRadius;
    public float attackRadius;
    public float speed;

    // Variable para guardar al jugador
    GameObject player;
    SpriteRenderer sr;

    // Variable para guardar la posición inicial
    Vector3 initialPosition;

    // Animador y cuerpo cinemático con la rotación en Z congelada
    Animator anim;
    Rigidbody2D rb2d;

    void Start()
    {

        // Recuperamos al jugador gracias al Tag
        player = GameObject.FindGameObjectWithTag("Player");

        //Recuperamos el sprite.
        sr = GetComponentInChildren<SpriteRenderer>();

        // Guardamos nuestra posición inicial
        initialPosition = transform.position;

        anim = GetComponentInChildren<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Compara la posición del jugador para saber a que lado mirar el enemigo.
        if (transform.position.x >= player.transform.position.x) { sr.flipX = false; } 
        else if (transform.position.x <= player.transform.position.x) { sr.flipX = true; }

        // Por defecto nuestro target siempre será nuestra posición inicial
        Vector3 target = initialPosition;

        // Comprobamos un Raycast del enemigo hasta el jugador
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            player.transform.position - transform.position,
            visionRadius,
            1 << LayerMask.NameToLayer("Default")
        // Poner el propio Enemy en una layer distinta a Default para evitar el raycast
        // También poner al objeto Attack y al Prefab Slash una Layer Attack 
        // Sino los detectará como entorno y se mueve atrás al hacer ataques
        );
        RaycastHit2D hitPJ = Physics2D.Raycast(
            transform.position,
            player.transform.position - transform.position,
            visionRadius,
            1 << LayerMask.NameToLayer("Player")
        );

        // Aquí podemos debugear el Raycast
        Vector3 forward = transform.TransformDirection(player.transform.position - transform.position);
        Debug.DrawRay(transform.position, forward, Color.red);

        // Si el Raycast encuentra al jugador lo ponemos de target
            //Si hago que a cierta distancia como el radio de ataque para usar y que se pare, pues si es < pues +0.1?
        if (hitPJ.collider != null)
        {
            if (hitPJ.collider.tag == "Player")
            {
                target = player.transform.position;
            }

        }


        // Calculamos la distancia y dirección actual hasta el target
        float distance = Vector3.Distance(target, transform.position);
        Vector3 dir = (target - transform.position).normalized;
        Vector3 evade = new Vector3(0.3f, 0f, 0f);

        // Se mueve hasta el jugador.
        if (!(target != initialPosition && distance < attackRadius))
        {
            rb2d.MovePosition(transform.position + dir * speed * Time.deltaTime);

            // Al movernos establecemos la animación de movimiento
            anim.speed = 1;
            anim.SetBool("walking", true);
        }

        // Una última comprobación para evitar bugs forzando la posición inicial
        if (target == initialPosition && distance < 0.02f)
        {
            transform.position = initialPosition;
            // Y cambiamos la animación de nuevo a Idle
            anim.SetBool("walking", false);
        }

        // Y un debug optativo con una línea hasta el target
        Debug.DrawLine(transform.position, target, Color.green);
    }

    // Podemos dibujar el radio de visión y ataque sobre la escena dibujando una esfera
    void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
        Gizmos.DrawWireSphere(transform.position, attackRadius);

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
                }
                    //Puntuacion +100
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
            case "Map":
                
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
                else if (collision.gameObject.tag == "Hacha")
                {
                    gameObject.SetActive(false);
                    Destroy(gameObject, 0.5f);
                    //Puntuacion +100
                }
                break;
        }
        if(collision.gameObject.tag == "Map")
        {
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
        else if (collision.gameObject.tag == "Bullet Arpon")
        {
            gameObject.SetActive(false);
            Destroy(gameObject, 0.5f);
            //Puntuacion +100
        }
    }

    void OnParticleCollision(GameObject other)
    {
        gameObject.SetActive(false);
        Destroy(gameObject, 0.5f);
        //Puntuacion +100
    }
}
