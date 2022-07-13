using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class pause : MonoBehaviour
{
    public player_mover player_mover;
    public gun_values gun_values;

    [SerializeField] GameObject dropdownexample;
     [SerializeField] GameObject textexample;

     GameObject playerUI;
    GameObject pause1;
    GameObject pause2;
     GameObject pausecover;
    Dropdown dropdown1;

    public bool ispaused=false;
    // Start is called before the first frame update
    void Start()
    {
        LoadFile();
        playerUI=GameObject.Find("playerUI");
            droploads();

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
     if(Input.GetKeyDown(gun_values.keylabels[3])) {
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
            pause2.SetActive(false);
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
    public void map() {
        SceneManager.LoadSceneAsync("map");
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
    void Keyitemchange(Dropdown dropdown,int key) {
        var dropdownlist=GameObject.FindGameObjectsWithTag("SettingDropdown");
        foreach (GameObject drop in dropdownlist)
        {
            if (drop.GetComponent<RectTransform>().position==dropdown.GetComponent<RectTransform>().position) {
            }
            else {
                if (drop.transform.GetComponent<Dropdown>().options[drop.transform.GetComponent<Dropdown>().value].text==dropdown.options[dropdown.value].text) {
                    dropdown.value = dropdown.options.FindIndex(option => option.text ==  gun_values.keylabels[key].ToString());
                   return;
                }
            }
        }
        gun_values.keylabels[key]= (KeyCode) System.Enum.Parse(typeof(KeyCode), dropdown.options[dropdown.value].text);
          string destination = "Assets/scripts/settings.dat";
         FileStream file;
 
         if(File.Exists(destination)) file = File.OpenWrite(destination);
         else file = File.Create(destination);
 
         KeyCode[] data=gun_values.keylabels;
         data[key]=(KeyCode) System.Enum.Parse(typeof(KeyCode), dropdown.options[dropdown.value].text);
         BinaryFormatter bf = new BinaryFormatter();
         bf.Serialize(file, data);
         file.Close();
    } 
    public void LoadFile()
     {
         string destination ="Assets/scripts/settings.dat";
         FileStream file;
 
         if(File.Exists(destination)) file = File.OpenRead(destination);
         else
         {
             Debug.LogError("File not found");
             return;
         }
        BinaryFormatter bf = new BinaryFormatter();
        KeyCode[] data = (KeyCode[]) bf.Deserialize(file);
         file.Close();
         gun_values.keylabels=data;
     }
     void droploads() {
        string[] keycodes= new string[] {"Backspace","Delete","Tab","Return","Escape","Space","UpArrow","DownArrow","RightArrow","LeftArrow",
        "LeftShift","Mouse0","Mouse1","Alpha0","Alpha1","Alpha2","Alpha3","Alpha4","Alpha5","Alpha6","Alpha7","Alpha8","Alpha9"
        ,"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};

        
        string[] keyident= new string[] {"Fire","Alternate fire","Reload","Pause","Uzi","Pistol","Assault rifle","Shotgun","Jump"};

        for (int i=0;i<8;i++) {
        GameObject currentdropdown=Instantiate(dropdownexample, new Vector3(100,180-(i*40),0), Quaternion.Euler(0,0,0));
        currentdropdown.transform.SetParent(GameObject.Find("pause2").transform,false);
        Dropdown dropcomponent=currentdropdown.transform.GetComponent<Dropdown>();
        dropcomponent.options.Clear();

        GameObject currenttextfield=Instantiate(textexample, new Vector3(-100,180-(i*40),0), Quaternion.Euler(0,0,0));
        currenttextfield.transform.SetParent(GameObject.Find("pause2").transform,false);
        currenttextfield.GetComponent<Text>().text=keyident[i];
        foreach (string key in keycodes) 
        {
            dropcomponent.options.Add(new Dropdown.OptionData() {text=key});
        }
        dropcomponent.value = dropcomponent.options.FindIndex(option => option.text ==  gun_values.keylabels[i].ToString());
        int tempnum=i;
        Dropdown droptemp=dropcomponent;
        dropcomponent.onValueChanged.AddListener(delegate {Keyitemchange(droptemp,tempnum);});
        }
     }
}
