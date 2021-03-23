using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossHead : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform[] firePoint;
    public Transform[] puntito;
    public GameObject bulletMiniboss;

    private GameObject bullet;

    private float fireRate;
    private float nextFire;

    void Start()
    {
        fireRate = 0.3f;
        nextFire = Time.time;
    }

    void Update()
    {
        if (Time.time > nextFire)
            {
            Disparo();
            nextFire = Time.time + fireRate;
            }
    }
    
    //Instancia la bala y le da el comienzo y el final para la direccion de la bala.
    public void Disparo()
    {

        int j = Random.Range(0, firePoint.Length);
        float v = Random.Range(2f, 5f);

         for (int i = 0; i < firePoint.Length; i++)
        {

        bullet = Instantiate(bulletMiniboss, firePoint[j].position, Quaternion.identity);
        bullet.GetComponent<BulletMiniboss>().firePoint = firePoint[j];
        bullet.GetComponent<BulletMiniboss>().puntito = puntito[j];
        bullet.GetComponent<BulletMiniboss>().bulletSpeed = v;
        
        }

    }
}
