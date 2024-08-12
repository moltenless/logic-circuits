using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using LogicCircuits.Elements;
using System.IO;

namespace LogicCircuits
{
    public static class Serialization
    {
        public static void SerializeCircuit(string name, DateTime dateTime, List<IElement> circuit)
        {
            BinaryFormatter bf = new BinaryFormatter();

            if (!Directory.Exists("Circuits"))
                Directory.CreateDirectory("Circuits");

            using (FileStream stream = new FileStream("Circuits\\" + name + ".dat", FileMode.Create))
            {
                bf.Serialize(stream, (name, dateTime, circuit));
            }

            System.Windows.Forms.MessageBox.Show("Збережено успішно");
        }

        public static void DeserializeCircuit(string fileName, out string name, out DateTime dateTime, out List<IElement> circuit)
        {
            BinaryFormatter bf = new BinaryFormatter();

            using (FileStream stream = new FileStream(fileName, FileMode.Open))
            {
                (name, dateTime, circuit) = ((string, DateTime, List<IElement>))bf.Deserialize(stream);
            }
        }
    }
}