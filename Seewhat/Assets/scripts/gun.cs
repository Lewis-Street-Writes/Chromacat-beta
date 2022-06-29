using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gun : MonoBehaviour
{
    public player_mover player_mover;
    public gun_values gun_values;
    public powerup powerup;

    public AudioSource currentsound;

    public RaycastHit rayhit;

    //bullet holes
    public GameObject bulletholeasset;
    public GameObject muzzleflashasset;

    public float currentfirerate;
    public float currentshotdelay;
    public float currentrecoil;
    public float currentcompensation;
    public float currentreload;

    // Start is called before the first frame update
   
   void Start()
    {
    
    }
    public IEnumerator shoot() 
    {
      if (gun_values.automatic) {
        gun_values.shooting=Input.GetKey(KeyCode.Mouse0);
      }
      else {
        gun_values.shooting=Input.GetKeyDown(KeyCode.Mouse0);
      }
    if (gun_values.canshoot && gun_values.shooting && !gun_values.isReloading && gun_values.ammocount >0 && gun_values.shotgun) {
      gun_values.canshoot=false;
      yield return StartCoroutine(shotguncode());
      Invoke("resetfire", currentfirerate);
    }
    if (gun_values.canshoot && gun_values.shooting && !gun_values.isReloading && gun_values.ammocount >0 && !gun_values.shotgun) {
      gun_values.canshoot=false;
      if (gun_values.pershot>1) {

      for (int i =0; i<gun_values.pershot;i++) {
      yield return StartCoroutine(shootfunc());
      yield return new WaitForSeconds(currentshotdelay);
      }
      }
      else {
        StartCoroutine(shootfunc());
      }
      Invoke("resetfire", currentfirerate);
    }
    if (Input.GetKeyDown(KeyCode.R) && gun_values.ammocount<gun_values.magsize && gun_values.isReloading==false ) {
      StartCoroutine(reload());
    }
    if (gun_values.shootValueY>0) {
    gun_values.shootValueY-=gun_values.slowfall;
    }
    if (gun_values.shootValueY<0) {
      gun_values.shootValueY=0;
    }
    yield return null;
    }

    public IEnumerator shootfunc() 
    {
      //bloom
      float spreadx=Random.Range(-gun_values.spread,gun_values.spread);
      float spready=Random.Range(-gun_values.spread,gun_values.spread);

      if (gun_values.shootValueY<gun_values.maxrecoil) {
      gun_values.shootValueY+=Mathf.Clamp(currentrecoil,0,gun_values.recoil);
      gun_values.slowfall=gun_values.shootValueY/(currentcompensation);
      }
      Vector3 direction =player_mover.CylinderCamera.transform.forward + new Vector3(spreadx,spready,0);
      //Raycasting
      int layerMask = 1 << 3;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;
      if (Physics.Raycast(player_mover.CylinderCamera.transform.position, direction, out rayhit,gun_values.range,layerMask,  QueryTriggerInteraction.UseGlobal)) {       
        if (rayhit.collider.CompareTag("Enemy")) {
          rayhit.collider.GetComponentInParent<enemy_behaviour>().TakeDamage(gun_values.damage);
          rayhit.collider.GetComponentInParent<enemy_behaviour>().ishostile=true;
          rayhit.collider.GetComponentInParent<enemy_behaviour>().target=GameObject.Find("Cylinder");;
          foreach(GameObject enem in GameObject.FindGameObjectsWithTag("Enemy")) {
                enem.GetComponentInParent<enemy_behaviour>().ishostile=true;
                 enem.GetComponentInParent<enemy_behaviour>().target=GameObject.Find("Cylinder");;
            }
        }    

        else {
                Instantiate(bulletholeasset, rayhit.point, Quaternion.Euler(0,180,0)); 
        }
      }

      //Bullets
      gun_values.ammocount--;
      Instantiate(muzzleflashasset, gameObject.transform.position+ gameObject.transform.forward, Quaternion.identity); 
      currentsound.Play();
      gun_values.Currentammo.text="Ammo count:" + gun_values.ammocount;
      yield return StartCoroutine(player_mover.MouseLook());
      }

      public IEnumerator shotguncode() 
    {
      for (int i=0;i<gun_values.pershot; i++) {
      //bloom
      float spreadx=Random.Range(-gun_values.spread,gun_values.spread);
      float spready=Random.Range(-gun_values.spread,gun_values.spread);

      Vector3 direction =player_mover.CylinderCamera.transform.forward + new Vector3(spreadx,spready,0);
      //Raycasting
         int layerMask = 1 << 3;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;
      if (Physics.Raycast(player_mover.CylinderCamera.transform.position, direction, out rayhit,gun_values.range,layerMask,  QueryTriggerInteraction.UseGlobal)) {       
       if (rayhit.collider.CompareTag("Enemy")) {
          rayhit.collider.GetComponentInParent<enemy_behaviour>().TakeDamage(gun_values.damage);
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
      if (gun_values.shootValueY<gun_values.maxrecoil) {
      gun_values.shootValueY+=Mathf.Clamp(currentrecoil,0,gun_values.recoil);;
      gun_values.slowfall=gun_values.shootValueY/(currentcompensation);
      }
      gun_values.ammocount--;
      Instantiate(muzzleflashasset,gameObject.transform.position+ gameObject.transform.forward, Quaternion.identity); 
      currentsound.Play();
      gun_values.Currentammo.text="Ammo count:" + gun_values.ammocount;
      //Bullets      
      yield return StartCoroutine(player_mover.MouseLook());
      }


    public void resetfire() {
      gun_values.canshoot=true;
    }


    public IEnumerator reload()
    {
      gun_values.isReloading =true;
      Invoke("ReloadFinished",currentreload);
      gun_values.Currentammo.text="Ammo count: Reloading...";
      yield return null;
    }
    public void ReloadFinished() {
      gun_values.ammocount=gun_values.magsize;
      gun_values.isReloading=false;
      gun_values.Currentammo.text="Ammo count:" + gun_values.ammocount;
    }

    
    void OnApplicationQuit()
    {
      gun_values.pistol.SetActive(true);
      gun_values.revolver.SetActive(true);
    }
}
