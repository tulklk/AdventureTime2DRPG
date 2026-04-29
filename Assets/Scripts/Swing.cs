using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    public float speed = 2.0f;
    public float angle = 20.0f;
    public int startDirection = 1; // 1 = phải trước, -1 = trái trước

    private float currentAngle = 0;
    private float timer;

    private void Start()
    {
        // Đảm bảo startDirection chỉ có giá trị 1 hoặc -1
        startDirection = (startDirection >= 0) ? 1 : -1;
    }

    private void Update()
    {
        timer += Time.deltaTime * speed;
        float swingAngle = Mathf.Sin(timer) * angle;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, swingAngle * startDirection + currentAngle));
    }
}