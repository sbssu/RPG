using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Button connectButton;              // 연결 버튼.
    [SerializeField] TMP_InputField nameInputField;     // 이름 입력 필드.
    [SerializeField] TMP_Text statusText;               // 네트워크 상태 텍스트.

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();                   // 포톤 마스터 서버 접속 (외부 기본 세팅값을 사용)
        connectButton.onClick.AddListener(ConnectToRoom);       // 버튼에 이벤트 함수 등록.
        statusText.text = "Ready..";
        nameInputField.ActivateInputField();
    }    

    private void ConnectToRoom()
    {
        if (string.IsNullOrEmpty(nameInputField.text))
        {
            Debug.Log("닉네임을 입력해주세요.");
            return;
        }

        PhotonNetwork.LocalPlayer.NickName = nameInputField.text;   // 로컬플레이어(=나)의 닉네임을 설정.
        PhotonNetwork.JoinRandomRoom();                             // 랜덤한 룸에 접속.
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"OnJoinRandomFailed : {message}({returnCode})");
        PhotonNetwork.CreateRoom("채팅테스트", new Photon.Realtime.RoomOptions { MaxPlayers = 10 });
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.IsMessageQueueRunning = true;      // 메세지 큐를 수신하겠다.
        PhotonNetwork.LoadLevel("2.Game");               // 채팅 씬으로 이동.
    }
    private void Update()
    {
        statusText.text = PhotonNetwork.NetworkClientState.ToString();  // 네트워크 상태 표시.
        if (!PhotonNetwork.IsConnected)
            return;

        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && !string.IsNullOrEmpty(nameInputField.text))
            ConnectToRoom();
    }
    
}

