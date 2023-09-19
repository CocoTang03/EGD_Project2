using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public GameObject listenerObj;
    AudioLoudnessDetection listener;
    public float detectedLoudness;
    public float peakLoudness;

    public GameObject audioDisplayerObj;
    audioDisplayerController audioDisplayer;

    // Start is called before the first frame update
    void Start()
    {
        listener = listenerObj.GetComponent<AudioLoudnessDetection>();
        audioDisplayer = audioDisplayerObj.GetComponent<audioDisplayerController>();
    }
    void cheerUpdate()
    {
        detectedLoudness = listener.GetLoudnessFromMicrophone();
        peakLoudness = Mathf.Max(detectedLoudness, peakLoudness);
        audioDisplayer.setLoudness(detectedLoudness / peakLoudness);
    }

    // Update is called once per frame
    void Update()
    {
        cheerUpdate();
    }
}
