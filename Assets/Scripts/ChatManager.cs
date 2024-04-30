using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChatManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text chatText;             // ä�� ���� �ؽ�Ʈ.
    [SerializeField] TMP_InputField chatInputField; // ä�� �Է� �ʵ�.

    Queue<string> messageQueue = new Queue<string>();

    void Start()
    {
        chatText.text = string.Empty;
        chatInputField.text = string.Empty;
        PhotonNetwork.IsMessageQueueRunning = true;  // �޼��� ť�� �����ϰڴ�.
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            // ���� �������� �̺�Ʈ ������Ʈ�� �Է� �ʵ��� ���.
            if(EventSystem.current.currentSelectedGameObject == chatInputField.gameObject)
            {
                if (string.IsNullOrEmpty(chatInputField.text))
                {
                    Debug.Log("DEACTIVATE!");
                    chatInputField.DeactivateInputField();
                }
                else
                {
                    // �Է��� ä�� ������ �����ڴ�.
                    Debug.Log(PhotonNetwork.LocalPlayer.NickName);
                    Debug.Log(photonView);
                    photonView.RPC("ReceiveMessage", RpcTarget.All, $"{PhotonNetwork.LocalPlayer.NickName} : {chatInputField.text}");
                    chatInputField.text = string.Empty;
                    chatInputField.ActivateInputField();
                }
            }
            else
            {
                chatInputField.ActivateInputField();
            }            
        }
    }

    [PunRPC]
    private void ReceiveMessage(string message)
    {
        // ä�� ������ �޾��� �� ó��.
        messageQueue.Enqueue(message);
        if(messageQueue.Count() > 30)
            messageQueue.Dequeue();
        chatText.text = string.Join('\n', messageQueue.ToArray());
    }
}
