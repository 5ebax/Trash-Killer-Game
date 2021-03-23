using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public SpriteRenderer flip;
    public SpriteRenderer sr;
    public Transform mira;

    private Vector2 mousePos;

    void Update()
    {
        //Detectar el mouse y colocar ahí el puntero.
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mira.position = mousePos;

        //Mueve el arma en direccion al puntero.
        Vector2 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //Hace flip al sprite para que se vea correctamente al moverse.
        if (angle > 90 || angle < -90)
        {
            flip.flipY = true;
            sr.flipX = true;
        }
        else
        {
            flip.flipY = false;
            sr.flipX = false;
        }
    }
}
