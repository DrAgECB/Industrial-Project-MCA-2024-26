using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UFO : MonoBehaviour
{
    public enum UFOStates
    {
        Idle,
        Attacking
    }

    [SerializeField] private UFOStates _currentState;

    [SerializeField]
    private List<Vector3> _trajectoryVectors =
        new List<Vector3>();

    [SerializeField] private int _trajectoriesPerSpawn = 2;

    [SerializeField] private float _spawnDistanceFromPlayer = 20;

    [SerializeField] private float _movementSpeed = 5;

    [SerializeField] private int _cooldownMinTime = 5;
    [SerializeField] private int _cooldownMaxTime = 15;

    [SerializeField] private GameState _gameState;

    [SerializeField] private AudioSfx _ufoOnScene;

    [SerializeField] private UnityEvent OnDie;
    [SerializeField] private UnityEvent OnStartAttacking;
    [SerializeField] private UnityEvent OnStopAttacking;

    private Transform _player;

    private Coroutine _attackRoutine;

    public UFOStates CurrentState
    {
        get => _currentState;

        set
        {
            _currentState = value;

            if (_currentState == UFOStates.Attacking)
            {
                OnStartAttacking?.Invoke();
            }
            else
            {
                OnStopAttacking?.Invoke();
            }
        }
    }

    private void Start()
    {
        GameObject playerObject =
            GameObject.Find("Player");

        if (playerObject != null)
        {
            _player = playerObject.transform;
        }

        CurrentState = UFOStates.Idle;
    }

    private IEnumerator IdleRoutine()
    {
        transform.position = Vector3.one * 1000f;

        _trajectoryVectors.Clear();

        yield return new WaitForSeconds(
            Random.Range(
                _cooldownMinTime,
                _cooldownMaxTime));

        CurrentState = UFOStates.Attacking;
    }

    public void StartCooldown()
    {
        StartCoroutine(IdleRoutine());

        _ufoOnScene.StopAudio();
    }

    public void StartAttacking()
    {
        if (_player == null)
            return;

        transform.position = GetNewPositionVector();

        _trajectoryVectors.Clear();

        for (int i = 0; i < _trajectoriesPerSpawn; i++)
        {
            _trajectoryVectors.Add(
                GetNewPositionVector());
        }

        _ufoOnScene.PlayAudio(gameObject);

        if (_attackRoutine != null)
        {
            StopCoroutine(_attackRoutine);
        }

        _attackRoutine =
            StartCoroutine(AttackMovement());
    }

    private Vector3 GetNewPositionVector()
    {
        if (_player == null)
            return transform.position;

        Vector3 forwardDirection =
            _player.forward;

        forwardDirection.y = 0;

        Vector3 spawnCenter =
            _player.position +
            forwardDirection.normalized *
            _spawnDistanceFromPlayer;

        Vector3 randomOffset =
            new Vector3(
                Random.Range(-5f, 5f),
                Random.Range(-2f, 2f),
                0);

        return spawnCenter + randomOffset;
    }
    IEnumerator AttackMovement()
    {
        for (int i = 0; i < _trajectoryVectors.Count; i++)
        {
            while (
                Vector3.Distance(
                    transform.position,
                    _trajectoryVectors[i]) > 0.5f
                &&
                !_gameState.GameOver)
            {
                yield return null;

                transform.position =
                    Vector3.MoveTowards(
                        transform.position,
                        _trajectoryVectors[i],
                        Time.deltaTime * _movementSpeed);
            }
        }

        CurrentState = UFOStates.Idle;
    }

    public void Die()
    {
        OnDie?.Invoke();

        StopAllCoroutines();

        StartCooldown();

        _ufoOnScene.StopAudio();
    }
}