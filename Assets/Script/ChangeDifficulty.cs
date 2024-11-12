using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDifficulty : MonoBehaviour
{
    public GameManager gm;
    public TextsOfValues texts;
    public GameObject DifficultiesPanel;
    private Sounds PlaySounds;
    // Start is called before the first frame update

    void Start()
    {
        gm = GetComponent<GameManager>();
        texts = GetComponent<TextsOfValues>();
        PlaySounds = GetComponent<Sounds>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DifficultiesPanel.activeSelf)
        {
            Set_Easy();
            Set_Medium();
            Set_Hard();
        }
      
    }
    private void Set_Easy()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DifficultiesPanel.SetActive(false);
            gm.difficulty = "Easy";
            PlayerPrefs.SetString("difficulty", gm.difficulty);
            texts.UpdateTexts(gm.points, gm.difficulty);

            gm.SwitchDifficulties(gm.difficulty);

            PlaySounds.PlaySound("select");
        }
    }
    private void Set_Medium()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            DifficultiesPanel.SetActive(false);
            gm.difficulty = "Medium";
            PlayerPrefs.SetString("difficulty", gm.difficulty);
            texts.UpdateTexts(gm.points, gm.difficulty);

            gm.SwitchDifficulties(gm.difficulty);

            PlaySounds.PlaySound("select");
        }
    }
    private void Set_Hard()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            DifficultiesPanel.SetActive(false);
            gm.difficulty = "Hard";
            PlayerPrefs.SetString("difficulty", gm.difficulty);
            texts.UpdateTexts(gm.points, gm.difficulty);

            gm.SwitchDifficulties(gm.difficulty);

            PlaySounds.PlaySound("select");
        }
    }
   
    
}
