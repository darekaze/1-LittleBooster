using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(10f,10f,10f);
    [SerializeField] float period = 2f;

    [Range(0,1)]
    [SerializeField] float movementFactor; // 0 for not move, 1 for full

    Vector3 startingPos;

    void Start () {
        startingPos = transform.position;
	}
	
	void Update () {
        if(period <= Mathf.Epsilon) { return; }
        float cycle = Time.time / period; // Frequency: grow continually from 0

        const float tau = Mathf.PI * 2f; // Around 6.28
        float rawSinWave = Mathf.Sin(cycle * tau);

        movementFactor = rawSinWave / 2f + 0.5f; // Decrease amplitute and adjust amplitude position
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
	}
}
