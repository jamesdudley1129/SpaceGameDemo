using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Linq;
using System.Text;
public class HostClient
{
    public Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    public IPEndPoint this_endpoint;
    public IPEndPoint sender;
    public EndPoint remoteEP;
    int rcve = new int();
    public byte[] buffer = new byte[2024];
    public bool connected = false;
    public bool cleanedUp;
    public HostClient()
    {
        cleanedUp = false;
        this_endpoint = new IPEndPoint(IPAddress.Loopback, 6969);
        sender = new IPEndPoint(IPAddress.Any, 6969);
        socket.ReceiveTimeout = 3;
        socket.Bind(this_endpoint);
    }
   
    public void Listen()
    {
        //socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
        
        remoteEP = (EndPoint)sender;

        //CleanUp();

    }

    public void FirstReceive()
    {
        Debug.Log("waiting for HandShakes...");
        rcve = socket.ReceiveFrom(buffer, ref remoteEP);
        socket.Connect(remoteEP);
        Debug.Log("incomming connection from:" + remoteEP.ToString() +"/content:" + Encoding.ASCII.GetString(buffer, 0, rcve));

        if (rcve == 0)
        {
            CleanUp();
        }
        else
        {
            connected = true;
            FirstSend();
        }

    }

    public void FirstSend()
    {
        buffer = Encoding.ASCII.GetBytes("Hand Shake Completed. /");
        Debug.Log("sending:" + "Hand Shake Completed. ");
        socket.SendTo(buffer, remoteEP);
    }
    
    public void Pong()
    {

        //rcvs ping
        buffer = new byte[2024];
        rcve = socket.ReceiveFrom(buffer, ref remoteEP);
        if (rcve == 0)
        {
            CleanUp();
        }
        //sends pong
        buffer = new byte[2024];
        buffer = Encoding.ASCII.GetBytes("pong");
        socket.SendTo(buffer, remoteEP);
    }

    public void Send(string msg ,int bufferSize)
    {
        //Debug.Log("Sending: " + msg);
        buffer = new byte[bufferSize];
        buffer = Encoding.ASCII.GetBytes(msg);
        socket.SendTo(buffer, remoteEP);
    }

    public string Receive(int bufferSize)
    {
        string msg = "null";

        buffer = new byte[bufferSize];

        rcve = socket.ReceiveFrom(buffer, ref remoteEP);
        if (rcve != 0)
        {
            msg = Encoding.ASCII.GetString(buffer, 0, rcve);
            //Debug.Log("Recived: "+ msg);
        }
        else
        {
            CleanUp();
        }
        return msg;
    }

    public void CleanUp()
    {
        socket.Close();
        remoteEP = null;
        connected = false;
        cleanedUp = true;
    }
}

