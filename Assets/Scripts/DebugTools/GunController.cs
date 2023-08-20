using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject hazardPrefab;
    public Transform firePoint;
    public float hazardSpeed = 10f;

    public void Fire()
    {
        GameObject hazard = Instantiate(hazardPrefab, firePoint.position, firePoint.rotation);
        hazard.GetComponent<Rigidbody>().velocity = firePoint.forward * hazardSpeed;
        Destroy(hazard, 5f);
    }
}
