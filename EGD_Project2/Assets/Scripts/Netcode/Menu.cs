using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject clientPrefab;
    public GameObject serverPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClientClick()
    {
        DontDestroyOnLoad(Instantiate(clientPrefab).gameObject);
        //SceneManager.LoadScene("TestScene");
    }

    public void OnServerClick()
    {
        DontDestroyOnLoad(Instantiate(serverPrefab).gameObject);
        //SceneManager.LoadScene("TestScene");
    }
    public void OnClientServerClick()
    {
        OnClientClick();
        OnServerClick();
    }
}
