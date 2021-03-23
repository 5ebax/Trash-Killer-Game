using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPlayer : MonoBehaviour
{

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite mediumHeart;

    private PlayerController pjCont;
    private int MaxHealth;
    private bool restore;

    void Awake()
    {
        
    }

    void Start()
    {
        pjCont = GetComponent<PlayerController>();
        restore = false;
        MaxHealth = pjCont.Vidas;
    }

    void Update()
    {
        //Si la vida vuelve estar al maximo se rehabilitan los corazones.
        if (pjCont.Vidas == MaxHealth)
        {
            restore = true;
        }
        else { restore = false; }

        //Manejo de la Healthbar.
        for (int i = 0; i < hearts.Length; i++)
        {
            if(i*2+1 < pjCont.Vidas)
            {
                    hearts[i].sprite = fullHeart;
            }
            else
            {
                    hearts[i].sprite = mediumHeart;
            }

            if (i*2 == pjCont.Vidas)
            {
                hearts[i].enabled = false;
            }

            if(restore)
                hearts[i].enabled = true;
        }
    }
}
