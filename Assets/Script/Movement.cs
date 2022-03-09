using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update

    Vector3 dir = Vector3.up;
    float xBound;
    float yBound;
    Vector3 prevPosition;

    [SerializeField] float speed = 0.5f;
    [SerializeField] GameObject food;
    [SerializeField] GameObject tail;

    void Start()
    {
        InvokeRepeating("MoveSnake", 0.3f, 0.3f);
        xBound = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        yBound = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) & dir != Vector3.down)
        {
            dir = Vector3.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) & dir != Vector3.up)
        {
            dir = Vector3.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) & dir != Vector3.right)
        {
            dir = Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) & dir != Vector3.left)
        {
            dir = Vector3.right;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Food")
        {
            Destroy(other.gameObject);
            AddTail();
            GenarateNewFood();
        } else {
            CancelInvoke();

        }
    }

    private void GenarateNewFood()
    {
        Debug.Log("EAT");
        Instantiate(food, new Vector3(UnityEngine.Random.Range(-xBound + speed, xBound - speed), UnityEngine.Random.Range(-yBound + speed, yBound - speed), 0), Quaternion.identity);
    }

    private async void AddTail()
    {
        Instantiate(tail, prevPosition, Quaternion.identity).transform.SetParent(transform);
    }

    void MoveSnake()
    {
        prevPosition = transform.GetChild(0).transform.position;
        transform.GetChild(0).transform.position = transform.GetChild(0).transform.position + (dir * speed);
        for (int i = 1; i < transform.childCount; i++)
        {
            Vector3 curPosition = transform.GetChild(i).transform.position;
            transform.GetChild(i).transform.position = prevPosition;
            prevPosition = curPosition;
        }
    }
}
