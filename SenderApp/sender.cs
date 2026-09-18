using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;
using System.Text;
using Encryption;
using Encryption_Library;

namespace SenderApp
{
    public class sender
    {
        public void Start()
        {
            Console.Write("Enter the receiver IP address : ");
            string IP = Console.ReadLine();

                Console.Write("Enter the message to send :");
                string Message = Console.ReadLine(); 
                 
                Message = ErrorHandling.CheckValidInput(Message);


            using (TcpClient client = new TcpClient())
            {
                client.Connect(IP, 5000);
                Console.WriteLine("Connected to receiver");

                using (NetworkStream networkStream = client.GetStream())
                {


                    using (StreamReader reader = new StreamReader(networkStream))

                    using (StreamWriter writer = new StreamWriter(networkStream))
                    {
                        writer.AutoFlush = true;


                        // Getting te reciers RSA public key 
                        string publicKeyXml = reader.ReadLine();
                        Console.WriteLine("Public key received from receiver");



                        var (key, iv) = encryption.GenerateKeyAndIV();


                        //Encryptiong the des key & iv using reevier public key        
                        //And then Sending  OTP key + IV

                        string encryptedKey = RSA_Encryption.EncryptionWithRSA(key, publicKeyXml); 
                        string encryptedIv = RSA_Encryption.EncryptionWithRSA(iv, publicKeyXml); 
                  
                        writer.WriteLine(encryptedKey);
                        writer.WriteLine(encryptedIv);
                        Console.WriteLine("OTP sent");



                        // Encrypt the message and send it 
                        string cipherText = encryption.Encrption(Message, key, iv);

                        string tag = Hashing.CreateHMAC(cipherText, key); 
                        writer.WriteLine(cipherText); 
                        writer.WriteLine(tag); 

                        Console.WriteLine($"Encryption message sent {cipherText}");
                    }
                }

            }

            Console.WriteLine("Message sent sucessfully"); 
        }
    }
}

