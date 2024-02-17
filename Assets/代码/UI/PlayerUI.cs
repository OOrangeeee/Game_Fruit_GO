using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Image healthImage;
    public Image healthDeleteImage;
    public Image tiliImage;
    public Image xiadunImage;

    private void Update()
    {
        if (healthDeleteImage.fillAmount > healthImage.fillAmount)
        {
            healthDeleteImage.fillAmount -= Time.deltaTime * 0.4f;
        }
        else
        {
            healthDeleteImage.fillAmount = healthImage.fillAmount;
        }
    }

    /// <summary>
    /// ����Ѫ��
    /// </summary>
    /// <param name="persentage">Ѫ���ٷֱ�</param>
    public void OnHealthChange(float persentage)
    {
        healthImage.fillAmount = persentage;
    }
    public void OnXiadunChange(float persentage)
    {
        xiadunImage.fillAmount = persentage;
    }
    public void OnHuaChanChange(float persentage)
    {
        tiliImage.fillAmount = persentage;
    }

}
