using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerManager;

public static class SocketPasser {
    private static ConnectionManager CM;

    public static void setCM(ConnectionManager param){
        CM = param;
    }

    public static ConnectionManager getCM(){
        return CM;
    }
}
