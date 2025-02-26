using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class StartPage : MonoBehaviour
{
    [SerializeField] TMP_Text _touchToStart;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _touchToStart.DOFade(0, 1f)
            .SetLoops(-1, LoopType.Yoyo) // ���� �ݺ�
            .SetEase(Ease.InOutSine); // �ε巴�� ���̵�
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("ChallengeScene");
        }
    }
}
