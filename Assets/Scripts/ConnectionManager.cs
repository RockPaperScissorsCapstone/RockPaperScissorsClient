using System; 
using System.Net; 
using System.Net.Sockets; 
using System.Text; 
using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

namespace ConnectionManager{
    class ConnectionsManager {
		private IPHostEntry ipHostInfo;
		private IPAddress ipAddress;
		private IPEndPoint remoteEP;
		private Socket sender;

        public ConnectionsManager(){

        }
        public int StartClient() { 
            // Connect to a remote device.  
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                ipHostInfo = Dns.GetHostEntry("ec2-18-221-141-4.us-east-2.compute.amazonaws.com");
                ipAddress = ipHostInfo.AddressList[0]; 
                remoteEP = new IPEndPoint(ipAddress, 65432); 

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
            byte[] msgFunction = Encoding.ASCII.GetBytes("CreateAccount");
            response[0] = Messenger(msgFunction);

			byte[] msgUsername = Encoding.ASCII.GetBytes(param[0]);
			response[1] = Messenger(msgUsername);

			byte[] msgEmail = Encoding.ASCII.GetBytes(param[1]);
			response[2] = Messenger(msgEmail);

			byte[] msgFName = Encoding.ASCII.GetBytes(param[2]);
			response[3] = Messenger(msgFName);

			byte[] msgLName = Encoding.ASCII.GetBytes(param[3]);
			response[4] = Messenger(msgLName);

			byte[] msgPassword = Encoding.ASCII.GetBytes(param[4]);
			response[5] = Messenger(msgPassword);
			
			EndMessages();
			//byte[] msgAge = Encoding.ASCII.GetBytes(param[5]);
			response[6] = receive();
            return response;
		}

        public string[] GetAccountInfo(string param) {
            string[] response = new string[5];
            Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());
            //Data buffer for incoming data.
            byte[] msgFunction = Encoding.ASCII.GetBytes("GetAccountInfo");
            response[0] = Messenger(msgFunction);

            byte[] msgUserID = Encoding.ASCII.GetBytes(param);
            response[1] = Messenger(msgUserID);

            //byte[] msgUserWin = Encoding.ASCII.GetBytes(param[1]);
            //response[2] = Messenger(msgUserWin);

            //byte[] msgUserLoss = Encoding.ASCII.GetBytes(param[2]);
            //response[3] = Messenger(msgUserLoss);

            EndMessages();
            response[4] = receive();
            return response;
        }

		private void EndMessages(){
			send(Encoding.ASCII.GetBytes("end"));
		}

		private int send(byte[] msg){
				int bytesSent = sender.Send(msg);
				return bytesSent;
		}

		private string receive(){
			byte[] bytes = new byte[1024];
			int bytesRec = sender.Receive(bytes);
            if(bytes.Length > 0){
			string results = (Encoding.ASCII.GetString(bytes, 0, bytes.Length));
            Debug.Log("This is in the recieve class" + results);
            return (Encoding.ASCII.GetString(bytes, 0, bytes.Length));
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

        public void LogOff(){
                    // Release the socket.  
                    sender.Shutdown(SocketShutdown.Both); 
                    sender.Close(); 
		}

        public string[] SubmitLogin(string[] param) {
            string[] response = new string[4];
            Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());

            //prepare to send Login info to server

            //refer to server's ConnectionManager's main if/else statement to see which function name to match
            byte[] msgFunction = Encoding.ASCII.GetBytes("Login"); 
            response[0] = Messenger(msgFunction);

            byte[] msgUsername = Encoding.ASCII.GetBytes(param[0]);
            response[1] = Messenger(msgUsername);

            byte[] msgPassword = Encoding.ASCII.GetBytes(param[1]);
            response[2] = Messenger(msgPassword);

            EndMessages();

            response[3] = receive();

            return response;
        }
    }
}