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
    public float[] accumPoints;

    public GameObject endFlag;

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
        }
    }

    public IEnumerator accumulateVoice(float seconds)
    {
        recording = true;
        yield return new WaitForSeconds(seconds);
        recording = false;
        audioDisplayer.setLoudness(0);
        //Debug.Log("Accumulated " + progressPoints + " points");
    }

    public IEnumerator AccumulateVarying(float seconds, int horseID)
    {
        float runtime = 0;
        while (runtime < seconds)
        {
            detectedLoudness = listener.GetLoudnessFromMicrophone();
            peakLoudness = Mathf.Max(detectedLoudness, peakLoudness);
            float effectiveVolume = detectedLoudness / peakLoudness;
            audioDisplayer.setLoudness(effectiveVolume);
            float heightModifier = horses[horseID].GetComponent<xHorseScript>().horse.transform.position.y - horses[horseID].transform.position.y;
            progressPoints += (effectiveVolume * heightModifier + .5f) * Time.deltaTime * globalNoiseToMovementModifier;
            runtime += Time.deltaTime;
            yield return null;
        }
        audioDisplayer.setLoudness(0);

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

    public IEnumerator BackHorse(float distance, int horseID)
    {
        //float increment = .1f;
        for (float remDist = distance; remDist <= 0; remDist += horseSpeed * Time.deltaTime)
        {
            //Debug.Log("Looping " + remDist);
            horses[horseID].transform.position = (Vector2)horses[horseID].transform.position + Vector2.right * horseSpeed * Time.deltaTime;
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
            accumPoints[i] += progressPoints;
            yield return new WaitForSeconds(3f);
        }
        StartCoroutine(SecondStage());
    }

    public IEnumerator SecondStage()
    {
        BigSign(2f, "Round 2, Begin!");
        yield return new WaitForSeconds(4f);

        //calc catchups
        float avgx = (horses[0].transform.position.x + horses[1].transform.position.x + horses[2].transform.position.x) / 3f;

        for (int i = 0; i < 3; i++)
        {
            Debug.Log("Cheering Horse " + i);
            //big sign
            BigSign(2f, "Round 2\n" + DecodeHorse(i) + " team, Cheer!");
            yield return new WaitForSeconds(3.4f);

            //background sign indicates its a go
            Debug.Log("Passed Sign");
            float raceTime = 7f + (avgx - horses[i].transform.position.x) * 1.1f;
            SmallSign(raceTime, "Round 2\n" + DecodeHorse(i) + " team, Cheer!");
            yield return new WaitForSeconds(.4f);

            progressPoints = 0;
            StartCoroutine(accumulateVoice(raceTime));
            yield return new WaitForSeconds(raceTime);

            StartCoroutine(MoveHorse(progressPoints, i));
            accumPoints[i] += progressPoints;
            yield return new WaitForSeconds(3f);
        }
        StartCoroutine(VotingA());
    }

    public IEnumerator VotingA()
    {
        float readTime = 3f;
        BigSign(readTime, "Intermission 1: Race Modifiers");
        yield return new WaitForSeconds(readTime + 2f);

        BigSign(readTime, "Cheer for the Modifier you want for this round!");
        yield return new WaitForSeconds(readTime + 2f);

        string opA = "Rythmic Cheering";
        string opB = "Jeering";
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
            StartCoroutine(AltStageA());
        } else
        {
            StartCoroutine(AltStageB());
        }
    }
    public IEnumerator AltStageA()
    {
        float readTime = 3f;
        BigSign(readTime, "Round 3: Cheer in Time!");
        yield return new WaitForSeconds(readTime + 2f);

        readTime = 5f;
        BigSign(readTime, "Cheer when your horse at at its apex and be silent when its at the bottom.");
        yield return new WaitForSeconds(readTime + 2f);

        //calc catchups
        float avgx = (horses[0].transform.position.x + horses[1].transform.position.x + horses[2].transform.position.x) / 3f;

        for (int i = 0; i < 3; i++)
        {
            Debug.Log("Cheering Horse " + i);
            //big sign
            BigSign(2f, "Round 3\n" + DecodeHorse(i) + " team, Cheer!");
            yield return new WaitForSeconds(3.4f);

            //background sign indicates its a go
            Debug.Log("Passed Sign");
            float raceTime = 7f + (avgx - horses[i].transform.position.x) * 1.1f;
            SmallSign(raceTime, "Round 3\n" + DecodeHorse(i) + " team, Cheer!");
            yield return new WaitForSeconds(.4f);

            progressPoints = 0;
            StartCoroutine(AccumulateVarying(raceTime, i));
            yield return new WaitForSeconds(raceTime);

            StartCoroutine(MoveHorse(progressPoints, i));
            accumPoints[i] += progressPoints;
            yield return new WaitForSeconds(3f);
        }
        StartCoroutine(EndRace());
    }
    public IEnumerator AltStageB()
    {
        float readTime = 3f;
        BigSign(readTime, "Round 3: Jeer your rivals!");
        yield return new WaitForSeconds(readTime + 2f);

        readTime = 5f;
        BigSign(readTime, "Jeer as loud as you can when it's NOT your horse's turn!");
        yield return new WaitForSeconds(readTime + 2f);

        //calc catchups
        float avgx = (horses[0].transform.position.x + horses[1].transform.position.x + horses[2].transform.position.x) / 3f;

        for (int i = 0; i < 3; i++)
        {
            Debug.Log("Cheering Horse " + i);
            //big sign
            BigSign(2f, "Round 3\n" + DecodeHorse((i+1)%3) + " and " + DecodeHorse((i + 2) % 3) + " teams, Jeer!");
            yield return new WaitForSeconds(3.4f);

            //background sign indicates its a go
            Debug.Log("Passed Sign");
            float raceTime = 7f - (avgx - horses[i].transform.position.x) * 1.1f;
            SmallSign(raceTime, "Round 3\n" + DecodeHorse((i + 1) % 3) + " and " + DecodeHorse((i + 2) % 3) + " teams, Jeer!");
            yield return new WaitForSeconds(.4f);

            progressPoints = 0;
            StartCoroutine(accumulateVoice(raceTime));
            yield return new WaitForSeconds(raceTime);

            progressPoints *= -.7f;
            StartCoroutine(BackHorse(progressPoints, i));
            accumPoints[i] += progressPoints;
            yield return new WaitForSeconds(3f);
        }
        StartCoroutine(EndRace());
    }

    public IEnumerator EndRace()
    {
        StartCoroutine(MoveFlag());
        yield return new WaitForSeconds(4f);

        int bestTeam = -1;
        float bestScore = 0;
        for(int i = 0; i < 3; i++)
        {
            if (accumPoints[i] > bestScore)
            {
                bestTeam = i;
                bestScore = accumPoints[i];
            }
        }

        float readTime = 12f;
        BigSign(readTime, DecodeHorse(bestTeam) + " team Won!\nScore: " + bestScore);
        yield return new WaitForSeconds(readTime + 2f);

    }

    public IEnumerator MoveFlag()
    {
        for (float mTIme = 0; mTIme < 10f; mTIme += Time.deltaTime)
        {
            endFlag.transform.position = endFlag.transform.position + Vector3.left * 5f * Time.deltaTime;
            yield return null;
        }
    }

}
