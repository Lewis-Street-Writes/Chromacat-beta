using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class menuscreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(GameObject.Find("testground").GetComponent<Button>());
        var levelist=GameObject.FindGameObjectsWithTag("level");
        foreach (GameObject level in levelist)
        {
            Debug.Log(level);
            level.AddComponent(typeof(Button));
            level.GetComponent<Button>().onClick.AddListener(delegate {movetolevel(level.name); });
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void movetotest() {

    }
    public void movetolevel(string level) {
        SceneManager.LoadSceneAsync(level);
    }
}
