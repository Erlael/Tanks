using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _timeShoot;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _trunkPosition;
    [SerializeField] private Text _enemyScore;
    [SerializeField] private Enemy _opponent;

    private float _time;
    private float _rotate;
    private int _hits = 0;
    private Vector3 _startPosition;
    private Vector2 _velocity;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _startPosition = transform.position;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _velocity = transform.up * Input.GetAxis("Vertical") * _speed;
        _rotate = -Input.GetAxis("Horizontal") * _rotateSpeed;
       
        if (_time <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Instantiate(_bullet, _trunkPosition.position, _trunkPosition.rotation);
                _time = _timeShoot;
            }
        }
        else
        {
            _time -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + _velocity * Time.deltaTime);
        _rigidbody.MoveRotation(_rigidbody.rotation + _rotate * Time.deltaTime);
    }

    public void TakeDamage()
    {
        var objects = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject obj in objects)
        {
            if ((obj.CompareTag("EnemyBullet")) || (obj.CompareTag("Bullet")))
            {
                Destroy(obj);
            }
        }
        _hits++;
        _enemyScore.text = "Enemy: " + _hits;
        StartPosition();
        _opponent.StartPosition();
    }
    public void StartPosition()
    {
        transform.position = _startPosition;
    }
}
