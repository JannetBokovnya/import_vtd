using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using importVtd.startTable;

namespace importVtd.Business
{
    public class BindRepers : BoundedRepers
    {
        /// <summary>
        /// добавляет строку  увязанных реперов
        /// </summary>
        /// <param name="dbRow">строка реперов из БД</param>
        /// <param name="fileRow">строка реперов из файла</param>
        /// <param name="boundedRepers">ссылка на список увязанных реперов</param>
        /// <returns>пустая строка или сообщение об ошибке</returns>
        public string addReper(DbRepersTableList dbRow, FileRepersList fileRow, string impKey, ref List<BoundedRepers> boundedRepers, List<FileRepersList>  fileReper)
        {
            if (dbRow == null | fileRow == null)
                return "не инициализированы входные данные";

            if (boundedRepers.Count == 0)
            {
                boundedRepers.Add(new BoundedRepers()
                {
                    CountPoint = String.Empty,//кол-во пунктов
                    DbReperKey = dbRow.ObjKey,//ключ репера из базы
                    DifferenceInKm = String.Empty,//
                    FileReperKey = fileRow.RawKey,//ключ репера из файла
                    KmByMg = dbRow.Km,//км из бд
                    KmByVtd = fileRow.Km,//км из файла
                    LenByVtd = String.Empty,
                    LenByMg = String.Empty,
                    Num = "1"
                });
            }
            else
            {
                BoundedRepers calcRow = new BoundedRepers
                {
                    CountPoint = String.Empty,
                    DbReperKey = String.Empty,
                    KmByVtd = String.Empty,
                    KmByMg = String.Empty
                };

                //this.countPoint;
                //километраж по мг i
                string mg1Str = Regex.Replace(dbRow.Km, @"\s", "")
                                     .Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator)
                                     .Replace(".", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);
                double mg1 = Convert.ToDouble(mg1Str, CultureInfo.InvariantCulture);
                //километраж по мг i-1
                string mg2Str = Regex.Replace(boundedRepers[boundedRepers.Count - 1].KmByMg, @"\s", "")
                                     .Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator)
                                     .Replace(".", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);
                double mg2 = Convert.ToDouble(mg2Str, CultureInfo.InvariantCulture);
                double dMg = Math.Round(mg1 - mg2, 2); //разница
                //километраж по де i
                double de1 = Convert.ToDouble(fileRow.Km.Replace(" ", "").Replace(" ", "")
                                                        .Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator)
                                                        .Replace(".", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator)
                                               , CultureInfo.InvariantCulture);
                //километраж по де i-1
                double de2 = Convert.ToDouble(boundedRepers[boundedRepers.Count - 1].KmByVtd.Replace(" ", "").Replace(" ", "")
                                                                                            .Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator)
                                                                                            .Replace(".", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator)
                                               , CultureInfo.InvariantCulture);
                double dDe = Math.Round(de1 - de2, 2); //разница



                double koef = Math.Abs(((dMg - dDe) / dMg) * 100);

               
                calcRow.Koef = koef.ToString("0.000");

                calcRow.LenByVtd = dDe.ToString();
                calcRow.LenByMg = dMg.ToString();


                for (int i = 0; i < fileReper.Count; i++)
                {
                    if (fileReper[i] == fileRow)
                    {
                        calcRow.DifferenceInKm = (i + 1).ToString();
                        break;
                    }
                }

                boundedRepers.Add(calcRow);

                boundedRepers.Add(new BoundedRepers()
                {
                    CountPoint = string.Empty,//кол-во пунктов
                    DbReperKey = dbRow.ObjKey,//ключ репера из базы
                    DifferenceInKm = string.Empty,//разница с км
                    FileReperKey = fileRow.RawKey,//ключ репера из файла
                    KmByMg = dbRow.Km,//км из бд
                    KmByVtd = fileRow.Km,//км из файла
                    LenByVtd = string.Empty,
                    LenByMg = string.Empty,
                    Num = (Convert.ToDouble(boundedRepers[boundedRepers.Count - 2].Num.Replace(" ", "").Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                                                                                                       .Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator)
                                                                                                       ) + 1).ToString()
                });

            }
            return string.Empty;
        }
    }
}
