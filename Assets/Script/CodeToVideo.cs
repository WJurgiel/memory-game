using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CodeToVideo : MonoBehaviour
{
    public Image img;
    public Sprite spr;
    // Start is called before the first frame update
    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            img.gameObject.SetActive(true);
        }
    }
}
