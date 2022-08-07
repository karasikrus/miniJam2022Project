using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]

public class Robot : MonoBehaviour
{
    [SerializeField] private Sprite _spriteRed;
    [SerializeField] private Sprite _spriteYellow;
    [SerializeField] private float _timeMove;

    private Transform _oldPosition;
    private SpriteRenderer _spriteRenderer;
    private int _type;
    private GameController _gameController;
    private int _number;
    private Robot _nextRobot;
    private string _color;
    private bool _doRotate = false;
    private float _timer;
    private bool _rotateRight = false;

    private LukScript _luk;

    [SerializeField] private int _rotateIntZ;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _gameController = FindObjectOfType<GameController>();
        _luk = FindObjectOfType<LukScript>();
    }

    private void Update()
    {
        if( _doRotate && (_timer > _timeMove/2))
        {
            if (_rotateRight)
            {
                _rotateIntZ = 10;
                _rotateRight = false;
            }
            else
            {
                _rotateIntZ = -10;
                _rotateRight = true;
            }

            transform.DORotate(new Vector3(0, 0, _rotateIntZ), _timeMove/2, RotateMode.Fast).SetLoops(1, LoopType.Yoyo);
            _timer = 0;
        }
        _timer += Time.deltaTime;
    }

    public void SetCollor(int _colorInt)
    {
        if(_colorInt == 0)
        {
            _spriteRenderer.sprite = _spriteRed;
            _color = "Red";
            _type = 1;
        }
        else
        {
            _spriteRenderer.sprite = _spriteYellow;
            _color = "Yellow";
            _type = 0;
        }
    }

    public Transform GoNextPozition(Transform _nextposition)
    {
        _oldPosition = transform;
        transform.DOMove(_nextposition.position, _timeMove);
        _doRotate = true;
        StartCoroutine(DontRotate());
        return _oldPosition;
    }

    public void DoRotate()
    {
        _doRotate = true;
        StartCoroutine(StartRotate());
    }

    public string GetColor()
    {
        return _color;
    }

    public Robot GetNextRobot()
    {
        return _nextRobot;
    }

    public void SetNextRobot(Robot robot)
    {
        _nextRobot = robot;
    }

    public int GetTypeRobot()
    {
        return _type;
    }

    public void SetTypeRobot(int type)
    {
        _type = type;
    }

    public int GetNumber()
    {
        return _number;
    }

    public void SetNumber(int number)
    {
        number++;
        _number = number;
    }

    public void DestroyRobot()
    {
        _doRotate = false;
        transform.DORotate(new Vector3(0, 0, 180), _timeMove / 2, RotateMode.Fast).SetLoops(1, LoopType.Yoyo);
        _luk.DontVisible();
        StartCoroutine(DoDead());
    }

    public void WaitDontDiRotate()
    {
        StartCoroutine(DontRotate());
    }

    IEnumerator DoDead()
    {
        yield return new WaitForSeconds(_timeMove + 0.5f);
        gameObject.SetActive(false);
    }


    IEnumerator DontRotate()
    {
        yield return new WaitForSeconds(_timeMove);
        _doRotate = false;
        transform.DORotate(new Vector3(0, 0, 0), _timeMove / 2, RotateMode.Fast).SetLoops(1, LoopType.Yoyo);
    }

    IEnumerator StartRotate()
    {
        yield return new WaitForSeconds(_timeMove);
        _doRotate = true;
    }

}
