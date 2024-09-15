using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public float intensity_ = 1f;
    public bool earthquake = false;
    public List<int> lighroutine = new List<int> {5,5,5,4,3,6,4};
    public Light light_;
    private int current_index=0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (earthquake)
        {
            StartCoroutine(flicker());
        }
        
    }
    IEnumerator flicker()
    {
        light_.intensity = lighroutine[(int)Random.Range(0f,lighroutine.Count)]*intensity_;
        current_index++;
        yield return new WaitForSeconds(0.5f);
    }
}
