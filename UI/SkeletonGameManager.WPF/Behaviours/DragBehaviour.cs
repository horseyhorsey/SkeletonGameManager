﻿using SkeletonGameManager.WPF.Controls;
using SkeletonGameManager.WPF.ViewModels.Machine;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace SkeletonGameManager.WPF.Behaviours
{
    public class DragBehavior : Behavior<FrameworkElement>
    {
        private Point elementStartPosition;
        private Point mouseStartPosition;
        private TranslateTransform transform = new TranslateTransform();

        protected override void OnAttached()
        {
            Window parent = Application.Current.MainWindow;
            AssociatedObject.RenderTransform = transform;

            AssociatedObject.PreviewMouseLeftButtonDown += (sender, e) =>
            {                
                elementStartPosition = AssociatedObject.TranslatePoint(new Point(), parent);
                mouseStartPosition = e.GetPosition(parent);
                AssociatedObject.CaptureMouse();
            };

            AssociatedObject.PreviewMouseLeftButtonUp += (sender, e) =>
            {
                AssociatedObject.ReleaseMouseCapture();                                
            };

            AssociatedObject.MouseMove += (sender, e) =>
            {
                Vector diff = e.GetPosition(parent) - mouseStartPosition;                               

                if (AssociatedObject.IsMouseCaptured)
                {

                    transform.X = diff.X;
                    transform.Y = diff.Y;

                    var objParent  = VisualTreeHelper.GetParent(AssociatedObject);

                    var btn = (Button)AssociatedObject as Button;

                    if (btn != null)
                    {
                        var ctx = btn.DataContext as SwitchViewModel;
                        ctx.PosTop = (int)diff.Y;
                        ctx.PosLeft = (int)diff.X;
                    }
                }
            };
        }
    }

}
