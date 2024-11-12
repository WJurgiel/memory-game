using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{    
    public GameObject musicPlayer;
    static bool isPlaying = false;
    // Start is called before the first frame update
    private void Awake()
    {
        musicPlayer = GameObject.Find("Music");
        if(musicPlayer == null)
        {
            musicPlayer = this.gameObject;
            musicPlayer.name = "Music";
            DontDestroyOnLoad(musicPlayer);
        }
        else
        {
            if(this.gameObject.name != "Music")
            {
                Destroy(this.gameObject);
            }
        }
        
       
    }

    private void playMusic()
    {
        
        
    }
}
