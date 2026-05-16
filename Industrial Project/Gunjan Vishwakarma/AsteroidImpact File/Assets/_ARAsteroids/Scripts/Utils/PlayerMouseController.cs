using System.Collections;
using UnityEngine;

public class PlayerMouseController : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] private GameObject _objectToCreate;
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private float _fireRate = 0.3f;

    [Header("Crosshair")]
    [SerializeField] private RectTransform _crosshairUI;

    private Camera _mainCamera;
    private bool _canShoot = true;

    void Start()
    {
        _mainCamera = Camera.main;

        if (_mainCamera == null)
        {
            Debug.LogError("Main Camera not found!");
        }

#if !UNITY_EDITOR
        // Keep crosshair centered on mobile
        if (_crosshairUI != null)
        {
            _crosshairUI.anchorMin =
                new Vector2(0.5f, 0.5f);

            _crosshairUI.anchorMax =
                new Vector2(0.5f, 0.5f);

            _crosshairUI.anchoredPosition =
                Vector2.zero;
        }
#endif
    }

    void Update()
    {
#if UNITY_EDITOR
        UpdateEditorCrosshair();

        if (Input.GetMouseButtonDown(0))
        {
            TryShoot();
        }
#else
        // Mobile AR
        if (Input.touchCount > 0 &&
            Input.GetTouch(0).phase ==
            TouchPhase.Began)
        {
            TryShoot();
        }
#endif
    }

#if UNITY_EDITOR
    void UpdateEditorCrosshair()
    {
        if (_crosshairUI == null)
            return;

        _crosshairUI.position =
            Input.mousePosition;
    }
#endif

    void TryShoot()
    {
        if (!_canShoot)
            return;

        ShootBullet();

        StartCoroutine(ShootCooldown());
    }

    void ShootBullet()
    {
        if (_objectToCreate == null)
        {
            Debug.LogError("Bullet prefab missing!");

            return;
        }

        Ray ray;

#if UNITY_EDITOR
        // Shoot toward mouse
        ray =
            _mainCamera.ScreenPointToRay(
                Input.mousePosition);
#else
        // Shoot from screen center
        ray =
            _mainCamera.ViewportPointToRay(
                new Vector3(
                    0.5f,
                    0.5f,
                    0));
#endif

        Vector3 targetPoint;

        if (Physics.Raycast(ray,
            out RaycastHit hit))
        {
            targetPoint =
                hit.point;
        }
        else
        {
            targetPoint =
                ray.GetPoint(50f);
        }

        Vector3 spawnPos =
            _bulletSpawnPoint != null
            ? _bulletSpawnPoint.position
            : transform.position;

        Vector3 direction =
            (targetPoint - spawnPos)
            .normalized;

        Quaternion rotation =
            Quaternion.LookRotation(
                direction);

        GameObject bullet =
            Instantiate(
                _objectToCreate,
                spawnPos,
                rotation);

        bullet.transform.forward =
            direction;
    }

    IEnumerator ShootCooldown()
    {
        _canShoot = false;

        yield return new WaitForSeconds(
            _fireRate);

        _canShoot = true;
    }
}