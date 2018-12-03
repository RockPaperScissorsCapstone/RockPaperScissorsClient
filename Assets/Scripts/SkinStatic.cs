using UnityEngine;
using UnityEngine.EventSystems;

public static class SkinStatic {
    private static GameObject selectedSkin;

    public static GameObject SelectedSkin{
        get {
            return selectedSkin;
        }
        set {
            selectedSkin = value;
        }
    }
}