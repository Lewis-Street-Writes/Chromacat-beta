using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gun_values : MonoBehaviour
{
    [SerializeField] public Text Currentgun;
    [SerializeField] public Text Currentammo;

    public gun gun;
    public player_mover player_mover;
    public powerup powerup;

    GameObject basenemy;

    public GameObject revolver;
    public GameObject pistol;
    public GameObject current_gun;
    public AudioClip currentaudio;

    public float shootValueY=0.0f;
    public float shootValueX=0.0f;
    public float slowfall=0.0f;
    
    public int damage,magsize,pershot, compensation;
    public float firerate, spread, currentspread, range,reloadtime,shotdelay, minspread,maxspread;
    public bool automatic,shotgun;
    public int ammocount, shotsfired;

    public KeyCode fire,altfire,reload,pause,gun_one,gun_two,gun_three,gun_four;
    public KeyCode jumpKey;

    public bool shooting, canshoot,isReloading;
    // Start is called before the first frame update
    void Start()
    {
        pistol=GameObject.Find("pistol_model");
        revolver=GameObject.Find("revolver_model");
        if(revolver.activeSelf==true) {
        revolver.SetActive(false);
        }
        current_gun=pistol;
        gun=current_gun.GetComponent<gun>();
        ammocount=magsize;
        canshoot=true;
        Currentgun.text="Selected gun:Uzi(0)";
        gun.currentsound=gun.gameObject.GetComponent<AudioSource>();
        gun.currentsound.clip=Resources.Load<AudioClip>("Sounds/uzisound");
        magsize=30;
        ammocount=magsize;
        Currentammo.text="Ammo count:" + ammocount;
        canshoot=true;
        damage=2;
        pershot=1;
        firerate=0.1f;
        range=1000000.0f;
        reloadtime=1.3f;
        shotdelay=0.0f;
        spread=0.02f;
        minspread=0.0f;
        maxspread=0.1f;
        compensation=7;
        automatic=true;
        shotgun=false;

        gun.currentfirerate=firerate;//-(powerup.totalheat/100);
      gun.currentshotdelay=shotdelay/10-(powerup.totalheat/100);
      gun.currentcompensation=compensation+(powerup.totalheat/50);
      gun.currentreload=reloadtime-(powerup.totalheat/80);

        currentspread=Mathf.Clamp(currentspread,minspread,maxspread);

        basenemy=GameObject.Find("simpleenemy");
        for (int i=-50;i<50;i+=10) {
        Instantiate(basenemy,new Vector3(i,7.18f,i), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    { 
      if (player_mover.ispaused==false) {
          if (Input.GetKeyDown(gun_one) || Input.GetKeyDown(gun_two) || Input.GetKeyDown(gun_three) || Input.GetKeyDown(gun_four)) {
        StartCoroutine(guns());}
      }
    }
    public IEnumerator guns() {
      //Uzi
        if (current_gun.activeSelf==true) {
            current_gun.SetActive(false);
        }
      if (Input.GetKeyDown(gun_one)) {
        damage=2;
        magsize=30;
        pershot=1;
        firerate=0.1f;
        range=1000.0f;
        reloadtime=1.3f;
        shotdelay=0.0f;
        spread=0.02f;
        minspread=0.0f;
        maxspread=0.1f;
        compensation=7;
        automatic=true;
        shotgun=false;
        Currentgun.text="Selected gun:Uzi(0)";
        current_gun=pistol;
        currentaudio=Resources.Load<AudioClip>("Sounds/uzisound");
      }
      //Pistol
      if (Input.GetKeyDown(gun_two)) {
        damage=5;
        magsize=10;
        pershot=1;
        firerate=0.2f;
        range=1000.0f;
        reloadtime=1.0f;
        shotdelay=0.0f;
        spread=0.02f;
        minspread=0.0f;
        maxspread=0.1f;
        compensation=9;
        automatic=false;
        shotgun=false;
        Currentgun.text="Selected gun:Pistol(1)";
        current_gun=revolver;
        currentaudio=Resources.Load<AudioClip>("Sounds/pistolsound");
      }
      //Assault rifle
      if (Input.GetKeyDown(gun_three)) {
        damage=9;
        magsize=30;
        pershot=1;
        firerate=0.4f;
        range=1000.0f;
        reloadtime=2.5f;
        shotdelay=0f;
        spread=0.03f;
        minspread=0f;
        maxspread=0.15f;
        compensation=9;
        automatic=true;
        shotgun=false;
        Currentgun.text="Selected gun:Assault rifle(2)";
        current_gun=revolver;
        currentaudio=Resources.Load<AudioClip>("Sounds/uzisound");
      }
      //Shotgun
      if (Input.GetKeyDown(gun_four)) {
        damage=4;
        magsize=6;
        pershot=5;
        firerate=1.5f;
        spread=0.1f;
        range=100.0f;
        reloadtime=2.7f;
        shotdelay=0f;
        spread=1f;
        minspread=0.2f;
       maxspread =0.2f;
        compensation=12;
        automatic=false;
        shotgun=true;
        Currentgun.text="Selected gun:Shotgun(3)";
        current_gun=revolver;
        currentaudio=Resources.Load<AudioClip>("Sounds/shotgunsound");
      }
      current_gun.SetActive(true);
      player_mover.CylinderCube=current_gun.transform;
      player_mover.gun=current_gun.GetComponent<gun>();
      gun=current_gun.GetComponent<gun>();
      gun.currentsound=gun.gameObject.GetComponent<AudioSource>();
      gun.currentsound.clip=currentaudio;

      gun.currentfirerate=firerate;//-(powerup.totalheat/100);
      gun.currentshotdelay=shotdelay/10-(powerup.totalheat/100);
      gun.currentcompensation=compensation+(powerup.totalheat/50);
      gun.currentreload=reloadtime-(powerup.totalheat/80);

      currentspread=Mathf.Clamp(currentspread,minspread,maxspread);

      StartCoroutine(gun.reload());
      yield return null;
    }
}
