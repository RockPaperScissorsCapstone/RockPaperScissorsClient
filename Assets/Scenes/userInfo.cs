using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo {

   public string firstname;
   public string lastname;
   public string email;
   public string username;

    public UserInfo(string firstname, string lastname, string email, string username)
    {
        this.firstname = firstname;
        this.lastname = lastname;
        this.email = email;
        this.username = username;
    }

}
