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
            .SetLoops(-1, LoopType.Yoyo) // 무한 반복
            .SetEase(Ease.InOutSine); // 부드럽게 페이드
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("ChallengeScene");
        }
    }
}
