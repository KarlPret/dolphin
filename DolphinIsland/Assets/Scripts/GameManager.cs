using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Sprite bgImage;

    public Sprite[] puzzles;

    public List<Sprite> gamePuzzles = new List<Sprite>();

    public List<Button> btns = new List<Button>();

    private bool firstGuess, secondGuess;

    private int countGuesses;
    private int countCorrectGuesses;
    private int gameGuesses;

    private int firstGuessIndex, secondGuessIndex;

    private string firstGuessPuzzle, secondGuessPuzzle;

    public GameObject GameWinPopUp;

    public GameObject NextButtonMessagePanel;   //NEW CODE 21 MAY 2024 - TO FLASH MESSAGE REGARDING NEXT LEVEL
    public GameObject GameFinishedText;   //NEW CODE 21 MAY 2024 - TO FLASH MESSAGE REGARDING GAME/ LEVEL FINISHED

    public AudioSource audioSource;     //NEW CODE 21 MAY 2024 - ADDED sFX TO INDICATE GAME OVER
    public AudioClip audioClip;     //NEW CODE 21 MAY 2024 - ADDED sFX TO INDICATE GAME OVER
    public AudioClip audioClip2;     //NEW CODE 21 MAY 2024 - ADDED sFX TO INDICATE MATCHING
    public AudioClip audioClip3;     //NEW CODE 21 MAY 2024 - ADDED sFX TO INDICATE MIS-MATCHING

    public TextMeshProUGUI MatchesText;    //NEW CODE 21 MAY 2024 - YOUR SCORE
    public TextMeshProUGUI TurnsText;    //NEW CODE 21 MAY 2024 - YOUR TURNS
    private int countGuesses2;   //NEW CODE 21 MAY 2024 - YOUR TURNS
    public TextMeshProUGUI TotalScoreText;   //NEW CODE 21 MAY 2024 - YOUR TOTAL SCORE
    public int totalScore;   //NEW CODE 21 MAY 2024 - YOUR TOTAL SCORE

    private void Awake()
    {
        puzzles = Resources.LoadAll<Sprite>("Images/island");
    }

    // Start is called before the first frame update
    void Start()
    {
        GetButtons();
        AddListeners();
        AddGamePuzzles();
        Shuffle(gamePuzzles);
        gameGuesses = gamePuzzles.Count / 2;
                
        totalScore = PlayerPrefs.GetInt("Total Score");  //NEW CODE 21 MAY 2024 - LOAD TOTAL SCORE
        TotalScoreText.text = " " + totalScore;   //NEW CODE 21 MAY 2024 - YOUR TOTAL SCORE
    }

    void GetButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("puzzleBtn");

        for (int i = 0; i < objects.Length; i++) 
        {
            btns.Add(objects[i].GetComponent<Button>());
            btns[i].image.sprite = bgImage;

        }
        
    }

    void AddGamePuzzles()
    {
        int looper = btns.Count;
        int index = 0;

        for (int i = 0;i < looper;i++) 
        {
            if(index == looper/2) 
            {
                index = 0;
            }
            gamePuzzles.Add(puzzles[index]);
            index++;
        }
    }

    void AddListeners()
    {
        foreach (Button btn in btns) 
        {
            btn.onClick.AddListener(() => PickPuzzle());
        }
    }

    public void PickPuzzle()
    {
        //string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        if(!firstGuess)
        {
            firstGuess = true;
            firstGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

            firstGuessPuzzle = gamePuzzles[firstGuessIndex].name;

            btns[firstGuessIndex].image.sprite = gamePuzzles[firstGuessIndex];
        }
        else if(!secondGuess)
        {
            secondGuess = true;
            secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

            secondGuessPuzzle = gamePuzzles[secondGuessIndex].name;

            btns[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];

            if(firstGuessPuzzle == secondGuessPuzzle)
            {
                print("Puzzle Match");

                countGuesses++;     //NEW CODE 21 MAY 2024 - TRY TO COUNT AND DISPLAY GUESSES - NOT CORRECT!!!
                MatchesText.text = " " + countGuesses;    //NEW CODE 21 MAY 2024 - YOUR SCORE

                countGuesses2++;     //NEW CODE 21 MAY 2024 - TRY TO COUNT AND DISPLAY GUESSES - NOT CORRECT!!!
                TurnsText.text = " " + countGuesses2;    //NEW CODE 21 MAY 2024 - YOUR TURNS

                totalScore++;
                TotalScoreText.text = " " + totalScore;   //NEW CODE 21 MAY 2024 - YOUR TOTAL SCORE
                PlayerPrefs.SetInt("Total Score", totalScore);    //NEW CODE 21 MAY 2024 - SAVE TOTAL SCORE

                audioSource.Stop();    //NEW CODE 21 MAY 2024 - ADDED sFX TO INDICATE MATCHING SOUND
                audioSource.clip = audioClip2;    //NEW CODE 21 MAY 2024 - ADDED sFX TO INDICATE MATCHING SOUND
                audioSource.Play();    //NEW CODE 21 MAY 2024 - ADDED sFX TO INDICATE MATCHING SOUND
            }
            else
            {
                print("Puzzle don't Match");

                countGuesses2++;     //NEW CODE 21 MAY 2024 - TRY TO COUNT AND DISPLAY GUESSES - NOT CORRECT!!!
                TurnsText.text = " " + countGuesses2;    //NEW CODE 21 MAY 2024 - YOUR TURNS

                audioSource.Stop();    //NEW CODE 21 MAY 2024 - ADDED sFX TO INDICATE MIS-MATCHING SOUND
                audioSource.clip = audioClip3;    //NEW CODE 21 MAY 2024 - ADDED sFX TO INDICATE MIS-MATCHING SOUND
                audioSource.Play();    //NEW CODE 21 MAY 2024 - ADDED sFX TO INDICATE MIS-MATCHING SOUND
            }

            StartCoroutine(checkThePuzzleMatch());
        }

    }

    IEnumerator checkThePuzzleMatch()
    {
        yield return new WaitForSeconds(0.5f);

        if (firstGuessPuzzle == secondGuessPuzzle)
        {
            yield return new WaitForSeconds(0.1f);
            btns[firstGuessIndex].interactable = false;
            btns[secondGuessIndex].interactable = false;

            btns[firstGuessIndex].image.color = new Color(0, 0, 0, 0);
            btns[secondGuessIndex].image.color = new Color(0, 0, 0, 0);

            CheckTheGameFinished();

        }
        else
        {
            btns[firstGuessIndex].image.sprite = bgImage;
            btns[secondGuessIndex].image.sprite = bgImage;
        }
        yield return new WaitForSeconds(0.1f);

        firstGuess = secondGuess = false;
    }

    void CheckTheGameFinished()
    {
        countCorrectGuesses++;

        //countGuesses++;     //NEW CODE 21 MAY 2024 - TRY TO COUNT AND DISPLAY GUESSES - NOT CORRECT!!!

        if (countCorrectGuesses == gameGuesses)
        {
            print("Game Finished");

            audioSource.Stop();    //NEW CODE 21 MAY 2024 - ADDED sFX TO INDICATE GAME OVER
            audioSource.clip = audioClip;    //NEW CODE 21 MAY 2024 - ADDED sFX TO INDICATE GAME OVER
            audioSource.Play();    //NEW CODE 21 MAY 2024 - ADDED sFX TO INDICATE GAME OVER
            GameFinishedText.SetActive(true);   //NEW CODE 21 MAY 2024 - TO FLASH MESSAGE REGARDING NEXT LEVEL
            StartCoroutine(WaitForGameFinishedTextClose());   //NEW CODE 21 MAY 2024 - TO CLOSE FLASH MESSAGE REGARDING NEXT LEVEL

            GameWinPopUp.SetActive(true);

            print("It took you " + countGuesses + " Guesses");     //THIS IS NOT WORKING IN GAME - FIX!!! WRONG VARIABLE I THINK...
            //MatchesText.text = " " + countGuesses;    //NEW CODE 21 MAY 2024 - YOUR SCORE
        }
    }

    public void NextBtnClick()
    {
        print("next click");

        NextButtonMessagePanel.SetActive(true);        //NEW CODE 21 MAY 2024 - TO FLASH MESSAGE REGARDING NEXT LEVEL

        StartCoroutine(WaitForNextMessagePanelClose());        //NEW CODE 21 MAY 2024 - TO CLOSE FLASH MESSAGE REGARDING NEXT LEVEL
    }

    public void RetryBtnCklick()
    {
        print("retry click");
    }

    void Shuffle(List<Sprite> list)
    {
        for(int i = 0; i < list.Count; i++) 
        {
            Sprite temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    //NEW CODE 21 MAY 2024 - TO FLASH MESSAGE REGARDING NEXT LEVEL
    IEnumerator WaitForNextMessagePanelClose()
    {
        yield return new WaitForSeconds(2.1f);

        NextButtonMessagePanel.SetActive(false);
    }
    //NEW CODE 21 MAY 2024 - TO FLASH MESSAGE REGARDING NEXT LEVEL END

    //NEW CODE 21 MAY 2024 - TO FLASH MESSAGE REGARDING GAME/ LEVEL FINISHED
    IEnumerator WaitForGameFinishedTextClose()
    {
        yield return new WaitForSeconds(3.1f);

        GameFinishedText.SetActive(false);
    }
    //NEW CODE 21 MAY 2024 - TO FLASH MESSAGE REGARDING GAME/ LEVEL FINISHED END
}
