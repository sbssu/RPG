using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChatManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text chatText;             // 채팅 내용 텍스트.
    [SerializeField] TMP_InputField chatInputField; // 채팅 입력 필드.

    Queue<string> messageQueue = new Queue<string>();

    void Start()
    {
        chatText.text = string.Empty;
        chatInputField.text = string.Empty;
        PhotonNetwork.IsMessageQueueRunning = true;  // 메세지 큐를 수신하겠다.
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            // 현재 선택중인 이벤트 오브젝트가 입력 필드일 경우.
            if(EventSystem.current.currentSelectedGameObject == chatInputField.gameObject)
            {
                if (string.IsNullOrEmpty(chatInputField.text))
                {
                    Debug.Log("DEACTIVATE!");
                    chatInputField.DeactivateInputField();
                }
                else
                {
                    // 입력한 채팅 내용을 보내겠다.
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
        // 채팅 내용을 받았을 때 처리.
        messageQueue.Enqueue(message);
        if(messageQueue.Count() > 30)
            messageQueue.Dequeue();
        chatText.text = string.Join('\n', messageQueue.ToArray());
    }
}
