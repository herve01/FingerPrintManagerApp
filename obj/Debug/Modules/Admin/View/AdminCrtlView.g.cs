﻿#pragma checksum "..\..\..\..\..\Modules\Admin\View\AdminCrtlView.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "80C8D6F21F5B88936E42AEEB92A95C2A36E04C694E19B9EE01E3C27AFCC92D57"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using FingerPrintManagerApp.ViewModel.Behavior;
using RootLibrary.WPF.Localization;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace FingerPrintManagerApp.Modules.Admin.View {
    
    
    /// <summary>
    /// AdminCrtlView
    /// </summary>
    public partial class AdminCrtlView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 73 "..\..\..\..\..\Modules\Admin\View\AdminCrtlView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border brdMenu;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\..\..\..\Modules\Admin\View\AdminCrtlView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnMenu;
        
        #line default
        #line hidden
        
        
        #line 108 "..\..\..\..\..\Modules\Admin\View\AdminCrtlView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid gridMenu;
        
        #line default
        #line hidden
        
        
        #line 118 "..\..\..\..\..\Modules\Admin\View\AdminCrtlView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnInfo;
        
        #line default
        #line hidden
        
        
        #line 126 "..\..\..\..\..\Modules\Admin\View\AdminCrtlView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.Popup popupInfo;
        
        #line default
        #line hidden
        
        
        #line 167 "..\..\..\..\..\Modules\Admin\View\AdminCrtlView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer scrollInfo;
        
        #line default
        #line hidden
        
        
        #line 188 "..\..\..\..\..\Modules\Admin\View\AdminCrtlView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer scrollMenu;
        
        #line default
        #line hidden
        
        
        #line 192 "..\..\..\..\..\Modules\Admin\View\AdminCrtlView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid stpOption;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/FingerPrintManagerApp;component/modules/admin/view/admincrtlview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Modules\Admin\View\AdminCrtlView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.brdMenu = ((System.Windows.Controls.Border)(target));
            return;
            case 2:
            this.btnMenu = ((System.Windows.Controls.Button)(target));
            
            #line 77 "..\..\..\..\..\Modules\Admin\View\AdminCrtlView.xaml"
            this.btnMenu.Click += new System.Windows.RoutedEventHandler(this.btnMenu_Click_1);
            
            #line default
            #line hidden
            return;
            case 3:
            this.gridMenu = ((System.Windows.Controls.Grid)(target));
            return;
            case 4:
            this.btnInfo = ((System.Windows.Controls.Button)(target));
            
            #line 119 "..\..\..\..\..\Modules\Admin\View\AdminCrtlView.xaml"
            this.btnInfo.Click += new System.Windows.RoutedEventHandler(this.btnInfo_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.popupInfo = ((System.Windows.Controls.Primitives.Popup)(target));
            return;
            case 6:
            this.scrollInfo = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 7:
            this.scrollMenu = ((System.Windows.Controls.ScrollViewer)(target));
            
            #line 189 "..\..\..\..\..\Modules\Admin\View\AdminCrtlView.xaml"
            this.scrollMenu.SizeChanged += new System.Windows.SizeChangedEventHandler(this.scrollMenu_SizeChanged_1);
            
            #line default
            #line hidden
            return;
            case 8:
            this.stpOption = ((System.Windows.Controls.Grid)(target));
            
            #line 194 "..\..\..\..\..\Modules\Admin\View\AdminCrtlView.xaml"
            this.stpOption.MouseEnter += new System.Windows.Input.MouseEventHandler(this.stpOption_MouseEnter);
            
            #line default
            #line hidden
            
            #line 195 "..\..\..\..\..\Modules\Admin\View\AdminCrtlView.xaml"
            this.stpOption.MouseLeave += new System.Windows.Input.MouseEventHandler(this.stpOption_MouseLeave);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

