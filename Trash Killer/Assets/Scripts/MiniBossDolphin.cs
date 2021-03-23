using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class MiniBossDolphin : MonoBehaviour
{
    
    public float radiusWater;
    public HealthBar healthBar;
    public GameObject waterProjectile;
    public Transform spawnWater;
    public AudioClip muerte_clip;
    public BoxCollider2D miniBossWarp;
    [System.NonSerialized]
    public bool muerto;

    // Variable para guardar la posición inicial
    Vector3 initialPosition;

    private bool one;
    private int action;

    public int vidas;
    public int Vidas
    {
        get { return vidas; }
        set { vidas = value; }
    }

    Animator anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

        // Guardamos nuestra posición inicial
        initialPosition = transform.position;
        vidas = 100;
        healthBar.SetMaxHealth(vidas);
        action = 0;
        one = false;
        anim.speed = .4f;
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
        if (!muerto)
        {
            Comenzar();
        }
    }

    //Muerte del MiniBoss
    private void Muerte()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        miniBossWarp.enabled = true;
         muerto = true;
        anim.SetBool("move", false);
        anim.SetTrigger("muerte");
        Invoke("DeadSound", 1.57f);
    }
    private void DeadSound() { AudioManager.Instance.PlaySFX(muerte_clip, .2f); }

    #endregion

    #region Corutinas y movimientos.

    //Se mueve entre puntos.
    void Comenzar()
    {
        if (one == false)
           StartCoroutine(ChooseAction());
    }

    IEnumerator ChooseAction()
    {
        action = Random.Range(0, 3);
        one = true;
        int rdm = Random.Range(1, 5);
        if (action == 0 || action == 1)
        {
            anim.SetTrigger("shoot"+rdm);
            Invoke("Disparar", 0.5f);
            yield return new WaitForSecondsRealtime(8f);
        }
        else if (action == 2)
        {
            anim.SetTrigger("jump"+rdm);
            yield return new WaitForSecondsRealtime(5f);
        }

        one = false;
    }

    #endregion

    // Podemos dibujar el radio de visión sobre la escena dibujando una esfera
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(spawnWater.position, radiusWater);
    }

    #region Ataques MiniBoss
    private void Disparar()
    {
        for (int i = 0; i < Random.Range(8, 15); i++)
        {
            Instantiate(waterProjectile, spawnWater.position + Random.insideUnitSphere * radiusWater, Quaternion.identity);
        }
    }

    #endregion
}
