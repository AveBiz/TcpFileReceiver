namespace FileReceiver
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var receiver = new FileReceiver();

            receiver.Receive();
        }
    }
}
