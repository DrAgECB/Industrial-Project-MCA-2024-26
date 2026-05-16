using UnityEngine;

public class ScoreObject : MonoBehaviour
{
    [SerializeField] private GameState _gameState;

    [SerializeField] private int _scoreValue = 10;

    public void AddScore()
    {
        if (_gameState == null)
        {
            Debug.LogWarning("GameState missing!");
            return;
        }

        _gameState.IncreaseScore(_scoreValue);
    }
}