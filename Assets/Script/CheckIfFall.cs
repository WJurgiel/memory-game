using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfFall : MonoBehaviour
{
    public GameObject GOgm;
    public LevelSystem lvlSystem;
    private GameManager gm;
    public Animator anim;
    public Sounds PlaySounds;
    // Start is called before the first frame update
    void Start()
    {
        gm = GOgm.GetComponent<GameManager>();
        lvlSystem = FindObjectOfType<LevelSystem>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("black"))
        {
            Debug.Log("DEAD");
            anim.SetBool("isDead", true);
            gm.isDead = true;

            PlaySounds.PlaySound("dead");
        }
        else
        {           
           
                gm.points++;
                GOgm.GetComponent<TextsOfValues>().UpdateTexts(gm.points, gm.difficulty);

            lvlSystem.addExp();

            PlaySounds.PlaySound("point");
           
        }
    }

}
