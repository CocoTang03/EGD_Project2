using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Animator enterScene;
    private Animator intoScene;

    private bool creditOn = false;
    private bool rankingOn = false;
    // Start is called before the first frame update
    private void Awake()
    {
        enterScene = GameObject.Find("Background").GetComponent<Animator>();
        intoScene = GameObject.Find("Canvas").GetComponent<Animator>();
        
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

        SceneManager.LoadScene("XRace");
    }

    public void CreditsOnClick() 
    {
        GameObject credits = GameObject.Find("Credits");
        credits.GetComponent<Image>().enabled = !credits.GetComponent<Image>().enabled;
        credits.GetComponentInChildren<TextMeshProUGUI>().enabled = !credits.GetComponentInChildren<TextMeshProUGUI>().enabled;
        creditOn = !creditOn;
    }
    public void RankOnClick()
    {
        GameObject credits = GameObject.Find("Ranking");
        credits.GetComponent<Image>().enabled = !credits.GetComponent<Image>().enabled;
        credits.GetComponentInChildren<TextMeshProUGUI>().enabled = !credits.GetComponentInChildren<TextMeshProUGUI>().enabled;
        rankingOn = !rankingOn;
    }
}
