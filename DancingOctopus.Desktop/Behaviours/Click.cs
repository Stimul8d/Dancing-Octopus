using DancingOctopus.ViewModels;
using System.Windows;

namespace DancingOctopus.Desktop.Behaviours
{
    public class Click : DependencyObject
    {
        public static BindingCommand GetCommand(DependencyObject obj)
        {
            return (BindingCommand)obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, BindingCommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(BindingCommand), typeof(Click),
                new PropertyMetadata(Update));

        private static void Update(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = ((UIElement)d);
            element.PreviewMouseLeftButtonDown += (o, s) =>
            {
                ((BindingCommand)e.NewValue).Execute(null);
            };

        }
    }
}
