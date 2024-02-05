using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingObjCamera : MonoBehaviour
{
    public Transform target = null; // 카메라가 바라보는 대상
    [SerializeField]
    public float castRadius = 1f; //물체확인용 반원 캐스트
    RaycastHit[] hitBuffer = new RaycastHit[32]; //반환되는 충돌정보

    List<HidingObj> hiddenObjects = new List<HidingObj>();
    List<HidingObj> previouslyHiddenObjects = new List<HidingObj>();
    ThirdPersonOrbitCamBasicA thirdPerson;
    public GameObject targetPlayer;

    void LateUpdate()
    {
        
        RefreshHiddenObjects();
    }

    public void RefreshHiddenObjects()
    {
        testGameMgr someComponent = GameObject.FindWithTag("Player").GetComponent<testGameMgr>();
        if (someComponent != null)
        {
            targetPlayer = GameObject.FindWithTag("Player");
            someComponent.Starts();
        }
        targetPlayer = GameObject.FindWithTag("Player");

        if (targetPlayer != null)
        {
            target = targetPlayer.transform;
        }
        Vector3 toTarget = (target.position - transform.position);
        float targetDistance = toTarget.magnitude;
        Vector3 targetDirection = toTarget / targetDistance;

        targetDistance -= castRadius * 1.1f;

        hiddenObjects.Clear();
        int hitCount = Physics.SphereCastNonAlloc(transform.position, castRadius, targetDirection, hitBuffer, targetDistance, -1, QueryTriggerInteraction.Ignore);

        for(int i = 0; i < hitCount; i++)
        {
            var hit = hitBuffer[i];
            var hideable = HidingObj.GetRootHidingByCollider(hit.collider);

            if (hideable != null)
                hiddenObjects.Add(hideable);
        }
        foreach (var hideable in hiddenObjects)
            if (!previouslyHiddenObjects.Contains(hideable))
                hideable.SetVisible(false);

        foreach (var hideable in previouslyHiddenObjects)
            if (!hiddenObjects.Contains(hideable))
                hideable.SetVisible(true);

        var temp = hiddenObjects;
        hiddenObjects = previouslyHiddenObjects;
        previouslyHiddenObjects = temp;
    }

    
    void Update()
    {
        
    }
}
