using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class ForestBossIA : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject target;
    public GameObject bullet;
    public bool siguiendo;
    public bool start;
    private bool stop;
    public HealthBar healthBar;
    private PlayerController PJCont;
    public float visionRadius;
    public GameObject enemyGFX;
    public AudioClip muerte_clip;
    public AudioClip hitPJ_clip;
    public bool muerto;

    float fireRate;
    float nextFire;


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


     private int vidas;
    public int Vidas
    {
        get { return vidas; }
        set { vidas = value; }
    }

    Seeker seeker;
    Rigidbody2D rb;
    Vector3 force;
    Animator anim;

     void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        anim = enemyGFX.GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        PJCont = FindObjectOfType<PlayerController>();
    }


    void Start()
    {
        fireRate = 0.5f;
        nextFire = Time.time;
        
        // Guardamos nuestra posición inicial
        initialPosition = transform.position;
        muerto = false;
        start = false;
        stop = false;
        vidas = 200;
        healthBar.SetMaxHealth(vidas);
    }

    // Update is called once per frame
    void Update()
    {
        if (vidas <= 0)
        {
            if (!muerto)
                Muerte();
        }
    }


    private void Muerte()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        start = false;
        stop = true;
        muerto = true;
        anim.SetTrigger("muerte");
        Invoke("DeadSound", .8f);
    }
    private void DeadSound() { AudioManager.Instance.PlaySFX(muerte_clip, .2f); }

  //  #region FixedUpdate

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

    //#endregion

    //#region Corotinas y movimientos.

    //Perseguir al jugador.
    void Perseguir()
    {
        siguiendo = true;
        if (Time.time > lastRepath + repathRate && seeker.IsDone())
        {
            lastRepath = Time.time;
            // Calcula un nuevo camino a seguir y va a por el personaje.
            anim.SetBool("center", true);
            seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
            if(Time.time > nextFire){
                Instantiate(bullet, transform.position, Quaternion.identity);
                nextFire = Time.time + fireRate;
            }
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
     void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }


    //Al entrar en el collider del jugador.
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HitBox")
        {
            AudioManager.Instance.PlaySFX(hitPJ_clip, .5f);
            target.GetComponentInChildren<Animator>().SetTrigger("Daño");
            PJCont.Vidas--;
            target.transform.position = transform.position + force.normalized * 1.4f; //Empuja al jugador.
            Debug.Log("Un punto de vida menos.");
        }
    }

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
}
