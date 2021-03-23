using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Prime31;
using System.Collections;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public List<GameObject> tag_targets = new List<GameObject>();
    public float moveSpeed = 5f;
    public AudioClip muerte_clip;
    public AudioClip hitPJ_clip;
    public TextMeshProUGUI puntos;
    public static Vector3 position;
    private bool correr;
    private bool muerte;
    private bool muerteSound;
    private int fight;

    private Animator animator;
    private GameObject miniBossWarpExit;
    private CharacterController2D controller;
    private WeaponManager wpManager;
    private Vector2 movement;
    private CameraController theCamera;
    private Vector3 translate;
    private TazaBossAI tazaBoss;
    private CityBossAI cityBoss;
    private MiniBossDolphin dolphinBoss;

    public int vidas;
    public int Vidas
    {
        get { return vidas; }
        set { vidas = value; }
    }
    private int puntuacion;
    public int Puntuacion
    {
        get { return puntuacion; }
        set { puntuacion = value; }
    }

    private int enemyDeaths;
    public int EnemyDeaths
    {
        get { return enemyDeaths; }
        set { enemyDeaths = value; }
    }

    private void Awake()
    {
        if (PlayerPrefs.GetString("continue") == "True")
        {
            Puntuacion = PlayerPrefs.GetInt("PuntosCity");
        }
        miniBossWarpExit = GameObject.FindGameObjectWithTag("MiniBossWarpExit");
        tazaBoss = FindObjectOfType<TazaBossAI>();
        cityBoss = FindObjectOfType<CityBossAI>();
        dolphinBoss = GetComponentInParent<MiniBossDolphin>();
        animator = GetComponentInChildren<Animator>();
        theCamera = FindObjectOfType<CameraController>();
        
    }
    void Start()
    {
        controller = GetComponent<CharacterController2D>();
        wpManager = GetComponent<WeaponManager>();
        fight = 0;
        muerteSound = false;
        muerte = false;
        vidas = 10;
        AddDescendantsWithTag(transform, "Weapon", tag_targets);
        if (!PlayerPrefs.HasKey("Puntuacion"))
            PlayerPrefs.SetInt("Puntuacion", 0);
    }

    private void Update()
    {
        if (puntuacion <= 0)
            puntuacion = 0;
        puntos.text = "Puntos: "+ puntuacion + " / Record: "+ PlayerPrefs.GetInt("Puntuacion");
        if(puntuacion > PlayerPrefs.GetInt("Puntuacion"))
        {
            PlayerPrefs.SetInt("Puntuacion", puntuacion);
            Debug.Log("Nuevo record: " + puntuacion);
        }
    }

    void FixedUpdate()
    {
        position = transform.position;
        Muerte();
        if (!muerte) { Moving(); }
    }

    private void Muerte()
    {
        if (vidas <= 0)
        {
            vidas = 0;
            muerte = true;
            GetComponent<WeaponManager>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            foreach (GameObject weapon in tag_targets)
            {
                weapon.SetActive(false);
            }
            animator.SetTrigger("Muerte");
            if (!muerteSound) { StartCoroutine(MuertoSound()); }
        }
    }
    IEnumerator MuertoSound()
    {
        muerteSound = true;
        AudioManager.Instance.PlaySFX(muerte_clip, 1);
        yield return new WaitForSeconds(1.3f);
        AudioManager.Instance.PlaySFX(muerte_clip, 1);
    }

    private void Moving()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x == Vector2.left.x) { Corriendo(); }
        else if (movement.x == Vector2.right.x) { Corriendo(); }
        else if (movement.y == Vector2.up.y || movement.y == Vector2.down.y) { Corriendo(); }
        else
        {
            correr = false;
            animator.SetBool("Corriendo", correr);
        }

        translate = movement * moveSpeed * Time.deltaTime;
        controller.move(translate);
    }

    private void Corriendo()
    {
        if (correr == false)
        {
            correr = true;
            animator.SetBool("Corriendo", correr);
        }
    }
    //Todos los hijos con el tag dado de ese GameObject, en este caso del player.
    private void AddDescendantsWithTag(Transform parent, string tag, List<GameObject> list)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.tag == tag)
            {
                list.Add(child.gameObject);
            }
            AddDescendantsWithTag(child, tag, list);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bounds")
        {
            theCamera.SetBounds(collision.gameObject.GetComponent<BoxCollider2D>());
        }
        if(collision.gameObject.tag == "Lanzallamas")
        {
            wpManager.specialWeapon = collision.gameObject.tag;
            wpManager.numDeads = enemyDeaths + 30;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Hacha")
        {
            wpManager.specialWeapon = collision.gameObject.tag;
            wpManager.numDeads = enemyDeaths + 30;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Arpon")
        {
            wpManager.specialWeapon = collision.gameObject.tag;
            wpManager.numDeads = enemyDeaths + 30;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("BottleBoss")){
            Vidas--;
            animator.SetTrigger("Daño");
            AudioManager.Instance.PlaySFX(hitPJ_clip);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Mantiene al personaje dentro del mapa cuando sale de el, y activa y desactiva el warp al MiniBoss
        if (collision.gameObject.tag == "Bounds" && tazaBoss.start)
        {
            transform.position = collision.transform.position;
        }else if (collision.gameObject.tag == "MiniBossWarp")
        {
            if (tazaBoss.gameObject != null && fight == 0)
            {
                fight = 1;
                tazaBoss.start = true;
                tazaBoss.gameObject.SetActive(true);
                miniBossWarpExit.GetComponent<BoxCollider2D>().enabled = false;
            }else if(fight == 1)
            {
                tazaBoss.gameObject.SetActive(false);
                Destroy(tazaBoss.gameObject,.1f);
            }
            if(dolphinBoss.gameObject != null)
            {
                dolphinBoss.gameObject.SetActive(true);
            }

        }else if (collision.gameObject.tag == "BossWarp") 
        {
            cityBoss.gameObject.SetActive(true);
        }
    }
}
