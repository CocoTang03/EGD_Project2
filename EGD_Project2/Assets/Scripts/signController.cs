using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class signController : MonoBehaviour
{
    public Vector3 offScreenPos;
    public Vector3 onScreenPos;
    public float timeOnScreen;
    public TMP_Text mytext;
    //Text myText;
    //TextMeshProUGui
    public float moveTime = .5f;

    // Start is called before the first frame update
    void Start()
    {
        //myText = textHolder.GetComponent<Text>();
        StartCoroutine(MoveTrack());
    }

    public void SetVals(float seconds, string message)
    {
        timeOnScreen = seconds;
        mytext.text = message;
    }

    public IEnumerator MoveTrack()
    {
        for(float i = 0; i < moveTime; i += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(offScreenPos, onScreenPos, i / moveTime);
            yield return null;
        }

        transform.position = onScreenPos;
        yield return new WaitForSeconds(timeOnScreen);

        for (float i = 0; i < moveTime; i += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(onScreenPos, offScreenPos, i / moveTime);
            yield return null;
        }

        Destroy(this);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
