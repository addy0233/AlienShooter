using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gun1 : MonoBehaviour
{
    [Header("Projectile")]
    [Tooltip("Put an object that contain a Projectile1 script on it")]
    public Projectile2 projectile;
    [Space]
    public float ProjectileSpeed = 100f;
    public float ProjectileLifeTime = 2f;

    [Header("Damage")]
    public float ProjectileDamage = 20f;

    [Header("Fire Rate")]
    public float fireRate;
    private float nextTimeToFire;

    [Header("Ammo&Reloading")]
    public float reloadingTime = 2f;
    public float currentReloadingTime;
    public float maxAmmo = 30f;
    public float currentAmmo;

    [Header("Time Between Shots")]
    public float timeBetweenShots;
    private float shotCounter;

    [Header("Time Speed")]
    public float TimeSpeed = 1;

    [Header("Delay Before Shooting")]
    public bool DelayBeforeShootingOn = true;

    public float DelayBeforeShooting = 0.1f;
    private float CurrentDelayBeforeShooting;

    public AmmoCounter ammoCounter;

    public PlayerController playerController;

    public Transform firePoint;
    public ParticleSystem muzzleFlash;

    public TextMeshProUGUI damage1;
    public TextMeshProUGUI shootSpeed;

    public Transform UpgradesUI;

    void Start()
    {
        currentAmmo = maxAmmo;

        currentReloadingTime = reloadingTime;

        ammoCounter.SetMaxAmmo(maxAmmo);
    }

    void Update()
    {

        #region Shooting

        if (currentAmmo == 0 || currentAmmo <= 0)
        {
            currentReloadingTime -= Time.deltaTime * 1f;
        }

        if (currentReloadingTime <= 0)
        {
            currentAmmo = maxAmmo;

            currentReloadingTime = reloadingTime;

            ammoCounter.SetAmmo(currentAmmo);
        }

        if (Input.GetKey(KeyCode.R))
        {
            currentAmmo = 0f;
        }

        if (playerController.target != null && Time.time >= nextTimeToFire && currentAmmo >= 0f)
        {
            if (DelayBeforeShootingOn == true)
            {
                CurrentDelayBeforeShooting -= TimeSpeed * Time.deltaTime;

                if (CurrentDelayBeforeShooting <= 0)
                {
                    nextTimeToFire = Time.time + 1f / fireRate;

                    shotCounter -= Time.deltaTime;

                    if (shotCounter <= 0)
                    {
                        shotCounter = timeBetweenShots;

                        Shoot1();
                    }
                    else
                    {
                        shotCounter = 0;
                    }
                }
            }
            else if (DelayBeforeShootingOn == false)
            {
                nextTimeToFire = Time.time + 1f / fireRate;

                shotCounter -= Time.deltaTime;

                if (shotCounter <= 0)
                {
                    shotCounter = timeBetweenShots;

                    Shoot1();
                }
                else
                {
                    shotCounter = 0;
                }
            }
        }
        else
        {
            CurrentDelayBeforeShooting = DelayBeforeShooting;
        }

        #endregion

        damage1.text = ProjectileDamage.ToString();
        shootSpeed.text = fireRate.ToString();

    }

    public void UpgradeDamage()
    {
        ProjectileDamage += 15;

        UpgradesUI.gameObject.SetActive(false);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UpgradeAttackSpeed()
    {
        fireRate += 5;

        UpgradesUI.gameObject.SetActive(false);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Shoot1()
    {
        muzzleFlash.Play();

        currentAmmo -= 1f;

        ammoCounter.SetAmmo(currentAmmo);

        Projectile2 newProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation) as Projectile2;
        newProjectile.speed = ProjectileSpeed;
        newProjectile.Damage = ProjectileDamage;
        newProjectile.TimeToDestroy = ProjectileLifeTime;
    }
}
