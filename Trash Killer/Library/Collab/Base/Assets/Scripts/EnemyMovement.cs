using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //public float speed;
      private bool verPJ = false;
      private Transform target;
      public int MoveSpeed;
      public float MaxDist;
      public float MinDist;
    

    // Start is called before the first frame update
    void Start()
    {
       target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {  float distancia = Vector3.Distance(transform.position, target.position);

    if(distancia <= MinDist)
    {
       verPJ = true;
    }else if(distancia >= MaxDist){
       verPJ = false;
    }
    if (verPJ == true)
    {
       transform.position = Vector2.MoveTowards(transform.position, target.position, MoveSpeed * Time.deltaTime);
    }
   }

}
