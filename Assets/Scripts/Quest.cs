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

    [Header("ID")]
    public string questID;                  // 퀘스트 아이디.
    public string requstID;                 // 퀘스트 의뢰 NPC.
    public string guestID;                  // 수뢰자 Npc

    [Header("Reqeust")]
    public string questName;                // 퀘스트 이름.
    public string[] questDialogue;          // 수락 대화
    public string[] progressDialogue;       // 진행 중 대화.        

    [Header("Complete")]
    public string[] notMeetDialogue;        // 완료 전 대화
    public string[] completeDialogue;       // 와료 후 대화

    [Header("Require")]
    public QuestRequire[] requires;         // 요구사항.

    [Header("Progress")]
    public PROGRESS progress;               // 진행 상태.
    public Reward reward;                   // 보상
    public Quest[] nextQuests;              // 다음 열리는 퀘스트.

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
            // 퀘스트의 요구사항 완료
            // 다음 요구사항 띄우기.
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

        Debug.Log($"{questName}의 GetDialogue가 null입니다.");
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
