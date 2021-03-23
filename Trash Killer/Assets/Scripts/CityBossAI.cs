using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class CityBossAI : MonoBehaviour
{
    private GameObject target;
    public Transform throwPoint;
    public GameObject ball;
    public float timeTillHit = 1.3f;

    public Transform[] puntos;
    public GameObject[] generators;
    public GameObject enemyGFX;
    public HealthBar healthBar;
    public AudioClip muerte_clip;
    public AudioClip hitPJ_clip;
    private PlayerController PJCont;
    public float visionRadius;
    [System.NonSerialized]
    public bool muerto;

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
    private bool one;
    private int action;
    private int rdm;

    public int vidas;
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
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        PJCont = FindObjectOfType<PlayerController>();

        // Guardamos nuestra posición inicial
        initialPosition = transform.position;
        vidas = 200;
        healthBar.SetMaxHealth(vidas);
        rdm = Random.Range(0, puntos.Length);
        action = 0;
        one = false;
        gameObject.SetActive(false);
    }

    #region Update y ciclo de vida del MiniBoss.

    void Update()
    {
        if (vidas <= 0)
        {
            if (!muerto)
                Muerte();
        }
    }

    //Muerte del Boss
    private void Muerte()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        muerto = true;
        reachedEndOfPath = true;
        anim.SetBool("move", false);
        anim.SetTrigger("muerte");
        Invoke("DeadSound", 1.57f);
        PlayerPrefs.SetInt("PuntosCity", PJCont.Puntuacion);
        EnemyAI[] enemiesBoss = FindObjectsOfType<EnemyAI>();
        foreach (EnemyAI enemy in enemiesBoss)
        {
            Destroy(enemy.gameObject);
        }
        SceneManager.LoadScene(2);
    }
    private void DeadSound() { AudioManager.Instance.PlaySFX(muerte_clip, .2f); }

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
            if(!muerto)
                Comenzar();
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

        //Gira el Sprite mirando al target.

        Vector3 seeDirection = player - transform.position;

        if (seeDirection.x >= 0.01f)
        {
            enemyGFX.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (seeDirection.x <= 0.01f)
        {
            enemyGFX.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        // Y un debug optativo con una línea hasta el target
        Debug.DrawLine(transform.position, player, Color.green);
    }

    #endregion

    #region Corutinas y movimientos.

    //Se mueve entre puntos.
    void Comenzar()
    {
        if (Time.time > lastRepath + repathRate && seeker.IsDone())
        {
            lastRepath = Time.time;
            // Calcula un nuevo camino a seguir y va a por el personaje.
            anim.SetBool("move", true);
            seeker.StartPath(transform.position, puntos[rdm].position, OnPathComplete);
            if (reachedEndOfPath && one == false)
                StartCoroutine(ChoosePoint());
        }
    }

    IEnumerator ChoosePoint()
    {
        action = Random.Range(0, 3);
        one = true;
        reachedEndOfPath = true;//Comprobar si va?
        anim.SetBool("move", false);
        if (action == 0 || action == 1)
        {
            anim.SetTrigger("throw");
            Invoke("Throw", 1.2f);
        } else if (action == 2)
        {
            anim.SetTrigger("invoke");
            Invoke("Invocar", 1f);
        }

        yield return new WaitForSecondsRealtime(2.5f);
        rdm = Random.Range(0, puntos.Length);
        reachedEndOfPath = false;
        one = false;
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

    //Al entrar en el collider del jugador.
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HitBox")
        {
            AudioManager.Instance.PlaySFX(hitPJ_clip, .5f);
            target.GetComponentInChildren<Animator>().SetTrigger("Daño");
            PJCont.Vidas--;
            target.transform.position = transform.position + force.normalized * 1.4f; //Empuja al jugador.
        }
    }

    #endregion

    #region Ataques Boss

    //Lanzamiento de botella del Boss.
    void Throw()
    {
        float xdistance;
        xdistance = target.transform.position.x - throwPoint.position.x;

        float ydistance;
        ydistance = target.transform.position.y - throwPoint.position.y;

        float throwAngle; // in radian

        throwAngle = Mathf.Atan((ydistance + 4.905f * (timeTillHit * timeTillHit)) / xdistance);

        float totalVelo = xdistance / (Mathf.Cos(throwAngle) * timeTillHit);

        float xVelo, yVelo;
        xVelo = totalVelo * Mathf.Cos(throwAngle);
        yVelo = totalVelo * Mathf.Sin(throwAngle);

        GameObject bulletInstance = Instantiate(ball, throwPoint.position, Quaternion.identity) as GameObject;
        Rigidbody2D rigid;
        rigid = bulletInstance.GetComponent<Rigidbody2D>();
        rigid.velocity = new Vector2(xVelo, yVelo);
    }
    //Invocacion de enemigos.
    void Invocar()
    {
        foreach (GameObject gen in generators)
        {
            gen.SetActive(true);
        }
    }
    #endregion
}
