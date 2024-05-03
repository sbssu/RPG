using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Scriptable/Quest")]
public class Quest : ScriptableObject
{
    public class SaveData
    {
        public string id;
        public PROGRESS progress;
        public QuestRequire[] requires;
    }

    public enum PROGRESS
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

    [Header("ID")]
    public string questID;                  // ����Ʈ ���̵�.
    public string requstID;                 // ����Ʈ �Ƿ� NPC.
    public string guestID;                  // ������ Npc

    [Header("Reqeust")]
    public string questName;                // ����Ʈ �̸�.
    public string[] questDialogue;          // ���� ��ȭ
    public string[] progressDialogue;       // ���� �� ��ȭ.        

    [Header("Complete")]
    public string[] notMeetDialogue;        // �Ϸ� �� ��ȭ
    public string[] completeDialogue;       // �ͷ� �� ��ȭ

    [Header("Require")]
    public QuestRequire[] requires;         // �䱸����.

    [Header("Progress")]
    public PROGRESS progress;               // ���� ����.
    public Reward reward;                   // ����
    public Quest[] nextQuests;              // ���� ������ ����Ʈ.

    public string Description
    {
        get
        {
            string ment = string.Empty;
            for (int i = 0; i < requires.Length; i++)
            {
                ment = requires[i].description;
                if (!requires[i].isComplete)
                    break;
            }
            return ment;
        }
    }
    public void ResetQuest()
    {
        progress = (questID == "QE0001") ? PROGRESS.READY : PROGRESS.LOCK;
        foreach (QuestRequire r in requires)
            r.current = 0;
    }

    public bool Action(string id, int forceCount = -1)
    {
        QuestRequire r = null;
        foreach (QuestRequire requre in requires)
        {
            if(!requre.isComplete)
            {
                r = requre;                
                break;
            }
        }

        if(r != null)
        {
            // ����Ʈ�� �䱸���� �Ϸ�
            // ���� �䱸���� ����.
            return r.Action(id, forceCount);
        }

        return false;
    }

    public bool IsComplete()
    {
        bool isComplte = true;
        foreach (QuestRequire r in requires)
            isComplte = isComplte && r.isComplete;
        return isComplte;
    }
    public bool IsValidQuest(string id)
    {
        return id == requstID || id == guestID;
    }

    public string[] GetDialogue(string id, out bool isRequester)
    {
        isRequester = requstID == id;
        if (isRequester)
        {
            if (progress == PROGRESS.READY)
                return questDialogue;
            else if (progress == PROGRESS.PROGRESS || progress == PROGRESS.CAN_COMPLETE)
                return progressDialogue;
        }
        else
        {
            if (progress == PROGRESS.PROGRESS)
                return notMeetDialogue;
            else if (progress == PROGRESS.CAN_COMPLETE)
                return completeDialogue;
        }

        Debug.Log($"{questName}�� GetDialogue�� null�Դϴ�.");
        return null;
    }

    public SaveData Save()
    {
        SaveData data = new SaveData();
        data.id = questID;
        data.progress = progress;
        data.requires = requires;
        return data;
    }
    public void Load(SaveData load)
    {
        progress = load.progress;
        requires = load.requires;
    }
}
