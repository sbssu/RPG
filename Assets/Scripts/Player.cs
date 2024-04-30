using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public int hp;
    public float moveSpeed;
    public float rotateSpeed;
    public Transform interctPivot;
    public GameObject talkCamera;

    Animator anim;
    PhotonView photonView;
    NavMeshAgent agent;    
    Camera mainCam;
    HoverObject hoverObject;
    HoverObject prevHoverObject;

    bool isLockControl;
    bool hasPath;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        mainCam = Camera.main;
    }

    void Update()
    {
        if (!photonView.IsMine)
            return;

        if (!isLockControl)
            Movement();

        Interect();
        HoverObject();

        if (hasPath)
        {
            if (!agent.hasPath)
            {
                if(agent.enabled)
                    agent.isStopped = true; // For understanding - here the agent stops 

                anim.SetBool("isMove", true);
            }
            else
            {
                if (agent.enabled)
                    agent.isStopped = false; // The agent is moving

                anim.SetBool("isMove", false);
            }
        }

        anim.SetBool("isMove", agent.hasPath);
    }
    private void LateUpdate()
    {
        hasPath = agent.hasPath;
    }



    void Movement()
    {
        // 현재 마우스 아래에 UI가 있다면 이동할 수 없게 한다.
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, 1 << LayerMask.NameToLayer("Ground")))
                agent.SetDestination(hit.point);
        }             
    }
    void Interect()
    {
        if (!mainCam.enabled)
        {
            InterectUI.Instance.Close();
            return;
        }

        IInteract target = null;
        Collider[] colliers = Physics.OverlapSphere(transform.position, 2.5f);
        foreach(Collider collider in colliers)
        {
            IInteract find = collider.GetComponent<IInteract>();
            if (find != null && find.CanInterect)
            {
                target = find;
                break;
            }
        }

        if (target == null)
            InterectUI.Instance.Close();
        else
        {
            InterectUI.Instance.Show(target, target.InterectPivot ?? interctPivot);
            if (Input.GetKeyDown(KeyCode.G))
                target.OnInterect(gameObject);
        }
    }
    void HoverObject()
    {
        if (mainCam.enabled)
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                hoverObject = hit.collider.GetComponent<HoverObject>();
                if (hoverObject != null && hoverObject != prevHoverObject)
                {
                    hoverObject.OnEnterHover();
                }
            }
            else
                hoverObject = null;

            // 마우스 클릭 이벤트.
            if (hoverObject != null && Input.GetMouseButtonDown(0))
            {
                hoverObject.OnClickObject(gameObject);
            }
        }
        else
            hoverObject = null;

        // 이전 프레임에 선택 오브젝트가 있었고 현재 프레임에는 없을 경우.
        if (hoverObject == null && prevHoverObject != null)
            prevHoverObject.OnExitHover();                

        prevHoverObject = hoverObject;
    }


    public void LockControl(bool isLock)
    {
        isLockControl = isLock;
    }
    public void LockNavmesh(bool isLock)
    {
        agent.enabled = !isLock;
    }
    public void SetDestination(Vector3 point)
    {
        agent.SetDestination(point);
    }
    public void SwitchTalkCamera(bool isOn)
    {
        mainCam.enabled = !isOn;
        talkCamera.SetActive(isOn);
        if (isOn)
            Fader.Instance.FadeIn();
    }
}
