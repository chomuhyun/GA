using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRenderTail : MonoBehaviour
{
    public Player player;
    public float activeTime = 0.5f;
    [Header("Mesh Related")]
    public float meshRefreshRate = 0.1f;
    public Transform positionToSpawn;
    public float meshDestroyDelay;

    [Header("Shader Related")]
    public Material mat;
    public string shaderVarRef;
    public float shaderVarRate = 0.1f;
    public float shaderVarRefreshRate = 0.0f;

    private bool isTrailActive;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;
    void Start()
    {
        player = GetComponent<Player>();
    }


    void Update()
    {
        if (player.Desh)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isTrailActive = true;
                StartCoroutine(ActivateTrail(activeTime));
            }
    }

    IEnumerator ActivateTrail(float timeActive)
    {
        while (timeActive > 0)
        {
            timeActive -= meshRefreshRate;

            if (skinnedMeshRenderers == null)
                skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

            for (int i = 0; i < skinnedMeshRenderers.Length; i++)
            {
                GameObject obj = new GameObject();
                obj.transform.SetPositionAndRotation(positionToSpawn.position, positionToSpawn.rotation); ;

                MeshRenderer mr = obj.AddComponent<MeshRenderer>();
                MeshFilter mf = obj.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                skinnedMeshRenderers[i].BakeMesh(mesh);

                mf.mesh = mesh;
                mr.material = mat;

                StartCoroutine(AniamteMaterialFloat(mr.material, 0, shaderVarRate, shaderVarRefreshRate));

                Destroy(obj, meshDestroyDelay);

            }

            yield return new WaitForSeconds(meshRefreshRate);
        }

        isTrailActive = false;
        player.Desh = false;
        player.isDeshInvincible = false;
    }

    IEnumerator AniamteMaterialFloat(Material mat, float goal, float rate, float refrehRate)
    {
        float valueToAnimate = mat.GetFloat(shaderVarRef);

        while (valueToAnimate > goal)
        {
            valueToAnimate -= rate;
            mat.SetFloat(shaderVarRef, valueToAnimate);
            yield return new WaitForSeconds(refrehRate);
        }
    }
}
