using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HudManager : MonoBehaviour
{
    public Slider battery;
    public RawImage dashSprite;
    public RawImage warpSprite;
    public bool dashEnabled = true, warpEnabled = true;
    private bool alreadyEnabledDash, alreadyEnabledWarp;

    void Update()
    {
        if (dashEnabled && !alreadyEnabledDash)
        {
            dashSprite.color = new Color(0, 1, 1, 1);
        }
        else if (!dashEnabled)
        {
            alreadyEnabledDash = false;
            dashSprite.color = new Color(0, 1, 1, 0.1f);
        }

        if (warpEnabled && !alreadyEnabledWarp)
        {
            alreadyEnabledWarp = true;
            warpSprite.color = new Color(1, 0, 0, 1);
        }
        else if (!warpEnabled)
        {
            alreadyEnabledWarp = false;
            warpSprite.color = new Color(1, 0, 0, 0.1f);
        }

    }
}
