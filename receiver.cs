using Encryption;
using Encryption_Library;
using System;
using System.Net.WebSockets;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Secure_Communication
{
    public class receiver
    {  
        public void Start()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 5000);
            listener.Start();
            Console.WriteLine("Waiting for sender...");

            using TcpClient client = listener.AcceptTcpClient();
            using NetworkStream netStream = client.GetStream();
            using StreamReader reader = new StreamReader(netStream);
            Console.WriteLine("Sender connected.");

            //  Receive OTP key + IV (sent as Base64 strings, one per line)
            string keyBase64 = reader.ReadLine();
            string ivBase64 = reader.ReadLine(); 

             

            byte[] key = Convert.FromBase64String(keyBase64);
            byte[] iv = Convert.FromBase64String(ivBase64);
            Console.WriteLine("OTP received.");





            //  Receive the encrypted message (Base64 string)
            // Decrypt using the OTP key/IV
            string cipherText = reader.ReadLine();

            string receivedTag = reader.ReadLine(); 

            if (!Hashing.VerifyHmac (cipherText, key , receivedTag))
            {
                Console.WriteLine("Message intergrity check failed , someone prolly read your message buddy ");
                return; 
            }

            string message = encryption.Decryprt(cipherText, key, iv);
            Console.WriteLine($"Decrypted message: {message}");

            listener.Stop();
        }


    }
}
