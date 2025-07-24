using UnityEngine;

public class ShootSystem : MonoBehaviour
{
    public float range = 100f;
    public float damage = 10f;
    public float fireRate = 10f;
    private float nextTimeToFire = 0f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public GunRecoil recoilScript;
    public AudioSource gunAudio;
    public AudioClip audioClip;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        if (muzzleFlash != null) muzzleFlash.Play();
        if (recoilScript != null) recoilScript.ApplyRecoil();
        if (gunAudio != null && audioClip != null) gunAudio.PlayOneShot(audioClip);

        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.Log("Hit: " + hit.collider.name);

            Target target = hit.transform.GetComponentInParent<Target>();
            if (target != null)
            {
                // Create directional force
                Vector3 force = -hit.normal * 20f;

                // Add randomness to make ragdoll look more natural
                force += new Vector3(
                    Random.Range(-2f, 2f),
                    Random.Range(0f, 3f),
                    Random.Range(-2f, 2f)
                );

                target.TakeDamage(damage, hit.collider, force, hit.point);
            }

            if (impactEffect != null)
            {
                GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 1f);
            }
        }
    }
}
