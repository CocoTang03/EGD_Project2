using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracklooper : MonoBehaviour
{
    public GameObject[] tracks;
    public float tileWidth;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        tracks[0].transform.position = new Vector2(-tileWidth, 0);
        tracks[1].transform.position = new Vector2(tileWidth, 0);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < tracks.Length; i++)
        {
            Vector2 curpos = tracks[i].transform.position;
            if (curpos.x < tileWidth * -1.5)
            {
                curpos.x += tileWidth * 3;
            }
            curpos.x -= speed * Time.deltaTime;
            tracks[i].transform.position = curpos;
        }
    }
}
