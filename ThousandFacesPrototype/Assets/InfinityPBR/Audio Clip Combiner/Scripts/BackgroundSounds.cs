using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSounds : MonoBehaviour
{
    public bool moving = false;
    public float speed = 4f;
    public Vector3 desiredPosition;
    private Vector3 originalPosition;
    public float maxDistance = 20f;
    public AudioSource audioSource;

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        desiredPosition = new Vector3(maxDistance, transform.position.y, transform.position.z);
        originalPosition = transform.position;
    }

    public void ToggleMovement(bool toggle)
    {
        moving = toggle;
    }

    public void Update()
    {
        if (!moving)
            DoMovement(true);
        else
            DoMovement(false);
    }

    public void DoMovement(bool goHome)
    {
        if (goHome)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, speed * Time.maximumDeltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, speed * Time.maximumDeltaTime);
        }

        if (transform.position == desiredPosition)
        {
            desiredPosition = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
        }
    }

    public void ChangeSource(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
