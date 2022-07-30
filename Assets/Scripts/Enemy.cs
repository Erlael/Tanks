using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _trunkPosition;
    [SerializeField] private Text _playerScore;
    [SerializeField] private Player _opponent;
    [SerializeField] private float _rotationSpeed = 90;
    [SerializeField] private float _timeShoot;
    [SerializeField] private float _speed;
    [SerializeField] private LayerMask _whatIsSolid;

    private NavMeshAgent _agent;
    private Rigidbody2D _rb;
    private float _rotation;
    private float _time;
    private int _hits = 0;
    private RaycastHit2D _hitInfo;
    private Vector3 _startPosition;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody2D>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _startPosition = transform.position;
    }

    private void Update()
    {
        _agent.SetDestination(_target.position);
        _rotation = Vector2.SignedAngle(Vector2.up, _agent.velocity);
        _hitInfo = Physics2D.Raycast(_trunkPosition.position, transform.up, 20, _whatIsSolid);

        if (_time <= 0)
        {
            if (_hitInfo.collider != null)
            {
                if (_hitInfo.collider.CompareTag("Player"))
                {
                    Instantiate(_bullet, _trunkPosition.position, _trunkPosition.rotation);
                    _time = _timeShoot;
                }
            }
        }
        else
        {
            _time -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        _rb.MoveRotation(
            Quaternion.RotateTowards(
                Quaternion.Euler(0, 0, _rb.rotation),
                Quaternion.Euler(0, 0, _rotation),
                _rotationSpeed * Time.deltaTime
                ));
    }

    public void TakeDamage()
    {
        var objects = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject obj in objects)
        {
            if ((obj.CompareTag("EnemyBullet"))||(obj.CompareTag("Bullet")))
            {
                Destroy(obj);
            }
        }
        _hits++;
        _playerScore.text = "Player: " + _hits;
        StartPosition();
        _opponent.StartPosition();
    }
    public void StartPosition()
    {
        transform.position = _startPosition;
    }
}
