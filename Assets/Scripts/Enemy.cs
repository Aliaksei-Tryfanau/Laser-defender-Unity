﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField]
    float health = 100f;
    [SerializeField]
    int scoreValue = 150;

    [Header("Enemy shooting")]
    [SerializeField]
    float shotCounter;
    [SerializeField]
    float minTimeBetweenShots = 0.2f;
    [SerializeField]
    float maxTimeBetweenShots = 3f;
    [SerializeField]
    GameObject projectile;
    [SerializeField]
    float projectileSpeed = 10f;

    [Header("Enemy VFX and SFX")]
    [SerializeField]
    GameObject deathVFX;
    [SerializeField]
    float durationOfExplosion;
    [SerializeField]
    AudioClip deathSound;
    [SerializeField]
    AudioClip shootSound;
    [SerializeField] [Range(0,1)]
    float deathSoundVolume = 0.75f;
    [SerializeField] [Range(0, 1)]
    float shootSoundVolume = 0.25f;

    // Use this for initialization
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(projectile, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        Destroy(gameObject);
        GameObject expolosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(expolosion, durationOfExplosion);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
    }
}
