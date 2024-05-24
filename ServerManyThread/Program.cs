using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace MultiThreadServer
{
    class ExampleTcpListener
    {
        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                int MaxThreadsCount = Environment.ProcessorCount * 4;
                ThreadPool.SetMaxThreads(MaxThreadsCount, MaxThreadsCount);
                // Установим минимальное количество рабочих потоков
                ThreadPool.SetMinThreads(2, 2);
                // Устанавливаем порт для TcpListener = 9595.
                Int32 port = 9595;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                int counter = 0;
                server = new TcpListener(localAddr, port);
                Console.OutputEncoding = Encoding.GetEncoding(866);
                Console.WriteLine("Конфигурация многопоточного сервера:");
                Console.WriteLine("IP-адрес: 127.0.0.1");
                Console.WriteLine("Порт: " + port.ToString());
                Console.WriteLine("Потоки: " + MaxThreadsCount.ToString());
                Console.WriteLine("Сервер запущен");
                // Запускаем TcpListener и начинаем слушать клиентов,
                server.Start();
                // Принимаем клиентов в бесконечном цикле,
                while (true)
                {
                    Console.Write("Ожидание соединения...");
                    ThreadPool.QueueUserWorkItem(Clientprocessing, server.AcceptTcpClient());
                    // Выводим информацию о подключении.
                    Console.WriteLine("Соединение №" + counter.ToString() + "!");
                    counter++;
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine($"SocketException: {e}");
            }
            finally
            {
                // Останавливаем сервер
                server.Stop();
            }
            Console.WriteLine("Нажмите Enter...");
            Console.Read();
        }
        static void Clientprocessing(object clientobj)
        {
            // Буфер для принимаемых данных.
            byte[] bytes = new byte[256];
            string data = null;
            TcpClient client = clientobj as TcpClient;
            data = null;
            // Получаем информацию от клиента
            NetworkStream stream = client.GetStream();
            int i;
            // Принимаем данные от клиента в цикле, пока не дойдем до конца,
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                // Преобразуем данные в ASCII string.
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                // Преобразуем строку к верхнему регистру,
                data = data.ToUpper();
                // Преобразуем полученную строку в массив байт.
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                // Отправляем данные обратно клиенту (ответ),
                stream.Write(msg, 0, msg.Length);
            }
            // Закрываем соединение,
            client.Close();
        }
    }
}
