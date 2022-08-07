using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private Robot _robotPref;
    [SerializeField] private int _spawnCount;
    [SerializeField] private Transform[] _spawnPoints;

    public Stack<Robot> Spawn()
    {
        Stack<Robot> _robotsStack = new Stack<Robot>();

        for (int i = 0; i < _spawnCount; i++)
        {
            Robot robot = Instantiate(_robotPref, _spawnPoints[i].position, Quaternion.identity, _spawnPosition);
            robot.SetCollor(Random.Range(0, 2));
            robot.SetNumber(i);

            if(i > 0)
            {
                _robotsStack.Peek().SetNextRobot(robot);
            }

            _robotsStack.Push(robot);

        }

        return _robotsStack;

    }
}
