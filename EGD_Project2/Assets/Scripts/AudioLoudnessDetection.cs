using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoudnessDetection : MonoBehaviour
{
    public int sampleWindow = 64;
    private AudioClip microphoneClip;
    public int chosenMic = 0;

    // Start is called before the first frame update
    void Start()
    {
        GetMics();
        MicrophoneToAudioClip();
        print(Microphone.devices);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MicrophoneToAudioClip()
    {
        //gets first microphone in devices list
        string microphoneName = Microphone.devices[chosenMic];
        microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
        print(microphoneName);
    }

    public float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microphoneClip);
    }

    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;

        if (startPosition < 0)
            startPosition = 0;

        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);

        //compute loudness
        float totalLoudness = 0;

        for (int i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
            print(totalLoudness);
        }

        return totalLoudness / sampleWindow;
    }

    public string GetMics()
    {
        string mics = string.Empty;
        for (int i = 0; i < Microphone.devices.Length; i++)
        {
            mics += " ";
            mics += Microphone.devices[i];
        }

        print(mics);
        return mics;
    }
}
