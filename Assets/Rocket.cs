using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float rThrust = 100f;
    [SerializeField] float mThrust = 50f;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        Thrust();
        Rotate();
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
        switch (collision.gameObject.tag) { // TODO: implement it
            case "Friendly":
                print("Safe"); 
                break;
            case "Fuel":
                print("Add oil");
                break;
            default:
                print("Died");
                break;
        }
    }
}
