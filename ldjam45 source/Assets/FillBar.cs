using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FillBar : MonoBehaviour
{
    Image im;
    Image imKey;
    public GameScriptableObject gm;
    public GameObject text;
    void Start()
    {
        im = GetComponentInChildren<Image>();
        imKey = text.GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        text.SetActive(gm.isKnockedDown);

        im.fillAmount = Mathf.Lerp(im.fillAmount, gm.Fame / 100f, Time.deltaTime);
        imKey.fillAmount = Mathf.Lerp(imKey.fillAmount, gm.keyPresses / 50f, Time.deltaTime * 5);
    }
}
