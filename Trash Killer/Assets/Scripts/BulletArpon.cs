using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletArpon : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D collision)
    {

        gameObject.SetActive(false);

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bounds")
        {
            if (gameObject.tag == "Bullet Arpon")
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
