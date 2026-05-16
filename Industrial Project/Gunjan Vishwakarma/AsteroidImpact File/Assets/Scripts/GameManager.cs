using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameState _gameState;

    private float _cachedSpeed;

    private void Awake()
    {
        _gameState.Score = 0;
        _gameState.GameOver = false;

        _cachedSpeed = _gameState.GameSpeed;

        Time.timeScale = _cachedSpeed;
    }

    private void Update()
    {
        if (_cachedSpeed != _gameState.GameSpeed)
        {
            _cachedSpeed = _gameState.GameSpeed;

            Time.timeScale = _cachedSpeed;
        }
    }
}