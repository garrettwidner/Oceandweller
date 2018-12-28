using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHorizontalMover : MonoBehaviour
{
    public float speed;
    public float length;
    private Vector2 startLocation;
    private bool isMovingRight = true;

    private Vector2 rightMost;
    private Vector2 leftMost;

    private void Start()
    {
        isMovingRight = (Random.RandomRange(0, 1) == 1 ? true : false);

        if (length <= .2)
        {
            length = .2f;
        }

        startLocation = transform.position;
        rightMost = leftMost = transform.position;
        rightMost.x += .5f * length;
        leftMost.x -= .5f * length;

        print(rightMost);
        print(leftMost);
    }

    private void Update()
    {
        if(isMovingRight)
        {
            Vector2 newVector = transform.position;
            newVector.x += speed * Time.deltaTime;
            transform.position = newVector;

            if(newVector.x > rightMost.x)
            {
                isMovingRight = false;
            }
        }
        else
        {
            Vector2 newVector = transform.position;
            newVector.x -= speed * Time.deltaTime;
            transform.position = newVector;

            if(newVector.x < leftMost.x)
            {
                isMovingRight = true;
            }
        }
    }

}
