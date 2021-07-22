using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    public float speed = 40f;

    public LayerMask blockingLayer;
    Rigidbody2D rigidBody2D;
    BoxCollider2D boxCollider2D;
    protected virtual void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    //자식만 쓸 수 있음
    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);
        boxCollider2D.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider2D.enabled = true;

        if(hit.transform == null)
        {
            // 움직이는 코드
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        return false;

    }
    
    IEnumerator SmoothMovement(Vector3 end) 
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        while(sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rigidBody2D.position, end, speed * Time.deltaTime);
            rigidBody2D.MovePosition(newPosition);
            yield return null;
        }
    }
    
    protected virtual void AttemptMove<T>(int xDir, int yDir)
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);

        if (hit.transform == null)
            return;

        T hitComponent = hit.transform.GetComponent<T>();
        if (!canMove && hitComponent != null)
            OnCantMove<T>(hitComponent);
        
    }

    protected abstract void OnCantMove<T>(T component)
        where T : Component;

}
