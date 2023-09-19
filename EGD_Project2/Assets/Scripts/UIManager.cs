using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private Animator enterScene;
    private Animator intoScene;
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
        
    }

    //
    public void StartOnClick()
    {
        // Reverse play the animation 

        SceneManager.LoadScene("XRace");
    }
}
