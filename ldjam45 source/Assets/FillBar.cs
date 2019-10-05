using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FillBar : MonoBehaviour
{
    Image im;
    public GameScriptableObject gm;
    void Start()
    {
        im = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        im.fillAmount = Mathf.Lerp(im.fillAmount, gm.Fame / 100f, Time.deltaTime);
    }
}
