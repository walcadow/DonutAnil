using System.Collections.Generic;
using UnityEngine;

public enum DonutState
{ 
    Idle,
    Rising,
    GoingTarget
}

public class Stand : MonoBehaviour
{
    public List<Donut> donuts;
    public List<Material> materials;
    public Transform riseTransform;
    public DonutState donutState = DonutState.Idle;
    public float speed = 4.0f;

    public const float distBetweenDonuts = 0.10517764f;

    private void Start()
    {
        for (int i = 0; i < donuts.Count; i++)
        {
            donuts[i].enabled = false;
            MeshRenderer renderer = donuts[i].GetComponent<MeshRenderer>();
            int materialIndex = Random.Range(0, materials.Count - 1);
            donuts[i].color = materialIndex;
            renderer.sharedMaterial = materials[materialIndex];
        }
    }

    public void ClearDonuts()
    { 
        for(int i = 0; i < donuts.Count;i++)
        {  
            Destroy(donuts[i].gameObject);
        }
        donuts.Clear();
    }

    public void ClickDonut()
    {
        if (donuts.Count == 0)
        {
            return;
        }

        if (donutState == DonutState.Rising)
        {
            donutState = DonutState.Idle;
        }
        else
        {
            donutState = DonutState.Rising;
        }
    }

    private void Update()
    {
        if (donuts.Count == 0)
        {
            return;
        }

        if (donutState == DonutState.Rising)
        {
            Transform lastDonut = donuts[^1].transform;
            lastDonut.position = Vector3.Lerp(lastDonut.position, riseTransform.position, Time.deltaTime * speed);
        }
        else if (donutState == DonutState.Idle)
        {
            Transform lastDonut = donuts[^1].transform;
            float targetY = 0.06748586f + ((donuts.Count-1) * distBetweenDonuts);
            
            Vector3 targetPos = riseTransform.localPosition;
            targetPos.y = targetY;

            lastDonut.localPosition = Vector3.Lerp(lastDonut.localPosition, targetPos, Time.deltaTime * speed);
        }
    }
}
