using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 targetPos;
    public float moveSpeed;

    public BoxCollider2D boundBox;
    private Vector3 minBounds;
    private Vector3 maxBounds;

    private Camera theCamera;
    private float halfHeight;
    private float halfWidth;

    // Duracion del shake
    private float shakeDuration = 0f;

    // Tamaño del Shake
    private float shakeMagnitude = .2f;

    // Velocidad del Shake
    private float dampingSpeed = 7f;

    void Awake()
    {
        theCamera = Camera.main;
        
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        minBounds = boundBox.bounds.min;
        maxBounds = boundBox.bounds.max;

        halfHeight = theCamera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;
    }


    void Update()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = transform.position + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;

            targetPos = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);

            if (boundBox == null) return;

            float clampedX = Mathf.Clamp(transform.position.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
            float clampedY = Mathf.Clamp(transform.position.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);
            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
    }


    public void SetBounds(BoxCollider2D newBounds)
    {
        boundBox = newBounds;

        minBounds = boundBox.bounds.min;
        maxBounds = boundBox.bounds.max;
    }

    public void TriggerShake()
    {
        shakeDuration = .1f;
    }
}
