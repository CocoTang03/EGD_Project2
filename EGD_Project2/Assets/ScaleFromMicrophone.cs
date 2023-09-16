using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFromMicrophone : MonoBehaviour
{
    public AudioSource source;
    public Vector3 minScale;
    public Vector3 maxScale;
    public AudioLoudnessDetection detector;

    public float loudness;
    public float loudnessSensibility = 100;
    public float threshold = 0.1f;

    Vector3 direction = Vector2.right;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
     void Update()
     {

     }

    private void FixedUpdate()
    {
        loudness = detector.GetLoudnessFromMicrophone();

        if (loudness > threshold)
        {
            Moving();
        }
        else
        {
            Pause();
        }
    }

    public void Moving()
    {
        transform.Translate(direction * (1 + loudness) * Time.deltaTime);
    }

    public void Pause()
    {
        transform.Translate(direction * 0 * Time.deltaTime);
    }

}
