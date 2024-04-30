using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Scriptable/Quest")]
public class Quest : ScriptableObject
{
    public enum CONDITION
    {
        LOCK,               // 열리지 않았다
        READY,              // 수락 대기
        PROGRESS,           // 진행중
        CAN_COMPLETE,       // 완료 가능
        COMPLETE,           // 완료
    }

    [System.Serializable]
    public struct Reward
    {
        public int gold;
        public Item[] items;        
    }

    [Header("Reqeust")]
    public string questName;                // 케스트 이름.
    public string requstID;                 // 퀘스트 의뢰 NPC.
    public string[] questDialogue;          // 수락 대화
    public string[] progressDialogue;       // 진행 중 대화.

    [Header("Require")]
    public QuestRequire[] requires;         // 요구사항.

    [Header("Complete")]
    public string guestID;                  // 수뢰자 Npc
    public string[] notMeetDialogue;        // 완료 전 대화
    public string[] completeDialogue;       // 와료 후 대화

    [Header("Progress")]
    public CONDITION condition;             // 진행 상태.
    public Reward reward;                   // 보상
    public Quest[] nextQuests;              // 다음 열리는 퀘스트.

    public string Requrement()
    {
        StringBuilder sb = new StringBuilder();
        foreach(QuestRequire r in requires)
            sb.AppendLine(r.ToString());
        return sb.ToString();
    }
}
