using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace DrawPipe2D.Classes
{
    public class ComparisonDefects
    {
        public class DefectType
        {
            public string Id { get; private set; }
            public string Name { get; private set; }
            public string Color { get; private set; }
            public List<string> KeyList { get; private set; }

            public DefectType(string id, string name, string color)
            {
                Id = id;
                Name = name;
                Color = color;
                KeyList = new List<string>();
            }
        }
        public class Defects
        {
            public List<DefectType> DefectList { get; private set; }

            public Defects(string xml)
            {
                DefectList = new List<DefectType>();

                XDocument xdoc = XDocument.Parse(xml);

                XElement root = xdoc.Element("defects");

                foreach (XElement x in root.Elements("defectType"))
                {
                    string id = x.Attribute("id").Value;
                    string name = x.Attribute("name").Value;

                    string color = string.Empty;

                    if (x.Attribute("color") != null)
                    {
                        color = x.Attribute("color").Value;
                    }

                    DefectType defectType = new DefectType(id, name, color);

                    foreach (XElement xk in x.Elements("key"))
                    {
                        string keyName = xk.Value;
                        defectType.KeyList.Add(keyName);
                    }

                    DefectList.Add(defectType);
                }
            }
        }
    }
}
