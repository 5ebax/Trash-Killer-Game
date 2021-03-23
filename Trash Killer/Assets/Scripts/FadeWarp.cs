using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeWarp : MonoBehaviour
{
    public Image fade;

    private void Start()
    {
        fade.gameObject.SetActive(true);
        fade.canvasRenderer.SetAlpha(0.0f);
    }

    public void FadeOut()
    {
        fade.canvasRenderer.SetAlpha(1.0f);
        fade.CrossFadeAlpha(0, 1, false);
    }

    public void FadeIn()
    {
        fade.canvasRenderer.SetAlpha(0.0f);
        fade.CrossFadeAlpha(1, 1, false);
    }
}
