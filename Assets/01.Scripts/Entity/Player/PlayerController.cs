using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _knockbackPlaceHolder;





    private Player player;
    private Rigidbody2D _rigid;

    private bool _dashCan;
    private bool _parryCan;

    private int _stare;

    private bool _isJump;
    public float _Jump;
    public float _MaxSpeed;

    private Collider2D _collider2D;


    [SerializeField] private float _parryRadius;
    private void Start()
    {
        player = GetComponent<Player>();
        _rigid = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        StartCoroutine(ParryCool());
        StartCoroutine(DashCool());
    }

    private void Update()
    {
        IsAttached();
        PlayerJump();
        StaminaGen();
        PlayerParry();
        PlayerDash();
    }

    private void FixedUpdate()
    {
        PlayerMove();

    }

    // �÷��̾� ������ ����
    private void PlayerMove()
    {
        float h = Input.GetAxisRaw("Horizontal");

        _rigid.AddForce(Vector2.right * h * player.Stat.Get(StatType.MoveSpeed), ForceMode2D.Impulse);

        if (_rigid.velocity.x != 0)
            _stare = _rigid.velocity.x > 0 ? 1 : -1;

        if (Mathf.Abs(_rigid.velocity.x) > _MaxSpeed)
        {
            _rigid.velocity = new Vector2(_MaxSpeed * _stare, _rigid.velocity.y);
        }
    }
 
    // �÷��̾� ���� ����
    private void PlayerJump()
    {
        /*if (Input.GetButtonDown("Jump") && !_isJump)
        {
            _isJump = true;
            _rigid.velocity = new Vector2(_rigid.velocity.x, player.Stat.Get(StatType.JumpForce));
        }*/
        if (Input.GetButtonDown("Jump"))
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector3(transform.position.x, _collider2D.bounds.min.y), Vector2.down, 0.1f);
            if (hit.collider is not null)
            {
                _rigid.velocity = new Vector2(_rigid.velocity.x, player.Stat.Get(StatType.JumpForce));
            }
        }
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
                        t.Damage(player.Stat.Get(StatType.ParryingAttackForce));
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

            player.DashStamina -= player.Stat.Get(StatType.ParryingCost);
            StartCoroutine(ParryCool());
        }
    }

    IEnumerator ParryCool()
    {
        _parryCan = false;
        yield return new WaitForSeconds(player.Stat.Get(StatType.ParryingTime));
        _parryCan = true;
    }

    private void StaminaGen()
    {
        if (player.DashStamina < player.MaxDashStamina)
            player.DashStamina += Time.deltaTime * 3;
        else
            player.DashStamina = player.MaxDashStamina;
        if (player.ParryingStamina < player.MaxParryingStamina)
            player.ParryingStamina += Time.deltaTime * 3;
        else
            player.ParryingStamina = player.MaxParryingStamina;
    }

    private void PlayerDash()
    {
        //������ ��Ȳ���� Ȯ��
        if (_dashCan && Input.GetKeyDown(KeyCode.LeftShift))
        {
            //����ĳ��Ʈ ���
            Debug.DrawRay(transform.position, new Vector3(_stare * player.Stat.Get(StatType.DashLength), 0, 0), Color.green, 0.7f);
            //���̾� ����ũ�� ���Ŀ� ����Ƽ �������� �߰��ϰ� �ڵ忡�� �߰�
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(_stare, 0), player.Stat.Get(StatType.DashLength), LayerMask.GetMask("DashStop"));
            //����ĳ��Ʈ ������
            if (hit.collider != null)
                transform.position = new Vector2(hit.transform.position.x + -_stare * .6f, transform.position.y);
            //�ƴϸ� �̵�
            else
                transform.Translate(new Vector2(_stare * player.Stat.Get(StatType.DashLength), 0));

            player.DashStamina -= player.Stat.Get(StatType.DashCost);
            StartCoroutine(DashCool());
        }
    }

    IEnumerator DashCool()
    {
        //�뽬 ���� ��� ���߰�
        _rigid.velocity = Vector2.zero;

        _dashCan = false;
        yield return new WaitForSeconds(player.Stat.Get(StatType.DashCooldown));
        _dashCan = true;
    }

    private void IsAttached()
    {
        Vector3 floorSpot = transform.position + new Vector3(0, -1.1f, 0);
        Debug.DrawRay(floorSpot, new Vector2(0, -.1f), Color.green, 0.1f);
        RaycastHit2D hit = Physics2D.Raycast(floorSpot, Vector2.down, -0.2f);
        if (hit.collider != null)
            _isJump = false;
        else
            _isJump = true;
    }
}
