using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject snakeBody;

    Vector3 direction = Vector3.zero;

    List<Transform> segments = new List<Transform>();

    Vector3 bound;
    void Start()
    {
        segments.Add(transform);
        InvokeRepeating("MoveSnake", 0.2f, 0.2f);
        bound = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) & direction != Vector3.down)
        {
            direction = Vector3.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) & direction != Vector3.up)
        {
            direction = Vector3.down;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) & direction != Vector3.left)
        {
            direction = Vector3.right;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) & direction != Vector3.right)
        {
            direction = Vector3.left;
        }
    }

    private async void MoveSnake()
    {
        for (int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i - 1].position;
        }
        transform.position = transform.position + direction;
        if (Mathf.Floor(transform.position.x) > Mathf.Floor(Mathf.Abs(bound.x)))
        {
            transform.position = new Vector3(-Mathf.Floor(Mathf.Abs(bound.x)), transform.position.y, 0);
        }
        else if (Mathf.Floor(transform.position.x) < -Mathf.Floor(Mathf.Abs(bound.x)))
        {
            transform.position = new Vector3(Mathf.Floor(Mathf.Abs(bound.x)), transform.position.y, 0);
        }
        if (Mathf.Floor(transform.position.y) > Mathf.Floor(Mathf.Abs(bound.y)))
        {
            transform.position = new Vector3(transform.position.x, -Mathf.Floor(Mathf.Abs(bound.y)), 0);
        }
        else if (Mathf.Floor(transform.position.y) < -Mathf.Floor(Mathf.Abs(bound.y)))
        {
            transform.position = new Vector3(transform.position.x, Mathf.Floor(Mathf.Abs(bound.y)), 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Food")
        {
            Grow();
            ChangeFoodPosition(other.gameObject);
        }
        else if (other.tag == "Obstacles")
        {
            CancelInvoke();
            StartCoroutine(RestartGame());
        }
    }

    private void ChangeFoodPosition(GameObject gameObject)
    {
        float newXPosition = UnityEngine.Random.Range(-Mathf.Floor(Mathf.Abs(bound.x)), Mathf.Floor(Mathf.Abs(bound.x)));
        float newYPosition = UnityEngine.Random.Range(-Mathf.Floor(Mathf.Abs(bound.y)), Mathf.Floor(Mathf.Abs(bound.y)));
        gameObject.transform.position = new Vector3(Mathf.Floor(newXPosition), Mathf.Floor(newYPosition), 0);
    }

    public IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 1; i < segments.Count; i++)
        {
            Destroy(segments[i].gameObject);
        }
        segments.Clear();
        segments.Add(transform);
        direction = Vector3.zero;
        InvokeRepeating("MoveSnake", 0.2f, 0.2f);
    }

    private void Grow()
    {
        GameObject body = Instantiate(snakeBody);
        body.transform.position = segments[segments.Count - 1].position;
        segments.Add(body.transform);
        StartCoroutine(ChangeTag(body));
    }

    public IEnumerator ChangeTag(GameObject body)
    {
        yield return new WaitForSeconds(0.3f);

        body.tag = "Obstacles";

        yield return null;
    }

    public void MobileControl(string dir)
    {
        if (dir == "UP" & direction != Vector3.down)
        {
            direction = Vector3.up;
        }
        else if (dir == "DOWN" & direction != Vector3.up)
        {
            direction = Vector3.down;
        }
        if (dir == "RIGHT" & direction != Vector3.left)
        {
            direction = Vector3.right;
        }
        else if (dir == "LEFT" & direction != Vector3.right)
        {
            direction = Vector3.left;
        }
    }


}
