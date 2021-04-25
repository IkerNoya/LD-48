using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 8f;
    Vector3 Direction;
    float timer = 0;
    [SerializeField] float lifeTimeBullet = 3;
    [SerializeField] float damage;
    void Update()
    {
        transform.position += Direction.normalized * speed * Time.deltaTime;

        if (timer >= lifeTimeBullet)
        {
            Destroy(gameObject);
            timer = 0;
        }
        timer += Time.deltaTime;
    }

    public void SetDirection(Vector3 direction) => Direction = direction;

    public void SetSpeed(float value) => speed = value;

    public float GetSpeed() { return speed; }

    public void SetDamage(float value) => damage = value;

    public float GetDamage() { return damage; }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
    }
}
