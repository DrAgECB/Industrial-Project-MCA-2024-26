using UnityEngine;

public class FlashEffect : MonoBehaviour
{
    [SerializeField] private Renderer[] _renderers;
    [SerializeField] private Color _flashColor;
    [SerializeField] private Color _originalColor;
    [SerializeField] private float _cooldownSpeed = 5;
    [SerializeField] private bool _playOnAwake;

    private void Awake()
    {
        if (_playOnAwake)
        {
            PlayFlash();
        }
    }

    public void PlayFlash()
    {
        foreach (Renderer renderer in _renderers)
        {
            renderer.sharedMaterial.color = _flashColor;
        }
    }

    private void Update()
    {
        if (_renderers.Length == 0)
            return;

        foreach (Renderer renderer in _renderers)
        {
            renderer.sharedMaterial.color =
                Color.Lerp(
                    renderer.sharedMaterial.color,
                    _originalColor,
                    Time.deltaTime * _cooldownSpeed);
        }
    }
}