﻿#pragma checksum "C:\My_Work\DotNet\Import_vtd\src_files\importVtd\Controls\DrawPipe2D\View\Control\PipeControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "202AB343E2ED9590373F3C8E74E0A30A"
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


namespace DrawPipe2D.View.Control {
    
    
    public partial class PipeControl : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Telerik.Windows.Controls.Label lblFirstPipeKm;
        
        internal System.Windows.Controls.Canvas canvas;
        
        internal System.Windows.Controls.Canvas drawCanvas;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/importVtd;component/Controls/DrawPipe2D/View/Control/PipeControl.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.lblFirstPipeKm = ((Telerik.Windows.Controls.Label)(this.FindName("lblFirstPipeKm")));
            this.canvas = ((System.Windows.Controls.Canvas)(this.FindName("canvas")));
            this.drawCanvas = ((System.Windows.Controls.Canvas)(this.FindName("drawCanvas")));
        }
    }
}

