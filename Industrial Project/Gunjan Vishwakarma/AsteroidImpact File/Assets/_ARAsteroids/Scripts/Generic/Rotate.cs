using UnityEngine;
using Random = UnityEngine.Random;

public class Rotate : MonoBehaviour
{
    [SerializeField] private Vector3 _speed;

    [SerializeField] private bool _randomizeInitialRotation;

    [SerializeField] private float _randomizationFactor = 2;

    private void Start()
    {
        if (_randomizeInitialRotation)
        {
            float multiplier =
                Random.Range(0.5f, _randomizationFactor);

            _speed *= multiplier;
        }
    }

    void Update()
    {
        transform.Rotate(
            _speed * Time.deltaTime,
            Space.Self);
    }
}