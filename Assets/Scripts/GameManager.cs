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

    //게임 상태 상수
    public enum GameState
    {
        Ready,
        Go,
        Pause,
        GameOver
    }

    public GameState gState;

    //UI오브젝트 변수
    public GameObject gameLabel;
    //게임 상태 텍스트
    Text gameText;
    public GameObject gameOption;

    // Start is called before the first frame update
    void Start()
    {
        //초기 상태를 준비로 설정
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
        //2초 대기
        yield return new WaitForSeconds(2f);
        //go 띄우기
        gameText.text = "Go!";
        //대기
        yield return new WaitForSeconds(0.5f);
        //텍스트 비활성화
        gameLabel.SetActive(false);
        //상태 변경
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
