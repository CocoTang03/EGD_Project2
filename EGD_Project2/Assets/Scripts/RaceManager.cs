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

    public GameObject[] horses;
    public float horseSpeed;

    public float globalNoiseToMovementModifier;

    public bool recording;

    public float progressPoints;

    public GameObject bigSign;
    public GameObject smallSign;

    // Start is called before the first frame update
    void Start()
    {
        listener = listenerObj.GetComponent<AudioLoudnessDetection>();
        audioDisplayer = audioDisplayerObj.GetComponent<audioDisplayerController>();
        StartCoroutine(FirstStage());
        progressPoints = 0;
        recording = false;
    }
    void cheerUpdate()
    {
        detectedLoudness = listener.GetLoudnessFromMicrophone();
        peakLoudness = Mathf.Max(detectedLoudness, peakLoudness);
        float effectiveVolume = detectedLoudness / peakLoudness;
        audioDisplayer.setLoudness(effectiveVolume);
        progressPoints += effectiveVolume * Time.deltaTime * globalNoiseToMovementModifier;
    }

    // Update is called once per frame
    void Update()
    {
        if (recording)
        {
            cheerUpdate();
        } else
        {
            audioDisplayer.setLoudness(0);
        }
    }

    public IEnumerator accumulateVoice(float seconds)
    {
        recording = true;
        yield return new WaitForSeconds(seconds);
        recording = false;
        //Debug.Log("Accumulated " + progressPoints + " points");
    }

    public IEnumerator MoveHorse(float distance, int horseID)
    {
        //float increment = .1f;
        for (float remDist = distance; remDist >= 0; remDist -= horseSpeed * Time.deltaTime)
        {
            //Debug.Log("Looping " + remDist);
            horses[horseID].transform.position = (Vector2) horses[horseID].transform.position + Vector2.right * horseSpeed * Time.deltaTime;
            yield return null;
        }
    }

    string DecodeHorse(int horseID)
    {
        switch (horseID)
        {
            case 0:
                return "Pink";
            case 1:
                return "Cyan";
            case 2:
                return "Yellow";
            default:
                return "Racer";
        }
    }

    void BigSign(float time, string message)
    {
        GameObject cur = Instantiate(bigSign, new Vector3(0, 12, 01), transform.rotation);
        cur.GetComponent<signController>().SetVals(time, message);
    }

    void SmallSign(float time, string message)
    {
        GameObject cur = Instantiate(smallSign, new Vector3(0, 12, 01), transform.rotation);
        cur.GetComponent<signController>().SetVals(time, message);
    }

    // START OF SEQUENTAIL CODE

    public IEnumerator FirstStage()
    {
        yield return new WaitForSeconds(11f);

        for (int i = 0; i < 3; i++)
        {
            Debug.Log("Cheering Horse " + i);
            //big sign
            BigSign(2f, "Round 1\n" + DecodeHorse(i) + " team, Cheer!");
            yield return new WaitForSeconds(3.4f);

            //background sign indicates its a go
            Debug.Log("Passed Sign");
            SmallSign(7f, "Round 1\n" + DecodeHorse(i) + " team, Cheer!");
            yield return new WaitForSeconds(.4f);

            progressPoints = 0;
            StartCoroutine(accumulateVoice(7f));
            yield return new WaitForSeconds(7f);

            StartCoroutine(MoveHorse(progressPoints, i));
            yield return new WaitForSeconds(3f);
        }
        StartCoroutine(SecondStage());
    }

    public IEnumerator SecondStage()
    {
        BigSign(4f, "Round 2, Begin!");
        yield return new WaitForSeconds(6f);


        for (int i = 0; i < 3; i++)
        {
            Debug.Log("Cheering Horse " + i);
            //big sign
            BigSign(2f, "Round 1\n" + DecodeHorse(i) + " team, Cheer!");
            yield return new WaitForSeconds(3.4f);

            //background sign indicates its a go
            Debug.Log("Passed Sign");
            SmallSign(7f, "Round 1\n" + DecodeHorse(i) + " team, Cheer!");
            yield return new WaitForSeconds(.4f);

            progressPoints = 0;
            StartCoroutine(accumulateVoice(7f));
            yield return new WaitForSeconds(7f);

            StartCoroutine(MoveHorse(progressPoints, i));
            yield return new WaitForSeconds(3f);
        }
        StartCoroutine(VotingA());
    }

    public IEnumerator VotingA()
    {
        float readTime = 3f;
        BigSign(readTime, "Round 3: Mixup");
        yield return new WaitForSeconds(readTime + 2f);

        BigSign(readTime, "Cheer for the Mixup you want to play!");
        yield return new WaitForSeconds(readTime + 2f);

        string opA = "reverse";
        string opB = "double";
        float voteTime = 5f;
        BigSign(voteTime, "Cheer now for " + opA + "!");
        progressPoints = 0;
        StartCoroutine(accumulateVoice(voteTime));
        yield return new WaitForSeconds(voteTime+2f);
        float pointsForA = progressPoints;

        BigSign(voteTime, "Cheer now for " + opB + "!");
        progressPoints = 0;
        StartCoroutine(accumulateVoice(voteTime));
        yield return new WaitForSeconds(voteTime+2f);
        float pointsForB = progressPoints;
        if (pointsForA > pointsForB)
        {
            StartCoroutine()
        }
    }
    public IEnumerator AltStageA()
    {
    }
    public IEnumerator AltStageB()
    {
    }

}
