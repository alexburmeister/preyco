using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public GameObject startUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame


    public void CloseMessage()
    {
        print("soll client löschen");
        string s = "break\n";
        byte[] hit = System.Text.Encoding.ASCII.GetBytes(s);
        main.handler.Send(hit);
    }

    public void RdyMessageToUI()
    {
        print("i am ready");
        string s = SymbolHandler.ownIdentifier + "#\n";
        byte[] hit = System.Text.Encoding.ASCII.GetBytes(s);
        main.handler.Send(hit);
    }

    public void retryClick()
    {
        //SceneManager.LoadScene("Game");
        GameEvents.current.newgenANDsendEventMethodMainThread();
    }

    public void loadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void closeStartUI()
    {
        startUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            closeStartUI();
        }
    }  

}
