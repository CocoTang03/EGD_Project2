using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class signController : MonoBehaviour
{
    public Vector3 offScreenPos;
    public Vector3 onScreenPos;
    public float timeOnScreen;
    public GameObject textHolder;
    Text myText;
    public float moveTime = .5f;
    private float downness = 0f;

    // Start is called before the first frame update
    void Start()
    {
        myText = textHolder.GetComponent<Text>();
        MoveDown();
    }

    void Setup(float duration, string message)
    {
        myText.text = message;
        timeOnScreen = duration;
    }

    void MoveDown()
    {
        transform.position = onScreenPos;
        Invoke(nameof(MoveUp), timeOnScreen);
    }
    void MoveUp()
    {
        transform.position = offScreenPos;
        Invoke(nameof(Despawn), moveTime);
    }

    void Despawn()
    {
        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
