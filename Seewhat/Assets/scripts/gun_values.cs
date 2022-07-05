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

    object[,] gun_stats;
    int gun_number;

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
      gun_stats=new object[,] {
        //damage,magsize,pershot(burst+shotgun),firerate,range,reloadtime,shotdelay,spread,minspread,maxspread,compensation,automatic,shotgun,text,weapon type, sound
      {6, 30, 1, 0.1f, 1000f, 1.3f, 0.0f, 0.02f, 0.0f, 0.1f,  7,  true,  false, "Selected gun:Uzi(0)",           pistol,   Resources.Load<AudioClip>("Sounds/uzisound")},
      {25, 10, 1, 0.2f, 1000f, 1.0f, 0.0f, 0.02f, 0.0f, 0.1f,  9,  false, false, "Selected gun:Pistol(1)",        revolver, Resources.Load<AudioClip>("Sounds/pistolsound")},
      {10, 30, 1, 0.4f, 1000f, 2.5f, 0.0f, 0.03f, 0.0f, 0.15f, 9,  true,  false, "Selected gun:Assault rifle(2)", revolver, Resources.Load<AudioClip>("Sounds/uzisound")},
      {8, 6,  5, 1.5f, 1000f, 2.7f, 0.0f, 1f   , 0.2f, 0.2f,  12, false, true,  "Selected gun:Shotgun(3)",       revolver, Resources.Load<AudioClip>("Sounds/shotgunsound")}};
        pistol=GameObject.Find("pistol_model");
        revolver=GameObject.Find("revolver_model");
        if(revolver.activeSelf==true) {
        revolver.SetActive(false);
        }
        current_gun=pistol;
        gun=current_gun.GetComponent<gun>();
        gun_number=0;
        canshoot=true;


        damage=(int) gun_stats[gun_number,0];
        magsize=(int) gun_stats[gun_number,1];
        ammocount=magsize;
        pershot=(int) gun_stats[gun_number,2];
        firerate=(float) gun_stats[gun_number,3];
        range=(float) gun_stats[gun_number,4];
        reloadtime=(float) gun_stats[gun_number,5];
        shotdelay=(float) gun_stats[gun_number,6];
        spread=(float) gun_stats[gun_number,7];
        minspread=(float) gun_stats[gun_number,8];
        maxspread=(float) gun_stats[gun_number,9];
        compensation=(int) gun_stats[gun_number,10];
        automatic=(bool) gun_stats[gun_number,11];
        shotgun=(bool) gun_stats[gun_number,12];

        Currentgun.text=(string) gun_stats[gun_number,13];
        Currentammo.text="Ammo count:" + ammocount;

        current_gun=(GameObject)gun_stats[gun_number,14];
        currentaudio=(AudioClip) gun_stats[gun_number,15];

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
        gun_number=0;
      }
      //Pistol
      if (Input.GetKeyDown(gun_two)) {
        gun_number=1;
      }
      //Assault rifle
      if (Input.GetKeyDown(gun_three)) {
       gun_number=2;
      }
      //Shotgun
      if (Input.GetKeyDown(gun_four)) {
       gun_number=3;
      }
         damage=(int) gun_stats[gun_number,0];
        magsize=(int) gun_stats[gun_number,1];
        pershot=(int) gun_stats[gun_number,2];
        firerate=(float) gun_stats[gun_number,3];
        range=(float) gun_stats[gun_number,4];
        reloadtime=(float) gun_stats[gun_number,5];
        shotdelay=(float) gun_stats[gun_number,6];
        spread=(float) gun_stats[gun_number,7];
        minspread=(float) gun_stats[gun_number,8];
        maxspread=(float) gun_stats[gun_number,9];
        compensation=(int) gun_stats[gun_number,10];
        automatic=(bool) gun_stats[gun_number,11];
        shotgun=(bool) gun_stats[gun_number,12];
        Currentgun.text=(string) gun_stats[gun_number,13];
        current_gun=(GameObject)gun_stats[gun_number,14];
        currentaudio=(AudioClip) gun_stats[gun_number,15];

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
