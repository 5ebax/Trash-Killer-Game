using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using Random = UnityEngine.Random;

public class TazaBossAI : MonoBehaviour
{
    public GameObject target;
    public GameObject enemyGFX;
    public HealthBar healthBar;
    public AudioClip muerte_clip;
    public AudioClip hitPJ_clip;
    public BoxCollider2D miniBossWarp;
    private PlayerController PJCont;
    public float visionRadius;
    [System.NonSerialized]
    public bool siguiendo;
    [System.NonSerialized]
    public bool muerto;
    [System.NonSerialized]
    public bool start;
    private bool stop;
    private bool changeMode;

    public float speed;
    public float nextWaypointDistance = 3f;

    //Variable del Pathfinder.
    Path path;
    // Variable para guardar la posición inicial
    Vector3 initialPosition;

    int currentWaypoint = 0;
    public bool reachedEndOfPath;
    public float repathRate = 0.5f;
    private float lastRepath = float.NegativeInfinity;
    private int mode;

    private int vidas;
    public int Vidas
    {
        get { return vidas; }
        set { vidas = value; }
    }

    Seeker seeker;
    Rigidbody2D rb;
    Animator anim;
    Vector3 force;

    void Awake()
    {
        anim = enemyGFX.GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        PJCont = FindObjectOfType<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Guardamos nuestra posición inicial
        initialPosition = transform.position;
        muerto = false;
        start = false;
        stop = false;
        changeMode = true;
        vidas = 100;
        healthBar.SetMaxHealth(vidas);
        gameObject.SetActive(false);
    }

    #region Update y ciclo de vida del MiniBoss.

    void Update()
    {
        //Cambios de forma dependiendo de su salud.
        if (vidas <= 90 && vidas > 88 && changeMode)
        {
            StartCoroutine(Rage());
        }
        else if (vidas < 89 && vidas > 80) { changeMode = true; }
        else if (vidas <= 80 && vidas > 78 && changeMode)
        {
            ChangeMode();
        }
        else if (vidas < 79 && vidas > 70) { changeMode = true; }
        else if (vidas <= 70 && vidas > 68 && changeMode)
        {
            ChangeMode();
        }
        else if (vidas < 69 && vidas > 60) { changeMode = true; }
        else if (vidas <= 60 && vidas > 58 && changeMode)
        {
            ChangeMode();
        }
        else if (vidas < 59 && vidas > 50) { changeMode = true; }
        else if (vidas <= 50 && vidas > 48 && changeMode)
        {
            ChangeMode();
        }
        else if (vidas < 49 && vidas > 40) { changeMode = true; }
        else if (vidas <= 40 && vidas > 38 && changeMode)
        {
            ChangeMode();
        }
        else if (vidas < 39 && vidas > 30) { changeMode = true; }
        else if (vidas <= 30 && vidas > 28 && changeMode)
        {
            ChangeMode();
        }
        else if (vidas < 29 && vidas > 20) { changeMode = true; }
        else if (vidas <= 20 && vidas > 18 && changeMode)
        {
            ChangeMode();
        }
        else if (vidas < 19 && vidas > 10) { changeMode = true; }
        else if (vidas <= 10 && vidas > 8 && changeMode)
        {
            ChangeMode();
        }
        else if (vidas <= 0)
        {
            if (!muerto)
                Muerte();
        }
    }

    //Muerte del MiniBoss
    private void Muerte()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        start = false;
        stop = true;
        muerto = true;
        anim.SetBool("move", false);
        anim.SetBool("rage", false);
        anim.SetInteger("mode", 5);
        anim.SetTrigger("muerte");
        Invoke("DeadSound", .8f);
        miniBossWarp.enabled = true;
    }
    private void DeadSound() { AudioManager.Instance.PlaySFX(muerte_clip, .2f); }

    //Cambia de forma.
    private void ChangeMode()
    {
        mode = Random.Range(1, 5);
        anim.SetInteger("mode", mode);
        changeMode = false;
    }

    #endregion

    #region FixedUpdate

    void FixedUpdate()
    {
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

        // Aquí podemos debugear el Raycast
        Vector3 forward = transform.TransformDirection(target.transform.position - transform.position);
        Debug.DrawRay(transform.position, forward, Color.red);

        if (hit.collider != null)
        {
            if (hit.collider.tag == "Player")
            {
                player = target.transform.position;
            }
        }
        // Calculamos la distancia y dirección actual hasta el target
        float distancia = Vector3.Distance(player, transform.position);

        // Se mueve hasta el jugador.
        if (player != initialPosition && distancia < visionRadius)
        {
            if (!stop)
                Perseguir();
        }

        //Si no hay camino no hace nada.
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        force = direction * speed * Time.deltaTime;

        rb.MovePosition(transform.position + force);

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

    //Perseguir al jugador.
    void Perseguir()
    {
        siguiendo = true;
        if (Time.time > lastRepath + repathRate && seeker.IsDone())
        {
            lastRepath = Time.time;
            // Calcula un nuevo camino a seguir y va a por el personaje.
            anim.SetBool("move", true);
            seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
        }
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

    //Furia del miniboss.
    IEnumerator Rage()
    {
        speed = 3;
        stop = true;
        anim.SetBool("rage", true);
        anim.SetInteger("mode", 0);
        yield return new WaitForSecondsRealtime(2f);
        ChangeMode();
        stop = false;
    }
    #endregion

    // Podemos dibujar el radio de visión sobre la escena dibujando una esfera
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            vidas -= 1;
            healthBar.SetHealth(vidas);
            PJCont.Puntuacion += 10;
        }
        else if (collision.gameObject.CompareTag("Bullet Organic"))
        {
            if (mode == 1)
            {
                vidas -= 2;
                healthBar.SetHealth(vidas);
                PJCont.Puntuacion += 30;
            }
        }
        else if (collision.gameObject.CompareTag("Bullet Plastic"))
        {
            if (mode == 2)
            {
                vidas -= 2;
                healthBar.SetHealth(vidas);
                PJCont.Puntuacion += 30;
            }
        }
        else if (collision.gameObject.CompareTag("Bullet Vidrio"))
        {
            if (mode == 3)
            {
                vidas -= 2;
                healthBar.SetHealth(vidas);
                PJCont.Puntuacion += 30;
            }
        }
        else if (collision.gameObject.CompareTag("Bullet Paper"))
        {
            if (mode == 4)
            {
                vidas -= 2;
                healthBar.SetHealth(vidas);
                PJCont.Puntuacion += 30;
            }
        }
    }

    //Al entrar en el collider del jugador.
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HitBox")
        {
            AudioManager.Instance.PlaySFX(hitPJ_clip, .5f);
            target.GetComponentInChildren<Animator>().SetTrigger("Daño");
            PJCont.Vidas--;
            target.transform.position = transform.position + force.normalized*1.4f; //Empuja al jugador.
        }
    }
}
