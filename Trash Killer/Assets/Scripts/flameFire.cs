using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flameFire : MonoBehaviour
{
    public GameObject firePoint;
    private ParticleSystem fire;

    private void Start()
    {
        fire = firePoint.GetComponent<ParticleSystem>();
        firePoint.SetActive(false);
    }
    void Update()
    {
        var main = fire.main;
        if (Input.GetButtonDown("Fire1"))
        {
            main.loop = true;
            firePoint.SetActive(true);
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            main.loop = false;
            Invoke("Off", 1.5f);
        }
    }


    void Off()
    {
        firePoint.SetActive(false);
    }
}
