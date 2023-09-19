using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xHorseScript : MonoBehaviour
{
    public float gallopTime;
    public GameObject horse;
    public float frequency;
    public float verticalAmplitude;
    public float rotAmplitude;
    // Start is called before the first frame update
    void Start()
    {
        gallopTime += Random.Range(0, 8);
    }

    // Update is called once per frame
    void Update()
    {
        gallopTime += Time.deltaTime;

        Vector3 stepPos = new Vector3(0, verticalAmplitude * Mathf.Sin(gallopTime * frequency), transform.position.z);

        Vector2 stepDir = new Vector2(rotAmplitude * Mathf.Cos(gallopTime * frequency), 1);

        horse.transform.position = transform.position + stepPos;
        horse.transform.up = stepDir;
    }
}
