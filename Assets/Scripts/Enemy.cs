using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Rigidbody2D _rigidBody;
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private UnityEvent _death;

    private Collider2D _boardingPlatform;

    private bool _isMoving = false;
    private bool _movingLeft;
    private int _health = 1;

    private void Awake()
    {
        _isMoving = true;
        _animator.SetBool("IsMoving", true);
        _animator.SetBool("MovingLeft", true);
    }

    void Update()
    {
        if (_isMoving)
        {
            transform.Translate(Vector2.left * _speed * Time.deltaTime);
        }
        if (_boardingPlatform != null)
        {
            if (LeftEdgeReached())
            {
                _speed *= -1;
                _animator.SetBool("MovingLeft", false);
            }
            if (RightEdgeReached())
            {
                _speed *= -1;
                _animator.SetBool("MovingLeft", true);
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.TryGetComponent(out Platform platform))
        {
            _boardingPlatform = collision.collider;
        }
        if(collision.collider.TryGetComponent(out Bullet bullet))
        {
            Destroy(bullet.gameObject);
            GetDamage();
        }
    }

    private bool LeftEdgeReached()
    {
        if (_boardingPlatform != null)
        {
            return _boardingPlatform.transform.position.x > transform.position.x + Vector2.left.x * _speed * Time.deltaTime;
        }
        else
            return false;
    }

    private bool RightEdgeReached()
    {
        if (_boardingPlatform != null)
        {
            return _boardingPlatform.transform.position.x + _boardingPlatform.bounds.size.x < transform.position.x + _collider.bounds.size.x + Vector2.left.x * _speed * Time.deltaTime;
        }
        else
            return false;
    }

    private void GetDamage()
    {
        _health -= 1;
        if (_health <= 0)
        {
            _animator.SetBool("Destroy", true);
            _isMoving = false;
            _death.Invoke();
        }
    }
}
