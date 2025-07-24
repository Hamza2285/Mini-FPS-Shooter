using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    public float recoilStrength = 2f;
    public float recoilRecoverySpeed = 5f;

    private float currentRecoil = 0f;
    private float recoilVelocity = 0f;

    private float originalY;

    void Start()
    {
        // Store initial Y rotation so we don’t zero it out
        originalY = transform.localEulerAngles.y;
    }

    void Update()
    {
        currentRecoil = Mathf.SmoothDamp(currentRecoil, 0f, ref recoilVelocity, 1f / recoilRecoverySpeed);

        // Only affect X (pitch), keep Y as is
        transform.localRotation = Quaternion.Euler(currentRecoil, originalY, 0f);
    }

    public void ApplyRecoil()
    {
        currentRecoil += recoilStrength;
        currentRecoil = Mathf.Clamp(currentRecoil, -30f, 30f);
    }
}
