﻿#pragma checksum "..\..\..\..\..\Modules\Presence\View\PresenceCtrlView.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "CD51B4FEE1D10BF5562AB9ECAA26D24573BED2F78A2C33064FA57D5202E6B0EF"
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


namespace FingerPrintManagerApp.Modules.Presence.View {
    
    
    /// <summary>
    /// PresenceCtrlView
    /// </summary>
    public partial class PresenceCtrlView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 72 "..\..\..\..\..\Modules\Presence\View\PresenceCtrlView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border brdMenu;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\..\..\Modules\Presence\View\PresenceCtrlView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnMenu;
        
        #line default
        #line hidden
        
        
        #line 112 "..\..\..\..\..\Modules\Presence\View\PresenceCtrlView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnPresence;
        
        #line default
        #line hidden
        
        
        #line 125 "..\..\..\..\..\Modules\Presence\View\PresenceCtrlView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnInfo;
        
        #line default
        #line hidden
        
        
        #line 133 "..\..\..\..\..\Modules\Presence\View\PresenceCtrlView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.Popup popupInfo;
        
        #line default
        #line hidden
        
        
        #line 174 "..\..\..\..\..\Modules\Presence\View\PresenceCtrlView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer scrollInfo;
        
        #line default
        #line hidden
        
        
        #line 195 "..\..\..\..\..\Modules\Presence\View\PresenceCtrlView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer scrollMenu;
        
        #line default
        #line hidden
        
        
        #line 199 "..\..\..\..\..\Modules\Presence\View\PresenceCtrlView.xaml"
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
            System.Uri resourceLocater = new System.Uri("/FingerPrintManagerApp;component/modules/presence/view/presencectrlview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Modules\Presence\View\PresenceCtrlView.xaml"
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
            
            #line 76 "..\..\..\..\..\Modules\Presence\View\PresenceCtrlView.xaml"
            this.btnMenu.Click += new System.Windows.RoutedEventHandler(this.btnMenu_Click_1);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnPresence = ((System.Windows.Controls.Button)(target));
            
            #line 113 "..\..\..\..\..\Modules\Presence\View\PresenceCtrlView.xaml"
            this.btnPresence.Click += new System.Windows.RoutedEventHandler(this.BtnPresence_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnInfo = ((System.Windows.Controls.Button)(target));
            
            #line 126 "..\..\..\..\..\Modules\Presence\View\PresenceCtrlView.xaml"
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
            
            #line 196 "..\..\..\..\..\Modules\Presence\View\PresenceCtrlView.xaml"
            this.scrollMenu.SizeChanged += new System.Windows.SizeChangedEventHandler(this.scrollMenu_SizeChanged_1);
            
            #line default
            #line hidden
            return;
            case 8:
            this.stpOption = ((System.Windows.Controls.Grid)(target));
            
            #line 201 "..\..\..\..\..\Modules\Presence\View\PresenceCtrlView.xaml"
            this.stpOption.MouseEnter += new System.Windows.Input.MouseEventHandler(this.stpOption_MouseEnter);
            
            #line default
            #line hidden
            
            #line 202 "..\..\..\..\..\Modules\Presence\View\PresenceCtrlView.xaml"
            this.stpOption.MouseLeave += new System.Windows.Input.MouseEventHandler(this.stpOption_MouseLeave);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

