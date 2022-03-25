using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    Vector3 mousePos;

    private GameObject[] alerts;
    private GameObject[] alerts2;

    [SerializeField] private GameObject alert;
    [SerializeField] private GameObject alert2;

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        alerts = GameObject.FindGameObjectsWithTag("Alert");
        alerts2 = GameObject.FindGameObjectsWithTag("Alert2");

        if (Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < alerts.Length; i++)
                UnityEngine.Object.Destroy(alerts[i]);
            UnityEngine.Object.Instantiate(alert, new Vector3(mousePos.x, mousePos.y, -0.1f), Quaternion.identity);
        }

        if (Input.GetMouseButtonDown(1))
        {
            for (int i = 0; i < alerts2.Length; i++)
                UnityEngine.Object.Destroy(alerts2[i]);
            UnityEngine.Object.Instantiate(alert2, new Vector3(mousePos.x, mousePos.y, -0.1f), Quaternion.identity);
        }
    }
}
