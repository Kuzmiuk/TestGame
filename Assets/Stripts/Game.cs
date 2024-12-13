using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Game : MonoBehaviour
{
    //BACKGROUND COLOR 
    public Image background;

    //TEXT
    public TMP_Text resultText;
    public TMP_Text statsText;

    //BUTTON
    public Button rockbutton;
    public Button paperbutton;
    public Button scissorsbutton;

    //AUDIO
    public AudioSource winSounds;
    public AudioSource loseSounds;
    public AudioSource drawSounds;

    //ANIMACJE
    public Animator playeranimator;
    public Animator AIAnimator;
    public Animator resultAnimator;

    //SYSTEM 
    private int playerWins = 0;
    private int Aiwin = 0;
    private int draws = 0;
    private string[] choices = { "Rock", "Paper", "Scissiors" };

    //BOOL 
    private bool buttonEnabled = true;

    void Start()
    {
        rockbutton.onClick.AddListener(() => PlayerChoice("Rock"));
        paperbutton.onClick.AddListener(() => PlayerChoice("Paper"));
        scissorsbutton.onClick.AddListener(() => PlayerChoice("Scissiors"));
        UpdateStats();

        if (background != null)
        {
            background.color = new Color(0, 0, 0, 0); // Fully transparent at start
        }
    }

    void PlayerChoice(string playerchoice)
    {
        if (!buttonEnabled) return;
        StartCoroutine(HandleChoice(playerchoice));

    }
    IEnumerator HandleChoice(string playerchoice)
    {
        buttonEnabled = false;
        SetButtonsInteractable(false);

        string AiChoice = AIChoice();
        string result = DetermineWinner(playerchoice, AiChoice);
        resultText.text = $"Player: {playerchoice} AI : {AiChoice} Result: {result}";
        PlayAnimations(playerchoice, AiChoice);
        StartCoroutine(FadeBackgroundColor(result));
        PlaySound(result);
        UpdateStats();
        PlayResultAnimation(result);

        yield return new WaitForSeconds(5f);

        buttonEnabled = true;
        SetButtonsInteractable(true);

    }

    void SetButtonsInteractable(bool interactable)
    {
        rockbutton.interactable = interactable;
        paperbutton.interactable = interactable;
        scissorsbutton.interactable = interactable;
    }
    string AIChoice()
    {
        int randomIndex = Random.Range(0, choices.Length);
        return choices[randomIndex];
    }

    string DetermineWinner(string playerchoice, string AiChoice)
    {
        if (playerchoice == AiChoice)
        {
            draws++;
            return "Draw";
        }
        if ((playerchoice == "Rock" && AiChoice == "Scissiors")
            || (playerchoice == "Scissiors" && AiChoice == "Paper")
            || (playerchoice == "Paper" && AiChoice == "Rock"))
        {
            playerWins++;
            return "Win";
        }

        Aiwin++;
        return "Lose";

    }

    void UpdateStats()
    {
        statsText.text = $"Wins: {playerWins}  Losses: {Aiwin}  Draws: {draws}";
    }
    IEnumerator FadeBackgroundColor(string result)
    {
        Color targetColor = new Color(0, 0, 0, 0);

        switch (result)
        {
            case "Win":
                targetColor = new Color(0, 1, 0, 0.5f);
                break;
            case "Lose":
                targetColor = new Color(1, 0, 0, 0.5f);
                break;
            case "Draw":
                targetColor = new Color(1, 1, 0, 0.5f);
                break;
        }

        if (background != null)
        {
            float duration = 1f;
            Color initialColor = background.color;
            float elapsedTime = 0f;


            while (elapsedTime < duration)
            {
                background.color = Color.Lerp(initialColor, targetColor, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            background.color = targetColor;


            yield return new WaitForSeconds(1f);


            elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                background.color = Color.Lerp(targetColor, new Color(0, 0, 0, 0), elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            background.color = new Color(0, 0, 0, 0);
        }
    }

    void PlaySound(string result)
    {
        switch (result)
        {
            case "Win":
                winSounds.Play();
                break;
            case "Lose":
                loseSounds.Play();
                break;
            case "Draw":
                drawSounds.Play();
                break;
        }
    }


    void PlayAnimations(string playerChoice, string aiChoice)
    {
        switch (playerChoice)
        {
            case "Rock":
                playeranimator.SetTrigger("Rock");
                break;
            case "Scissiors":
                playeranimator.SetTrigger("Scissiors");
                break;
            case "Paper":
                playeranimator.SetTrigger("Paper");
                break;
        }
        switch (aiChoice)
        {
            case "Rock":
                AIAnimator.SetTrigger("Rock");
                break;
            case "Scissiors":
                AIAnimator.SetTrigger("Scissiors");
                break;
            case "Paper":
                AIAnimator.SetTrigger("Paper");
                break;
        }
    }

    void PlayResultAnimation(string result)
    {
        switch(result)
        {
            case "Win":
                AIAnimator.SetTrigger("Win");
                break;
            case "Lose":
                AIAnimator.SetTrigger("Lose");
                break;
            case "Draw":
                AIAnimator.SetTrigger("Draw");
                break;
        }    
    }

}
