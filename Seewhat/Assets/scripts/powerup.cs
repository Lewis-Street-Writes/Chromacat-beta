using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class powerup : MonoBehaviour
{
    public player_mover player_mover;
    [SerializeField] public Image heatgauge;
    public float totalheat=0.0f;
    // Start is called before the first frame update
    void Start()
    {
        heatgauge.fillAmount = Mathf.Clamp(totalheat/100,0.0f, 1f);
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
    }
    public void decreaseheat() {
        totalheat-=5.0f;
        totalheat=Mathf.Clamp(totalheat,0.0f, 100f);
        heatgauge.fillAmount = Mathf.Clamp(totalheat/100,0.0f, 1f);
        player_mover.music.pitch=0.7f+ Mathf.Clamp(totalheat/250,0.0f, 0.4f);
    }
    
}
