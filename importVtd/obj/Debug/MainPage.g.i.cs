﻿#pragma checksum "C:\My_Work\DotNet\Import_vtd\src_files\importVtd\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "FC29018A5414615518E2B2FEBC2267F6"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.34209
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;
using Telerik.Windows.Controls;


namespace importVtd {
    
    
    public partial class MainPage : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid GridAll;
        
        internal Telerik.Windows.Controls.RadTabControl tabControlBase;
        
        internal Telerik.Windows.Controls.RadTabItem tabItemStateProcess;
        
        internal System.Windows.Controls.Border stackPanelStateProcess;
        
        internal Telerik.Windows.Controls.RadTabItem tabItemNewImport;
        
        internal System.Windows.Controls.Border stackPanelNewImport;
        
        internal Telerik.Windows.Controls.RadTabItem tabItemDictDefect;
        
        internal System.Windows.Controls.Border stackPanelDictDefect;
        
        internal Telerik.Windows.Controls.RadTabItem tabItemLinkReper;
        
        internal System.Windows.Controls.Border stackPanelLinkReper;
        
        internal Telerik.Windows.Controls.RadTabItem tabCompareTube;
        
        internal System.Windows.Controls.Border stackPanelCompareTube;
        
        internal Telerik.Windows.Controls.RadTabItem tabImportData;
        
        internal System.Windows.Controls.Border borderImportData;
        
        internal System.Windows.Controls.ListBox reportListBox;
        
        internal System.Windows.Controls.BusyIndicator busyIndicatorAll;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/importVtd;component/MainPage.xaml", System.UriKind.Relative));
            this.GridAll = ((System.Windows.Controls.Grid)(this.FindName("GridAll")));
            this.tabControlBase = ((Telerik.Windows.Controls.RadTabControl)(this.FindName("tabControlBase")));
            this.tabItemStateProcess = ((Telerik.Windows.Controls.RadTabItem)(this.FindName("tabItemStateProcess")));
            this.stackPanelStateProcess = ((System.Windows.Controls.Border)(this.FindName("stackPanelStateProcess")));
            this.tabItemNewImport = ((Telerik.Windows.Controls.RadTabItem)(this.FindName("tabItemNewImport")));
            this.stackPanelNewImport = ((System.Windows.Controls.Border)(this.FindName("stackPanelNewImport")));
            this.tabItemDictDefect = ((Telerik.Windows.Controls.RadTabItem)(this.FindName("tabItemDictDefect")));
            this.stackPanelDictDefect = ((System.Windows.Controls.Border)(this.FindName("stackPanelDictDefect")));
            this.tabItemLinkReper = ((Telerik.Windows.Controls.RadTabItem)(this.FindName("tabItemLinkReper")));
            this.stackPanelLinkReper = ((System.Windows.Controls.Border)(this.FindName("stackPanelLinkReper")));
            this.tabCompareTube = ((Telerik.Windows.Controls.RadTabItem)(this.FindName("tabCompareTube")));
            this.stackPanelCompareTube = ((System.Windows.Controls.Border)(this.FindName("stackPanelCompareTube")));
            this.tabImportData = ((Telerik.Windows.Controls.RadTabItem)(this.FindName("tabImportData")));
            this.borderImportData = ((System.Windows.Controls.Border)(this.FindName("borderImportData")));
            this.reportListBox = ((System.Windows.Controls.ListBox)(this.FindName("reportListBox")));
            this.busyIndicatorAll = ((System.Windows.Controls.BusyIndicator)(this.FindName("busyIndicatorAll")));
        }
    }
}
