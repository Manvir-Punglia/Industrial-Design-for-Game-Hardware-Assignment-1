using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float bulletSpeed = 3f;
    public float fireRate = 0.5f;

    public float burstRate = 0.1f;
    public int burstCount = 3;
    private float nextFireTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= nextFireTime)
        {
            Shoot();
            // Update the next fire time
        }
        
        if (Input.GetKeyDown(KeyCode.Q) && Time.time >= nextFireTime)
        {
            StartCoroutine(BurstShot());
            // Update the next fire time
        }
    }


    public void Shoot()
    {
        Debug.Log("Shoot");
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();


        bulletRb.AddForce(shootPoint.forward * bulletSpeed, ForceMode.Impulse);
        nextFireTime = Time.time + fireRate;

        Destroy(bullet, 5f);
    }

    public void Burst()
    {
        StartCoroutine(BurstShot());
    }

     public IEnumerator BurstShot()
    {
        for (int i = 0; i < burstCount; i++)
        {
            Shoot(); // Fire a single bullet
            yield return new WaitForSeconds(burstRate); // Wait for the burst interval
        }
    }
}