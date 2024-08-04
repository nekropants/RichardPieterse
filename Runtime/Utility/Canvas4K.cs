using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RichardPieterse
{
    public class Canvas4K : MonoBehaviour
    {
        private void Reset()
        {
            GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            GetComponent<CanvasScaler>().referenceResolution = new Vector2(3840, 2160);
        }
    }
}
