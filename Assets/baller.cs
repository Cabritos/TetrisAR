using System.Collections;
using System.Collections.Generic;
using easyar;
using UnityEditor;
using UnityEngine;

public class baller : MonoBehaviour
{

    public GameObject _prefab;
    public Camera _cam;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            {
                Ray r = _cam.ScreenPointToRay(Input.mousePosition);

                Vector3 dir = r.GetPoint(1) - r.GetPoint(0);

                // position of spanwed object could be 'GetPoint(0).. 1.. 2' half random choice ;)
                GameObject bullet = Instantiate(_prefab, r.GetPoint(2), Quaternion.LookRotation(dir));

                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 20;
                Destroy(bullet, 3);
            }

        }
    }
}
