using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameManager gm;    
    public Slider levelSlider;
    [SerializeField] TextMeshProUGUI levelText;
    TextsOfValues texts;
    [Header("Values")]
    public float currentExp;
    public int currentLevel;
    public string[] ranks = { "Noob", "Begginer", "Intermediate", "Mega Mind", "Elephant" };
    public Color[] ranksColor = new Color[5];
    [Header("Values to add")]
    public float maxValue;
    public float expToAdd;
    public float expBonus;
    public float expBasic; //20?
    // Start is called before the first frame update
    private void Awake()
    {
        texts = GetComponent<TextsOfValues>();
        
    }
    void Start()
    {      
        if (PlayerPrefs.GetInt("currentLevel") > 0)
        {
            currentLevel = PlayerPrefs.GetInt("currentLevel");
        }
        else
        {
            currentLevel = 1;
            PlayerPrefs.SetInt("currentLevel", currentLevel);
        }
        updateMaxValue();
        loadSliderValue();
        switchRanks();
        levelText.text = "LVL:  " + currentLevel.ToString();       
        
        
        expToAdd = expBasic;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(1)) addExp();
    }
    public void updateBonus(GameManager gm)
    {
        switch (gm.difficulty)
        {
            case "Easy": expBonus = 0f; break;
            case "Medium": expBonus = 10f; break;
            case "Hard": expBonus = 20f; break;
        }
        expToAdd = expBasic + expBonus;
        Debug.Log("bonus: " + expBonus);
    }
    private void updateMaxValue()
    {
        maxValue = currentLevel * 30f;
        levelSlider.maxValue = maxValue;
    }
    public void CheckIfNewLevel()
    {
        if (currentExp >= levelSlider.maxValue)
        {
            Debug.Log(currentExp + "-" + maxValue);
            float toAddAfterNewLevel = (currentExp) - maxValue;
            Debug.Log(toAddAfterNewLevel);
            //
            currentExp = toAddAfterNewLevel;
            //
            levelSlider.value = currentExp;
            currentLevel++;
            PlayerPrefs.SetInt("currentLevel", currentLevel);
            levelText.text = "LVL:  " + currentLevel.ToString();
            updateMaxValue();
            switchRanks();
        }
    }
    public void addExp()
    {
        //levelSlider.value += expToAdd;
        currentExp += expToAdd;
        levelSlider.value = currentExp;

        texts.ShowHowMuchAdded(expToAdd);
        updateMaxValue();
        CheckIfNewLevel();

        PlayerPrefs.SetFloat("CurrentExp", currentExp);
        //currentExp = levelSlider.value;


        Debug.Log("Level: " + currentLevel + "\nMaxValue: " + maxValue + "\nCurrentExp:" + currentExp);
    }
    public void switchRanks()
    {
        if (currentLevel < 3) { texts.TMP_Rank.text = ranks[0]; texts.TMP_Rank.color = ranksColor[0]; }
        if (currentLevel >= 3 && currentLevel < 5) { texts.TMP_Rank.text = ranks[1]; texts.TMP_Rank.color = ranksColor[1]; }
        if (currentLevel >= 5 && currentLevel < 8) { texts.TMP_Rank.text = ranks[2]; texts.TMP_Rank.color = ranksColor[2]; }
        if (currentLevel >= 8 && currentLevel < 11) { texts.TMP_Rank.text = ranks[3]; texts.TMP_Rank.color = ranksColor[3]; }
        if (currentLevel >= 11) { texts.TMP_Rank.text = ranks[4]; texts.TMP_Rank.color = ranksColor[4]; }
    }
    private void loadSliderValue()
    {
        //if(levelSlider.value == 2)
        //{
            currentExp = PlayerPrefs.GetFloat("CurrentExp");
            levelSlider.value = currentExp;
        //}
        
    }
}
