using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PatternStatic {
	private static string pattern;

    public static string SelectedPattern{
        get {
            return pattern;
        }
        set {
            pattern = value;
        }
    }
}
