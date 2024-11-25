using UnityEngine;
using UnityEngine.EventSystems;

public class MySystemButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    bool ButtonPushFlag;
    void Start()
    {
        ButtonPushFlag = false;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        //�{�^���������ꂽ�Ƃ�
        if(ButtonPushFlag == false)
            ButtonPushFlag = true;
        else
            ButtonPushFlag = false;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        //�{�^�������ꂽ��
        if (ButtonPushFlag == false)
             ButtonPushFlag = true;
        else
            ButtonPushFlag = false;

    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        //�{�^����������A���̌�h���b�O���삪�Ȃ��{�^�����痣�ꂽ��
        if (ButtonPushFlag == false)
            ButtonPushFlag = true;
        else
            ButtonPushFlag = false;
    }
}
