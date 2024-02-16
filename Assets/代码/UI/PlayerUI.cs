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

}
