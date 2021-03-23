using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropMiniBossWeapon : MonoBehaviour
{
    public GameObject miniBoss;
    private Animator anim;
    private int one;
    private bool muerto;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        one = 0;
        muerto = false;
    }

    void Update()
    {
        if (miniBoss.GetComponent<TazaBossAI>() != null || miniBoss != null)
            muerto = miniBoss.GetComponent<TazaBossAI>().muerto;

        if (muerto && one == 0)
        {
            one++;
            anim.SetTrigger("drop");
        }
    }
}
