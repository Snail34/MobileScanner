﻿

#pragma checksum "D:\Projects\Scanner\Windows\Scanner.Windows\Views\PhotoView\CropControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5863AD0CF7D6DD49C2CC18E136D912AC"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Scanner.Windows.Views.PhotoView
{
    partial class CropControl : global::Windows.UI.Xaml.Controls.UserControl, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 34 "..\..\..\..\Views\PhotoView\CropControl.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).SizeChanged += this.Viewbox_SizeChanged;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 41 "..\..\..\..\Views\PhotoView\CropControl.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).ManipulationDelta += this.Control_ManipulationDelta;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 48 "..\..\..\..\Views\PhotoView\CropControl.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).ManipulationDelta += this.Control_ManipulationDelta;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 55 "..\..\..\..\Views\PhotoView\CropControl.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).ManipulationDelta += this.Control_ManipulationDelta;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 62 "..\..\..\..\Views\PhotoView\CropControl.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).ManipulationDelta += this.Control_ManipulationDelta;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


