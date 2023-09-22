using System;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private int _distance = 75;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _shootForce;
    [SerializeField] private float _upwardForce;
    
    public void Shoot()
    {
        var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out var hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(_distance);

        var path = targetPoint - _shootPoint.position;

        var currentBullet = Instantiate(_bulletPrefab, _shootPoint.position, _bulletPrefab.transform.rotation);
        
        currentBullet.GetComponent<Rigidbody>().AddForce(path.normalized * _shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(Camera.main.transform.up * _upwardForce,ForceMode.Impulse);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Shoot();
        }
    }
}