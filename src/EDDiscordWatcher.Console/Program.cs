using System.Text;
using Ionic.Zlib;
using NetMQ;
using NetMQ.Sockets;

using (var writer = new StreamWriter($"log-{DateTime.Now.Ticks}.txt"))
{
    try
    {
        using (var client = new SubscriberSocket())
        {
            int count = 1;

            client.Options.ReceiveHighWatermark = 1000;
            client.Connect("tcp://eddn.edcd.io:9500");
            client.SubscribeToAnyTopic();

            while (!Console.KeyAvailable)
            {
                byte[] bytes;
                if (!client.TryReceiveFrameBytes(TimeSpan.FromMinutes(1), out bytes))
                {
                    continue;
                }

                var uncompressed = ZlibStream.UncompressBuffer(bytes);
                var result = Encoding.UTF8.GetString(uncompressed);

                Console.WriteLine(result);
                
                writer.WriteLine(result);
            }
        }
    }
    catch (Exception e)
    {
        Console.WriteLine("ERROR");
    }
}
