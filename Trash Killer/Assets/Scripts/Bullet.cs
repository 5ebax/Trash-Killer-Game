using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public GameObject hitEffect;

    //Buscará los objetos con el tag Water para evitarlos.
    void OnEnable()
    {
        GameObject[] otherObjects = GameObject.FindGameObjectsWithTag("Water");

        //Recorre los GameObjects ignorando las físicas de sus Colliders para poder atravesar el agua.
        foreach (GameObject obj in otherObjects)
        {
            if (obj.GetComponent<Collider2D>())
            {
                Physics2D.IgnoreCollision(obj.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }
            else
            {
                Physics2D.IgnoreCollision(obj.GetComponent<PolygonCollider2D>(), GetComponent<Collider2D>());
            }
        }
        
    }

    //Se desactiva al colisionar.
    void OnCollisionEnter2D(Collision2D collision)
    {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, .40f);

            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            gameObject.SetActive(false);
        }
    }

    //Se desactiva al salir de la cámara.
    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

    //Se desactiva al salir del Bounds que rodea el mapa.
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bounds")
        {
            if (gameObject.tag == "Bullet")
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
