using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player _player;
    private Rigidbody2D _rigid;
    private int _platformLayer;

    private bool _dashCan;
    private bool _parryCan;

    private int _stare;

    private bool _canJump = false;

    private Collider2D _collider2D;


    [SerializeField] private float _parryRadius;
    private void Start()
    {
        _player = GetComponent<Player>();
        _rigid = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        _platformLayer = LayerMask.NameToLayer("Platform");
        StartCoroutine(ParryCool());
        StartCoroutine(DashCool());
    }

    private void Update()
    {
        PlayerJump();
        StaminaGen();
        PlayerParry();
        PlayerDash();
        PlayerMove();
    }

    // �÷��̾� ������ ����
    private void PlayerMove()
    {
        float h = Input.GetAxisRaw("Horizontal");

        _player.MovingVelocity = h * _player.Stat.Get(StatType.MoveSpeed);

        if (Mathf.Abs(h) > Mathf.Epsilon)
            _stare = h > 0 ? 1 : -1;
    }
 
    // �÷��̾� ���� ����
    private void PlayerJump()
    {
        if (Input.GetButton("Jump") && _canJump)
        {
            _canJump = false;
            _rigid.velocity = new Vector2(_rigid.velocity.x, _player.Stat.Get(StatType.JumpForce));
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        bool isOnGround = false;
        for(int i = 0; i < collision.contactCount; i++)
        {
            if(_collider2D.bounds.min.y + 0.1f >= collision.GetContact(i).point.y)
            {
                isOnGround = true;
                break;
            }
        }
        if ((collision.gameObject.layer & _platformLayer) != 0 && isOnGround)
        {
            _canJump = true;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((collision.gameObject.layer & _platformLayer) != 0)
            _canJump = false;
    }

    private void PlayerParry()
    {
        if (_parryCan && Input.GetMouseButtonDown(0))
        {
            //�� �ݶ��̴� ����
            RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, _parryRadius, Vector2.zero);
            //�÷��̾�� ���콺 ���� �������ϱ�
            float parryDirection = ExtraMath.DirectionToAngle(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            //������ ����, ����ü���� Ȯ��
            foreach (RaycastHit2D inst in hit)
            {

                if (inst.transform.TryGetComponent(out Entity t) && t is not Player)
                {
                    print(inst.collider.gameObject);
                    //���� �ش� ����/���� �ȿ� �ִ��� Ȯ��
                    float MonsterDirection = ExtraMath.DirectionToAngle(inst.transform.position - transform.position);
                    //�ش� ������ �������� �߰�: �� 50���� ���� ���� ����
                    //�и� ����
                    //���Ͱ� ���������� �Ǵ��ؼ� �и� �������� �Ǵ�
                    if (Mathf.Abs(parryDirection - MonsterDirection) < 25)
                    {
                        //�и� ���� ���� ����
                        t.Damage(_player.Stat.Get(StatType.ParryingAttackForce));
                        //�и� �� ȿ��
                        //������ �÷��̾ �ڷ� �и��°ɷ�
                        /*if (t is MeleeMonster)
                        {
                            //�������� ��� ~90��, 270~
                            //������ ��� 90< �������� <270
                            _rigid.velocity = new Vector2(_knockbackPlaceHolder * , 0);
                        }*/

                    }



                }

            }

            _player.DashStamina -= _player.Stat.Get(StatType.ParryingCost);
            StartCoroutine(ParryCool());
        }
    }

    IEnumerator ParryCool()
    {
        _parryCan = false;
        yield return new WaitForSeconds(_player.Stat.Get(StatType.ParryingTime));
        _parryCan = true;
    }

    private void StaminaGen()
    {
        if (_player.DashStamina < _player.MaxDashStamina)
            _player.DashStamina += Time.deltaTime * 3;
        else
            _player.DashStamina = _player.MaxDashStamina;
        if (_player.ParryingStamina < _player.MaxParryingStamina)
            _player.ParryingStamina += Time.deltaTime * 3;
        else
            _player.ParryingStamina = _player.MaxParryingStamina;
    }

    private void PlayerDash()
    {
        //������ ��Ȳ���� Ȯ��
        if (_dashCan && Input.GetKeyDown(KeyCode.LeftShift))
        {
            //����ĳ��Ʈ ���
            Debug.DrawRay(transform.position, new Vector3(_stare * _player.Stat.Get(StatType.DashLength), 0, 0), Color.green, 0.7f);
            //���̾� ����ũ�� ���Ŀ� ����Ƽ �������� �߰��ϰ� �ڵ忡�� �߰�
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(_stare, 0), _player.Stat.Get(StatType.DashLength), LayerMask.GetMask("DashStop"));
            //����ĳ��Ʈ ������
            if (hit.collider != null)
                transform.position = new Vector2(hit.transform.position.x + -_stare * .6f, transform.position.y);
            //�ƴϸ� �̵�
            else
                transform.Translate(new Vector2(_stare * _player.Stat.Get(StatType.DashLength), 0));

            _player.DashStamina -= _player.Stat.Get(StatType.DashCost);
            StartCoroutine(DashCool());
        }
    }

    IEnumerator DashCool()
    {
        //�뽬 ���� ��� ���߰�
        _rigid.velocity = Vector2.zero;

        _dashCan = false;
        yield return new WaitForSeconds(_player.Stat.Get(StatType.DashCooldown));
        _dashCan = true;
    }
}
