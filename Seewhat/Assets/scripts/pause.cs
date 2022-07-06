using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pause : MonoBehaviour
{
    public player_mover player_mover;
    public gun_values gun_values;

     GameObject playerUI;
    GameObject pause1;
    GameObject pause2;
     GameObject pausecover;
    Dropdown dropdown1;

    public bool ispaused=false;
    // Start is called before the first frame update
    void Start()
    {
        playerUI=GameObject.Find("playerUI");
        string[] keycodes= new string[] {"Backspace","Delete","Tab","Return","Escape","Space","UpArrow","DownArrow","RightArrow","LeftArrow",
        "LeftShift","Mouse0","Mouse1","Alpha0","Alpha1","Alpha2","Alpha3","Alpha4","Alpha5","Alpha6","Alpha7","Alpha8","Alpha9"
        ,"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};
        dropdown1=GameObject.Find("dropdown1").transform.GetComponent<Dropdown>();
        dropdown1.options.Clear();
        List<string> keyoptions=new List<string>();
        foreach (string key in keycodes) 
        {
            dropdown1.options.Add(new Dropdown.OptionData() {text=key});
        }
        dropdown1.onValueChanged.AddListener(delegate {Keyitemchange(dropdown1);});

        pause1=GameObject.Find("pause1");
        pause2=GameObject.Find("pause2");
        pausecover=GameObject.Find("pausecover");
        pause1.SetActive(false);
        pause2.SetActive(false);
        pausecover.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
     if(Input.GetKeyDown(gun_values.pausekey)) {
            pausemenu();
        }   
    }
    void pausemenu() {
        if (ispaused) {
            ispaused=false;
            player_mover.lockCursor=true;
            cursorlock();
            pause1.SetActive(false);
            pausecover.SetActive(false);
            playerUI.SetActive(true);
        }
        else {
            ispaused=true;
            player_mover.lockCursor=false;
            cursorlock();
            pause1.SetActive(true);
            pausecover.SetActive(true);
            playerUI.SetActive(false);
        }
    }
    void cursorlock() {
        if(player_mover.lockCursor) {
            Cursor.lockState=CursorLockMode.Locked;
            Cursor.visible=false;
        }
        else {
            Cursor.lockState=CursorLockMode.None;
            Cursor.visible=true;
        }
    }
    public void quit() {
        SceneManager.LoadSceneAsync("Titlescreen");
    }
    public void settingsmenu() {
        pause2.SetActive(true);
        pause1.SetActive(false);
    }
    public void backtopause() {
        pause2.SetActive(false);
        pause1.SetActive(true);
    }
    //only changes fire key currently
    void Keyitemchange(Dropdown dropdown) {
        Debug.Log(dropdown.options[dropdown.value].text);
        gun_values.fire= (KeyCode) System.Enum.Parse(typeof(KeyCode), dropdown.options[dropdown.value].text);
    } 
}
