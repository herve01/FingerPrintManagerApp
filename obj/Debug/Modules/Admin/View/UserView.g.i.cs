// Updated by XamlIntelliSenseFileGenerator 18/04/2024 11:32:18
#pragma checksum "..\..\..\..\..\Modules\Admin\View\UserView.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "1ABDE19A46B9C1C130493F6988086A428485BC4D1BA051C813DE237ED7545C7D"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using ARG.Controls;
using FingerPrintManagerApp.View;
using FingerPrintManagerApp.View.Converter;
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
using WpfAnimatedGif;


namespace FingerPrintManagerApp.Modules.Admin.View
{


    /// <summary>
    /// UserView
    /// </summary>
    public partial class UserView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {

#line default
#line hidden


#line 210 "..\..\..\..\..\Modules\Admin\View\UserView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border sep;

#line default
#line hidden


#line 437 "..\..\..\..\..\Modules\Admin\View\UserView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtPwd;

#line default
#line hidden


#line 444 "..\..\..\..\..\Modules\Admin\View\UserView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox pbPwd;

#line default
#line hidden


#line 466 "..\..\..\..\..\Modules\Admin\View\UserView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtConfPwd;

#line default
#line hidden


#line 473 "..\..\..\..\..\Modules\Admin\View\UserView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox pbConfPwd;

#line default
#line hidden


#line 636 "..\..\..\..\..\Modules\Admin\View\UserView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border brd;

#line default
#line hidden


#line 701 "..\..\..\..\..\Modules\Admin\View\UserView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView list;

#line default
#line hidden


#line 765 "..\..\..\..\..\Modules\Admin\View\UserView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border brdStatus;

#line default
#line hidden


#line 776 "..\..\..\..\..\Modules\Admin\View\UserView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtStatus;

#line default
#line hidden

        private bool _contentLoaded;

        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/FingerPrintManagerApp;component/modules/admin/view/userview.xaml", System.UriKind.Relative);

#line 1 "..\..\..\..\..\Modules\Admin\View\UserView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);

#line default
#line hidden
        }

        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler)
        {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }

        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.@this = ((FingerPrintManagerApp.Modules.Admin.View.UserView)(target));

#line 13 "..\..\..\..\..\Modules\Admin\View\UserView.xaml"
                    this.@this.Loaded += new System.Windows.RoutedEventHandler(this.This_Loaded);

#line default
#line hidden
                    return;
                case 2:
                    this.sep = ((System.Windows.Controls.Border)(target));
                    return;
                case 3:
                    this.txtPwd = ((System.Windows.Controls.TextBox)(target));

#line 438 "..\..\..\..\..\Modules\Admin\View\UserView.xaml"
                    this.txtPwd.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TxtPwd_TextChanged);

#line default
#line hidden
                    return;
                case 4:
                    this.pbPwd = ((System.Windows.Controls.PasswordBox)(target));

#line 445 "..\..\..\..\..\Modules\Admin\View\UserView.xaml"
                    this.pbPwd.PasswordChanged += new System.Windows.RoutedEventHandler(this.Password_Changed);

#line default
#line hidden
                    return;
                case 5:
                    this.txtConfPwd = ((System.Windows.Controls.TextBox)(target));

#line 467 "..\..\..\..\..\Modules\Admin\View\UserView.xaml"
                    this.txtConfPwd.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TxtConfPwd_TextChanged);

#line default
#line hidden
                    return;
                case 6:
                    this.pbConfPwd = ((System.Windows.Controls.PasswordBox)(target));

#line 475 "..\..\..\..\..\Modules\Admin\View\UserView.xaml"
                    this.pbConfPwd.PasswordChanged += new System.Windows.RoutedEventHandler(this.ConfirmedPassword_Changed);

#line default
#line hidden
                    return;
                case 7:
                    this.brd = ((System.Windows.Controls.Border)(target));
                    return;
                case 8:
                    this.list = ((System.Windows.Controls.ListView)(target));
                    return;
                case 9:
                    this.brdStatus = ((System.Windows.Controls.Border)(target));
                    return;
                case 10:
                    this.txtStatus = ((System.Windows.Controls.TextBlock)(target));
                    return;
            }
            this._contentLoaded = true;
        }

        internal System.Windows.Controls.UserControl @this;
    }
}

