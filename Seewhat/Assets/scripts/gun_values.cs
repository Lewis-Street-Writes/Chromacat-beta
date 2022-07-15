using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gun_values : MonoBehaviour
{

    public gun gun;
    public player_mover player_mover;
    public powerup powerup;
    public pause pause;

    GameObject basenemy;

    public GameObject revolver;
    public GameObject pistol;
    public GameObject shotgunmod;
    public GameObject current_gun;
    public AudioClip currentaudio;

    int gun_number;

    //                                       fire,          altfire,        reload,     pause,    gun one,        gun two,        gun three,       gun four,       jump
    public KeyCode[] keylabels=new KeyCode[]{KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.R, KeyCode.P, KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Space};

    public bool shooting, canshoot,isReloading;
    // Start is called before the first frame update
    void Start()
    {
        pistol=GameObject.Find("pistol_model");
        revolver=GameObject.Find("revolver_model");
        shotgunmod=GameObject.Find("shotgun_model");
        if(revolver.activeSelf==true) {
        revolver.SetActive(false);
        }
        if(shotgunmod.activeSelf==true) {
        shotgunmod.SetActive(false);
        }
        current_gun=pistol;
        gun=current_gun.GetComponent<gun>();
        player_mover.gun=current_gun.GetComponent<gun>();
        gun_number=0;
        gun.canshoot=true;
    gun.Currentgun.text=(string) "Selected gun:Uzi(0)";
        gun.Currentammo.text="Ammo count:" + gun.ammocount;


        basenemy=GameObject.Find("simpleenemy");
        for (int i=-50;i<50;i+=10) {
        Instantiate(basenemy,new Vector3(i,7.18f,i), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    { 
      if (pause.ispaused==false) {
          if (Input.GetKeyDown(keylabels[4]) || Input.GetKeyDown(keylabels[5]) || Input.GetKeyDown(keylabels[6]) || Input.GetKey(keylabels[7])) {
        StartCoroutine(guns());}
      }
    }
    public IEnumerator guns() {
      //Uzi
        if (current_gun.activeSelf==true) {
            current_gun.SetActive(false);
        }
      if (Input.GetKeyDown(keylabels[4])) {
        gun_number=0;
      }
      //Pistol
      if (Input.GetKeyDown(keylabels[5])) {
        gun_number=1;
      }
      //Assault rifle
      if (Input.GetKeyDown(keylabels[6])) {
       //gun_number=2;
      }
      //Shotgun
      if (Input.GetKeyDown(keylabels[7])) {
       gun_number=2;
      }
     
        gun.Currentgun.text=GameObject.Find("Camera").transform.GetChild(gun_number).gameObject.name;
        current_gun=GameObject.Find("Camera").transform.GetChild(gun_number).gameObject;
        

      current_gun.SetActive(true);
      player_mover.CylinderCube=current_gun.transform;
      player_mover.gun=current_gun.GetComponent<gun>();
      gun=current_gun.GetComponent<gun>();


      gun.currentfirerate=gun.firerate;
      gun.currentshotdelay=gun.shotdelay/10-(powerup.totalheat/100);
      gun.currentcompensation=gun.compensation+(powerup.totalheat/50);
      gun.currentreload=gun.reloadtime-(powerup.totalheat/80);
      

      gun.currentspread=Mathf.Clamp(gun.currentspread,gun.minspread,gun.maxspread);

      gun.canshoot=true;

      StartCoroutine(gun.reload());
      yield return null;
    }
}
