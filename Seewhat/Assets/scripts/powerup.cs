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
        heatgauge.fillAmount = Mathf.Clamp(totalheat/100,0.0f, 1f);
        heatedges=GameObject.FindGameObjectsWithTag("heatedge");
        foreach (GameObject edge in heatedges)
        {
            edge.GetComponent<RectTransform>().localScale=new Vector3(0,edge.GetComponent<RectTransform>().localScale.y,0);
            
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
        foreach (GameObject edge in heatedges)
        {
            edge.GetComponent<RectTransform>().localScale=new Vector3(0f+(totalheat*0.00182f),edge.GetComponent<RectTransform>().localScale.y,0);
            
        }
    }
    public void decreaseheat() {
        totalheat-=5.0f;
        totalheat=Mathf.Clamp(totalheat,0.0f, 100f);
        heatgauge.fillAmount = Mathf.Clamp(totalheat/100,0.0f, 1f);
        player_mover.music.pitch=0.7f+ Mathf.Clamp(totalheat/250,0.0f, 0.4f);
        foreach (GameObject edge in heatedges)
        {
            edge.GetComponent<RectTransform>().localScale=new Vector3(0f+(totalheat*0.00182f),edge.GetComponent<RectTransform>().localScale.y,0);
            
        }
    }
}