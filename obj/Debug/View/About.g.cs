﻿#pragma checksum "..\..\..\View\About.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "2B0B97ABCAD4EFFBEF953ED3E4366F89D337194BF8D274848305095E7B826C25"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace FingerPrintManagerApp.View {
    
    
    /// <summary>
    /// About
    /// </summary>
    public partial class About : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\View\About.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal FingerPrintManagerApp.View.About about;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\..\View\About.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.ScaleTransform scaleMe;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\View\About.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border brdShadow;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\View\About.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border borderSplash;
        
        #line default
        #line hidden
        
        
        #line 89 "..\..\..\View\About.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtEdition;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\..\View\About.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtVersion;
        
        #line default
        #line hidden
        
        
        #line 92 "..\..\..\View\About.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btClose;
        
        #line default
        #line hidden
        
        
        #line 129 "..\..\..\View\About.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel stpLicence;
        
        #line default
        #line hidden
        
        
        #line 133 "..\..\..\View\About.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtFinEvaluation;
        
        #line default
        #line hidden
        
        
        #line 177 "..\..\..\View\About.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button txtLink;
        
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
            System.Uri resourceLocater = new System.Uri("/FingerPrintManagerApp;component/view/about.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\View\About.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
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
            this.about = ((FingerPrintManagerApp.View.About)(target));
            
            #line 11 "..\..\..\View\About.xaml"
            this.about.Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded_1);
            
            #line default
            #line hidden
            return;
            case 2:
            this.scaleMe = ((System.Windows.Media.ScaleTransform)(target));
            return;
            case 3:
            this.brdShadow = ((System.Windows.Controls.Border)(target));
            return;
            case 4:
            this.borderSplash = ((System.Windows.Controls.Border)(target));
            return;
            case 5:
            
            #line 79 "..\..\..\View\About.xaml"
            ((System.Windows.Controls.Grid)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Grid_MouseDown_1);
            
            #line default
            #line hidden
            return;
            case 6:
            this.txtEdition = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.txtVersion = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.btClose = ((System.Windows.Controls.Button)(target));
            
            #line 92 "..\..\..\View\About.xaml"
            this.btClose.Click += new System.Windows.RoutedEventHandler(this.btClose_Click_1);
            
            #line default
            #line hidden
            return;
            case 9:
            this.stpLicence = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 10:
            this.txtFinEvaluation = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 11:
            this.txtLink = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

