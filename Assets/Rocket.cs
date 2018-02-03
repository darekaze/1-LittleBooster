using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource;

    [SerializeField] float rThrust = 100f;
    [SerializeField] float mThrust = 50f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip death;

    //[SerializeField] ParticleSystem mainEngineParticle;
    //[SerializeField] ParticleSystem successParticle;
    //[SerializeField] ParticleSystem deathParticle;


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
            RespondToThrust();
            RespondToRotate();
        }
	}

    private void RespondToThrust() {
        if (Input.GetKey(KeyCode.Space)) {
            ApplyThrust();
        }
        else {
            audioSource.Stop();
           // mainEngineParticle.Stop();
        }
    }

    private void ApplyThrust() {
        rigidBody.AddRelativeForce(Vector3.up * mThrust);
        if (!audioSource.isPlaying) {
            audioSource.PlayOneShot(mainEngine);
        }
        // mainEngineParticle.Play();
    }

    private void RespondToRotate() {
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
                StartSuccess();
                break;
            default:
                StartDeath();
                break;
        }
    }

    

    private void StartSuccess() {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        Invoke("LoadNextScene", 3f); // Delay loading next scene
    }

    private void StartDeath() {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        Invoke("LoadStartScene", 3f);
    }

    private void LoadStartScene() {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene() {
        SceneManager.LoadScene(1);
    }
}
