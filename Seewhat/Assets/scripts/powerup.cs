using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class powerup : MonoBehaviour
{
    public player_mover player_mover;
    [SerializeField] public Image heatgauge;
    public float totalheat=0.0f;

    GameObject[] heatedges;
    // Start is called before the first frame update
    void Start()
    {
        player_mover=GameObject.Find("Cylinder").GetComponent<player_mover>();
        heatgauge=GameObject.Find("heatfill").GetComponent<Image>();
        heatgauge.fillAmount = Mathf.Clamp(totalheat/100,0.0f, 1f);
        heatedges=GameObject.FindGameObjectsWithTag("heatedge");
        foreach (GameObject edge in heatedges)
        {
            edge.GetComponent<Image>().color=new Color32(241,170,93,0);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void increaseheat() {
        totalheat+=5.0f;
        totalheat=Mathf.Clamp(totalheat,0.0f, 100f);
        heatgauge.fillAmount = Mathf.Clamp(totalheat/100,0.0f, 1f);
        player_mover.music.pitch=0.7f+ Mathf.Clamp(totalheat/250,0.0f, 0.4f);
        byte currentcolour=(byte) Mathf.Round((255/100)*totalheat);
        foreach (GameObject edge in heatedges)
        {
            edge.GetComponent<Image>().color=new Color32(241,170,93,currentcolour);
            
        }
    }
    public void decreaseheat() {
        totalheat-=5.0f;
        totalheat=Mathf.Clamp(totalheat,0.0f, 100f);
        heatgauge.fillAmount = Mathf.Clamp(totalheat/100,0.0f, 1f);
        player_mover.music.pitch=0.7f+ Mathf.Clamp(totalheat/250,0.0f, 0.4f);
        byte currentcolour=(byte) Mathf.Round((255/100)*totalheat);
        foreach (GameObject edge in heatedges)
        {
            edge.GetComponent<Image>().color=new Color32(241,170,93,currentcolour);
        }
    }
    public void refillammo(Collider sprit) {
        player_mover.gun.ammocount=player_mover.gun.magsize;
        player_mover.gun.reserveammo=player_mover.gun.magsize*5;
        player_mover.gun.Currentammo.text="Ammo count:" + player_mover.gun.ammocount + "/" + player_mover.gun.reserveammo;
        Destroy(sprit.gameObject);
    }
}