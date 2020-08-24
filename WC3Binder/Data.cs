using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System;
namespace WC3Binder
{
    class Data
    {
        public int[] type { get; }
        Keys[] oldK;
        public Keys[] newK { get; }
        public Point[] p { get; }
        public short[] code { get; }
        public Data()
        {
            type = new int[18] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 };
            oldK = new Keys[18];
            newK = new Keys[18];
            p = new Point[18];
            code = new short[18] { 0x4D, 0x4A, 0x48, 0x56, 0x50, 0, 0, 0, 0, 0, 0, 0, 0x67, 0x68, 0x64, 0x65, 0x61, 0x62 };
            XDocument xdoc = XDocument.Load("Bindings.xml");
            KeysConverter kc = new KeysConverter();
            int i = 0;
            foreach (XElement bind in xdoc.Element("Bindings").Elements("Bind"))
            {
                oldK[i] = (Keys)kc.ConvertFromInvariantString(bind.Element("oldKey").Value);
                if (kc.ConvertFromInvariantString(bind.Element("newKey").Value) != null)
                    newK[i] = (Keys)kc.ConvertFromInvariantString(bind.Element("newKey").Value);
                i++;
            }
            p[5].X = 1452; p[5].Y = 936;
            p[6].X = 1535; p[6].Y = 936;
            p[7].X = 1602; p[7].Y = 936;
            p[8].X = 1375; p[8].Y = 1009;
            p[9].X = 1452; p[9].Y = 1009;
            p[10].X = 1535; p[10].Y = 1009;
            p[11].X = 1602; p[11].Y = 1009;
        }
        public void setNewKey(int button, Keys k)
        {
            XDocument xdoc = XDocument.Load("Bindings.xml");
            XElement root = xdoc.Element("Bindings");
            foreach (XElement xe in root.Elements("Bind").ToList())
            {
                if (xe.Attribute("button").Value == button.ToString())
                {
                    if (k != Keys.None)
                    {
                        xe.Element("newKey").Value = k.ToString();
                        newK[button - 1] = k;
                    }
                    else
                    {
                        xe.Element("newKey").Value = k.ToString();
                        newK[button - 1] = k;
                    }
                }
            }
            xdoc.Save("Bindings.xml");
        }
        public int getNumber(Keys nwK)
        {
            for (int i = 0; i < 18; i++)
                if (nwK == newK[i])
                    return i;
            return -1;
        }
        public void AddExeLinks(string filename, int counter)
        {
            try
            {
                XDocument xdoc = XDocument.Load("Config.xml");
                XElement root = xdoc.Element("Root");
                bool check = false;
                foreach (XElement xe in root.Elements("Path").ToList())
                {
                    if (Int32.Parse(xe.Attribute("Number").Value) == counter)
                    {
                        xe.Element("Filename").Value = filename;
                        check = true;
                    }
                }
                if(!check)
                root.Add(new XElement("Path", new XAttribute("Number", counter),
                        new XElement("Filename", filename)));
                xdoc.Save("Config.xml");
                
            }
            catch (System.IO.FileNotFoundException)
            {
                XDocument xdoc = new XDocument();
                XElement root = new XElement("Path");
                XAttribute xcounter = new XAttribute("Number", counter);
                XElement xfilename = new XElement("Filename", filename);
                root.Add(xcounter, xfilename);
                xdoc.Add(root);
                xdoc.Save("Config.xml");

            }
        }
        public bool GetExeLinks(string[] name)
        {
            XDocument xdoc = XDocument.Load("Config.xml");
            XElement root = xdoc.Element("Root");
            if (root != null)
            {
                int i = 0;
                foreach (XElement xe in root.Elements("Path").ToList())
                {
                    name[i] = xe.Element("Filename").Value;
                    i++;
                }
                return true;
            }
            else return false;
        }
    }
}