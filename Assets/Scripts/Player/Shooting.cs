using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject regularBulletPrefab;
    public GameObject powerBulletPrefab;
    public AudioClip regularShotAudio;
    public AudioClip powerShotAudio;
    public float bulletSpeed = 2f;
    public int bulletRegularForce = 1;
    public int bulletPowerForce = 2;
    public float delay = 1f;
    public float shot_accuracy = 0.2f;
    private bool canShoot = true;
    public AudioSource audioSource;
    public Image[] modifiersSprites;


    private Conductor conductor;
    private List<IModifier> modifiers;
    private void Start()
    {
        conductor = FindObjectOfType<Conductor>();
        modifiers = new List<IModifier>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // check if the player can shoot already
        if (!canShoot)
            return;

        // get shot precision from the conductor
        var power = GetShootPower();
        GameObject chosenBulletPrefab;
        int damage;
        bool isEmpowered;

        // is the shot empowered?
        if(1 - power < shot_accuracy)
        {
            chosenBulletPrefab = powerBulletPrefab;
            damage = bulletPowerForce;
            isEmpowered = true;
            audioSource.clip = powerShotAudio;
        }
        else
        {
            chosenBulletPrefab = regularBulletPrefab;
            damage = bulletRegularForce;
            isEmpowered = false;
            audioSource.clip = regularShotAudio;
        }


        List<BulletParameters> bullets = new List<BulletParameters>
        {
            new BulletParameters(damage, isEmpowered, 0, chosenBulletPrefab, firePoint.position, firePoint.rotation, firePoint.up)
        };


        // apply modifiers
        foreach (IModifier modifier in modifiers)
        {
            modifier.Modify(bullets);
        }
        foreach (BulletParameters bulletParameters in bullets)
        {
            StartCoroutine(SpawnBullet(bulletParameters));
        }

        // set delay
        canShoot = false;
        StartCoroutine(ShootDelay());
    }

    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }

    // get power of the shot - float between 1 and 0
    private float GetShootPower()
    {
        var beat = conductor.getBeatValue();
        var diff = Mathf.Abs(beat - Mathf.Round(beat)) * 2;
        var power = 1 - diff;
        return power;
    }

    private void onEnemyHit(bool isEmpowered)
    {
       foreach(IModifier modifier in modifiers)
        {
            modifier.EnemyHit(isEmpowered);
        }
    }

    public void AddModifier(IModifier modifier, Sprite modifierSprite)
    {
        modifiers.Add(modifier);
        for (int i = 0; i < modifiersSprites.Length; i++)
        {
            Debug.Log(modifierSprite);
            if (modifiersSprites[i].sprite == null)
            {
                modifiersSprites[i].sprite = modifierSprite;
                modifiersSprites[i].enabled = true;
                Debug.Log(modifiersSprites[i].sprite);
                break;
            }

        }
    }

    private IEnumerator SpawnBullet(BulletParameters bulletParameters)
    {
        yield return new WaitForSeconds(bulletParameters.delay);
        // play the sound
        audioSource.Play();
        // spawn the bullet
        GameObject bullet = Instantiate(bulletParameters.chosenBulletPrefab, bulletParameters.position, bulletParameters.rotation);
        // set tag
        bullet.gameObject.tag = "PlayerBullet";
        // set damage and empowered status
        bullet.GetComponent<Bullet>().SetBulletParameters(bulletParameters.dmg, bulletParameters.isEmpowered);
        // subscribe to enemy hit event
        bullet.GetComponent<Bullet>().enemyHitEvent.AddListener(onEnemyHit);
        // add force
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(bulletParameters.up * bulletSpeed, ForceMode2D.Impulse);
    }
}

public class BulletParameters
{
    public BulletParameters(int dmg, bool isEmpowered, int delay, GameObject chosenBulletPrefab, Vector3 position, Quaternion rotation, Vector3 up)
    {
        this.dmg = dmg;
        this.isEmpowered = isEmpowered;
        this.delay = delay;
        this.chosenBulletPrefab = chosenBulletPrefab;
        this.position = position;
        this.rotation = rotation;
        this.up = up;
    }

    public BulletParameters(BulletParameters bulletParameters)
    {
        dmg = bulletParameters.dmg;
        isEmpowered = bulletParameters.isEmpowered;
        delay = bulletParameters.delay;
        chosenBulletPrefab = bulletParameters.chosenBulletPrefab;
        position = bulletParameters.position;
        rotation = bulletParameters.rotation;
        up = bulletParameters.up;
    }
    public int dmg;
    public bool isEmpowered;
    public float delay;
    public GameObject chosenBulletPrefab;
    public Vector3 position;
    public Vector3 up;
    public Quaternion rotation;
}
