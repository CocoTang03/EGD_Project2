using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : MonoBehaviour
{
    public Animator anim;
    public float tiltTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tiltTime += Time.deltaTime;

        if (tiltTime >= 5)
        {
            Tilt();
            tiltTime = 0;
        }
    }

    public void Tilt()
    {
        anim.SetTrigger("Tilt");
    }
}
