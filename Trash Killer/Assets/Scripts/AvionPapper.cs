using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvionPapper : MonoBehaviour
{
    // Start is called before the first frame update
    float moveSpeed = 10f;

    Rigidbody2D rb;

    public GameObject target;

    Vector2 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        moveDirection = (target.transform.position - transform.position).normalized * moveSpeed;
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("HitBox")) {
            target.GetComponentInChildren<Animator>().SetTrigger("Daño");
            target.GetComponent<PlayerController>().Vidas--;
            Debug.Log("Un punto de vida menos.");
			Debug.Log ("Hit!");
			Destroy (gameObject);
		}
    }
}
