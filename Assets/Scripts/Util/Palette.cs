using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Palette {

    public static Color DefaultBGColor = Color255(220, 225, 226);
    public static Color DefaultTextColor = Color255(50, 50, 50);
    public static Color DisabledTextColor = Color255(200, 200, 200);
    public static Color PrimaryTextColor = Color255(52, 152, 219);
    public static Color GrayTextColor = Color255(158, 158, 158);
    public static Color Transparent = Color255(255, 255, 255, 81);

	public static Color Color255(float r, float g, float b, float a = 255f)
    {
        return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
    }
}
