using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DolphinCollision : MonoBehaviour
{
    private MiniBossDolphin dolphin;
    private PlayerController PJCont;

    private void Awake()
    {
        dolphin = GetComponentInParent<MiniBossDolphin>();
    }
    private void Start()
    {
        PJCont = FindObjectOfType<PlayerController>();
    }

    #region Colisiones

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            dolphin.Vidas -= 1;
            dolphin.healthBar.SetHealth(dolphin.Vidas);
            PJCont.Puntuacion += 20;
        }
        else if (collision.gameObject.CompareTag("Bullet Organic"))
        {
            dolphin.Vidas -= 2;
            dolphin.healthBar.SetHealth(dolphin.Vidas);
            PJCont.Puntuacion += 20;
        }
        else if (collision.gameObject.CompareTag("Bullet Plastic"))
        {
            dolphin.Vidas -= 1;
            dolphin.healthBar.SetHealth(dolphin.Vidas);
            PJCont.Puntuacion += 20;
        }
        else if (collision.gameObject.CompareTag("Bullet Vidrio"))
        {
            dolphin.Vidas -= 1;
            dolphin.healthBar.SetHealth(dolphin.Vidas);
            PJCont.Puntuacion += 20;
        }
        else if (collision.gameObject.CompareTag("Bullet Paper"))
        {
            dolphin.Vidas -= 1;
            dolphin.healthBar.SetHealth(dolphin.Vidas);
            PJCont.Puntuacion += 20;
        }
    }

    #endregion

}
