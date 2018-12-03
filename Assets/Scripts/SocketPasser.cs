using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerManager;

public static class SocketPasser {
    private ConnectionManager CM;

    public void setCM(ConnectionManager param){
        this.CM = param;
    }

    public ConnectionManager getCM(){
        return this.CM;
    }
}
