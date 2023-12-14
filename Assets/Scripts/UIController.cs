using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

public class UIController : MonoBehaviour
{
    [SerializeField] private RectTransform choicePanel;
    [SerializeField] private RectTransform messagePanel;
    [SerializeField] private RectTransform correctPanel;
    [SerializeField] private Color[] buttonColors;

    private Tweener messageAnim;
    private Tweener choiceAnim;
    private Tweener correctAnim;

    private TMP_Text messageText;
    private Button correctButton;
    private Button[] worngButtons = new Button[3];

    private Number correctAnswer;
    private void Start()
    {
        messageAnim = messagePanel.DOAnchorPosY(0, 1f, true).Pause().SetAutoKill(false);
        choiceAnim = choicePanel.DOAnchorPosY(0, 1, true).Pause().SetAutoKill(false);
        correctAnim = correctPanel.DOAnchorPosY(928, 1, true).Pause().SetAutoKill(false);

        messageText = messagePanel.GetComponentInChildren<TMP_Text>();
        correctButton = choicePanel.GetChild(0).GetComponent<Button>();
        correctButton.onClick.AddListener(CorrectAnswer);

        for (int i = 0; i < choicePanel.childCount - 1; i++)
        {
            worngButtons[i] = choicePanel.GetChild(i + 1).GetComponent<Button>();
            worngButtons[i].onClick.AddListener(WrongAnswer);
        }
    }
    private void OnDestroy()
    {
        correctButton.onClick.RemoveAllListeners();
        foreach (Button button in worngButtons) { button.onClick.RemoveAllListeners(); }
    }
    public void FindNewNumber(Number number)
    {
        if (correctAnswer == number) return;
        AudioMgr.Instance.PlaySound(Sounds.read);
        correctAnswer = number;
        SetButtons();

        choiceAnim.PlayForward();
        PopMessage("Please read the number out loud!");
    }
    void SetButtons()
    {
        correctButton.transform.SetSiblingIndex(Random.Range(0, 5));
        correctButton.GetComponentInChildren<TMP_Text>().text = System.Enum.GetName(typeof(Numberstext), correctAnswer.CurNumber);

        List<int> results = new List<int>();
        results.Add(correctAnswer.CurNumber);
        int num;

        foreach (Button button in worngButtons)
        {
            do
            {
                num = Random.Range(1, 11);
            }
            while (results.Contains(num));
            button.GetComponentInChildren<TMP_Text>().text = System.Enum.GetName(typeof(Numberstext), num);
            results.Add(num);
        }
        for (int i = 0; i < choicePanel.childCount; i++)
        {
            choicePanel.GetChild(i).GetComponent<Image>().color = buttonColors[i];
        }
    }
    void CorrectAnswer()
    {
        EventCenter.Broadcast(FunctionType.correct, AnimType.RightAnswer);

        AudioMgr.Instance.PlaySound(Sounds.correct);
        GameMgr.Instance.NewNumber();
        correctAnswer.KillNumber();
        correctAnim.PlayForward();
        Observable.Timer(System.TimeSpan.FromSeconds(2.5f)).Subscribe(x =>
        {
            correctAnim.PlayBackwards();
        });

        choiceAnim.PlayBackwards();
        messageAnim.PlayBackwards();
    }
    void WrongAnswer()
    {
        EventCenter.Broadcast(FunctionType.wrong, AnimType.WrongAnswer);

        AudioMgr.Instance.PlaySound(Sounds.wrong);
        GameMgr.Instance.AddTry();
        PopMessage("Try harder, little one!");
    }
    void PopMessage(string message)
    {
        messageText.text = message;
        messageAnim.Restart();
        messageAnim.PlayForward();
    }
}
