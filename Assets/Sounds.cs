using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public GameObject[] sounds;
    int index;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlaySound(string name)
    {        
        switch (name)
        {
            case "point": index = 0; break;
            case "select": index = 1; break;
            case "destroy": index = 2; break;
            case "dead": index = 3; break;
            case "lvlup": index = 4; break;
            case "click": index = 5; break;
            case "rewind": index = 6; break;
        }
        Instantiate(sounds[index]);
        
    }
}
