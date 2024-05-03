using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public List<Quest> allQuestList;
    
    List<Quest> lockList;
    List<Quest> readyList;
    List<Quest> progressList;
    List<Quest> completeList;

    Dictionary<string, Npc> npcTable;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        lockList = new List<Quest>();
        readyList = new List<Quest>();
        progressList = new List<Quest>();
        completeList = new List<Quest>();
        npcTable = new Dictionary<string, Npc>();

        // 퀘스트를 현재 상태에 따라 분류한다.
        // request는 준 사람, guest는 완료해주는 사람.
        Npc[] npcs = FindObjectsOfType<Npc>();
        foreach (Npc n in npcs)
            npcTable.Add(n.ID, n);

        // 모든 퀘스트를 분류한다.
        allQuestList = Resources.LoadAll<Quest>("Scriptable/Quest").ToList();
        allQuestList.ForEach(q => q.ResetQuest());
        foreach (Quest n in allQuestList)
        {
            List<Quest> list = n.progress switch
            {
                Quest.PROGRESS.LOCK => lockList,
                Quest.PROGRESS.READY => readyList,
                Quest.PROGRESS.PROGRESS => progressList,
                Quest.PROGRESS.CAN_COMPLETE => progressList,
                Quest.PROGRESS.COMPLETE => completeList,
                _ => null
            };

            if(list != null)
                list.Add(n);
        }

        // 세이브 데이터 불러오기.
        InitializeData();

        readyList.ForEach(q => UpdateQuest(q));
        progressList.ForEach(q => UpdateQuest(q));
    }

    public void CheckAction(string interectID)
    {
        Debug.Log($"Check action : {interectID}");
        foreach(Quest item in progressList)
        {
            if (item.Action(interectID))
            {
                // 요구사항을 수행했고 퀘스트가 완료상태라면...
                item.progress = item.IsComplete() ? Quest.PROGRESS.CAN_COMPLETE : Quest.PROGRESS.PROGRESS;
                UpdateQuest(item);
            }
        }
    }
    public void CheckFromNPC(string id, out string[] dialogues, out Action callback)
    {
        Quest quest = readyList.Find(q => q.IsValidQuest(id));
        quest = quest ?? progressList.Find(q => q.IsValidQuest(id));
        if (quest == null)
        {
            dialogues = null;
            callback = null;
            return;
        }

        // 주는 사람과 받는 사람의 처리가 달라야한다.
        dialogues = quest.GetDialogue(id, out bool isRequester);
        callback = () => AcceptQuest(isRequester, quest);
        return;
    }
    private void AcceptQuest(bool isRequester, Quest quest)
    {
        // 퀘스트를 주는 사람과의 대화가 끝난 경우 (=수락)
        if(isRequester && quest.progress == Quest.PROGRESS.READY)
        {
            Debug.Log($"퀘스트 수락! : {quest.questName}");

            readyList.Remove(quest);
            progressList.Add(quest);
            quest.progress = quest.IsComplete() ? Quest.PROGRESS.CAN_COMPLETE : Quest.PROGRESS.PROGRESS;

            UpdateQuest(quest);
        }
        // 퀘스트를 완료해주는 사람과 대화가 끝난 경우 (=완료)
        else if(!isRequester && quest.progress == Quest.PROGRESS.CAN_COMPLETE)
        {
            progressList.Remove(quest);
            completeList.Add(quest);
            quest.progress = Quest.PROGRESS.COMPLETE;

            UpdateQuest(quest);

            // quest.reward; 보상 받기

            // 다음 진행 퀘스트 열기
            foreach (Quest next in quest.nextQuests)
            {
                lockList.Remove(next);
                readyList.Add(next);
                next.progress = Quest.PROGRESS.READY;

                UpdateQuest(next);
            }
        }
    }
    private void UpdateQuest(Quest quest)
    {
        if (string.IsNullOrEmpty(quest.requstID))
            return;

        Debug.Log($"Check quest : {quest.questName}({quest.progress}) <npc:{quest.requstID}>");
        Npc requester = npcTable[quest.requstID];
        Npc guest = npcTable[quest.guestID];
        switch (quest.progress)
        {
            case Quest.PROGRESS.READY:
                QuestUI.Instance.CreateIcon(requester.ID, requester.InterectPivot, QuestUI.ICON.READY);
                break;

            case Quest.PROGRESS.PROGRESS:
                QuestUI.Instance.CreateIcon(requester.ID, requester.InterectPivot, QuestUI.ICON.PROGRESS);
                QuestUI.Instance.CreateIcon(guest.ID, guest.InterectPivot, QuestUI.ICON.PROGRESS);
                QuestUI.Instance.CreateWindow(quest.questID, quest.questName, quest.Description);
                break;

            case Quest.PROGRESS.CAN_COMPLETE:
                QuestUI.Instance.CreateIcon(requester.ID, requester.InterectPivot, QuestUI.ICON.PROGRESS);
                QuestUI.Instance.CreateIcon(guest.ID, guest.InterectPivot, QuestUI.ICON.COMPLETE);
                QuestUI.Instance.CreateWindow(quest.questID, quest.questName, quest.Description);
                break;

            case Quest.PROGRESS.COMPLETE:
                QuestUI.Instance.DeleteIcon(requester.ID);
                QuestUI.Instance.DeleteIcon(guest.ID);
                QuestUI.Instance.DeleteWindow(quest.questID);
                break;
        }
    }

    public void OnApplicationQuit()
    {
        StringBuilder sb = new StringBuilder();
        foreach (Quest q in allQuestList)
        {
            string json = JsonUtility.ToJson(q.Save());
            sb.AppendLine(json);
        }
        PlayerPrefs.SetString("QUEST_DATA", sb.ToString());
        Debug.Log("SAVE_DATA");
    }
    private void InitializeData()
    {        
        string json = PlayerPrefs.GetString("QUEST_DATA", string.Empty);
        if (string.IsNullOrEmpty(json))
            return;

        string[] splits = json.Trim().Split('\n');
        List<Quest.SaveData> datas = splits.Select(data => JsonUtility.FromJson<Quest.SaveData>(data)).ToList();
        foreach(Quest quest in allQuestList)
        {
            Quest.SaveData data = datas.Find(d => d.id == quest.questID);
            if (data == null)
                continue;

            quest.Load(data);
        }
        Debug.Log("LOAD_DATA");
    }
}
