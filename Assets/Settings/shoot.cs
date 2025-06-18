using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Oculus.Haptics;


public class GunShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletForce = 700f;

    public ParticleSystem muzzleFlash;     // Muzzle flash particle system
    public Animator gunAnimator;           // Reference to the Animator
    public AudioSource gunShotAudio;       // Gunshot sound

    public HapticSource hapticSource;      // 👈 Assign this via Inspector

    private InputDevice rightController;
    private bool hasFired = false;

    void Start()
    {
        TryInitializeRightController();
    }

    void Update()
    {
        if (!rightController.isValid)
        {
            TryInitializeRightController();
            return;
        }

        bool triggerPressed;
        if (rightController.TryGetFeatureValue(CommonUsages.triggerButton, out triggerPressed))
        {
            if (triggerPressed && !hasFired)
            {
                Shoot();
                hasFired = true;
            }
            else if (!triggerPressed)
            {
                hasFired = false;
            }
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        // Spawn bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(firePoint.forward * bulletForce);
        }

        // Muzzle flash
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        // Gun animation
        if (gunAnimator != null)
        {
            gunAnimator.SetTrigger("trig");
        }

        // Gunshot sound
        if (gunShotAudio != null)
        {
            gunShotAudio.Play();
        }

        // Haptic feedback
        if (hapticSource != null)
        {
            hapticSource.Play();  // 👈 Trigger the haptic pulse
        }
    }

    void TryInitializeRightController()
    {
        List<InputDevice> rightHandedControllers = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandedControllers);

        if (rightHandedControllers.Count > 0)
        {
            rightController = rightHandedControllers[0];
        }
    }
}
