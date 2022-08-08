using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    private GameController _gameController;
    private float _delay;

    [SerializeField] private Button _buttonAttack;
    [SerializeField] private Button _buttonDeffense;
    [SerializeField] private Button _buttonHacking;

    [SerializeField] private Image imageAttack;
    [SerializeField] private Image imageDefence;
    [SerializeField] private Image imageHack;


    [SerializeField] private Sprite _spriteAttack;
    [SerializeField] private Sprite _spriteAttackUz;

    [SerializeField] private Sprite _spriteDefence;
    [SerializeField] private Sprite _spriteDefenceUz;

    [SerializeField] private Sprite _spriteHack;
    [SerializeField] private Sprite _spriteHackUz;


    private void Start()
    {
        _gameController = FindObjectOfType<GameController>();
        _delay = 0;
    }

    private void Update()
    {
        if(_delay > 0.5)
        {
            if (Input.GetKey(KeyCode.A))
            {
                _buttonAttack.onClick.Invoke();
            }

            if (Input.GetKey(KeyCode.S))
            {
               _buttonDeffense.onClick.Invoke();
            }

            if (Input.GetKey(KeyCode.D))
            {
                _buttonHacking.onClick.Invoke();
            }
        }

        _delay += Time.deltaTime;
        Debug.Log(_delay);
    }

    public void Attack()
    {
        if (_delay > 0.5 && _gameController.GetIsActiveGame())
        {
            _buttonAttack.gameObject.GetComponent<AudioSource>().Play();
            ChangeSprite(imageAttack, _spriteAttackUz, _spriteAttack);
            _gameController.PunchRobot(0);
            _delay = 0;
            Debug.Log("0");
        }
    }

    public void Deeffence()
    {
        if (_delay > 0.5 && _gameController.GetIsActiveGame())
        {
            _buttonDeffense.gameObject.GetComponent<AudioSource>().Play();
            ChangeSprite(imageDefence, _spriteDefenceUz, _spriteDefence);
            _gameController.PunchRobot(1);
            _delay = 0;
            Debug.Log("1");
        }
    }

    public void Hacking()
    {
        if (_delay > 0.5 && _gameController.GetIsActiveGame())
        {
            _buttonHacking.gameObject.GetComponent<AudioSource>().Play();
            ChangeSprite(imageHack, _spriteHackUz, _spriteHack);
            _gameController.PunchRobot(2);
            _delay = 0;
            Debug.Log("2");
        }
    }

    public void ChangeSprite(Image image, Sprite onsprite, Sprite offsprite)
    {
        image.sprite = onsprite;
        StartCoroutine(OffChangeSprite(image, offsprite));

    }

    IEnumerator OffChangeSprite(Image image, Sprite offsprite)
    {
        yield return new WaitForSeconds(0.5f);
        image.sprite = offsprite;
    }
}
