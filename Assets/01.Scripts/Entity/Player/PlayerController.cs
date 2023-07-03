using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D _rigid;

    private bool _isJump;
    public float _Jump;
    public float _MaxSpeed;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _PlayerJump();
    }

    private void FixedUpdate()
    {
        _PlayerMove();
        
    }

    // �÷��̾� ������ ����
    private void _PlayerMove()
    {
        float h = Input.GetAxisRaw("Horizontal");

        _rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (_rigid.velocity.x > _MaxSpeed)
        {
            _rigid.velocity = new Vector2(_MaxSpeed, _rigid.velocity.y);
        }
        
        else if (-_rigid.velocity.x > _MaxSpeed)
        {
            _rigid.velocity = new Vector2(-_MaxSpeed, _rigid.velocity.y);
        }
    }

    // �÷��̾� ���� ����
    private void _PlayerJump()
    {
        if (Input.GetButtonDown("Jump") && !_isJump)
        {
            _isJump = true;
            _rigid.velocity = new Vector2(_rigid.velocity.x, _Jump);
        }
    }

    // �÷��̾ �ٴڿ����� �����ϵ��� ����
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            _isJump = false;
        }
    }
}
