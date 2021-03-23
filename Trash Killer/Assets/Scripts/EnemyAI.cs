using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    private GameObject target;

    public GameObject enemyGFX;
    public AudioClip muerte_clip;
    public AudioClip hitPJ_clip;
    public float visionRadius;
    [System.NonSerialized]
    public bool siguiendo;
    public bool daPuntos;
    public bool cinematic;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    //Variable del Pathfinder.
    Path path;
    // Variable para guardar la posición inicial
    Vector3 initialPosition;

    int count = 0;
    int currentWaypoint = 0;
    public bool reachedEndOfPath;
    public float repathRate = 0.5f;
    private float lastRepath = float.NegativeInfinity;

    Seeker seeker;
    Rigidbody2D rb;
    Animator anim;
    Vector3[] patrol;
    PlayerController pjController;


    void Awake()
    {
        //cinematicObj = GameObject.FindGameObjectWithTag("Cinematic"); //Tener en mente cambiarlo a [] si va a haber más de una "cinematica".
        anim = enemyGFX.GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
       
    }

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        pjController = target.GetComponent<PlayerController>();
        // Guardamos nuestra posición inicial
        initialPosition = transform.position;
        daPuntos = true;
        //Guardamos posiciones aleatorias en Vectores3, para que los enemigos den vueltas.
        patrol = new Vector3[4];
        for (int i = 0; i < 4; i++)
        {
            patrol[i] = new Vector3(initialPosition.x + Random.Range(-3f, +3f), initialPosition.y + Random.Range(-3f, +3f),0f);
        }
    }

    #region FixedUpdate

    void FixedUpdate()
    {
        BackInCinematic();

        Vector3 player = initialPosition;
        // Comprobamos un Raycast del enemigo hasta el jugador
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            target.transform.position - transform.position,
            visionRadius,
            1 << LayerMask.NameToLayer("Player")
        // Poner el propio Enemy en una layer distinta a Player para evitar el raycast
        // Sino los detectará como entorno y se mueve atrás al hacer ataques
        );

        /* Aquí podemos debugear el Raycast
         * Vector3 forward = transform.TransformDirection(target.transform.position - transform.position);
         * Debug.DrawRay(transform.position, forward, Color.red);
         */

        if (hit.collider != null)
        {
            if (hit.collider.tag == "Player")
            { 
                player = target.transform.position;
            }
        }
        // Calculamos la distancia y dirección actual hasta el target
        float distancia = Vector3.Distance(player, transform.position);
        
        // Se activa, mueve hasta el jugador o vuelve a su posicion.
         if (player != initialPosition && distancia < visionRadius)
        {
            speed = 400f;
            Perseguir();
        } 
        else
        {
            speed = 300f;
            StartCoroutine(Volver());
        }

        //Si no hay camino no hace nada.
        if (path == null)
            return;

        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else 
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        //Añade la fuerza para mover al enemigo.
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        //Gira el Sprite dependiendo de a donde mire.
        if (force.x >= 0.01f)
        {
            enemyGFX.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (force.x <= 0.01f)
        {
            enemyGFX.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        // Y un debug optativo con una línea hasta el target
        Debug.DrawLine(transform.position, player, Color.green);
    }

    #endregion

    #region Corotinas y movimientos.

    //Corutina para volver a la posición inicial.
    public IEnumerator Volver()
    {
        siguiendo = false;
        if (Time.time > lastRepath + repathRate && seeker.IsDone())
        {
            lastRepath = Time.time;
            yield return new WaitForSecondsRealtime(3f);
            //Crea un nuevo camino y vuelve a la posicion inicial si deja de verlo.
            seeker.StartPath(transform.position, initialPosition, OnPathComplete);
            if (initialPosition.x < initialPosition.x + new Vector3(-0.1f, 0f, 0f).x)
            {
                anim.SetBool("walking", true);
            }
        }
        if (gameObject.tag != "Plastic") //Este enemigo en concreto no patrulla.
        {
            yield return new WaitForSecondsRealtime(3f);
            StartCoroutine(Patrullar());
        }
    }

    //Corutina para patrullar.
    IEnumerator Patrullar()
    {
        if (Time.time > lastRepath + repathRate && seeker.IsDone())
        {
            lastRepath = Time.time;
            // Calcula un nuevo camino a seguir para ir patrullando.
            anim.SetBool("walking", true);
            if (patrol.Length != 0 && count < patrol.Length)
            {
                seeker.StartPath(transform.position, patrol[count], OnPathComplete);
            }
        }
        yield return StartCoroutine(Count());
    }

    //Corutina del contador.
    IEnumerator Count()
    {
        if (count <= 3)
        {
            count++;
        }
        else
        {
            count = 0;
        }
        yield return new WaitForSecondsRealtime(3f);
        StartCoroutine(Volver());
    }

    //Perseguir al jugador.
    void Perseguir()
    {
        siguiendo = true;
        if (Time.time > lastRepath + repathRate && seeker.IsDone())
        {
            lastRepath = Time.time;
            // Calcula un nuevo camino a seguir y va a por el personaje.
            anim.SetBool("walking", true);
            seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
        }
    }

    void BackInCinematic()
    {
        if (PlayerPrefs.GetString("cinematic")=="True") { StartCoroutine(Volver());}   
    }

    //Los caminos se guardan, así que si pasase algo o se cierra los borra.
    void OnPathComplete(Path p)
    {
        p.Claim(this);
        if (!p.error)
        {
            if (path != null) path.Release(this);
            path = p;
            currentWaypoint = 0;
        }
        else
        {
            p.Release(this);
        }
    }

    #endregion

    // Podemos dibujar el radio de visión sobre la escena dibujando una esfera
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }

    #region Colisiones

    //Al entrar en colsiion con cada tipo de bala gana o pierde puntos segun.
    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Bullet Plastic":
                if (gameObject.tag == "Plastic")
                {
                    Die();
                    Invoke("Off", 1f);
                    Puntos(1);
                }
                else if (gameObject.tag == "Vidrio" || gameObject.tag == "Paper" || gameObject.tag == "Organic")
                {
                    Die();
                    Invoke("Off", 1f);
                    Puntos(0);
                    Debug.Log("-50 puntos amigo.");
                }
                break;
            case "Bullet Vidrio":
                if (gameObject.tag == "Vidrio")
                {
                    Die();
                    Invoke("Off", 1f);
                    Puntos(1);
                }
                else if (gameObject.tag == "Plastic" || gameObject.tag == "Paper" || gameObject.tag == "Organic")
                {
                    Die();
                    Invoke("Off", 1f);
                    Puntos(0);
                }
                break;
            case "Bullet Paper":
                if (gameObject.tag == "Paper")
                {
                    Die();
                    Invoke("Off", 1f);
                    Puntos(1);
                }
                else if (gameObject.tag == "Vidrio" || gameObject.tag == "Plastic" || gameObject.tag == "Organic")
                {
                    Die();
                    Invoke("Off", 1f);
                    Puntos(0);
                }
                break;
            case "Bullet Organic":
                if (gameObject.tag == "Organic")
                {
                    Die();
                    Invoke("Off", 1f);
                    Puntos(1);
                }
                else if (gameObject.tag == "Vidrio" || gameObject.tag == "Paper" || gameObject.tag == "Plastic")
                {
                    Die();
                    Invoke("Off", 1f);
                    Puntos(0);
                }
                break;
            case "Bullet":
                if (gameObject.tag == "Metal")
                {
                    Die();
                    Invoke("Off", 1f);
                    Puntos(1);
                }
                else
                {
                    Die();
                    Invoke("Off", 1f);
                    Puntos(2);
                }
                break;
            case "Hacha":
                Die();
                Off();
                Puntos(1);
                break;
            default:
                break;
        }
    }

    //Al entrar en el collider del jugador o recibir disparo del arpon.
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HitBox")
        {
            if (PlayerPrefs.GetString("cinematic") == "False")
            {
                AudioManager.Instance.PlaySFX(hitPJ_clip, .5f);
                target.GetComponentInChildren<Animator>().SetTrigger("Daño");
                pjController.Vidas--;
                pjController.EnemyDeaths++;
                siguiendo = false;
                gameObject.SetActive(false);
                Destroy(gameObject, 0.5f);
                Debug.Log("Un punto de vida menos.");
            }
        }
        else if (collision.gameObject.tag == "Bullet Arpon")
        {
            Die();
            Invoke("Off", 1f);
            Puntos(1);
        }
        else if (collision.gameObject.tag == "BoundsEnemiesNoPoints")
        {
            daPuntos = false;
        }
    }

    //Al colisionar con particulas como las del lanzallamas.
    void OnParticleCollision(GameObject other)
    {
        Die();
        Invoke("Off", 1f);
        Puntos(1);
    }

    #endregion
    private void Puntos(int NegPost)
    {
        if(!daPuntos) { /*Sin puntos*/ }
        else if(NegPost == 0) { pjController.Puntuacion -= 50; }
        else if(NegPost == 1){ pjController.Puntuacion += 100; }
        else if(NegPost == 2) { pjController.Puntuacion += 50; }

    }

    private void Die()
    {
        pjController.EnemyDeaths++;
        anim.SetTrigger("die");
        AudioManager.Instance.PlaySFX(muerte_clip, .5f);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
    private void Off()
    {
        siguiendo = false;
        gameObject.SetActive(false);
        Destroy(gameObject, 0.5f);
    }
}
