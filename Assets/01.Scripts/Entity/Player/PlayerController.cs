using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool _canParry;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //����ü�� �ĵ��� ª��
    //���� ������ ������ �ĵ��� ���, �˹� ����
    private void Parry()
    {
        //���콺 ������ ���⿡ ���� ��ä�÷� �и��ǰ�
        //�� �ݶ��̴� ��ȯ�Ͽ� ��� ������Ʈ �Ǻ�
        //RaycastHit2D hit = ;
        //���콺 �����Ϳ� �÷��̾� ���� ���� Ȯ��

        //�ش� ������ ����, ����� ��쿡 �и� ���� �ֱ�
        //foreach������ Ȯ��

        ParryCool(.5f);
    }

    private void ParryEffect()
    {

    }

    IEnumerator ParryCool(float time)
    {
        _canParry = false;
        yield return new WaitForSeconds(time);
        _canParry = true;
    }
}
