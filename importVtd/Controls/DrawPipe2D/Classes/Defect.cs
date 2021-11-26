

namespace DrawPipe2D.ViewModel
{
    public class Defect
    {
        public double Angle { get; private set; } // Radian угловое положение дефекта
        public double W { get; private set; }//длина дефекта
        public double H { get; private set; }//высота(ширина) дефекта
        public double ShiftX { get; private set; }// расстояние от начала трубы
        public string KeySegmentOnDefect { get; private set; } // ключ сегмента трубы на котором находится дефект
        public string TypeDefect { get; private set; } // тип дефекта(ключ)
        public string PercentDepth { get; private set; } // процент глубины дефекта(нужно для определения цвета)
        public string HintDefect { get; private set; } // хинт дефекта

       

        //public Defect(double angleRadian, double w, double h, double shiftX, string keySegmentOnDefect, string colorDefect)
        public Defect(double angleRadian, double w, double h, double shiftX, string keySegmentOnDefect, string typeDefect, string percentDepth, string hintDefect)
                 
        {
            Angle = angleRadian;
            W = w;
            H = h;
            ShiftX = shiftX;
           // Scale = scale;
            KeySegmentOnDefect = keySegmentOnDefect;
            TypeDefect = typeDefect;
            PercentDepth = percentDepth;
            HintDefect = hintDefect;

        }
        
    }
}


//public static Path GeneratePath(string data)
//{
//    string pathEnvelope =
//        "<Path xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" Data=\"{0}\"/>";
//    return XamlReader.Load(String.Format(pathEnvelope, data)) as Path;
//}

//Из радиан (Radians) в градусы (Degrees):

//Degrees = 180*Radians/(Math.PI);

//Из градусов в радианы:

//Radians = (Math.PI/180)*Degrees;


