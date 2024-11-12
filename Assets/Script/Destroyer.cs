using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public GameManager gm;
    private bool y = false;
    private string currentColor; 
    // Start is called before the first frame update
    void Start()
    {
        //currentColor = "orange";
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    void Destroy()
    {
        if(transform.tag != currentColor)
        {
            GetComponent<SpriteRenderer>().color = Color.black;
        }
    }
    void ColorToWhite()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    
}
