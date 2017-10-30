using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ShiYing
{
    /// <summary>
    /// TimeSpaneraction logic for UserControl1.xaml
    /// </summary>
    public partial class CountdownControl : UserControl
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromSeconds(1)
        };

        public CountdownControl()
        {
            InitializeComponent();

            _timer.Tick += _timer_Tick;
        }

        #region DependencyProperty Current

        private static readonly DependencyPropertyKey CurrentPropertyKey = DependencyProperty.RegisterReadOnly(
            "Current", typeof(TimeSpan),
            typeof(CountdownControl),
            new FrameworkPropertyMetadata(defaultValue: 0));
        public static readonly DependencyProperty CurrentProperty = CurrentPropertyKey.DependencyProperty;
        public TimeSpan Current { get => (TimeSpan)GetValue(CurrentProperty); }

        #endregion

        #region DependencyProperty IsRunning
        public static readonly DependencyProperty IsRunningProperty = DependencyProperty.Register(
            "IsRunning",
            typeof(bool),
            typeof(CountdownControl),
            new FrameworkPropertyMetadata(defaultValue: false, propertyChangedCallback: OnIsRunningChanged));

        private static void OnIsRunningChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (CountdownControl)d;
            var oldIsRunning = (bool)e.OldValue;
            var newIsRunning = target.IsRunning;
            target.OnIsRunningChanged(oldIsRunning, newIsRunning);
        }

        protected virtual void OnIsRunningChanged(bool oldIsRunning, bool newIsRunning)
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                if (newIsRunning)
                    _timer.Start();
                else
                    _timer.Stop();
            }
        }

        #endregion

        public bool IsRunning
        {
            get { return (bool)GetValue(IsRunningProperty); }
            set { SetValue(IsRunningProperty, value); }
        }

        #region DependencyProperty From

        public static readonly DependencyProperty FromProperty = DependencyProperty.Register(
            "From",
            typeof(TimeSpan),
            typeof(CountdownControl),
            new FrameworkPropertyMetadata(defaultValue: 0, propertyChangedCallback: OnFromChanged));

        public TimeSpan From
        {
            get { return (TimeSpan)GetValue(FromProperty); }
            set { SetValue(FromProperty, value); }
        }

        private static void OnFromChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (CountdownControl)d;
            var oldFrom = (TimeSpan)e.OldValue;
            var newFrom = target.From;
            target.OnFromChanged(oldFrom, newFrom);
        }

        protected virtual void OnFromChanged(TimeSpan oldFrom, TimeSpan newFrom)
        {
            SetValue(CurrentPropertyKey, newFrom);
            IsRunning = (newFrom > TimeSpan.Zero);
        }

        #endregion

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (Current > TimeSpan.Zero)
            {
                SetValue(CurrentPropertyKey, Current - TimeSpan.FromSeconds(1));
                if (Current == TimeSpan.Zero)
                {
                    IsRunning = false;
                }
            }
        }
    }
}
