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
    public class DefectPercentColor
    {
        public class DefectPercent
        {
            public string Id { get; private set; }
            public string Name { get; private set; }
            public string Color { get; private set; }
            public string MinPercent { get; private set; }
            public string MaxPercent { get; private set; }
            public string Parent { get; private set; }

            public DefectPercent(string id, string name, string color, string minPercent, string maxPercent, string parent)
            {
                Id = id;
                Name = name;
                Color = color;
                MinPercent = minPercent;
                MaxPercent = maxPercent;
                Parent = parent;

            }
        }
        public class DefectColor
        {
            public List<DefectPercent> DefectColorList { get; private set; }
            public DefectColor(string xml)
            {

                DefectColorList = new List<DefectPercent>();
                XDocument xdoc = XDocument.Parse(xml);

                XElement root = xdoc.Element("koorozdefects");

                foreach (XElement x in root.Elements("defectType"))
                {
                    string id = x.Attribute("id").Value;
                    string name = x.Attribute("name").Value;
                    string minPercent = x.Attribute("minPercent").Value;
                    string maxPercent = x.Attribute("maxPercent").Value;
                    string parent = x.Attribute("parent").Value;
                    string color = string.Empty;

                    if (x.Attribute("color") != null)
                    {
                        color = x.Attribute("color").Value;
                    }
                    DefectPercent defectPercent = new DefectPercent(id, name, color, minPercent, maxPercent,parent);
                    DefectColorList.Add(defectPercent);
                }
            }
        }
    }
}
