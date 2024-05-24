using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace NewClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.GetEncoding(866);
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("\nСоединение #" + i.ToString() + "\n");
                Connect("127.0.0.1", "HelloWorld!#" + i.ToString());
            }
            Console.WriteLine("\nНажмите Enter...");
            Console.Read();
        }

        static void Connect(string server, string message)
        {
            try
            {
                // Создаём TcpClient.
                // Для созданного в предыдущем проекте TcpListener
                // Настраиваем его на IP нашего сервера и тот же порт.
                Int32 port = 9595;
                TcpClient client = new TcpClient(server, port);
                // Переводим наше сообщение в ASCII, а затем в массив Byte.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                // Получаем поток для чтения и записи данных.
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Отправлено: {0}", message);
                // Получаем ответ от сервера.
                // Буфер для хранения принятого массива bytes,
                data = new Byte[256];
                // Строка для хранения полученных ASCII данных.
                String responseData = String.Empty;
                // Читаем первый пакет ответа сервера.
                // Можно читать весь ответное сообщение.
                // Для этого надо организовать чтение в цикле как на сервере.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Получено: {0}", responseData);
                // Закрываем всё.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }
    }
}