using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextsOfValues : MonoBehaviour
{
    private GameManager gm;
    private LevelSystem lvl; 
    public TextMeshProUGUI TMP_pressToStart;

    public TextMeshProUGUI TMP_points;
    public TextMeshProUGUI TMP_highscore;
    public TextMeshProUGUI TMP_difficulty;
    public TextMeshProUGUI TMP_Rank;
    public TextMeshProUGUI TMP_addedScore;
    
    public float deleteTIme = 3f;
    private float currentTime;
    // Start is called before the first frame update
    private void Awake()
    {
       
    }
    void Start()
    {
        gm = GetComponent<GameManager>();
        lvl = GetComponent<LevelSystem>();
        currentTime = deleteTIme;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            ResetPlayerPrefs();
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            currentTime = deleteTIme;
            Debug.Log("Current time: " + currentTime);
        }
    }
    public void UpdateTexts(int points, string difficulty)
    {
        TMP_points.text = "Points: " + points.ToString();
        TMP_highscore.text = "Hi: " + PlayerPrefs.GetInt("Highscore").ToString();
        TMP_difficulty.text = "Dif: " + difficulty;        
    }
    public void StateOfPressToStart(int state)
    {
        switch (state)
        {
            case 0: TMP_pressToStart.text = "Press <<Space>> to start"; break;
            case 1: TMP_pressToStart.text = ""; break;
            case 2: TMP_pressToStart.text = "Press <<Space>> to restart"; break;
            case 3: TMP_pressToStart.text = "Reopen  app  to  apply  changes"; TMP_pressToStart.color = Color.red; break;
            case 4: TMP_pressToStart.text = "Select difficulty to start";break;
        }
    }
    private void ResetPlayerPrefs()
    {
        if(currentTime <= 0f)
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("DELETED");
            StateOfPressToStart(3);
        }
        else
        {
            currentTime -= Time.deltaTime;
        }
    }
    public void ShowHowMuchAdded(float value)
    {
        TMP_addedScore.text = "+" + value.ToString();
        TMP_addedScore.gameObject.SetActive(true);
        Invoke("HideHowMuchAdded", 1f);
    }
    private void HideHowMuchAdded()
    {
        TMP_addedScore.gameObject.SetActive(false);
    }
}
