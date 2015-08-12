using System.Windows;
using System.Windows.Controls.Primitives;

namespace ZappChat_v3.Controls
{
    public class StatusButton : ToggleButton
    {
        static StatusButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StatusButton), new FrameworkPropertyMetadata(typeof(StatusButton)));
        }
    }
}
