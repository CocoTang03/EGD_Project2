using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioDisplayerController : MonoBehaviour
{
    public SpriteRenderer myRenderer;
    public Sprite[] spriteOptions;

    // Start is called before the first frame update
    void Start()
    {
        myRenderer = gameObject.GetComponent<SpriteRenderer>();
        myRenderer.sprite = spriteOptions[0];
    }

    public void setLoudness(float proportionOfMax)
    {
        int index = (int) ((float) spriteOptions.Length * proportionOfMax);
        if (index >= spriteOptions.Length)
        {
            Debug.LogWarning("index too high at " + index + "/" + spriteOptions.Length);
            index = spriteOptions.Length - 1;
        }
        myRenderer.sprite = spriteOptions[index];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
