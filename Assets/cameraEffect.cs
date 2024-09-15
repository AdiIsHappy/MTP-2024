using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class cameraEffect : MonoBehaviour
{
    public bool earthquake = false;
    public float intensity_=10f;
    public PostProcessVolume ppv;
    public LensDistortion ld;
    private int current_index = 0;
    // Start is called before the first frame update
    void Start()
    {
        ld = ScriptableObject.CreateInstance<LensDistortion>();
        ld.enabled.Override(true);

        // Use the QuickVolume method to create a volume with a priority of 100, and assign the vignette to this volume


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

        ld.intensity.Override(Random.Range(-1,1)*intensity_);
        ppv = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, ld);
        current_index++;
        yield return new WaitForSeconds(0.75f);
    }
}
