using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCityBoss : MonoBehaviour
{
    private Vector3 targetPosition;
    private Rigidbody2D rb;
    private PlayerController pjCont;

    void Start()
    {
        targetPosition = PlayerController.position;
        rb = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate()
    {
        //Comprueba que llega a la posicion para mantenerse ahí.
        if (Mathf.FloorToInt(transform.position.y)==Mathf.FloorToInt(targetPosition.y) 
            && Mathf.FloorToInt(transform.position.x) == Mathf.FloorToInt(targetPosition.x))
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(0f,0f);
            GetComponent<CircleCollider2D>().radius = 0.42f;
            GetComponentInChildren<Animator>().SetTrigger("crash");
            Invoke("Off", 10f);
        }

        //Gira el Sprite dependiendo de a donde mire.
        if (rb.velocity.x >= 0.01f)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
        else if (rb.velocity.x <= 0.01f)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = false;
        }

        if (rb.velocity.y >= 0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (rb.velocity.y <= 0.01f)
        {
            transform.localScale = new Vector3(1f, -1f, 1f);
        }
    }

    void Off()
    {
        gameObject.SetActive(false);
        Destroy(gameObject, .2f);
    }
}
