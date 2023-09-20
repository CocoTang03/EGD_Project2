using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject bg;
    public GameObject cv;
    private Animator enterScene = null;
    private Animator intoScene = null;

    private bool creditOn = false;
    private bool rankingOn = false;
    // Start is called before the first frame update
    private void Awake()
    {
        enterScene = bg.GetComponent<Animator>();
        intoScene = cv.GetComponent<Animator>();
        
    }
    void Start()
    {
        enterScene.Play("EnterScene");
        intoScene.Play("IntoScene");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Escape))
        {
            if(creditOn)
            {
                CreditsOnClick();
            }
            if(rankingOn)
            {
                RankOnClick();
            }
        }
    }

    //
    public void StartOnClick()
    {
        // Reverse play the animation 

        //enterScene.Play("EnterScene");
        //intoScene.Play("exitScene");
        ExampleCoroutine(2f);
        SceneManager.LoadScene("XRace");
    }

    public void CreditsOnClick() 
    {
        if (!creditOn)
        {
            //intoScene.Play("exitScene");
            intoScene.Play("ShowPage");
        }
        else
        {
            intoScene.Play("ExitPage");
            //intoScene.Play("IntoScene");
        }
        GameObject credits = GameObject.Find("Credits");
        //credits.GetComponent<Image>().enabled = !credits.GetComponent<Image>().enabled;
        credits.GetComponent<TextMeshProUGUI>().enabled = !credits.GetComponent<TextMeshProUGUI>().enabled;
        creditOn = !creditOn;
    }
    public void RankOnClick()
    {
        if (!rankingOn)
        {
            //Debug.Log("what");
            //intoScene.Play("exitScene");
            intoScene.Play("ShowPage");
        }
        else
        {
            intoScene.Play("ExitPage");
            ExampleCoroutine(1f);
            //intoScene.Play("IntoScene");
        }
        GameObject ranking = GameObject.Find("Ranking");
        //credits.GetComponent<Image>().enabled = !credits.GetComponent<Image>().enabled;
        ranking.GetComponent<TextMeshProUGUI>().enabled = !ranking.GetComponent<TextMeshProUGUI>().enabled;
        rankingOn = !rankingOn;
    }
    IEnumerator ExampleCoroutine(float time)
    {
            yield return new WaitForSeconds(time);

    }
}
