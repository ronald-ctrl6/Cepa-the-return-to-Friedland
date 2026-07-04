using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamara : MonoBehaviour
{
    [SerializeField] private float speedX;
    [SerializeField] private float speedY;
    [SerializeField] private bool autoMoveX;
    [SerializeField] private bool autoMoveY;
    // Update is called once per frame
    private void Update()
    {
        float moveX = autoMoveX ? speedX * Time.deltaTime : Input.GetAxis("Horizontal") * speedX * Time.deltaTime;
        float moveY = autoMoveY ? speedY * Time.deltaTime : Input.GetAxis("Vertical") * speedY * Time.deltaTime;
        transform.position += new Vector3(moveX, moveY, 0);
    }
}
