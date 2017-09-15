using DancingOctopus.Domain;
using System;
using System.Globalization;
using System.Windows.Data;

namespace DancingOctopus.Desktop.Conveters
{
    public class DeploymentStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = ((DeploymentStatus)value);
            switch (status)
            {
                case DeploymentStatus.NotStarted: return "#333";
                case DeploymentStatus.InProgress: return "#705F27";
                case DeploymentStatus.Successful: return "#70A427";
                case DeploymentStatus.Failed: return "#702627";
                default: return "333";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
