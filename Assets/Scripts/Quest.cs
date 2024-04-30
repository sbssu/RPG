using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Scriptable/Quest")]
public class Quest : ScriptableObject
{
    public enum CONDITION
    {
        LOCK,               // ������ �ʾҴ�
        READY,              // ���� ���
        PROGRESS,           // ������
        CAN_COMPLETE,       // �Ϸ� ����
        COMPLETE,           // �Ϸ�
    }

    [System.Serializable]
    public struct Reward
    {
        public int gold;
        public Item[] items;        
    }

    [Header("Reqeust")]
    public string questName;                // �ɽ�Ʈ �̸�.
    public string requstID;                 // ����Ʈ �Ƿ� NPC.
    public string[] questDialogue;          // ���� ��ȭ
    public string[] progressDialogue;       // ���� �� ��ȭ.

    [Header("Require")]
    public QuestRequire[] requires;         // �䱸����.

    [Header("Complete")]
    public string guestID;                  // ������ Npc
    public string[] notMeetDialogue;        // �Ϸ� �� ��ȭ
    public string[] completeDialogue;       // �ͷ� �� ��ȭ

    [Header("Progress")]
    public CONDITION condition;             // ���� ����.
    public Reward reward;                   // ����
    public Quest[] nextQuests;              // ���� ������ ����Ʈ.

    public string Requrement()
    {
        StringBuilder sb = new StringBuilder();
        foreach(QuestRequire r in requires)
            sb.AppendLine(r.ToString());
        return sb.ToString();
    }
}
