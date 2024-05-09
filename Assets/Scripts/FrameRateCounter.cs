using UnityEngine;
using TMPro;

// Showing the Frame Rate without needing unity's editor
public class FrameRateCounter : MonoBehaviour
{
    [SerializeField] 
    TextMeshProUGUI display;

    /* 
    frame rate is the time passed between the previous and current frame rate
    this information is available in time.deltatime
    but this value is subject to scaling (for slowmotion, fastforward or stopping time)
    so we'll use time.UnscalledDeltaTime

    */
    void Update (){
        float frameDuration = Time.unscaledDeltaTime;

        // adjust the displayed text
        // frameduration will replace {0}
        //frame per second is the inverse of frame duration
        // 0:0 indicates the number of digits after , in this case 0 => rounded to a whole number
        display.SetText("FPS\n{0:0}\n000\n000", 1f / frameDuration); 
    }

}
