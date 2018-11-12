using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardItem {

    public string place;
    public string points;
    public string name;

    public LeaderboardItem(string place, string points, string name)
    {
        this.place = place;
        this.points = points;
        this.name = name;
    }
}
