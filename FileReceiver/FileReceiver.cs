using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using NetworkExtension;

namespace FileReceiver
{
    internal sealed class FileReceiver
    {
        private readonly TcpListener _receiver;

        internal FileReceiver()
        {
            var port = ConnectionInfo.GetPortFromUserInput();

            var ipEndPoint = new IPEndPoint(IpAddress.GetMyIpAddress(), port.PortNumber);

            _receiver = new TcpListener(ipEndPoint);
            _receiver.Start();
        }

        public void Receive()
        {
            const int receiveTimeout = 2000;

            var client = _receiver.AcceptTcpClient();
            client.ReceiveTimeout = receiveTimeout;

            var clientSocket = client.Client;
            clientSocket.ReceiveTimeout = receiveTimeout;

            var buffer = new byte[client.ReceiveBufferSize];

            FileStream fileStream = null;

            while (true)
            {
                try
                {
                    var packageLength = clientSocket.Receive(buffer);

                    if (packageLength == 0)
                    {
                        break;
                    }

                    var receivedBytes = GetReceivedPackage(buffer, packageLength).ToArray();
                    
                    var data = Encoding.Unicode.GetString(receivedBytes);
                    
                    if (FileExtension.IsFileName(data))
                    {
                        fileStream = FileExtension.CreateFile(data);
                        continue;
                    }
                    
                    fileStream?.Write(receivedBytes, 0, receivedBytes.Length);
                }
                catch
                {
                    break;
                }
            }

            Console.Out.WriteLine(fileStream != null ? $"File path: {fileStream.Name}" : "File did not received");
            
            fileStream?.Close();
        }


        private static IEnumerable<byte> GetReceivedPackage(IReadOnlyList<byte> package, int receivedPackageLength)
        {
            for (var i = 0; i < receivedPackageLength; i++)
            {
                yield return package[i];
            }
        }
    }
}