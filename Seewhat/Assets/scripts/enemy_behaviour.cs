using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_behaviour : MonoBehaviour
{
    public player_mover player_mover;
    GameObject player;
    float health;
    bool alive;
    float respawnTime;
    GameObject currentData;
    Vector3 spawnLocation;
    System.Timers.Timer enemyfirerate;
    int timingtime=0;

    public bool ishostile=false;
    public GameObject target;

    public RaycastHit rayhit;

    //bullet holes
    public GameObject bulletholeasset;
     public GameObject muzzleflashasset;

    // Start is called before the first frame update
    void Start()
    {
        player=GameObject.Find("Cylinder");
        health=100.0f;
        alive=true;
        enemyfirerate = new System.Timers.Timer(2000);
        enemyfirerate.Elapsed += updatetime;
        enemyfirerate.AutoReset = true;
        enemyfirerate.Enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        enemyshoot();
    }

    public void TakeDamage(float damage) {
        if (alive) {
            health-=damage;
            currentData=gameObject;
            //spawnLocation=currentData.transform.position;
            spawnLocation= new Vector3(Random.Range(-47f,47f),7.18f,Random.Range(-30f,30f));
            Debug.Log(health);
            if (health<=0) {
                health=0;
                death();
            }
        }
    }
    void death() {
        alive=false;
        ishostile=false;
        gameObject.SetActive(false);
        Invoke("respawn",respawnTime);
    }
    void respawn() {
        health=100.0f;
        gameObject.transform.position=(spawnLocation);
        gameObject.SetActive(true);
        alive=true;
    }
    public void enemyshoot() {
        if (ishostile) {
        //relative direction
        Vector3 dir = gameObject.transform.position - target.transform.position;
        //turn into quaternion       
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        //interpolates between current and new rotation
        Vector3 rotation = Quaternion.Lerp(gameObject.transform.rotation, lookRotation, Time.deltaTime * 2).eulerAngles;
        //apply rotate
        gameObject.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        if (timingtime==2000) {
        int layerMask = 1 << 6;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;
        Vector3 shotdirection=target.transform.position-(gameObject.transform.GetChild(0).transform.position+ gameObject.transform.GetChild(0).transform.forward);
        //Debug.DrawRay(gameObject.transform.GetChild(0).transform.position+ gameObject.transform.GetChild(0).transform.forward, shotdirection*1000,Color.red,1000);
        if (Physics.Raycast(gameObject.transform.GetChild(0).transform.position+ gameObject.transform.GetChild(0).transform.forward, shotdirection, out rayhit,1000,layerMask,  QueryTriggerInteraction.UseGlobal)) {
        if (rayhit.collider.CompareTag("Player")) {
            player_mover.playertakeDamage(1);
        }
        else {
          Instantiate(bulletholeasset, rayhit.point, Quaternion.Euler(0,180,0));
        }
      }
      Instantiate(muzzleflashasset,gameObject.transform.GetChild(0).transform.position+ gameObject.transform.GetChild(0).transform.forward, Quaternion.identity);
      timingtime=0;
      }
      if (Mathf.Sqrt((gameObject.transform.position.x-target.transform.position.x)*(gameObject.transform.position.x-target.transform.position.x)+(gameObject.transform.position.z-target.transform.position.z)*(gameObject.transform.position.z-target.transform.position.z))>10) {
        gameObject.transform.position += -transform.forward * Time.deltaTime * 2;
      }
    }
    }
    void OnApplicationQuit()
    {
        enemyfirerate.Dispose();
    }
    void updatetime(object sender, System.EventArgs e) {
        timingtime=2000;
    }
}
