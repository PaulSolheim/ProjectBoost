﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    // Fix Lighting bug

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // todo somewhere stop sound on death
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; }       // ignore collisions when not alive

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        if (audioSource.isPlaying) { audioSource.Stop(); }
        audioSource.PlayOneShot(success);
        Invoke("LoadNextLevel", 1f);
    }

    private void StartDeathSequence()
    {
        print("hit something deadly!");
        state = State.Dying;
        if (audioSource.isPlaying) { audioSource.Stop(); }
        audioSource.PlayOneShot(death);
        Invoke("LoadFirstLevel", 1f);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
        state = State.Alive;
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);          // todo allow for more than 2 scenes
    }

    private void RespondToThrustInput()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
    }

    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true;                // take manual control of rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false;               // resume physics control of rotation

    }
}
