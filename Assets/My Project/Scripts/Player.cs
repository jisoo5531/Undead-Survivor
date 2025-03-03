using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVector;
    public float speed;

    public Scanner scanner;
    private Rigidbody2D rigid;
    private SpriteRenderer spriter;
    private Animator anim;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
    }

    private void OnEnable()
    {
        speed *= Character.Speed;
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }

    private void Update()
    {
        if (false == GameManager.instance.isLive)
            return;

    }

    private void FixedUpdate()
    {
        if (false == GameManager.instance.isLive)
            return;
        
        Vector2 nextVec = inputVector * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    private void LateUpdate()
    {
        if (false == GameManager.instance.isLive)
            return;

        anim.SetFloat("Speed", inputVector.magnitude);

        if (inputVector.x != 0)
        {
            spriter.flipX = inputVector.x < 0;
        }
    }

    private void OnMove(InputValue value)
    {
        inputVector = value.Get<Vector2>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (false == GameManager.instance.isLive)
            return;

        GameManager.instance.health -= Time.deltaTime * 10;

        if (GameManager.instance.health < 0)
        {
            for (int i = 2; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }
}
