using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTrigger : MonoBehaviour
{

    public bool EnteredTrigger = false;
    [System.NonSerialized]
    public GameObject pjHit;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            EnteredTrigger = true;
            pjHit = collision.gameObject;
        }
        else { EnteredTrigger = false; }
    }
}
