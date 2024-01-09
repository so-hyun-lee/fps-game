using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
    }

    //���� ���� ���
    public enum GameState
    {
        Ready,
        Go,
        Pause,
        GameOver
    }

    public GameState gState;

    //UI������Ʈ ����
    public GameObject gameLabel;
    //���� ���� �ؽ�Ʈ
    Text gameText;
    public GameObject gameOption;

    // Start is called before the first frame update
    void Start()
    {
        //�ʱ� ���¸� �غ�� ����
        gState = GameState.Ready;
        gameText = gameLabel.GetComponent<Text>();
        gameText.text = "Ready";

        StartCoroutine(ReadyToStart());
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ReadyToStart()
    {
        //2�� ���
        yield return new WaitForSeconds(2f);
        //go ����
        gameText.text = "Go!";
        //���
        yield return new WaitForSeconds(0.5f);
        //�ؽ�Ʈ ��Ȱ��ȭ
        gameLabel.SetActive(false);
        //���� ����
        gState = GameState.Go;
    }

    public void OpenOption()
    {
        gameOption.SetActive(true);
        Time.timeScale = 0;
        gState = GameState.Pause;
    }

    public void CloseOption()
    {
        gameOption.SetActive(false);
        Time.timeScale = 1;
        gState = GameState.Go;
    }

}
