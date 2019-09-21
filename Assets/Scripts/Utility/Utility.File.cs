using System.Text;

public static partial class Utility 
{
    public class File
    {
        public static void Write(string paht, byte[] data)
        {
            if (System.IO.File.Exists(paht))
            {
                System.IO.File.WriteAllBytes(paht, data);
            }
            else
            {
                System.IO.FileStream stream = System.IO.File.Create(paht);
                stream.Write(data, 0, data.Length);
                stream.Flush();
                stream.Close();
            }
        }

        public static void WriteAllText(string path, string contents)
        {
            Write(path, Encoding.UTF8.GetBytes(contents));
        }

        public static string ReadAllText(string filePath)
        {
            return System.IO.File.ReadAllText(filePath);
        }

        public static byte[] ReadAllBytes(string filePath)
        {
            return System.IO.File.ReadAllBytes(filePath);
        }
    }
}
