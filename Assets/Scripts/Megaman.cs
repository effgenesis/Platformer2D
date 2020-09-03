using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Megaman : MonoBehaviour
{
    private static bool _faceLeft = true;
    private static bool _faceRight = false;
    [SerializeField] private float _speed;
    [SerializeField] private Rigidbody2D _rigidBody;
    [SerializeField] private float _jumpImpulse;
    [SerializeField] private GameObject _bulletPrefab;
    private bool _isRunning = false;
    private bool _isJumping = false;
    private bool _isShooting = false;
    private float _shootingAnimationTimeout = 0f;
    private bool _controlEnabled = true;
    private Animator _animator;
    private Vector2 _bulletHolePosition;
    private float _reloadTimer = 0f;

    private bool IsMovingVertical
    {
        get
        {
            return Mathf.Abs(_rigidBody.velocity.y) > 0.2;
        }
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        _bulletHolePosition = new Vector2(0.25f, 0);
    }
    void Update()
    {
        _isRunning = false;
        if (_controlEnabled)
        {
            HandlePressedButtons();
            if (_isShooting)
            {
                ShootingAnimationTimerCountdown();
            }
            if (_reloadTimer > 0)
            {
                _reloadTimer -= Time.deltaTime;
            }
            if (IsMovingVertical)
            {
                _isJumping = true;
                _animator.SetBool("IsJumping", true);
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Platform platform) && _rigidBody.velocity.y <= 0)
        {
            _isJumping = false;
            _animator.SetBool("IsJumping", false);
        }
        if (collision.collider.TryGetComponent(out Bonus bonus))
        {
            Destroy(bonus.gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Bonus bonus))
        {
            Destroy(bonus.gameObject);
        }
    }

    private void HandlePressedButtons()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Walk(_faceLeft);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Walk(_faceRight);
        }
        else
        {
            _animator.SetBool("IsRunning", false);
        }
        if (Input.GetKey(KeyCode.Space) && !_isJumping)
        {
            Jump();
        }
        if (Input.GetKey(KeyCode.Backspace))
        {
            Shoot();
        }
    }

    private void Walk(bool isLeft)
    {
        Turn(isLeft);
        _isRunning = true;
        _animator.SetBool("IsRunning", true);
        if (isLeft)
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }
        
    }

    private void Jump()
    {
        _isJumping = true;
        _animator.SetBool("IsJumping", true);
        _rigidBody.AddForce(Vector2.up * _jumpImpulse, ForceMode2D.Impulse);
    }

    private void Shoot()
    {
        if(_reloadTimer <= 0)
        {
            _reloadTimer = 0.2f;
            Bullet bullet = Instantiate(_bulletPrefab).GetComponent<Bullet>();
            bullet.StartMoving(new Vector2(transform.localScale.x, 0));
            bullet.transform.position = (Vector2)transform.position + _bulletHolePosition * transform.localScale.x;
            _isShooting = true;
            _animator.SetBool("IsShooting", true);
            _shootingAnimationTimeout = 0.5f;
        }
        
    }

    private void ShootingAnimationTimerCountdown()
    {
        _shootingAnimationTimeout -= Time.deltaTime;
        if (_shootingAnimationTimeout <= 0)
        {
            _isShooting = false;
            _animator.SetBool("IsShooting", false);
        }
    }

    private void Turn(bool isLeft)
    {
        if (isLeft)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
