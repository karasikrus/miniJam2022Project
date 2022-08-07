using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private Spawner _spawner;
    [SerializeField] private GameObject _gameObjectDead;

    [SerializeField] private Button[] _buttons;

    private Stack<Robot> _robotsStack;
    private bool _goNextLevel = true;
    [SerializeField] private int _level;
    [SerializeField] private TextMeshProUGUI _textMeshProTimer;
    [SerializeField] private TextMeshProUGUI _textMeshProRules;
    [SerializeField] private TextMeshProUGUI _textMeshProLevel;
    [TextArea] [SerializeField] private string[] _rulesArray;

    [SerializeField] private Transform _endPosition;
    [SerializeField] private float _startTimer;
    [SerializeField] private Button _goButton;

    private bool _timeGo = true;
    private bool _goTimer = true;
    private Transform _nextTranform;


    private void Start()
    {
        PlayerPrefs.DeleteAll();
        _robotsStack = _spawner.Spawn();
        _textMeshProTimer.text = Mathf.Round(_startTimer).ToString();

        if (_level == 0)
        {
            if (PlayerPrefs.HasKey("Level"))
            {
                _level = PlayerPrefs.GetInt("Level");
            }
            else
            {
                _level = 1;
            }
        }

        _textMeshProLevel.text = _level.ToString();

        if (_rulesArray.Length >= _level)
        {
            string rule = _rulesArray[_level - 1];
            _textMeshProRules.text = rule;
        }
        else
        {
            _textMeshProRules.text = _rulesArray[_rulesArray.Length - 1];
        }

        if (_level > 1)
        {
            foreach (Robot robot in _robotsStack)
            {
                if (robot.GetNumber() % 2 == 0)
                {
                    robot.SetTypeRobot(2);
                }
            }

        }

        if(_level > 2)
        {
            foreach (Robot robot in _robotsStack)
            {
                if (robot.GetNumber() % 2 != 0)
                {
                    Robot nextRobot = robot.GetNextRobot();
                    if ( nextRobot != null && robot.GetColor() == "Red")
                    {
                        nextRobot.SetTypeRobot(0);
                    }
                }
            }

        }

        if(_level > 3)
        {
            foreach (Robot robot in _robotsStack)
            {
               if(robot.GetNumber() < 4 && robot.GetColor() == "Red")
               {
                    robot.SetTypeRobot(2);
               }
            }

        }

    }

    private void Update()
    {
        if (_goTimer)
        {
            _startTimer -= Time.deltaTime;
            _textMeshProTimer.text = Mathf.Round(_startTimer).ToString();
        }

        if(_startTimer < 0 && _timeGo)
        {
            StartGame();
        }
        else if (_startTimer < 0)
        {
            GameOver();
        }


       if(_robotsStack.Count < 1 && _goNextLevel)
       {
            _goNextLevel = false;
            _goTimer = false;
            GoNextLevel();
       }
    }

    public void PunchRobot(int playerIndex)
    {
        if(_robotsStack != null)
        {
            if (_robotsStack.Peek().GetTypeRobot() == playerIndex)
            {
                int count = 0;
                _startTimer = 5f;

                foreach (Robot robotFor in _robotsStack)
                {

                    if (count == 0)
                    {
                        _nextTranform = robotFor.GoNextPozition(_endPosition);
                    }
                    else
                    {
                        _nextTranform = robotFor.GoNextPozition(_nextTranform);
                    }
                    count++;
                }

                Robot robot = _robotsStack.Pop();
                robot.DestroyRobot();
                _robotsStack.Peek().DoRotate();
            }
            else
            {
                GameOver();
            }
        }
    }

    public void GameOver()
    {
        _gameObjectDead.SetActive(true);
        _goTimer = false;
    }

    private void GoNextLevel()
    {
        _level++;
        PlayerPrefs.SetInt("Level", _level);
        StartCoroutine(DoNextLevel());
    }

    public void StartGame()
    {
        _timeGo = false;
        _goButton.gameObject.SetActive(false);
        foreach (Button button in _buttons)
        {
            button.gameObject.SetActive(true);
        }
        _robotsStack.Peek().DoRotate();
        _startTimer = 5f;
    }

    IEnumerator DoNextLevel()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
