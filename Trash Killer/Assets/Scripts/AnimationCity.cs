using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationCity : MonoBehaviour
{
    public GameObject[] coches;
    public GameObject finish;
    //private Camion trigger;
    private CameraController cameraScript;
    private Vector3[] initialPosition = new Vector3[5];
    private Vector3[] direc = new Vector3[5];
    private Vector3[] obj = new Vector3[5];
    //private bool ok = false;

    private void Awake()
    {
       // trigger = GameObject.FindObjectOfType<Camion>();
        cameraScript = GameObject.FindObjectOfType<CameraController>();
    }

    private void Start()
    {
        /*
        obj[0] = coches[0].transform.position + new Vector3(0f, 4f, 0f);
        obj[1] = coches[1].transform.position + new Vector3(2f, 4f, 0f);
        obj[2] = coches[2].transform.position + new Vector3(2f, 4f, 0f);
        obj[3] = coches[3].transform.position + new Vector3(1f, 1f, 0f);
        obj[4] = coches[4].transform.position + new Vector3(1f, -1f, 0f);
        for (int i = 0; i < 5; i++)
        {
            initialPosition[i] = coches[i].transform.position;
            direc[i] = (obj[i] - initialPosition[i]).normalized;
        }*/
    }
    // Update is called once per frame
    void Update()
    {
       /* if (Input.GetButtonDown("Fire3")) //Puntos a conseguir, o enemigos a matar.
        {
            cameraScript.enabled = true;
            if (!Mathf.Approximately(coches[5].transform.position.x, finish.transform.position.x) && ok)
            {
                Vector3 dir = (finish.transform.position - coches[5].transform.position).normalized;
                coches[5].GetComponent<Rigidbody2D>().MovePosition(coches[5].transform.position + dir * 5f * Time.deltaTime);
                Animated();
            }
            else
            {
                cameraScript.enabled = true;
            }
        }
        */
        if (Input.GetKeyDown(KeyCode.R)) { SceneManager.LoadScene("City"); }
    }

    IEnumerator Camion()
    {
        yield return new WaitForSecondsRealtime(5f);
        //ok = true;
    }

    void Animated()
    {
        //Desactivar Script camara, moverla, y devolverla a su sitio.
        //Crear rango de la animacion para sacar al pj si se encuentra cerca

        /*
        if (trigger.EnteredTrigger)
        {
            if(coches[0].name == trigger.carHit.name)
            {
                    coches[0].GetComponent<Rigidbody2D>().MovePosition(coches[0].transform.position + direc[0] * 5f * Time.deltaTime);
                    Debug.Log("Choca coche 1");
            }else if (coches[1].name == trigger.carHit.name)
            {
                    coches[1].GetComponent<Rigidbody2D>().MovePosition(coches[1].transform.position + direc[1] * 5f * Time.deltaTime);
                    Debug.Log("Choca coche 2");
            }
            else if (coches[2].name == trigger.carHit.name)
            {
                    coches[2].GetComponent<Rigidbody2D>().MovePosition(coches[2].transform.position + direc[2] * 7f * Time.deltaTime);
                    Debug.Log("Choca coche 3");
            }
            else if (coches[3].name == trigger.carHit.name)
            {
                    coches[3].GetComponent<Rigidbody2D>().MovePosition(coches[3].transform.position + direc[3] * 3f * Time.deltaTime);
                    coches[4].GetComponent<Rigidbody2D>().MovePosition(coches[4].transform.position + direc[4] * 3f * Time.deltaTime);
                    Debug.Log("Choca coche 4");
            }
            else if (coches[4].name == trigger.carHit.name)
            {
                if (coches[4].transform.position == initialPosition[4])
                {
                    coches[4].GetComponent<Rigidbody2D>().MovePosition(coches[4].transform.position + direc[4] * 5f * Time.deltaTime);
                    Debug.Log("Choca coche 5");
                }
            }
        }*/
    }
}
