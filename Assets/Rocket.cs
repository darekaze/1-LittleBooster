using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float rThrust = 100f;
    [SerializeField] float mThrust = 50f;
    enum State {
        Alive, Dying, Transcending
    }
    State state = State.Alive;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if(state == State.Alive) {
            Thrust();
            Rotate();
        }
	}

    private void Thrust() {
        if (Input.GetKey(KeyCode.Space)) {
            rigidBody.AddRelativeForce(Vector3.up * mThrust);
            if (!audioSource.isPlaying) {
                audioSource.Play();
            }
        }
        else {
            audioSource.Stop();
        }
    }

    private void Rotate() {
        rigidBody.freezeRotation = true; // manual
        float rotationThisFrame = rThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(Vector3.back * rotationThisFrame);
        }
        rigidBody.freezeRotation = false; // resume physics
    }

    private void OnCollisionEnter(Collision collision) {
        if (state != State.Alive) return;

        switch (collision.gameObject.tag) {
            case "Friendly":
                print("Safe"); 
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextScene",1f); // Delay loading next scene
                break;
            default:
                state = State.Dying;
                Invoke("LoadStartScene",4f);
                break;
        }
    }

    private void LoadStartScene() {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene() {
        SceneManager.LoadScene(1);
    }
}
