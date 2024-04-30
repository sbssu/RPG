using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Button connectButton;              // ���� ��ư.
    [SerializeField] TMP_InputField nameInputField;     // �̸� �Է� �ʵ�.
    [SerializeField] TMP_Text statusText;               // ��Ʈ��ũ ���� �ؽ�Ʈ.

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();                   // ���� ������ ���� ���� (�ܺ� �⺻ ���ð��� ���)
        connectButton.onClick.AddListener(ConnectToRoom);       // ��ư�� �̺�Ʈ �Լ� ���.
        statusText.text = "Ready..";
        nameInputField.ActivateInputField();
    }    

    private void ConnectToRoom()
    {
        if (string.IsNullOrEmpty(nameInputField.text))
        {
            Debug.Log("�г����� �Է����ּ���.");
            return;
        }

        PhotonNetwork.LocalPlayer.NickName = nameInputField.text;   // �����÷��̾�(=��)�� �г����� ����.
        PhotonNetwork.JoinRandomRoom();                             // ������ �뿡 ����.
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"OnJoinRandomFailed : {message}({returnCode})");
        PhotonNetwork.CreateRoom("ä���׽�Ʈ", new Photon.Realtime.RoomOptions { MaxPlayers = 10 });
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.IsMessageQueueRunning = true;      // �޼��� ť�� �����ϰڴ�.
        PhotonNetwork.LoadLevel("2.Game");               // ä�� ������ �̵�.
    }
    private void Update()
    {
        statusText.text = PhotonNetwork.NetworkClientState.ToString();  // ��Ʈ��ũ ���� ǥ��.
        if (!PhotonNetwork.IsConnected)
            return;

        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && !string.IsNullOrEmpty(nameInputField.text))
            ConnectToRoom();
    }
    
}

