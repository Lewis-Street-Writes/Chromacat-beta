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

    public GameObject revolver;
    public GameObject pistol;
    public GameObject current_gun;

    public float shootValueY=0.0f;
    public float shootValueX=0.0f;
    public float slowfall=0.0f;
    
    public int damage,magsize,pershot, compensation;
    public float firerate, spread,range,reloadtime,shotdelay,recoil, maxrecoil;
    public bool automatic,shotgun;
    public int ammocount, shotsfired;

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
        gun=pistol.GetComponent<gun>();
        ammocount=magsize;
        canshoot=true;
        Currentgun.text="Selected gun:Uzi(0)";
        magsize=30;
        ammocount=magsize;
        Currentammo.text="Ammo count:" + ammocount;
        canshoot=true;
        damage=2;
        pershot=1;
        firerate=0.1f;
        spread=0;
        range=1000000.0f;
        reloadtime=1.3f;
        shotdelay=0.0f;
        recoil=1.0f;
        maxrecoil=15.0f;
        compensation=7;
        automatic=true;
        shotgun=false;
    }

    // Update is called once per frame
    void Update()
    {
          if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3)) {
        StartCoroutine(guns());}
    }
    public IEnumerator guns() {
      //Uzi
        if (current_gun.activeSelf==true) {
            current_gun.SetActive(false);
        }
      if (Input.GetKeyDown(KeyCode.Alpha0)) {
        damage=2;
        magsize=30;
        pershot=1;
        firerate=0.1f;
        spread=0;
        range=1000.0f;
        reloadtime=1.3f;
        shotdelay=0.0f;
        recoil=1.0f;
        maxrecoil=15.0f;
        compensation=7;
        automatic=true;
        shotgun=false;
        Currentgun.text="Selected gun:Uzi(0)";
        current_gun=pistol;
        
      }
      //Pistol
      if (Input.GetKeyDown(KeyCode.Alpha1)) {
        damage=5;
        magsize=10;
        pershot=1;
        firerate=0.2f;
        spread=0;
        range=1000.0f;
        reloadtime=1.0f;
        shotdelay=0.0f;
        recoil=3.0f;
        maxrecoil=20.0f;
        compensation=5;
        automatic=false;
        shotgun=false;
        Currentgun.text="Selected gun:Pistol(1)";
        current_gun=revolver;
      }
      //Assault rifle
      if (Input.GetKeyDown(KeyCode.Alpha2)) {
        damage=9;
        magsize=30;
        pershot=1;
        firerate=0.4f;
        spread=0;
        range=1000.0f;
        reloadtime=2.5f;
        shotdelay=0f;
        recoil=2.5f;
        maxrecoil=15.0f;
        compensation=3;
        automatic=true;
        shotgun=false;
        Currentgun.text="Selected gun:Assault rifle(2)";
        current_gun=revolver;
      }
      //Shotgun
      if (Input.GetKeyDown(KeyCode.Alpha3)) {
        damage=4;
        magsize=6;
        pershot=5;
        firerate=1.5f;
        spread=0.1f;
        range=100.0f;
        reloadtime=2.7f;
        shotdelay=0f;
        recoil=15f;
        maxrecoil=20.0f;
        compensation=10;
        automatic=false;
        shotgun=true;
        Currentgun.text="Selected gun:Shotgun(3)";
        current_gun=revolver;
      }
      current_gun.SetActive(true);
      player_mover.CylinderCube=current_gun.transform;
      player_mover.gun=current_gun.GetComponent<gun>();
      gun=current_gun.GetComponent<gun>();
      StartCoroutine(gun.reload());
      yield return null;
    }
}
