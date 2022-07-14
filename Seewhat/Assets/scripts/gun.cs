using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gun : MonoBehaviour
{
    public player_mover player_mover;
    public gun_values gun_values;
    public powerup powerup;

    [SerializeField] public Text Currentgun;
    [SerializeField] public Text Currentammo;

    public RaycastHit rayhit;

    //bullet holes
    public GameObject bulletholeasset;
    public GameObject muzzleflashasset;

    public float shootValueY=0.0f;
    public float shootValueX=0.0f;
    public float slowfall=0.0f;

    public int damage,magsize,pershot, compensation;
    public float firerate, spread, currentspread, range,reloadtime,shotdelay, minspread,maxspread;
    public bool automatic,shotgun;
    public int ammocount, shotsfired;
    public bool shooting, canshoot,isReloading;

    public float currentfirerate;
    public float currentshotdelay;
    public float currentcompensation;
    public float currentreload;

    // Start is called before the first frame update
   
   void Start()
    {
      canshoot=true;
    Currentgun.text=(string) "Selected gun:Uzi(0)";
        Currentammo.text="Ammo count:" + ammocount;
    }
    public IEnumerator shoot() 
    {
      if (automatic) {
        shooting=Input.GetKey(gun_values.keylabels[0]);
      }
      else {
        shooting=Input.GetKeyDown(gun_values.keylabels[0]);
      }
    if (canshoot && shooting && !isReloading && ammocount >0 && shotgun) {
      canshoot=false;
      yield return StartCoroutine(shotguncode());
      Invoke("resetfire", currentfirerate);
    }
    if (canshoot && shooting && !isReloading && ammocount >0 && !shotgun) {
      canshoot=false;
      if (pershot>1) {

      for (int i =0; i<pershot;i++) {
      yield return StartCoroutine(shootfunc());
      yield return new WaitForSeconds(currentshotdelay);
      }
      }
      else {
        StartCoroutine(shootfunc());
      }
      Invoke("resetfire", currentfirerate);
    }
    if (Input.GetKeyDown(gun_values.keylabels[2]) && ammocount<magsize && isReloading==false ) {
      StartCoroutine(reload());
    }
    if (currentspread>0) {
    currentspread-=slowfall;
    currentspread=Mathf.Clamp(currentspread,minspread,maxspread);
    }
    if (currentspread<0) {
      currentspread=0;
    }
    yield return null;
    }

    public IEnumerator shootfunc() 
    {
      //bloom
      float spreadx=Random.Range(-currentspread,currentspread);
      float spready=Random.Range(-currentspread,currentspread);

      
      currentspread=Mathf.Clamp(currentspread+spread,minspread,maxspread);
      slowfall=currentspread/(compensation);

      Vector3 direction =player_mover.CylinderCamera.transform.forward + new Vector3(spreadx,spready,0);
      //Raycasting
      int layerMask = 1 << 3;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;
      if (Physics.Raycast(player_mover.CylinderCamera.transform.position, direction, out rayhit,range,layerMask,  QueryTriggerInteraction.UseGlobal)) {       
        if (rayhit.collider.CompareTag("Enemy")) {
          rayhit.collider.GetComponentInParent<enemy_behaviour>().TakeDamage(damage);
          rayhit.collider.GetComponentInParent<enemy_behaviour>().ishostile=true;
          rayhit.collider.GetComponentInParent<enemy_behaviour>().target=GameObject.Find("Cylinder");
          foreach(GameObject enem in GameObject.FindGameObjectsWithTag("Enemy")) {
                enem.GetComponentInParent<enemy_behaviour>().ishostile=true;
                 enem.GetComponentInParent<enemy_behaviour>().target=GameObject.Find("Cylinder");
            }
        }    

        else {
                Instantiate(bulletholeasset, rayhit.point, Quaternion.Euler(0,180,0)); 
        }
      }

      //Bullets
      ammocount--;
      Instantiate(muzzleflashasset, gameObject.transform.position+ gameObject.transform.forward, Quaternion.identity); 
      gameObject.GetComponent<AudioSource>().Play();
      Currentammo.text="Ammo count:" + ammocount;
      yield return StartCoroutine(player_mover.MouseLook());
      }

      public IEnumerator shotguncode() 
    {
      for (int i=0;i<pershot; i++) {
      //bloom
      float spreadx=Random.Range(-currentspread,currentspread);
      float spready=Random.Range(-currentspread,currentspread);

      Vector3 direction =player_mover.CylinderCamera.transform.forward + new Vector3(spreadx,spready,0);
      //Raycasting
         int layerMask = 1 << 3;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;
      if (Physics.Raycast(player_mover.CylinderCamera.transform.position, direction, out rayhit,range,layerMask,  QueryTriggerInteraction.UseGlobal)) {       
       if (rayhit.collider.CompareTag("Enemy")) {
          rayhit.collider.GetComponentInParent<enemy_behaviour>().TakeDamage(damage);
          rayhit.collider.GetComponentInParent<enemy_behaviour>().ishostile=true;
          rayhit.collider.GetComponentInParent<enemy_behaviour>().target=GameObject.Find("Cylinder");
          foreach(GameObject enem in GameObject.FindGameObjectsWithTag("Enemy")) {
                enem.GetComponentInParent<enemy_behaviour>().ishostile=true;
                enem.GetComponentInParent<enemy_behaviour>().target=GameObject.Find("Cylinder");
            }
        }
         else {
                Instantiate(bulletholeasset, rayhit.point, Quaternion.Euler(0,180,0)); 
        }   
      }
      }
      currentspread=Mathf.Clamp(currentspread+spread,minspread,maxspread);
      slowfall=currentspread/(currentcompensation);


      ammocount--;
      Instantiate(muzzleflashasset,gameObject.transform.position+ gameObject.transform.forward, Quaternion.identity); 
      gameObject.GetComponent<AudioSource>().Play();
      Currentammo.text="Ammo count:" + ammocount;
      //Bullets      
      yield return StartCoroutine(player_mover.MouseLook());
      }


    public void resetfire() {
      canshoot=true;
    }


    public IEnumerator reload()
    {
      isReloading =true;
      Invoke("ReloadFinished",currentreload);
      Currentammo.text="Ammo count: Reloading...";
      yield return null;
    }
    public void ReloadFinished() {
      ammocount=magsize;
      isReloading=false;
      Currentammo.text="Ammo count:" + ammocount;
    }

    
    void OnApplicationQuit()
    {
      gun_values.pistol.SetActive(true);
      gun_values.revolver.SetActive(true);
      gun_values.shotgunmod.SetActive(true);
    }
}
