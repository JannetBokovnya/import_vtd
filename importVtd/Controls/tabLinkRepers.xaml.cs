using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Controls.GridView;
using importVtd.Business;
using importVtd.Resources;
using importVtd.startTable;
using Telerik.Windows.Controls;

namespace importVtd.Controls
{
    public partial class TabLinkRepers
    {
        public MainViewModel Model { get; private set; }

        private List<DbRepersTableList> _dbReper;
        private List<DbRepersTableList> _dbReperTemplate;

        private List<FileRepersList> _fileReper;
        private List<FileRepersList> _fileReperTemplate;


        /// <summary>
        /// список увязанных реперов
        /// (не забыть - данные в шахматном порядке)
        /// </summary>
        private List<BoundedRepers> _boundedRepers = new List<BoundedRepers>();

        /// <summary>
        /// описывает пары удаленых ключей из списка
        /// </summary>
        private List<Point> _deletedRepers = new List<Point>();


        public TabLinkRepers(MainViewModel model)
        {
            InitializeComponent();


            Model = model;
            Model.PropertyChanged -= MainModelPropertyChanged;
            Model.PropertyChanged += MainModelPropertyChanged;

            if (Model.GetNewImportInfo.Count > 0)
            {
                LbreperSection.Content = Model.GetNewImportInfo[0].cNameVTG + " ( " + Model.GetNewImportInfo[0].nKmBegin 
                                                                            + " " + Resources_ImpVtd.cKm 
                                                                            + "  - " + Model.GetNewImportInfo[0].nKmEnd 
                                                                            + " " + Resources_ImpVtd.cKm + " )";    
            }
            
            //реперы из БД
            Model.GetDbRepers(Model.KeyImport);
        }

        //nSectionKey      -- Ключ  участка ВТД
        //cNameVTG,        -- название участка ВТД
        //dDateContract,   -- дата подписания
        //nKmBegin,        -- км. начала
        //nKmEnd,          -- км. конца
        //nLength,         -- длина
        //cMainExecutor,   -- генеральный подрядчик
        //cNameWork,       -- название комплекса работ
        //nContractKey     -- Ключ договора
        //cNumberContract, -- номер договора
        //cSubExecutor     -- исполнитель работ
        //nPipeKey         -- Ключ нити
        //сPipeName        -- Нить
        //nMTKey           -- Ключ газопровода
        //сMTName          -- Газопровод
       

    

        /// <summary>
        /// кнопка увязать реперы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBind_Click(object sender, RoutedEventArgs e)
        {
            BtnBind.IsEnabled = false;

            //добавляем в массив ключи выбранных реперов
            try
            {
                string s = new BindRepers().addReper((DbRepersTableList)GrdReperLeft.SelectedItem,
                                                     (FileRepersList)GrdReperRight.SelectedItem,
                                                      Model.KeyImport, ref _boundedRepers, _fileReper);
                if (!string.IsNullOrEmpty(s))
                {
                    MessageBox.Show(s);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources_ImpVtd.Message21 + " " + ex.Message, Resources_ImpVtd.Error,
                                MessageBoxButton.OK);
            }

            //если все ок, в таблицу увязанных реперов помещаем массив
            GrdReperBound.ItemsSource = null;
            GrdReperBound.ItemsSource = _boundedRepers;

            _deletedRepers.Add(new Point()
            {
                X = Convert.ToDouble(((DbRepersTableList)GrdReperLeft.SelectedItem).ObjKey),
                Y = Convert.ToDouble(((FileRepersList)GrdReperRight.SelectedItem).RawKey)
            });

            //удаляем из массива реперы БД увязанные
            for (int i = 0; i < _dbReper.Count; i++)
            {
                if (_dbReper[i] == (DbRepersTableList)GrdReperLeft.SelectedItem)
                {
                    List<DbRepersTableList> tmpList = _dbReper.ToList();
                    tmpList.RemoveRange(0, i + 1);
                    _dbReper = tmpList.ToList(); //возвращаем
                    break;
                }
            }
            //удаляем из массива реперы файла увязанные
            for (int i = 0; i < _fileReper.Count; i++)
            {
                if (_fileReper[i] == (FileRepersList)GrdReperRight.SelectedItem)
                {
                    List<FileRepersList> tmpList = _fileReper.ToList();
                    tmpList.RemoveRange(0, i + 1);
                    _fileReper = tmpList.ToList(); //возвращаем
                    break;
                }
            }
            GrdReperLeft.ItemsSource = null;
            GrdReperLeft.ItemsSource = _dbReper;

            GrdReperRight.ItemsSource = null;
            GrdReperRight.ItemsSource = _fileReper;
        }

        /// <summary>
        /// кнопка отвязкать реперы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUndo_OnClick(object sender, RoutedEventArgs e)
        {
            string dbKey = _boundedRepers[_boundedRepers.Count - 1].DbReperKey;
            string fileKey = _boundedRepers[_boundedRepers.Count - 1].FileReperKey;
            string prevDbKey = string.Empty; //хранит ключ предыдущего импорта
            string prevFileKey = string.Empty; //хранит ключ предыдущего импорта


            if (_deletedRepers.Count() > 1)
            {

                #region обработка реперов из БД

                //проходим по всему списку,  и пытаемся получить предыдущий ключ
                for (int i = 0; i < _deletedRepers.Count(); i++)
                {
                    if (_deletedRepers[i].X == Convert.ToDouble(dbKey))
                    {
                        break;
                    }
                    else
                    {
                        prevDbKey = _deletedRepers[i].X.ToString();
                    }
                }
                //если предыдущий ключ не получен,  получаем последний из массива
                if (string.IsNullOrEmpty(prevDbKey))
                    prevDbKey = _deletedRepers[_deletedRepers.Count - 1].X.ToString();


                int index = -1;
                //ищем начало участка для копирования
                for (int i = 0; i < _dbReperTemplate.Count(); i++)
                {
                    if (_dbReperTemplate[i].ObjKey == prevDbKey) //нашли начало участка, для копирования
                    {
                        index = i;
                        break;
                    }
                }
                if (index < 0) //если по каким либо причинам не получили ключ, сваливаемся с ошибкой
                {
                    //теоретически, это  сообщение мы никогда не увидим
                    //MessageBox.Show("Не удалось восстановить список реперов из БД,  дальнейшая работа остановлена");
                    Model.Report("Не удалось восстановить список реперов из БД,  дальнейшая работа остановлена");
                    return;
                }
                else
                {
                    //если  участок для копирования определен и все ок
                    _dbReper = null;
                    List<DbRepersTableList> tmpList = new List<DbRepersTableList>();
                    for (int i = index + 1; i < _dbReperTemplate.Count(); i++) //копируем участок в рабочий массив
                    {
                        tmpList.Add(new DbRepersTableList()
                        {
                            Name = _dbReperTemplate[i].Name,
                            Filtertype = _dbReperTemplate[i].Filtertype,
                            Km = _dbReperTemplate[i].Km,
                            ObjKey = _dbReperTemplate[i].ObjKey,
                            EntityName = _dbReperTemplate[i].EntityName
                        });
                    }

                    _dbReper = tmpList.ToList(); //ковертируем в массив
                    GrdReperLeft.ItemsSource = _dbReper; //показываем с таблице
                }

                #endregion

                #region обработка реперов из файла

                //проходим по всему списку,  и пытаемся получить предыдущий ключ
                for (int i = 0; i < _deletedRepers.Count(); i++)
                {
                    if (_deletedRepers[i].Y == Convert.ToDouble(fileKey))
                    {
                        break;
                    }
                    else
                    {
                        prevFileKey = _deletedRepers[i].Y.ToString();
                    }
                }
                //если предыдущий ключ не получен,  получаем последний из массива
                if (string.IsNullOrEmpty(prevFileKey))
                    prevFileKey = _deletedRepers[_deletedRepers.Count - 1].Y.ToString();


                index = -1;
                //ищем начало участка для копирования
                for (int i = 0; i < _fileReperTemplate.Count(); i++)
                {
                    if (_fileReperTemplate[i].RawKey == prevFileKey) //нашли начало участка, для копирования
                    {
                        index = i;
                        break;
                    }
                }
                if (index < 0) //если по каким либо причинам не получили ключ, сваливаемся с ошибкой
                {
                    //теоретически, это  сообщение мы никогда не увидим
                    //MessageBox.Show("Не удалось восстановить список реперов из файла,  дальнейшая работа остановлена");
                    Model.Report("Не удалось восстановить список реперов из файла,  дальнейшая работа остановлена");
                    return;
                }
                else
                {
                    //если  участок для копирования определен и все ок
                    _fileReper = null;
                    List<FileRepersList> tmpList = new List<FileRepersList>();
                    for (int i = index + 1; i < _fileReperTemplate.Count(); i++) //копируем участок в рабочий массив
                    {
                        tmpList.Add(new FileRepersList()
                        {
                            //cobject = fileReperTemplate[i].cobject,
                            Km = _fileReperTemplate[i].Km,
                            RawKey = _fileReperTemplate[i].RawKey,
                            Desc = _fileReperTemplate[i].Desc,
                            Type = _fileReperTemplate[i].Type
                        });

                    }
                   
                    _fileReper = tmpList.ToList(); //ковертируем в массив 
                    GrdReperRight.ItemsSource = _fileReper; //показываем с таблице
                }

                #endregion
            }
            else
            {
                //если в списке всего  одна увязанная строка, тогда показываем все строки
                //используем глупый способ копирования массивов,
                //можно было просто скопировать,  но этот  способ точно будет работать и не надо отлаживать
                //List<DBRepersTableList> tmpList = dbReperTemplate.ToList();
                //dbReper = tmpList.ToList();
                GrdReperLeft.ItemsSource = _dbReperTemplate.ToList(); //показываем с таблице

                List<FileRepersList> tmpList2 = _fileReperTemplate.ToList();
                _fileReper = tmpList2.ToList();
                GrdReperRight.ItemsSource = _fileReper; //показываем с таблице

            }
            _deletedRepers.RemoveAt(_deletedRepers.Count - 1); //удаляем последную строку


            if (_boundedRepers.Count == 1) //удаляем из  шахматного поля
            {
                _boundedRepers.RemoveAt(_boundedRepers.Count - 1);
            }
            else
            {
                _boundedRepers.RemoveAt(_boundedRepers.Count - 1);
                _boundedRepers.RemoveAt(_boundedRepers.Count - 1);
            }

            GrdReperBound.ItemsSource = null; //перестраиваем кол-во  увязанных
            GrdReperBound.ItemsSource = _boundedRepers;

        }

        private void BuNext_OnClick(object sender, RoutedEventArgs e)
        {
            if (_deletedRepers.Count == 0)
            {
                MessageBox.Show(Resources_ImpVtd.ReperBuNextMessage, Resources_ImpVtd.Information, MessageBoxButton.OK);
                return;
            }

            StringBuilder keyPairs = new StringBuilder();
            for (int i = 0; i < _deletedRepers.Count(); i++)
            {
                keyPairs.Append(_deletedRepers[i].X + "," + _deletedRepers[i].Y + ";");
            }

            Model.AddKeyMapRepers(Model.KeyImport, keyPairs.ToString().TrimEnd(';'));

        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Model.FirePropertyChanged("BackNewImport");
        }

        private void MainModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "GetDBRepersList")
            {
                List<DbRepersTableList> table = new List<DbRepersTableList>(Model.DbRepersList);

                if (table.Count == 0)
                {
                    MessageBox.Show(Resources_ImpVtd.linkNoReperBD, Resources_ImpVtd.Information, MessageBoxButton.OK);
                    Model.Report("не удалось наполнить  список  реперов из бд = 0");
                    //непонятно что делать если нет массива
                }

                //реперы из базы в массиве
                _dbReper = new List<DbRepersTableList>(table);
                _dbReperTemplate = new List<DbRepersTableList>(table);
                GrdReperLeft.ItemsSource = table; //наполняем таблицу

                //наполняем реперы из файла
                Model.GetFileRepers(Model.KeyImport);
             }
             //реперы из файла
             if (e.PropertyName == "GetFileRepersList")
             {
                 List<FileRepersList> table = new List<FileRepersList>(Model.FileRepersList);
                 if (table.Count == 0)
                 {
                     MessageBox.Show(Resources_ImpVtd.linkNoReperBD, Resources_ImpVtd.Information, MessageBoxButton.OK);
                     Model.Report("не удалось наполнить  список  реперов из файла = 0");
                     return;
                 }
                 GrdReperRight.ItemsSource = table;

                 //наполняем структуры реперами из файла
                 _fileReper = new List<FileRepersList>(table);
                 _fileReperTemplate = new List<FileRepersList>(table);
             }
        }

        private void GrdReperBound_OnRowLoaded(object sender, RowLoadedEventArgs e)
        {
            var gr = e.Row as GridViewRow;
            if (gr != null)
            {
                if (((BoundedRepers)((gr).Item)).Koef != null)
                {
                    try
                    {
                        double result = Convert.ToDouble(((BoundedRepers)((gr).Item)).Koef);
                        if (result > 3d)
                            gr.Foreground = new SolidColorBrush(Colors.Red);
                    }
                    catch (Exception)
                    {
                        Model.Report("Ошибка в конвертации GrdBound_OnRowLoaded =" + ((BoundedRepers)((gr).Item)).Koef);
                    }
                }
            }
        }

        private void GrdsReper_OnSelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            bool isBtnActive = GrdReperLeft.SelectedItem != null && GrdReperRight.SelectedItem != null;

            BtnBind.IsEnabled = isBtnActive;
        }
    }
}
