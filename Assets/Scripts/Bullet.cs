using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _distance;
    [SerializeField] private LayerMask _whatIsSolid;
    [SerializeField] private float _impuls;

    private float _rotate;
    private Vector2 _velocity;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _velocity = transform.up * _impuls;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {  
        if (collision.collider != null)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                Vector2 vector2 = Vector2.Reflect(_velocity, collision.contacts[0].normal).normalized;
                _velocity = vector2 * _impuls;
            }
            else if ((collision.gameObject.CompareTag("Enemy"))&&(gameObject.CompareTag("Bullet")))
            {
                collision.collider.GetComponent<Enemy>().TakeDamage();
                Destroy(gameObject);
            }
            else if ((collision.gameObject.CompareTag("Player")) && (gameObject.CompareTag("EnemyBullet")))
            {
                collision.collider.GetComponent<Player>().TakeDamage();
                Destroy(gameObject);
            }
        }      
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + _velocity * Time.deltaTime);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
