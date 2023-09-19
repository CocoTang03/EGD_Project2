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
        myRenderer.sprite = spriteOptions[(int)(spriteOptions.Length * proportionOfMax)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
