using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Player player;
    private Rigidbody2D _rigid;

    private bool _dashCan;

    private int _stare;

    private bool _isJump;
    public float _Jump;
    public float _MaxSpeed;


    [SerializeField] private float _parryRadius;
    private void Start()
    {
        player = GetComponent<Player>();
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        PlayerJump();
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
    private void PlayerJump()
    {
        if (Input.GetButtonDown("Jump") && !_isJump)
        {
            _isJump = true;
            _rigid.velocity = new Vector2(_rigid.velocity.x, _Jump);
        }
    }

    private void PlayerParry()
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
                //���� �ش� ����/���� �ȿ� �ִ��� Ȯ��
                float MonsterDirection = ExtraMath.DirectionToAngle(inst.transform.position - transform.position);
                //�ش� ������ �������� �߰�
                //�и� ����
                //���Ͱ� ���������� �Ǵ��ؼ� �и� �������� �Ǵ�
                if (Mathf.Abs(parryDirection - MonsterDirection) < 12)
                {
                    //�и� ���� ���� ����
                    t.Damage(player.Stat.Get(StatType.ParryingAttackForce));
                    //�и� �� ȿ��
                    if (t is MeleeMonster)
                    {
                        //�ٰŸ��� ��� �ڷ� �и�
                    }
                    //���Ÿ��� �ǵ��X

                }



            }

        }

        player.DashStamina -= player.Stat.Get(StatType.parry);
        StartCoroutine(ParryCool());
    }

    IEnumerator ParryCool()
    {
        yield return new WaitForSeconds(player.Stat.Get(StatType.ParryingTime));
    }

    private void StaminaGen()
    {

    }

    private void PlayerDash()
    {
        //������ ��Ȳ���� Ȯ��
        if (_dashCan)
        {
            //����ĳ��Ʈ ���
            Debug.DrawRay(transform.position, new Vector3(_stare * player.Stat.Get(StatType.DashLength), 0, 0), Color.green, 0.7f);
            //���̾� ����ũ�� ���Ŀ� ����Ƽ �������� �߰��ϰ� �ڵ忡�� �߰�
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(_stare, 0), player.Stat.Get(StatType.DashLength));
            //����ĳ��Ʈ ������
            if (hit.collider != null)
                transform.position = new Vector2(hit.transform.position.x, transform.position.y);
            //�ƴϸ� �̵�
            else
                transform.Translate(new Vector2(_stare * player.Stat.Get(StatType.DashLength), 0));
            _dashCan = false;
            StartCoroutine(DashCool());
        }
    }

    IEnumerator DashCool()
    {
        yield return new WaitForSeconds(player.Stat.Get(StatType.DashCooldown));
        _dashCan = true;
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
