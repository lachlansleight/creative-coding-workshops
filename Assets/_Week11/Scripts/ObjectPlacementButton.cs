using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPlacementButton : MonoBehaviour
{

    public Image MainImage;
    public Image CrossImage;

    private bool _isShowingCross;

    public void ShowMain()
    {
        _isShowingCross = false;
        MainImage.enabled = !_isShowingCross;
        CrossImage.enabled = _isShowingCross;
    }

    public void ShowCross()
    {
        _isShowingCross = true;
        MainImage.enabled = !_isShowingCross;
        CrossImage.enabled = _isShowingCross;
    }
    
    public void Toggle()
    {
        _isShowingCross = !_isShowingCross;
        
        MainImage.enabled = !_isShowingCross;
        CrossImage.enabled = _isShowingCross;
    }
}
