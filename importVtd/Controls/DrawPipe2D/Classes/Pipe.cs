using System;
using System.Collections.Generic;
using System.Net;
using DrawPipe2D.ViewModel;


namespace DrawPipe2D.Classes
{
    public class Pipe
    {
        public Pipe()
        {
            SegmentList = new List<PipeSegment>();

        }

        public List<PipeSegment> SegmentList { get; private set; }
        public ComparisonDefects.Defects d;


        public double Length
        {
            get
            {
                double result = 0d;
                foreach (var segment in SegmentList)
                {
                    result = result + segment.Length;
                }
                return result;

            }
        }

        public double RadiusTube { get; private set; }
        private const double COEFFICIENT_COMPRESS_X = 0.5d;


        //public void LoadData(List<MainPage.ContentTable> contentTableList, List<MainPage.DefectListOracle> defectListOracle)
        public void LoadData(List<MainPage.ContentTable> contentTableList)
        {

            RadiusTube = 1.4d / 2.0;

            double x1 = 0d;
            double x2 = 0d;
            double radiusTrube = 1.4d / 2.0;
            double nwidth = 0.1d;
            PipeSegment segment;
            Defect defect;
            string key;
            double angle;
            //double w = 10d;

            //public ContentTable(string npipe_element_montaj_key, string nnum_pipe_part, string loc_km_beg,
            //                    string nlength, string ndepth_pipe, string cType, string nAngle, string nCount)
            //{
            //    Npipe_element_montaj_key = npipe_element_montaj_key;//ключ элемента монтажа
            //    Nnum_pipe_part = nnum_pipe_part; //номер по отчету ВТД
            //    Loc_km_beg = loc_km_beg; //Километраж левого края
            //    Nlength = nlength;//длинна
            //    Ndepth_pipe = ndepth_pipe;//толщина стенки трубы (определена только если это труба - не тройник и т.п.
            //    CType = cType; //тип (я так понимаю имеется ввиду: труба, тройник ...)
            //    NAngle = nAngle; //угловое положение продольного св. шва
            //    NCount = nCount; //кол-во дефектов

            //}

            for (int i = 0; i < contentTableList.Count; i++)
            {
                double Nlength;
                bool isSuccess = double.TryParse(contentTableList[i].Nlength, out Nlength);
                double loc_km_beg;
                string hint = "Тип трубы: " + contentTableList[i].CType + "\n" +
                              "Номер трубы: " + contentTableList[i].Nnum_pipe_part + "\n" +
                              "Длина: " + contentTableList[i].Nlength + "\n" +
                              "Толщина стенки: " + contentTableList[i].Ndepth_pipe;
                isSuccess = double.TryParse(contentTableList[i].Loc_km_beg, out loc_km_beg);
                double nangle;
                isSuccess = double.TryParse(contentTableList[i].NAngle, out nangle);
                double nl = 0d;
                if (Nlength <= 2)
                {
                    nl = 3d;
                }
                else
                {
                    nl = Nlength;
                }
                x1 = x2;
                x2 = x1 + nl;
                segment = new PipeSegment(x1 * COEFFICIENT_COMPRESS_X, x2 * COEFFICIENT_COMPRESS_X, hint, contentTableList[i].Loc_km_beg,
                                          nangle, radiusTrube, contentTableList[i].Npipe_element_montaj_key, false, true, contentTableList[i].Ntype_pipe_key, contentTableList[i].Nlength);



                SegmentList.Add(segment);
                //x1 = x2;
                //x2 = x1 + w + i * 0.1;
            }

        }
        public void LoadDataDefect(List<MainPage.ContentTable> contentTableList, List<MainPage.DefectListOracle> defectListOracle)
        {
            Defect defect;
            double nwidth = 0.1d;

            for (int i = 0; i < SegmentList.Count; i++)
            {
                //SegmentList[ii].KeySegment
                for (int ii = 0; ii < defectListOracle.Count; ii++)
                {
                    if (SegmentList[i].KeySegment == defectListOracle[ii].S_pipe_elem)
                    {
                        string hintdefect = "Дефект трубы" + "\n" +
                                            "Тип дефекта:" + defectListOracle[ii].CType + "\n" +
                                            "Длина: " + defectListOracle[ii].Nlength + "\n" +
                                            "Ширина: " + defectListOracle[ii].Nwidth + "\n" +
                                            "Глубина: " + defectListOracle[ii].CDEPTH_POS + "\n" +
                                            "Расположение в металле: " + defectListOracle[ii].Ndepth;
                        if (defectListOracle[ii].Nwidth == 0d)
                        {
                            defect = new Defect(defectListOracle[ii].Nclockwise_pos, nwidth * COEFFICIENT_COMPRESS_X,
                            defectListOracle[ii].Nlength, defectListOracle[ii].Nprevseam_dist * COEFFICIENT_COMPRESS_X,
                            defectListOracle[ii].S_pipe_elem, defectListOracle[ii].Ndefect_type_key, defectListOracle[ii].PersentCorroz, hintdefect);
                        }
                        else
                        {
                            defect = new Defect(defectListOracle[ii].Nclockwise_pos, defectListOracle[ii].Nwidth * COEFFICIENT_COMPRESS_X,
                                defectListOracle[ii].Nlength, defectListOracle[ii].Nprevseam_dist * COEFFICIENT_COMPRESS_X,
                                defectListOracle[ii].S_pipe_elem, defectListOracle[ii].Ndefect_type_key, defectListOracle[ii].PersentCorroz, hintdefect);
                        }

                        SegmentList[i].DefectList.Add(defect);
                    }
                }
            }

        }
    }
}
//x1 = 0;
//x2 = 5.7466;
//key = "1201083944003";
//segment = new PipeSegment(x1, x2, "Двухшовная1\n123", "872289,6717", 2.9, 1.0, key, false);
////Defect(angle, width, height, shift, 100, "1201083947303",1);
//defect = new Defect(1.2, 200, 400, 100, 100, key);
//segment.DefectList.Add(defect);
//SegmentList.Add(segment);

//x1 = x1 + x2;
//x2 = x2 + 5.7527;
//key = "1201083947303";
//segment = new PipeSegment(x1, x2, "Двухшовная1\n123", "872289,6717", .9, 1.0, key, false);
//defect = new Defect(.7, 200, 400, 100, 100, key);
//segment.DefectList.Add(defect);
//SegmentList.Add(segment);
//double Nlength;
//bool isSuccess = double.TryParse(contentTableList[0].Nlength, out Nlength);
//double loc_km_beg;
//string hint = contentTableList[0].CType + "\n" + contentTableList[0].Nnum_pipe_part;
//isSuccess = double.TryParse(contentTableList[0].Loc_km_beg, out loc_km_beg);
//double nangle;
//isSuccess = double.TryParse(contentTableList[0].NAngle, out nangle);
//x2 = x1 + Nlength;
//segment = new PipeSegment(x1 * 0.5, x2 * 0.5, hint, contentTableList[0].Loc_km_beg, nangle, radiusTrube, contentTableList[0].Npipe_element_montaj_key, false, false);
//SegmentList.Add(segment);

//defect = new Defect(defectListOracle[0].Nclockwise_pos,defectListOracle[0].Nwidth,defectListOracle[0].Nlength,defectListOracle[0].Nprevseam_dist,defectListOracle[0].S_pipe_elem,defectListOracle[0].Ndefect_type_key,defectListOracle[0].PersentCorroz);
//segment.DefectList.Add(defect);


//SegmentList.Add(segment);

//for (int i=0; i)
//for (int i = 0; i < 1; i++)
//{
//    string mm = "";

//    angle = i;
//    key = i.ToString();
//    //segment = new PipeSegment(0 * 0.5, (0+ 11.7) * 0.5, "Двухшовная1\n123", i.ToString(), angle, radiusTrube, key, false, false);
//    //defect = new Defect(3.0, 3 * 0.5, 2, 4 * 0.5, key, colordefects.DefectList[0].Color);
//    //defect = new Defect(3.0, 3 * 0.5, 2, 4 * 0.5, key, "226101", "50");
//    //segment.DefectList.Add(defect);


//    //defect = new Defect(5.0, 2, 5, 7, key);
//    //segment.DefectList.Add(defect);


//    SegmentList.Add(segment);
//    x1 = x2;
//    x2 = x1 + w + i * 0.1;
//}
// for (int ii = 0; ii < defectListOracle.Count; ii++)
//{
//    if (contentTableList[i].Npipe_element_montaj_key == defectListOracle[ii].S_pipe_elem)
//    {
//        string hintdefect = "Дефект трубы" + "\n" + 
//                            "Тип дефекта:" + defectListOracle[ii].CType + "\n" +
//                            "Длина: " + defectListOracle[ii].Nlength + "\n" +
//                            "Ширина: " + defectListOracle[ii].Nwidth + "\n" +
//                            "Глубина: " + defectListOracle[ii].CDEPTH_POS + "\n" +
//                            "Расположение в металле: " + defectListOracle[ii].Ndepth;
//        if (defectListOracle[ii].Nwidth ==0d)
//        {
//            defect = new Defect(defectListOracle[ii].Nclockwise_pos, nwidth * COEFFICIENT_COMPRESS_X,
//            defectListOracle[ii].Nlength, defectListOracle[ii].Nprevseam_dist * COEFFICIENT_COMPRESS_X,
//            defectListOracle[ii].S_pipe_elem, defectListOracle[ii].Ndefect_type_key, defectListOracle[ii].PersentCorroz, hintdefect);
//        }
//        else
//        {
//            defect = new Defect(defectListOracle[ii].Nclockwise_pos, defectListOracle[ii].Nwidth * COEFFICIENT_COMPRESS_X,
//                defectListOracle[ii].Nlength, defectListOracle[ii].Nprevseam_dist * COEFFICIENT_COMPRESS_X,
//                defectListOracle[ii].S_pipe_elem, defectListOracle[ii].Ndefect_type_key, defectListOracle[ii].PersentCorroz, hintdefect);    
//        }

//        segment.DefectList.Add(defect);
//    }
//}