using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardDecision : MonoBehaviour
{
    float time = 0;
    Vector3 mousePos;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            time = Time.time;
        }
    }
}
