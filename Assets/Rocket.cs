using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource;

    [SerializeField] float rThrust = 100f;
    [SerializeField] float mThrust = 50f;
    [SerializeField] float levelLoadDelay = 3f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip death;

    [SerializeField] ParticleSystem engineParticle;
    [SerializeField] ParticleSystem winParticle;
    [SerializeField] ParticleSystem deadParticle;

    bool collisionDisabled = false;
    bool isTranscending = false;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if(!isTranscending) {
            RespondToThrust();
            RespondToRotate();
        }
        if (Debug.isDebugBuild) {
            DebugTestingTools();
        }
    }

    private void DebugTestingTools() {
        if (Input.GetKeyDown(KeyCode.L)) {
            LoadNextScene();
        }
        else if (Input.GetKeyDown(KeyCode.C)) {
            // Toggle collision
            collisionDisabled = !collisionDisabled;
        }
    }

    private void RespondToThrust() {
        if (Input.GetKey(KeyCode.Space)) {
            ApplyThrust();
        }
        else {
            StopApplyingThrust();
        }
    }

    private void StopApplyingThrust() {
        audioSource.Stop();
        engineParticle.Stop();
    }

    private void ApplyThrust() {
        rigidBody.AddRelativeForce(Vector3.up * mThrust * Time.deltaTime);
        if (!audioSource.isPlaying) {
            audioSource.PlayOneShot(mainEngine);
        }
        engineParticle.Play();
    }

    private void RespondToRotate() {
        rigidBody.angularVelocity = Vector3.zero;// manual
        float rotationThisFrame = rThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(Vector3.back * rotationThisFrame);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (isTranscending || collisionDisabled) return;

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
        isTranscending = true;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        winParticle.Play();
        Invoke("LoadNextScene", levelLoadDelay); // Delay loading next scene
    }

    private void StartDeath() {
        isTranscending = true;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        deadParticle.Play();
        Invoke("LoadStartScene", levelLoadDelay);
    }

    private void LoadStartScene() {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex < SceneManager.sceneCountInBuildSettings) {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else {
            LoadStartScene();
        }
        
    }
}
