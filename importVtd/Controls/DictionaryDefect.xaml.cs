using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using importVtd.Business;
using importVtd.Resources;
using importVtd.startTable;

namespace importVtd.Controls
{
    public partial class DictionaryDefect
    {
        private IoraWCFService_ImpVtdClient _proxy;

        private GetDBDefectList selectedRow; //выбранная строка в таблице дефектов
        private GetDBDefectListFile selectedRowFile; //выбранная строка в таблице дефектов из файла
        private GetDBDefectMapingList selectedRowMapingList; //выбранная строка в таблице сопоставлений

        private GetDBDefectList _defList; //при отмене мапинга 
        private GetDBDefectListFile _defListFile; //при отмене мапинга 
        private GetDBDefectMapingList _itemMaping;//при отмене мапинга 

        private IoraWCFService_ImpVtdClient Proxy
        {
            get { return _proxy ?? (_proxy = new IoraWCFService_ImpVtdClient()); }
        }

        public MainViewModel Model
        {
            get { return DataContext as MainViewModel; }
        }

        public DictionaryDefect()
        {
            InitializeComponent();

        }

        private void DictionaryDefect_OnLoaded(object sender, RoutedEventArgs e)
        {
            //список дефектов из базы
            Proxy.GetDBDefect_ListCompleted += Proxy_GetDBDefect_ListCompleted;
            Proxy.GetDBDefect_ListAsync();

            //список увязанных дефектов
            Proxy.GetDbDefectMapingCompleted += Proxy_GetDBDefectMapingCompleted;
            Proxy.GetDbDefectMapingAsync(Model.KeyImport);
        }

        /// <summary>
        /// сопоставление дефекта из бд и файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Proxy_GetDBDefectMapingCompleted(object sender, GetDbDefectMapingCompletedEventArgs e)
        {
            Proxy.GetDbDefectMapingCompleted -= Proxy_GetDBDefectMapingCompleted;
            if (e.Result.IsValid)
            {
                List<GetDBDefectMapingList> mapList = new List<GetDBDefectMapingList>(e.Result.GetDBDefectMapingList);
                GrdCompareDefect.ItemsSource = null;
                GrdCompareDefect.ItemsSource = mapList;
                GrdCompareDefect.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(MouseLeftButtonCompareDefect), true);

                //список дефектов из файла
                Proxy.GetDBDefect_ListFileCompleted += Proxy_GetDBDefect_ListFileCompleted;
                Proxy.GetDBDefect_ListFileAsync(Model.KeyImport);
            }
            else
            {
                List<GetDBDefectMapingList> mapList = new List<GetDBDefectMapingList>();
                GrdCompareDefect.ItemsSource = null;
                GrdCompareDefect.ItemsSource = mapList;
                Model.Report("Ошибка получения сопоставления GetDBDefectMaping - " + e.Result.ErrorMessage);
            }
        }

        /// <summary>
        /// список дефектов из файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Proxy_GetDBDefect_ListFileCompleted(object sender, GetDBDefect_ListFileCompletedEventArgs e)
        {
            Proxy.GetDBDefect_ListFileCompleted -= Proxy_GetDBDefect_ListFileCompleted;
            if (e.Result.IsValid)
            {
                List<GetDBDefectListFile> defectListFile = new List<GetDBDefectListFile>(e.Result.GetDBDefectListFile);
                GrdDefectFile.ItemsSource = null;
                GrdDefectFile.ItemsSource = defectListFile;
                GrdDefectFile.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(MouseLeftButtonDownDefectFile), true);
            }
            else
            {
                List<GetDBDefectListFile> defectListFile = new List<GetDBDefectListFile>();
                GrdDefectFile.ItemsSource = null;
                GrdDefectFile.ItemsSource = defectListFile;
                Model.Report("Ошибка получения дефектов GetDBDefect_ListFile - " + e.Result.ErrorMessage);
            }
        }

        void Proxy_GetDBDefect_ListCompleted(object sender, GetDBDefect_ListCompletedEventArgs e)
        {
            Proxy.GetDBDefect_ListCompleted -= Proxy_GetDBDefect_ListCompleted;
            if (e.Result.IsValid)
            {
                List<GetDBDefectList> defectList = new List<GetDBDefectList>(e.Result.GetDBDefectList);
                grdTypeDefect.ItemsSource = null;
                grdTypeDefect.ItemsSource = defectList;
                grdTypeDefect.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(MouseLeftButtonDownDefect), true);
            }
            else
            {
                List<GetDBDefectList> defectList = new List<GetDBDefectList>();
                grdTypeDefect.ItemsSource = null;
                grdTypeDefect.ItemsSource = defectList;
                Model.Report("Ошибка получения дефектов - " + e.Result.ErrorMessage);
            }
        }

        /// <summary>
        /// кнопка увязать дефекты
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMapingDefect_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                GetDBDefectMapingList mapList2 = new GetDBDefectMapingList
                {
                    nKeyMaping = "-1",
                    nKeyDefect = selectedRow.NKEYDEFECT,
                    cDefectName = selectedRow.CNAMEDEFECT,
                    cGroupDefectName = selectedRow.CNAMEGROUPDEFECT,
                    cFileDefectName = selectedRowFile.CNAMEDEFECT
                };

                ((List<GetDBDefectMapingList>)GrdCompareDefect.ItemsSource).Add(mapList2);
                GrdCompareDefect.Rebind();

                ((List<GetDBDefectListFile>)GrdDefectFile.ItemsSource).Remove(selectedRowFile);
                GrdDefectFile.Rebind();

                selectedRow = null;
                selectedRowFile = null;
                BtnMapingDefect.IsEnabled = GetBuEnable(selectedRow, selectedRowFile);
            }
            catch (Exception err)
            {
                MessageBox.Show(Resources_ImpVtd.Error + err.Message, Resources_ImpVtd.Error, MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// обработка клика мышки на таблицу дефектов из файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseLeftButtonDownDefectFile(object sender, MouseButtonEventArgs e)
        {
            try
            {
                UIElement clicked = e.OriginalSource as UIElement;
                if (clicked != null)
                {
                    var row = clicked.ParentOfType<GridViewRow>();
                    if (row != null)
                    {
                        selectedRowFile = ((GetDBDefectListFile)row.Item);
                    }
                }

                BtnMapingDefect.IsEnabled = GetBuEnable(selectedRow, selectedRowFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources_ImpVtd.Error + ex.Message, Resources_ImpVtd.Error, MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// обработка клика на таблицу дефектов из базы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseLeftButtonDownDefect(object sender, MouseButtonEventArgs e)
        {
            try
            {
                UIElement clicked = e.OriginalSource as UIElement;
                if (clicked != null)
                {
                    var row = clicked.ParentOfType<GridViewRow>();
                    if (row != null)
                    {
                        selectedRow = ((GetDBDefectList)row.Item);
                    }
                }

                BtnMapingDefect.IsEnabled = GetBuEnable(selectedRow, selectedRowFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources_ImpVtd.Error + ex.Message, Resources_ImpVtd.Error, MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// показывать или нет кнопку связать
        /// </summary>
        /// <param name="selectedDb"></param>
        /// <param name="selectedFile"></param>
        /// <returns></returns>
        private bool GetBuEnable(GetDBDefectList selectedDb, GetDBDefectListFile selectedFile)
        {
            return (selectedDb != null) && (selectedFile != null);
        }

        /// <summary>
        /// обработка клика на строку в таблице сопоставления дефектов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseLeftButtonCompareDefect(object sender, MouseButtonEventArgs e)
        {
            try
            {
                UIElement clicked = e.OriginalSource as UIElement;
                if (clicked != null)
                {
                    var row = clicked.ParentOfType<GridViewRow>();
                    if (row != null)
                    {
                        selectedRowMapingList = ((GetDBDefectMapingList)row.Item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources_ImpVtd.Error + ex.Message, Resources_ImpVtd.Error, MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// удаляем сопоставления из таблицы, если новое сопоставление - ключ =-1 то восстанавливаем в таблицах дефектов и дефектов из файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuDeleteMaping_OnClick(object sender, RoutedEventArgs e)
        {
            string keyMapDelete = string.Empty;

            try
            {
                if (GrdCompareDefect.SelectedItems.Count != 0)
                {
                    foreach (GetDBDefectMapingList item in GrdCompareDefect.SelectedItems)
                    {
                        if (item.nKeyMaping == "-1")
                        {
                            _defListFile = new GetDBDefectListFile {CNAMEDEFECT = item.cFileDefectName};

                            ((List<GetDBDefectListFile>)GrdDefectFile.ItemsSource).Add(_defListFile);
                            GrdDefectFile.Rebind();

                            ((List<GetDBDefectMapingList>)GrdCompareDefect.ItemsSource).Remove(item);
                        }
                        else
                        {
                            //сохраняем в элемент листа
                            _defList = new GetDBDefectList
                            {
                                NKEYDEFECT = item.nKeyDefect,
                                CNAMEGROUPDEFECT = item.cGroupDefectName,
                                CNAMEDEFECT = item.cDefectName
                            };

                            _defListFile = new GetDBDefectListFile {CNAMEDEFECT = item.cFileDefectName};

                            _itemMaping = new GetDBDefectMapingList();
                            _itemMaping = item;

                            keyMapDelete = keyMapDelete + item.nKeyMaping + ", ";
                        }
                    }
                }
                if (!String.IsNullOrEmpty(keyMapDelete))
                {
                    //удаляем из базы - если все хорошо то возвращаем данные в таблицы дефектов и дефектов из файла
                    Proxy.DeleteMapingDictCompleted += Proxy_DeleteMapingDictCompleted;
                    Proxy.DeleteMapingDictAsync(keyMapDelete);
                }
                else
                {
                    GrdCompareDefect.Rebind();    
                }
                
            }
            catch (Exception ex)
            {
                Model.Report("Ошибка удаления коллекции " + ex.Message);
            }
        }

        void Proxy_DeleteMapingDictCompleted(object sender, DeleteMapingDictCompletedEventArgs e)
        {
            try
            {
                e.Result.IsValid = false;
                if (e.Result.IsValid)
                {
                    ((List<GetDBDefectListFile>)GrdDefectFile.ItemsSource).Add(_defListFile);
                    GrdDefectFile.Rebind();

                    ((List<GetDBDefectMapingList>)GrdCompareDefect.ItemsSource).Remove(_itemMaping);
                    GrdCompareDefect.Rebind();

                    GrdCompareDefect.Rebind();  
                }
                else
                {
                    MessageBox.Show(Resources_ImpVtd.MessageBoxDictDefect1 + e.Result.ErrorMessage, Resources_ImpVtd.Error, MessageBoxButton.OK);
                    Model.Report("Ошибка получения параметров ProxyFillVtdSection +" + e.Result.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources_ImpVtd.MessageBoxDictDefect1 + ex.Message, Resources_ImpVtd.Error, MessageBoxButton.OK);
            }
        }

        private void BuNext_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                List<GetDBDefectMapingList> getDbDefectMapingList = (from GetDBDefectMapingList item in GrdCompareDefect.Items
                                                                     where item.nKeyMaping == "-1"
                                                                     select new GetDBDefectMapingList()
                                                                     {
                                                                         nKeyMaping = "-1", 
                                                                         nKeyDefect = item.nKeyDefect, 
                                                                         cDefectName = item.cDefectName, 
                                                                         cGroupDefectName = item.cGroupDefectName, 
                                                                         cFileDefectName = item.cFileDefectName,
                                                                     }).ToList();

                if (GrdDefectFile.Items.Count != 0)
                {
                    getDbDefectMapingList.AddRange(from GetDBDefectListFile itemDf in GrdDefectFile.Items
                                                   select new GetDBDefectMapingList()
                                                   {
                                                       nKeyMaping = "-1", 
                                                       nKeyDefect = "-1", 
                                                       cDefectName = "", 
                                                       cGroupDefectName = "", 
                                                       cFileDefectName = itemDf.CNAMEDEFECT,
                                                   });
                }
               
                Proxy.SaveMapingDictCompleted += Proxy_SaveMapingDictCompleted;
                Proxy.SaveMapingDictAsync(getDbDefectMapingList.ToArray(), Model.KeyImport);  
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources_ImpVtd.MessageBoxDictDefect2 + ex.Message, Resources_ImpVtd.Error, MessageBoxButton.OK);
            }
        }

        void Proxy_SaveMapingDictCompleted(object sender, SaveMapingDictCompletedEventArgs e)
        {
            if (e.Result.IsValid)
            {
                Model.FirePropertyChanged("CreateNewImport");
            }
            else
            {
                MessageBox.Show(Resources_ImpVtd.MessageBoxDictDefect2 + e.Result.ErrorMessage, Resources_ImpVtd.Error, MessageBoxButton.OK);
            }
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Model.FirePropertyChanged("BackNewImport");
        }
    }
}