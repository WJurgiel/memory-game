using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices.ComTypes;
using System.Reflection;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public GameObject player;
    public TextsOfValues ValuesTexts;
    public ChangeDifficulty ChangeDifficulties;
    public GameObject DisolveScreen;
    public LevelSystem lvlSystem;
    private Sounds PlaySounds;
    [Header("Game Essentials")]
    public Color[] colors = {Color.blue, Color.green, Color.gray, Color.cyan, Color.white, Color.magenta, Color.red, Color.yellow,
    new Color(212,212,212), new Color(0,104,0), new Color(255, 139, 0), new Color(115,46,115)};
    private Color defaultColor, defaultColor2;
    // 192,192,192 - silver --- 0,104,0 - dark green ----- 255,139,0 - orange ----- 128,0,128 - purple
    public bool[] alreadyUsed = new bool[12];
    public SpriteRenderer[] tiles = new SpriteRenderer[12];
    private string[] tags = {"blue", "green", "gray", "cyan", "white", "magenta", "red", "yellow", "silver", "dark green", "orange", "purple"};
    private string[] basicTags = { "white", "black" };

    public TextMeshProUGUI textColor;
    [Header("Game values")]    
    public bool timeOut = false;
    public string currentColor;
    public bool isDead;
    public bool isStarted;
    public bool deletingProgress;

    [Header("Timers")]
    public float timeForMemorize = 4f;
    public float timeForSearching = 4f;
    public float timeMultiplier = 1f;
    public float timeBreak = 1.5f; //break between states
    float Mem_currentTimer;
    float Sear_currentTimer;
    public float Break_currentTimer;
    
    [Header("States")]
    public string str_StateOfGame;
    private string str_previousState;

    [Header("Main values")]
    public string titleOfGame = "Colors";
    public int points;
    public int highscore;
    public string difficulty;

    // Start is called before the first frame update
    private void Awake()
    {
        switch (PlayerPrefs.GetInt("firstTime")){
            case 0: DisolveScreen.SetActive(true); break;
            case 1: DisolveScreen.SetActive(false); break;                
        }
        
    }
    void Start()
    {
        lvlSystem = GetComponent<LevelSystem>();
        ValuesTexts = GetComponent<TextsOfValues>();
        ChangeDifficulties = GetComponent<ChangeDifficulty>();
        PlaySounds = GetComponent<Sounds>();
        /////Main Texts/////
        defaultColor = textColor.color;
        defaultColor2 = ValuesTexts.TMP_pressToStart.color;
        textColor.text = titleOfGame;       
        ValuesTexts.StateOfPressToStart(0);
        /////Main Texts/////
        
        isDead = false;
        isStarted = false;
        deletingProgress = false;
        ColorToWhite();
        enablingTrigger(0);

       
        //////Values///////
        points = 0;
        //difficulty = "Medium";
        difficulty = PlayerPrefs.GetString("difficulty");
        PlayerPrefs.SetString("difficulty", difficulty);
        highscore = PlayerPrefs.GetInt("Highscore");
        PlayerPrefs.SetInt("firstTime", 1);
        ValuesTexts.UpdateTexts(points, difficulty);
        /////Values////////
        SwitchDifficulties(difficulty);
        str_StateOfGame = "Memorizing";
        str_previousState = str_StateOfGame;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(PlayerPrefs.GetFloat("CurrentExp"));
            Debug.Log(lvlSystem.levelSlider.maxValue);
        }
        ////////Poruszanie sie po menu//////////
        if((!isStarted || isDead)) 
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (!ChangeDifficulties.DifficultiesPanel.activeSelf)
                {
                    PlaySounds.PlaySound("click");
                }

                ChangeDifficulties.DifficultiesPanel.SetActive(true);
            }
            
                     
        }
        if (!ChangeDifficulties.DifficultiesPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Quiting");
                PlayerPrefs.SetInt("firstTime", 0);
                Debug.Log(PlayerPrefs.GetInt("firstTime"));
                Application.Quit();
            }
        }
        ////////Poruszanie sie po menu/////////
        ///Logika Gry////
        if (isStarted && !deletingProgress)
        {
            if (!isDead)
            {
                BoostTimeMultiplier();
                if (str_StateOfGame == "Memorizing")
                {
                    if (Mem_currentTimer <= 0f)
                    {
                        ColorToWhite();
                        SetText();
                        UpdateCurrentColor();

                        ResetPressToStart("", defaultColor2);

                        Mem_currentTimer = timeForMemorize; // reset timer to avoid a bug

                        str_previousState = str_StateOfGame;
                        str_StateOfGame = "Quick Break";                      
                    }
                    else
                    {
                        Mem_currentTimer -= Time.deltaTime * timeMultiplier;
                        textColor.text = "MEMORIZE";
                        UpdatePressToStartCounting(Mem_currentTimer);
                        if(textColor.color != defaultColor)
                        {
                            textColor.color = defaultColor;
                        }
                    }
                }
                if (str_StateOfGame == "Searching")
                {
                    if (Sear_currentTimer <= 0f)
                    {
                        Destroy();
                        ResetText();

                        ResetPressToStart("", defaultColor2);

                        Sear_currentTimer = timeForSearching;

                        str_previousState = str_StateOfGame;
                        str_StateOfGame = "Quick Break";
                       
                    }
                    else
                    {
                        Sear_currentTimer -= Time.deltaTime * timeMultiplier;
                        UpdatePressToStartCounting(Sear_currentTimer);
                    }
                }
                if (str_StateOfGame == "Quick Break")
                {
                    if (Break_currentTimer <= 0f)
                    {
                        switch (str_previousState)
                        {
                            case "Memorizing":
                                str_StateOfGame = "Searching";
                                break;
                            case "Searching":
                                str_StateOfGame = "Memorizing";
                                
                                SetColors();

                                enablingTrigger(0);
                                break;
                        }                        
                        Break_currentTimer = timeBreak;                        
                    }
                    else
                    {
                        if (!isDead)
                        {
                            Break_currentTimer -= Time.deltaTime;
                        }
                    }
                }
            }
            else
            {
                GameOver();
                return;
            }
        }
        else
        {
            StartingGame();
        }
        ///Logika gry///
        
    }
    private void ResetBools()
    {
        for (int j = 0; j < alreadyUsed.Length; j++)
        {
            alreadyUsed[j] = false;
        }
    }
    /*
        CO DO ZMIANY:
        *łatwy poziom - 3 kolory na 12 pól, po X rundach 4 kolory na 12 pól, mało expa
        *średni poziom - 6 kolorów na 12 pól, normalny exp, streak bonus
        *trudny poziom - 12 kolorów na 12 pól, extra exp, streak bonus
        *streak bonus - visual na ekranie (3x combo!, 5x combo! itd), 
        *achievements: Grinder (Reach lvl 100), Too_Easy (get 20x combo on level hard), Holy (get Revived) --> achievementy na sam koniec jak się bedzie chciało
        *
     */
    private void SetColors()
    {
        ResetBools();
        for(int i = 0; i < colors.Length; i++)
        {
            int r = Random.Range(0, 12);
            while (alreadyUsed[r])
            {
                r = Random.Range(0, 12);
            }
            tiles[i].color = colors[r];
            tiles[i].transform.tag = tags[r];
            alreadyUsed[r] = true;

        }
    }
    private void SetText()
    {
        int var = Random.Range(0, tags.Length);
        textColor.text = tags[var];
        if (points < 4)
        {
            textColor.color = colors[var];
        }
        if(points >= 4 && difficulty == "Hard")
        {
            int r = Random.Range(0, tags.Length);
            while(r == var)
            {
                r = Random.Range(0, tags.Length);
            }
            textColor.color = colors[r];
        }
    }
    private void ResetText()
    {
        textColor.text = "";
    }
    public void UpdateCurrentColor()
    {
        currentColor = textColor.text;
    }
    public void Destroy()
    {
        foreach(SpriteRenderer spr in tiles)
        {
            if(spr.transform.tag != currentColor)
            {
                spr.color = Color.black;
                spr.transform.tag = basicTags[1];
            }
            else
            {
                spr.color = textColor.color;
            }
        }
        enablingTrigger(1);
        //isDead = player.transform.GetChild(0).GetComponent<CheckIfFall>().isDead;
    }
    private void enablingTrigger(int state)
    {
        switch (state)
        {
            case 0: player.transform.GetChild(0).gameObject.SetActive(false); break;
            case 1: player.transform.GetChild(0).gameObject.SetActive(true); break;
        }
    }
    public void ColorToWhite()
    {
        foreach(SpriteRenderer spr in tiles)
        {
            spr.color = Color.white;            
        }
    }
    public void GameOver()
    {        

        textColor.text = "Game Over";
        textColor.color = Color.red;
        ValuesTexts.StateOfPressToStart(2);
        if(points > PlayerPrefs.GetInt("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", points);
        }
       
        StartingGame();
    }
    private void StartingGame()
    {        
        if (Input.GetKeyDown(KeyCode.Space) && !ChangeDifficulties.DifficultiesPanel.activeSelf)
        {
            if (isDead)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                if (PlayerPrefs.GetString("difficulty") != "") ///zmiana
                {
                    ValuesTexts.StateOfPressToStart(1);
                    isStarted = true;
                    isDead = false;
                    SetColors();
                }
                else Debug.Log("WYBIERZ POZIOM TRUDNOSCI");
            }          

        }
    }
    private void CountingToZero(float timer)
    {
        textColor.text = timer.ToString("0");
    }
    public void SwitchDifficulties(string difficulty)
    {
        switch (difficulty)
        {
            case "Easy":
                //You can skip time in Easy mode
                timeForMemorize = 25;
                timeForSearching= 10;
                timeBreak = 3;
                break;
            case "Medium":
                timeForMemorize= 20;
                timeForSearching= 10;
                timeBreak = 3;
                break;
            case "Hard":
                timeForMemorize = 15;
                timeForSearching = 5;
                timeBreak = 3;
                break;
        }

        Mem_currentTimer = timeForMemorize;
        Sear_currentTimer = timeForSearching;
        Break_currentTimer = timeBreak;

        lvlSystem.updateBonus(this);
        //if (ValuesTexts.TMP_pressToStart.text == "Select difficulty to start") ValuesTexts.StateOfPressToStart(1);

    }
    private void UpdatePressToStartCounting(float timer)
    {
        ValuesTexts.TMP_pressToStart.text = timer.ToString("0");
        if(timer < 5f) { ValuesTexts.TMP_pressToStart.color = Color.red; }
    }
    private void ResetPressToStart(string value, Color color)
    {
        ValuesTexts.TMP_pressToStart.text = value;
        ValuesTexts.TMP_pressToStart.color = color;
    }
    private void BoostTimeMultiplier()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            PlaySounds.PlaySound("rewind");
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            timeMultiplier = 2f;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            timeMultiplier = 1f;
        }
    }
 
}
