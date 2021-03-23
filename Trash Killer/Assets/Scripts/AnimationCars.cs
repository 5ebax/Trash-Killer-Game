using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationCars : MonoBehaviour
{
    private Animator anim;
    private CameraController cam;
    private PlayerController pjController;
    private int i;

    public GameObject[] enemies;
    public bool cinematic;
    public Transform finish;
    public GameObject player;

    void Awake()
    {
        cam = GameObject.FindObjectOfType<CameraController>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pjController = player.GetComponent<PlayerController>();
        i = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) { SceneManager.LoadScene("City"); }

        if (i == 0)
        {
            if (pjController.EnemyDeaths >= 20 && PlayerPrefs.GetString("cinematic") == "False")
            {
                PlayerPrefs.SetString("cinematic", "True");
                pjController.enabled = false;
                cam.player = finish.gameObject;
                StartCoroutine(Coches());
                i = 1;
            }
        }
    }

    IEnumerator Coches()
    {
        anim.SetTrigger("anim");
        yield return new WaitForSecondsRealtime(7f);
        cam.player = player;
        player.GetComponent<PlayerController>().enabled = true;
        PlayerPrefs.SetString("cinematic", "False");
    }
}
