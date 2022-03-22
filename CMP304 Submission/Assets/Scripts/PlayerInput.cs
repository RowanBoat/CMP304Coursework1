using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    Vector3 mousePos;

    private GameObject[] alerts;
    [SerializeField]
    private GameObject alert;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        alerts = GameObject.FindGameObjectsWithTag("Alert");

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("ALERT! (" + mousePos.x + "," + mousePos.y + ")");
            for (int i = 0; i < alerts.Length; i++)
                UnityEngine.Object.Destroy(alerts[i]);
            UnityEngine.Object.Instantiate(alert, new Vector3(mousePos.x, mousePos.y, -0.1f), Quaternion.identity);
        }
    }
}
