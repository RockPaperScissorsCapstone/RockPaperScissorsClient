using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo {

    public string user_id;
    public string firstname;
    public string lastname;
    public string email;
    public string username;
    public string wins;
    public string losses;
    public string currency;
    public string score;
    public string skintag;

    public UserInfo(string firstname, string lastname, string email, string username)
    {
        this.user_id = "";
        this.firstname = firstname;
        this.lastname = lastname;
        this.email = email;
        this.username = username;
        this.wins = "0";
        this.losses = "0";
        this.currency = "0";
        this.score = "0";
        this.skintag = "";
    }

    public UserInfo()
    {
        this.user_id = "";
        this.firstname = "";
        this.lastname = "";
        this.email = "";
        this.username = "";
        this.wins = "";
        this.losses = "";
        this.currency = "";
        this.score = "";
        this.skintag = "";
    }

    public string getUserId()
    {
        return this.user_id;
    }
    public string getFirstName()
    {
        return this.firstname;
    }

    public string getLastName()
    {
        return this.lastname;
    }

    public string getEmail()
    {
        return this.email;
    }

    public string getUsername()
    {
        return this.username;
    }

    public string getScore()
    {
        return this.score;
    }

    public string getCurrency()
    {
        return this.currency;
    }

    public string getWins()
    {
        return this.wins;
    }

    public string getLosses()
    {
        return this.losses;
    }
    
    public string getSkintag()
    {
        return this.skintag;
    }
    
    public void setFirstName(string new_firstname)
    {
        this.firstname = new_firstname;
    }

    public void setLastName(string new_lastname)
    {
        this.lastname = new_lastname;
    }

    public void setEmail(string new_email)
    {
        this.email = new_email;
    }

    public void setUsername(string new_username)
    {
        this.username = new_username;
    }

    public void setScore(string new_score)
    {
        this.score = new_score;
    }

    public void setCurrency(string new_score)
    {
        this.currency = new_score;
    }

    public void setWins(string new_wins)
    {
        this.wins = new_wins;
    }

    public void setLosses(string new_losses)
    {
        this.losses = new_losses;
    }

    public void setSkinTag(string skintag)
    {
        this.skintag = skintag;
    }




}
