namespace DragWindow
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interop;

    public static class Behaviors
    {
        private const int WM_NCLBUTTONDOWN = 0x00A1;
        private const int HT_CAPTION = 2;

        /// <devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/ms644953(v=vs.85).aspx</devdoc>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SendNotifyMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        #region IsDragArea

        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static bool GetIsDragArea(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDragAreaProperty);
        }

        public static void SetIsDragArea(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDragAreaProperty, value);
        }
        
        public static readonly DependencyProperty IsDragAreaProperty =
            DependencyProperty.RegisterAttached("IsDragArea", typeof(bool), typeof(Behaviors),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits, 
                    new PropertyChangedCallback((d, e) => 
                    {
                        FrameworkElement element = (FrameworkElement)d;

                        if (e.NewValue is bool == false)
                            return;

                        if ((bool)e.NewValue)
                            element.MouseDown += OnDragAreaMouseDown;
                        else
                            element.MouseDown -= OnDragAreaMouseDown;
                    })));


        private static void OnDragAreaMouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            
            HwndSource hWndSource = (HwndSource)HwndSource.FromVisual(element);
            IntPtr hWnd = hWndSource.Handle;

            SendNotifyMessage(hWnd, WM_NCLBUTTONDOWN, (IntPtr)HT_CAPTION, IntPtr.Zero);

            e.Handled = true;
        }
        
        #endregion IsDragArea


    }
}
