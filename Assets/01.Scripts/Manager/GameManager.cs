using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// ���� ���� ���¸� ǥ���ϰ�, ���� ������ UI�� �����ϴ� ���� �Ŵ���
// ������ �� �ϳ��� ���� �Ŵ����� ������ �� �ִ�.
public class GameManager : MonoBehaviour
{
    public static GameManager instance; // �̱����� �Ҵ��� ���� ����

    public bool IsGameover = false; // ���� ���� ����
    public TextMeshProUGUI ScoreText; // ������ ����� UI �ؽ�Ʈ
    public GameObject GameoverUI; // ���� ������ Ȱ��ȭ �� UI ���� ������Ʈ
    public float LimitTime;
    public TextMeshProUGUI TimerText;

    private int _score = 0; // ���� ����
    private float _timer = 0; // �ð� ����

    // ���� ���۰� ���ÿ� �̱����� ����
    void Awake()
    {
        // �̱��� ���� instance�� ����ִ°��
        if (instance == null)
        {
            // instance�� ����ִٸ�(null) �װ��� �ڱ� �ڽ��� �Ҵ�
            instance = this;
        }
        else
        {
            // instance�� �̹� �ٸ� GameManager ������Ʈ�� �Ҵ�Ǿ� �ִ� ���
            // ���� �ΰ� �̻��� GameManager ������Ʈ�� �����Ѵٴ� �ǹ�.
            // �̱��� ������Ʈ�� �ϳ��� �����ؾ� �ϹǷ� �ڽ��� ���� ������Ʈ�� �ı�
            Debug.LogWarning("���� �ΰ� �̻��� ���� �Ŵ����� �����մϴ�!");
            Destroy(gameObject);
        }

        Screen.SetResolution(1920, 1080, true);
    }

    //�ð� ������Ű�� �޼��� 
    void Start()
    {
        TimerText = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        _timer = 0;
        while (true)
        {
            _timer += Time.deltaTime;
            TimerText.text = "�ð� : " + Mathf.Round(_timer);
            yield return null;
        }
    }

    void Update()
    {
        // ���� ���� ���¿��� ������ ������� �� �ְ� �ϴ� ó��
        if (IsGameover && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
    }
    // �÷��̾� ĳ���Ͱ� ����� ���� ������ �����ϴ� �޼���
    public void OnPlayerDead()
    {
        IsGameover = true;
        GameoverUI.SetActive(true);
    }



    /*������ ������Ű�� �޼���
    public void AddScore(int newScore)
    {
        if (isGameover)
        {
            _score += newScore;
            ScoreText.text = "Score" + _score;  
        }
    }*/
}

