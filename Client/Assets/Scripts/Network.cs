using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

namespace GonePearShape.Client
{
	
	public class Network : MonoBehaviour
	{
			
	public static Network instance;
	public TcpClient playerSocket;
	public NetworkStream myStream;
	public StreamReader myReader;
	public StreamWriter myWriter;
	private byte[] aSyncBuff;
	public bool shouldHandleData;

	[Header("Network Settings")]
	public string ServerIP = "127.0.0.1";
	public int ServerPort = 5500;
	public bool isConnected;

		void awake ()
		{
			instance = this;
		}

		// Use this for initialization
		void Start ()
		{
			
		}
		
		// Update is called once per frame
		void Update ()
		{
			
		}

		void ConnectToServer()
		{
			if (playerSocket != null)
			{
				if (playerSocket.Connected || isConnected) 
				{
					return;
				}
					playerSocket.Close();
					playerSocket = null;

				playerSocket = new TcpClient();
				playerSocket.ReceiveBufferSize = 4096;
				playerSocket.SendBufferSize = 4096;
				playerSocket.NoDelay = false;
				Array.Resize (ref aSyncBuff, 8192);
			}
		}

		void OnConnectedCallBack (IAsyncResult result)
		{
			if (playerSocket != null)
			{
				playerSocket.EndConnect (result);
				if (playerSocket.Connected == false)
				{
					isConnected = false;
					return;
				}
				else
				{
					playerSocket.NoDelay = true;
					myStream = playerSocket.GetStream ();
					myStream.BeginRead (aSyncBuff, 0, 8192, OnReceiveDataCallBack, null);
					Debug.Log ("Connected to GameServer");
				}

			}
		}

		void OnReceiveDataCallBack (IAsyncResult result)
		{
			if (playerSocket != null)
			{
				if (playerSocket == null)
				{
					return;
				}

			int byteArray = myStream.EndRead (result);
			byte[] myBytes = null;
			Array.Resize (ref myBytes, byteArray);
			Buffer.BlockCopy (aSyncBuff, 0, myBytes, 0, byteArray);

				if (byteArray == 0)
				{
					playerSocket.Close ();
					Debug.Log ("Disconnected from GameServer");
					return;
				}
			}
		}
	}
}