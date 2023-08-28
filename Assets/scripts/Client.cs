using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Linq;
using System.Text;

public class Client
{
    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    public byte[] buffer = new byte[2024];
    public int rcve;
    public IPEndPoint remoteIP;
    public IPEndPoint this_epIP;
    public EndPoint remoteEP;
    public EndPoint this_ep;
    public bool cleanedUp = false;
    public bool connected = false;
    public Client()
    {
        remoteIP = new IPEndPoint(IPAddress.Loopback, 6969);
        remoteEP = (EndPoint)remoteIP;
        this_epIP = new IPEndPoint(IPAddress.Loopback, 6969);
        this_ep = this_epIP;
        socket.ReceiveTimeout = 3;

    }
    public void Connect()
    {
        string task = "Starting HandShake...";
        Debug.Log("Sending: " + task);
        buffer = Encoding.ASCII.GetBytes(task);
        socket.SendTo(buffer, remoteEP);
    }
    public void ConnectResponse()
    {
        buffer = new byte[2024];
        rcve = socket.ReceiveFrom(buffer, ref remoteEP);
        Debug.Log("incomming from :" + remoteEP.ToString() + "/ content: "+ ASCIIEncoding.ASCII.GetString(buffer,0,rcve));
        if (rcve != 0)
        {
            connected = true;
            socket.Connect(remoteEP);
            Debug.Log("connection succesfull");
            
        }
        else
        {
            CleanUp();
        }

    }
    public void Ping()
    {
        //send a ping
        buffer = new byte[2024];
        buffer = Encoding.ASCII.GetBytes("Ping");
        socket.SendTo(buffer, remoteEP);
        //rcvs pong
        buffer = new byte[2024];
        rcve = socket.ReceiveFrom(buffer, ref remoteEP);
        if (rcve == 0)
        {
            CleanUp();
        }   
       
    }
    public void Send(string send ,int bufferSize)
    {
        //Debug.Log("Sending:" + send);

        buffer = new byte[bufferSize];
        buffer = Encoding.ASCII.GetBytes(send);
        socket.SendTo(buffer, remoteEP);

    }
    public string Receive(int bufferSize)
    {
        buffer = new byte[bufferSize];
        rcve = socket.ReceiveFrom(buffer, ref remoteEP);
        //Debug.Log("Recived:" + Encoding.ASCII.GetString(buffer, 0, rcve));
        if (rcve != 0)
        {
            return Encoding.ASCII.GetString(buffer, 0, rcve);
        }
        else
        {   
            CleanUp();
            return "NetworkError";
        }
    }
    public void CleanUp()
    {
        socket.Close();
        cleanedUp = true;
        connected = false;
    }
}
