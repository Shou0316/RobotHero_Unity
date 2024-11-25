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
        //ボタンが押されたとき
        if(ButtonPushFlag == false)
            ButtonPushFlag = true;
        else
            ButtonPushFlag = false;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        //ボタンが離れた時
        if (ButtonPushFlag == false)
             ButtonPushFlag = true;
        else
            ButtonPushFlag = false;

    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        //ボタンが押され、その後ドラッグ操作がなくボタンから離れた時
        if (ButtonPushFlag == false)
            ButtonPushFlag = true;
        else
            ButtonPushFlag = false;
    }
}
