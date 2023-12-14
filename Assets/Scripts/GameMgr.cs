using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;

public enum Numberstext
{
    one = 1, two, three, four, five, six, seven, eight, nine, ten
}
public struct Result
{
    public int number;
    public int timesTried;
}

public class GameMgr : MonoBehaviour
{
    private static GameMgr instance;
    public static GameMgr Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GameMgr>();
            return instance;
        }
    }

    [SerializeField] private NumberFactory numberFactory;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private UIController uiController;

    private int level = -1;
    public int Level
    {
        get { return level; }
    }
    Result[] results = new Result[10];
    [SerializeField] private RectTransform winPanel;
    [SerializeField] private GameObject resultPrefab;
    private void Start()
    {
        NewNumber();
        winPanel.GetComponentInChildren<Button>().onClick.AddListener(RestartGame);
    }
    //private void OnDestroy()
    //{
    //    winPanel.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
    //}
    public void NumberSelected(Number number)
    {
        uiController.FindNewNumber(number);
    }
    public void NewNumber()
    {
        level++;
        if (level >= 10)
        {
            GameOver();
            return;
        }
        results[level].number = numberFactory.GenerateNewNumber();
    }
    public void AddTry()
    {
        results[level].timesTried++;
    }
    void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
    void GameOver()
    {
        DOTween.PauseAll();
        DOTween.KillAll();
        AudioMgr.Instance.PlaySound(Sounds.win);
        for (int i = 0; i < results.Length; i++)
        {
            TMP_Text text = Instantiate(resultPrefab, winPanel.GetChild(0)).GetComponent<TMP_Text>();
            text.text = results[i].number.ToString();
            if (results[i].timesTried == 0) text.color = Color.green;
            else text.color = Color.red;
        }

        winPanel.DOAnchorPosX(0, 0.5f);
    }
}
