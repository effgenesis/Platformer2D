using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _baseSpeed;
    [SerializeField] private float _lifeTime;
    private Vector2 _direction;

    void Update()
    {
        if (_direction != null)
        {
            transform.Translate(_direction * _baseSpeed);
        }
        _lifeTime -= Time.deltaTime;
        if (_lifeTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void StartMoving(Vector2 direction)
    {
        _direction = direction;
        transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
    }
}
