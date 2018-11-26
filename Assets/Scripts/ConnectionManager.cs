using System; 
using System.IO;
using System.Net; 
using System.Net.Sockets; 
using System.Text; 
using System.Collections; 
using System.Collections.Generic; 
using System.Linq;
using UnityEngine; 

namespace ServerManager{
    public class ConnectionManager {
		private IPHostEntry ipHostInfo;
		private IPAddress ipAddress;
		private IPEndPoint remoteEP;

        private IPAddress hostIPAddress;
		private Socket sender;

        public ConnectionManager(){

        }
        public int StartClient() { 
            // Connect to a remote device.  
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
             
                // //Production (Steve's Server)
                ipHostInfo = Dns.GetHostEntry("ec2-18-224-97-127.us-east-2.compute.amazonaws.com");
                ipAddress = ipHostInfo.AddressList[0]; 
                remoteEP = new IPEndPoint(ipAddress, 65432);
           

                //Nick's Test environment
                //ipHostInfo = Dns.GetHostEntry("ec2-18-217-146-155.us-east-2.compute.amazonaws.com");
                //ipAddress = ipHostInfo.AddressList[0]; 
                //remoteEP = new IPEndPoint(ipAddress, 65432); 

                // Create a TCP/IP  socket.  
                sender = new Socket(ipAddress.AddressFamily, 
                    SocketType.Stream, ProtocolType.Tcp ); 

                // Connect the socket to the remote endpoint. Catch any errors.  
                try {
                    sender.Connect(remoteEP); 
                    return 1;
				}catch (Exception e) {
                    Console.WriteLine(e.ToString()); 
                    Debug.Log(e.ToString());
                    return 0;
            	}
		}
		public string[] SubmitRegisteration(string[] param)	{
            string[] response = new string[7];
			Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());
			// Data buffer for incoming data.  
            byte[] msgFunction = EncodeToBytes("CreateAccount");
            response[0] = Messenger(msgFunction);

			byte[] msgUsername = EncodeToBytes(param[0]);
			response[1] = Messenger(msgUsername);

			byte[] msgEmail = EncodeToBytes(param[1]);
			response[2] = Messenger(msgEmail);

			byte[] msgFName = EncodeToBytes(param[2]);
			response[3] = Messenger(msgFName);

			byte[] msgLName = EncodeToBytes(param[3]);
			response[4] = Messenger(msgLName);

			byte[] msgPassword = EncodeToBytes(param[4]);
			response[5] = Messenger(msgPassword);
			
			EndMessages();
			//byte[] msgAge = Encoding.ASCII.GetBytes(param[5]);
			response[6] = receive();
            return response;
		}

        public string UpdateAccountInfo(string[] param) {
            string[] response = new string[4];
            Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());
            //Data buffer for incoming data.
            byte[] msgFunction = EncodeToBytes("UpdateAccountInfo");
            response[0] = Messenger(msgFunction);

            byte[] msgUserID = EncodeToBytes(param[0]);
            response[1] = Messenger(msgUserID);

            byte[] msgUserName = EncodeToBytes(param[1]);
            response[2] = Messenger(msgUserName);

            EndMessages();

            response[3] = receive();
            return response[3];
        }

        public string GetLeaderboard()
        {
            string[] response = new string[2];
            Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());

            byte[] msgFunction = EncodeToBytes("Leaderboard");
            response[0] = Messenger(msgFunction);

            EndMessages();

            response[1] = receive();
            return response[1];
        }

		private void EndMessages(){
			send(EncodeToBytes("end"));
		}

		private int send(byte[] msg){
				int bytesSent = sender.Send(msg);
				return bytesSent;
		}

		private string receive(){
			byte[] bytes = new byte[1024];
			int bytesRec = sender.Receive(bytes);
            Debug.Log(bytesRec);
            if(bytes.Length > 0){
			    string results = (DecodeToString(bytes));
                Debug.Log("This is in the receive class" + results);
                return (results);
            }
            else{
                return ("We received nothing from python");
            }
		}
		private string Messenger(byte[] msg){
            try{
			    int bytesSent = send(msg);
			    string response = receive(); 
                return response;
            }catch (ArgumentNullException ane) {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                return ane.ToString();
            }catch (SocketException se) {
                Console.WriteLine("SocketException : {0}", se.ToString());
                return se.ToString();
            }catch (Exception e) {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
                return e.ToString(); 
            }

		}

        private int send(byte[] msg, Socket multiplayer){
				int bytesSent = multiplayer.Send(msg);
				return bytesSent;
		}

		private string receive(Socket multiplayer){
			byte[] bytes = new byte[1024];
			int bytesRec = multiplayer.Receive(bytes);
            Debug.Log(bytesRec);
            if(bytes.Length > 0){
			    string results = (DecodeToString(bytes));
                Debug.Log("This is in the receive class" + results);
                return (results);
            }
            else{
                return ("We received nothing from python");
            }
		}
		private string Messenger(byte[] msg, Socket multiplayer){
            try{
			    int bytesSent = send(msg, multiplayer);
			    string response = receive(multiplayer); 
                return response;
            }catch (ArgumentNullException ane) {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                return ane.ToString();
            }catch (SocketException se) {
                Console.WriteLine("SocketException : {0}", se.ToString());
                return se.ToString();
            }catch (Exception e) {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
                return e.ToString(); 
            }

		}

        public string[] SubmitLogin(string[] param) {
            string[] response = new string[4];
            Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());

            //prepare to send Login info to server

            //refer to server's ConnectionManager's main if/else statement to see which function name to match
            byte[] msgFunction = EncodeToBytes("Login"); 
            response[0] = Messenger(msgFunction);

            byte[] msgUsername = EncodeToBytes(param[0]);
            response[1] = Messenger(msgUsername);

            byte[] msgPassword = EncodeToBytes(param[1]);
            response[2] = Messenger(msgPassword);

            EndMessages();

            response[3] = receive();
            
            Debug.Log(response[3]);

            return response;
        }

        public string startGameSession() {
            string[] response = new string[2];

            byte[] msgFunction = EncodeToBytes("Session");
            response[0] = Messenger(msgFunction);

            EndMessages();

            Debug.Log(response[0]);

            return response[0];
        }


        public string startPlayerWithRandom() {
            string[] response = new string[2];

            byte[] msgFunction = EncodeToBytes("PlayWithRandom");
            response[0] = Messenger(msgFunction);

            EndMessages();

            Debug.Log(response[0]);

            return response[0];
        }

        public string getResponse(){
            byte[] bytes = new byte[10];
			int bytesRec = sender.Receive(bytes);
            Debug.Log(bytesRec);
            if(bytes.Length > 0){
			    string results = (DecodeToString(bytes));
                Debug.Log("This is in the getResponse method " + results);
                return (results);
            }
            else{
                return ("We received nothing from python");
            }
        }

        public string getOneResponse(){
            byte[] bytes = new byte[1];
			int bytesRec = sender.Receive(bytes);
            Debug.Log(bytesRec);
            if(bytes.Length > 0){
			    string results = (DecodeToString(bytes));
                Debug.Log("This is in the getResponse method " + results);
                return (results);
            }
            else{
                return ("We received nothing from python");
            }
        }

        public void sendResponse(string param){
            send(EncodeToBytes(param));
        }

        public string getResponse(Socket multiplayer){
            byte[] bytes = new byte[1];
			int bytesRec = multiplayer.Receive(bytes);
            Debug.Log(bytesRec);
            if(bytes.Length > 0){
			    string results = (DecodeToString(bytes));
                Debug.Log("This is in the getResponse method " + results);
                return (results);
            }
            else{
                return ("We received nothing from python");
            }
        }

        public int sendMove(string currentMove){
            byte[] move = EncodeToBytes(currentMove.ToString());
            return send(move);
        }

        public int sendUserId(string userID) {
            byte[] userId = EncodeToBytes(userID);
            return send(userId);
        }

        public int sendMove(string currentMove, Socket multiplayer){
            byte[] move = EncodeToBytes(currentMove.ToString());
            return send(move, multiplayer);
        }

        public int sendUserId(string userID, Socket multiplayer) {
            byte[] userId = EncodeToBytes(userID);
            return send(userId, multiplayer);
        }


        public string updateWinLoss(string[] param) {
            string[] response = new string[6];
            Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());
            //Data buffer for incoming data.
            byte[] msgFunction = EncodeToBytes("UpdateWinLoss");
            response[0] = Messenger(msgFunction);

            byte[] msgWin = EncodeToBytes(param[0]);
            response[1] = Messenger(msgWin);

            byte[] msgLoss = EncodeToBytes(param[1]);
            response[2] = Messenger(msgLoss);

            byte[] msgUserID = EncodeToBytes(param[2]);
            response[3] = Messenger(msgUserID);

            EndMessages();

            response[4] = receive();
            return response[4];
        }

        public string getFriendsList(string username) {
            string response;
            
            byte[] msgFunction = EncodeToBytes("findFriends");
            response = Messenger(msgFunction);

            byte[] msgUserID = EncodeToBytes(username);
            response = Messenger(msgUserID);

            EndMessages();

            response = steve_receive();

            return response;
        }

        private string steve_receive(){
			byte[] bytes = new byte[1024];
			int bytesRec = sender.Receive(bytes);
            Debug.Log(bytesRec);
            if(bytesRec > 0){
			    string results = (DecodeToString(bytes));
                Debug.Log("This is in the receive class " + results);
                return (results);
            }
            else{
                return ("");
            }
		}
        public string[] AddNewFriend(string[] param){
            string[] response = new string[4];
            Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());
            //Data buffer for incoming data.
            byte[] msgFunction = EncodeToBytes("addFriend");
            response[0] = Messenger(msgFunction);

            byte[] myUsername = EncodeToBytes(param[0]);
            response[1] = Messenger(myUsername);

            byte[] friendUsername = EncodeToBytes(param[1]);
            response[2] = Messenger(friendUsername);

            EndMessages();

            response[3] = receive();

            return response;
        }

        public string CheckChallengesUpdate(string userId)
        {
            string response;
            Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());
            //Data buffer for incoming data.
            byte[] msgFunction = EncodeToBytes("CheckNewChallenges");
            response = Messenger(msgFunction);

            byte[] myUserId = EncodeToBytes(userId);
            response = Messenger(myUserId);

            EndMessages();

            response = receive();
            return response;
        }

        public string[] ChallengeFriend(string[] param)
        {
            string[] response = new string[5];
            Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());
            //Data buffer for incoming data.
            byte[] msgFunction = EncodeToBytes("challengeFriend");
            response[0] = Messenger(msgFunction);

            byte[] myUserId = EncodeToBytes(param[0]);
            response[1] = Messenger(myUserId);

            byte[] friendUsername = EncodeToBytes(param[1]);
            response[2] = Messenger(friendUsername);

            byte[] message = EncodeToBytes(param[2]);
            response[3] = Messenger(message);

            EndMessages();

            response[4] = receive();
            return response;
        }


        public string ChallengeDenied(string [] param)
        {
            string response;
            Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());
            //Data buffer for incoming data.
            byte[] msgFunction = EncodeToBytes("ChallengeDenied");
            response = Messenger(msgFunction);

            byte[] myUserId = EncodeToBytes(param[0]);
            response = Messenger(myUserId);

            byte[] ChallengerUsername = EncodeToBytes(param[1]);
            response = Messenger(ChallengerUsername);

            byte[] message = EncodeToBytes(param[2]);
            response = Messenger(message);
            EndMessages();

            response = receive();
            return response;
        }

        public string ChallengeAccepted(string[] param)
        {
            string response;
            Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());
            //Data buffer for incoming data.
            byte[] msgFunction = EncodeToBytes("ChallengeAccepted");
            response = Messenger(msgFunction);

            byte[] myUserId = EncodeToBytes(param[0]);
            response = Messenger(myUserId);

            byte[] ChallengerUsername = EncodeToBytes(param[1]);
            response = Messenger(ChallengerUsername);

            byte[] message = EncodeToBytes(param[2]);
            response = Messenger(message);
            EndMessages();

            response = receive();
            return response;
        }


        private byte[] EncodeToBytes(string param)
        {
            return Encoding.ASCII.GetBytes(param);
        }

        private string DecodeToString(byte[] param){
            return Encoding.ASCII.GetString(param, 0, param.Length);
        }
        

        public void LogOff(){
                    // Release the socket.  
                    sender.Shutdown(SocketShutdown.Both); 
                    sender.Close(); 
		}
    }
}