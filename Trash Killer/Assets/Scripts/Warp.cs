using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    public GameObject target;
    private GameObject player;
    private Camera myCamera;
    private bool warp;
    private Vector2 targetPosition;
    private FadeWarp fade;

    private void Awake()
    {
        myCamera = Camera.main;
        fade = FindObjectOfType<FadeWarp>();
    }

    // Start is called before the first frame update
    void Start()
    {
        warp = false;
        player = GameObject.FindGameObjectWithTag("Player");
        targetPosition.x = target.transform.GetChild(0).transform.position.x;
        targetPosition.y = target.transform.GetChild(0).transform.position.y;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "HitBox")
        {
            if (warp == false)
            {
                StartCoroutine(Wait());
                myCamera.transform.position = target.transform.GetChild(0).transform.position + new Vector3(0f, 0f, -10f);
                player.transform.position = new Vector3(targetPosition.x, targetPosition.y, 0f);
                fade.FadeOut();
            }
        }
    }

    IEnumerator Wait()
    {
        warp = true;
        yield return new WaitForSecondsRealtime(2f);
        warp = false;
    }
}
