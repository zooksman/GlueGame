using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    public GameObject Health;
    public Text health;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HealthChange()
    {
        //Who goes to dinner with a compiler error??
        health = Health.GetComponent<UnityEngine.UI.Text>(); 
        //changeHealth.text.GetComponent<Text>().text = "health";
    }
}
