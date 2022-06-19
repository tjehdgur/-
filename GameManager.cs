using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public bool Game_start = false; //게임 시작 체크

    public GameObject Character; //캐릭터

    public Transform Platform_Parents; // 정리를 위한 발판들의 부모 오브젝트

    public GameObject Platform; //발판 (계단)

    private List<GameObject> Platform_List = new List<GameObject>(); //발판 리스트 

    private List<int> Platform_Check_List = new List<int>(); //발판의 위치 리스트, 왼쪽: 0, 오른쪽: 1

    // Start is called before the first frame update
    void Start()
    {
        Data_Load();
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if(Game_start) { //키보드 입력 체크
            if(Input.GetKeyDown(KeyCode.RightArrow)) {
                Check_Platform(Character_Pos_Idx, 1);
            } else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
                Check_Platform(Character_Pos_Idx, 0);
            }
        }
    }

    public void Data_Load() //데이터 로드, 발판 오브젝트 생성
    {
        for(int i=0; i<20; i++) {
            GameObject t_Obj = Instantiate(Platform, Vector3.zero, Quaternion.identity);
            //Vector3.zero = Vector3(0, 0, 0);
            t_Obj.transform.parent = Platform_Parents;
            Platform_List.Add(t_Obj);
            Platform_Check_List.Add(0);
        }

        Platform.SetActive(false);
    }

    private int Pos_Idx = 0; //발판의 마지막 위치
    private int Character_Pos_Idx = 0; //캐릭터의 위치

    public void Init() //캐릭터, 발판 위치 초기화
    {
        Character.transform.position = Vector3.zero;

        for(Pos_Idx = 0; Pos_Idx < 20;) {
            Next_Platform(Pos_Idx);
        }

        Character_Pos_Idx = 0;
        Game_start = true; 
    }

    public void Next_Platform(int idx) {
        int pos = Random.Range(0,2);

        if(idx == 0) { //첫번쨰 발판의 경우
            Platform_List[idx].transform.position = new Vector3(0, -0.5f, 0);
        }else {
            if(pos == 0) { //왼쪽 발판의 경우
                Platform_Check_List[idx - 1] = pos;
                Platform_List[idx].transform.position = Platform_List[idx - 1].transform.position
                + new Vector3(-1f, 0.5f, 0);
            } else {
                Platform_Check_List[idx] = pos;
                Platform_List[idx].transform.position = Platform_List[idx - 1].transform.position
                + new Vector3(1f, 0.5f, 0);
           }
        }
        Pos_Idx++;
    }

    void Check_Platform(int idx, int direction)
    {
        Debug.Log("Idx : "+ idx + " /Platform : "+ Platform_Check_List[idx] + " /Direction : ");
        if(Platform_Check_List[idx] == direction) { //발판이 있음
            Character_Pos_Idx++;
            Character.transform.position = Platform_List[Character_Pos_Idx].transform.position + new Vector3(0f, 0.5f, 0);
        }else{
            Result();
        }
    }
    
    public void Result()
    {
        Debug.Log("Game Over");
        Game_start = false;
    }
}
