using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject _ScreenDead;

    private bool _isActiveGame = false;

    [SerializeField] private Spawner _spawner;
    [SerializeField] private GameObject _gameObjectDead;

    [SerializeField] private Button[] _buttons;
    [SerializeField] private Button[] _endButtons;

    private Stack<Robot> _robotsStack;
    private bool _goNextLevel = true;
    [SerializeField] private int _level;
    [SerializeField] private TextMeshProUGUI _textMeshProTimer;
    [SerializeField] private TextMeshProUGUI _textMeshProRules;
    [TextArea] [SerializeField] private string[] _rulesArray;

    [SerializeField] private Transform _endPosition;
    [SerializeField] private Transform _DeadPosition;

    [SerializeField] private float _startTimer;
    [SerializeField] private Button _goButton;


    [SerializeField] private DoorMove _rightDoor;
    [SerializeField] private DoorMove _leftDoor;

    private bool _timeGo = true;
    private bool _goTimer = true;
    private Transform _nextTranform;


    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        _robotsStack = _spawner.Spawn();
        _textMeshProTimer.text = TimeSpan.FromSeconds(_startTimer).ToString("ss") + ":" + TimeSpan.FromSeconds(_startTimer).ToString("ff");
        //_textMeshProTimer.text = Mathf.Round(_startTimer).ToString()

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
                if (robot.GetNumber() % 2 == 0 && robot.GetColor() == "Red")
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
            //foreach (Robot robot in _robotsStack)
            //{
            //   if(robot.GetNumber() < 4 && robot.GetColor() == "Red")
            //   {
            //        robot.SetTypeRobot(2);
            //   }
            //}

            foreach (Robot robot in _robotsStack)
            {
                if(robot.GetColor() == "Yellow")
                {
                    Robot nextRobot = robot.GetNextRobot();
                    if (nextRobot != null && nextRobot.GetColor() == "Blue")
                    {
                        nextRobot.SetTypeRobot(2);
                    }
                }
            }

        }

        if (_level > 4)
        {
            foreach (Robot robot in _robotsStack)
            {
                if (robot.GetColor() == "Yellow")
                {
                    Robot nextRobot = robot.GetNextRobot();
                    if (nextRobot != null && nextRobot.GetColor() == "Red")
                    {
                        robot.SetTypeRobot(2);
                    }

                    Robot afterRobot = robot.GetAfterRobot();
                    if (afterRobot != null && afterRobot.GetColor() == "Red")
                    {
                        robot.SetTypeRobot(2);
                    }

                }
            }
        }

        if (_level > 5)
        {
            foreach (Robot robot in _robotsStack)
            {
                Robot nextRobot = robot.GetNextRobot();
                if (nextRobot != null && nextRobot.GetColor() == "Yellow")
                {
                    robot.SetTypeRobot(1);
                }
            }
        }


    }

    private void Update()
    {
        if (_goTimer)
        {
            _startTimer -= Time.deltaTime;
            _textMeshProTimer.text = TimeSpan.FromSeconds(_startTimer).ToString("ss") + ":" + TimeSpan.FromSeconds(_startTimer).ToString("ff");
        }

        if(_startTimer < 0 && _timeGo)
        {
            StartGame();
        }
        else if (_startTimer < 0 && _isActiveGame)
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
        if(_robotsStack.Count > 0)
        {
            if (_robotsStack.Peek().GetTypeRobot() == playerIndex)
            {
                int count = 0;
                _startTimer = 4f;

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
                if(_robotsStack.Count > 0)
                {
                    _robotsStack.Peek().DoRotate();
                }
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
        _isActiveGame = false;
        if(_robotsStack.Count > 0)
        {
            _robotsStack.Peek().GoNextPozition(_DeadPosition);
            _robotsStack.Peek().PlayDeadSound();
        }
        _goTimer = false;
        foreach (Button button in _buttons)
        {
            button.gameObject.SetActive(false);
        }
        foreach (Button button in _endButtons)
        {
            button.gameObject.SetActive(true);
        }
        _ScreenDead.SetActive(true);
    }

    private void GoNextLevel()
    {
        _rightDoor.GoStartPositionDoor();
        _leftDoor.GoStartPositionDoor();
        _leftDoor.PlayCloseSound();
        _level++;
        PlayerPrefs.SetInt("Level", _level);
        StartCoroutine(DoNextLevel());
    }

    public void StartGame()
    {
        _timeGo = false;
        _isActiveGame = true;
        _goButton.gameObject.SetActive(false);
        foreach (Button button in _buttons)
        {
            button.gameObject.SetActive(true);
        }
        _robotsStack.Peek().DoRotate();
        _startTimer = 4f;
    }

    public void RetunrneGame()
    {
        if(_robotsStack.Count > 0)
        {
            _robotsStack.Peek().gameObject.SetActive(false);
        }
        _ScreenDead.GetComponent<GameImageScript>().GoOpacityMinus();
        _rightDoor.GoStartPositionDoor();
        _leftDoor.GoStartPositionDoor();
        _leftDoor.PlayCloseSound();
        StartCoroutine(DoNextLevel());
    }

    public bool GetIsActiveGame()
    {
        return _isActiveGame;
    }

    IEnumerator DoNextLevel()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
