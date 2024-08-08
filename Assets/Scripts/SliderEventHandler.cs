using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderEventHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Slider slider; // Reference to the slider
    public bool sliderMoving; // Flag to indicate if the slider is being touched or dragged

    // Method to handle pointer down event
    public void OnPointerDown(PointerEventData eventData)
    {
        if (slider != null)
        {
            sliderMoving = true;
            Debug.Log("Slider is touched");
        }
    }
    
    // Called when the pointer is released from the slider
    public void OnPointerUp(PointerEventData eventData)
    {
        if (slider != null)
        {
            sliderMoving = false;
            Debug.Log("Slider is no longer touched");
        }
    }

    // Called when the slider is dragged
    public void OnDrag(PointerEventData eventData)
    {
        if (slider != null)
        {
            sliderMoving = true;
        }
    }
}
