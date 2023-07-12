using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkulBoss : Boss
{
    [SerializeField] private GameObject _knifeTrap;

    private bool _isActing;
    private int _aiMode;
    private float _distance;

    private Player _player;
    private SpriteRenderer _renderer;
    private Animator _animator;

    private float _moveSpeed;
    private int _stare;

    private static float _backSpeed;
    private static float _frontSpeed;
    private static float _limitRange;

    [SerializeField] Vector2 _knifeThrowSpot;
    [SerializeField] BossAttackProjectile _knifePrefab;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _moveSpeed = _frontSpeed;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (_isActing)
            AIAct();
    }
    private void Staring()
    {
        if (_player.transform.position.x < transform.position.x)
        {
            _stare = -1;
            _renderer.flipX = true;
        }
        else
        {
            _stare = 1;
            _renderer.flipX = false;
        }
    }

    public void AIAct()
    {
        switch (_aiMode)
        {
            //������ �ִ� ������
            //�÷��̾� �ִ� ���� ó�ٺ���
            //���� ������ ���� �÷��̾�� �ǽð����� �Ÿ� ��α�
            //���Ͽ� ���� �ʿ��ϴ� �����ϰ� �÷��̾� ���� or �ݴ�������� �̵��ϱ�
            case 0:
                Staring();
                _distance = Mathf.Abs(_player.transform.position.x - transform.position.x);
                if (_distance > _limitRange)
                    MovingVelocity = _stare * _moveSpeed;
                else
                    MovingVelocity = _stare * _backSpeed;
                break;
            //���� 1
            case 1:
                StartCoroutine(Spin());
                break;
            //����1 ����
            case 5:
                MovingVelocity = _stare * _moveSpeed * 1.2f;
                break;
            case 2:
                StartCoroutine(KnifeStepping());
                break;
            case 3:
                StartCoroutine(KnifeThrowing());
                break;
        }

    }
    public void ChooseNextAct()
    {
        int minLimit = 1;
        int maxLimit = 4;
        //�ٰŸ�
        //Į ���ϸ�
        if (_distance < _player.Stat.Get(StatType.DashLength) * 1.5f)
            maxLimit = 2;
        //���Ÿ�
        //Į�� ����
        else if (_distance > _player.Stat.Get(StatType.DashLength) * 4.5f)
            minLimit = 2;
        //�߰Ÿ�
        //���� ��
        _aiMode = Mathf.FloorToInt(Random.Range(minLimit, maxLimit));

        //��� ���¿��� �÷��̾�� �ٰ�����, �־����� �Ǵ�

    }

    IEnumerator Spin()
    {
        _animator.SetBool("Spin", true);
        _aiMode = 5;

        yield return new WaitForSeconds(10.0f);
        StartCoroutine(PatternTerm());
    }
    IEnumerator KnifeStepping()
    {
        _isActing = false;
        _knifeTrap.GetComponent<Animator>().SetTrigger("Shoot");
        yield return new WaitForSeconds(3.0f);
        _knifeTrap.GetComponent<Animator>().SetTrigger("Shoot");
        yield return new WaitForSeconds(2.0f);
        _knifeTrap.GetComponent<Animator>().SetTrigger("Shoot");
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(PatternTerm());
    }
    IEnumerator KnifeThrowing()
    {
        _isActing = false;
        float x = Random.Range(2, 5);
        for (int i = 0; i < x; i++)
        {
            BossAttackProjectile copy = Instantiate(_knifePrefab, _knifeThrowSpot, transform.rotation);
            yield return new WaitForSeconds(2.0f);
            copy.Fire();
            yield return new WaitForSeconds(1.5f);
        }
        StartCoroutine(PatternTerm());
    }
    IEnumerator PatternTerm()
    {
        _aiMode = 0;
        _isActing = true;
        yield return new WaitForSeconds(5.0f);
        ChooseNextAct();
    }
}
/*
 * ����1: ȸ��. �ٰ߱Ÿ��϶� ������ ����.
 * ����2: Į�� �ٴڿ��� �ö���� ����. �ִϸ��̼����� ó��. �ٰŸ� or ���Ÿ��϶� ����ϴ� ����     
 * ����3: 
 */