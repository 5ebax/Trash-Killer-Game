using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMiniboss : MonoBehaviour
{
    [System.NonSerialized]
    public Transform puntito;
    [System.NonSerialized]
    public Transform firePoint;
    [System.NonSerialized]
    public float bulletSpeed;
    public GameObject target;

    private float speed;

    Vector3 dir;

    //Guarda la direccion recibida desde los puntos dados por la cabeza del MiniBoss.
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        dir = (puntito.position - firePoint.position).normalized;
        speed = bulletSpeed;
    }

    //Avanza la bala.
    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().MovePosition(transform.position + dir * speed * Time.deltaTime);
        Destroy(gameObject, 2f);
    }

    void OnTriggerEnter2D (Collider2D col)
	{
		if(col.gameObject.CompareTag("HitBox")) {
            target.GetComponentInChildren<Animator>().SetTrigger("Daño");
            target.GetComponent<PlayerController>().Vidas--;
            Debug.Log("Un punto de vida menos.");
			Debug.Log ("Hit!");
			Destroy (gameObject);
		}
	}


    //Se desactiva al salir del Bounds que rodea el mapa.
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bounds")
        {
            gameObject.SetActive(false);
            Destroy(gameObject, .5f);
        }

    }
}
