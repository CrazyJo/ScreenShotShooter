using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using WpfApplication1.Model;
using static WpfApplication1.Model.Verifier;

namespace WpfApplication1
{
    class ViewModel
    {
        private MainWindow _mainWindow;
        private KitchenTimer _kitchenTimer;
        private bool _onPause;
        private bool _controlsAreEnable = true;
        Stopwatch st = new Stopwatch();
        private IEnumerable<FrameworkElement> _controls;

        public ViewModel(MainWindow main)
        {
            //st.Start();
            InitializeAfterDeserialization();

            _mainWindow = main;

            InitializeMainWindowComponents();

            _controls = GetAllControls();
            //st.Stop();
        }

        private void InitializeMainWindowComponents()
        {
            _mainWindow.ButtonStartPause.Click += ButtonStartPause_Click;
            _mainWindow.ButtonStop.Click += ButtonStop_Click;
            _mainWindow.Closing += _mainWindow_Closing;

            InitializeButtonSetTime();
        }
        
        #region Button5Min - Button10Min
        private void InitializeButtonSetTime()
        {
            _mainWindow.Button5Min.Click += Button5Min_Click;
            _mainWindow.Button7Min.Click += Button7Min_Click;
            _mainWindow.Button10Min.Click += Button10Min_Click;
        }
        private void Button10Min_Click(object sender, RoutedEventArgs e)
        {
            SetTBoxValue("0", "10", "0");
        }

        private void Button5Min_Click(object sender, RoutedEventArgs e)
        {
            SetTBoxValue("0", "5", "0");
        }

        private void Button7Min_Click(object sender, RoutedEventArgs e)
        {
            SetTBoxValue("0", "7", "0");
        }

        private void SetTBoxValue(string h, string m, string s)
        {
            _mainWindow.Hours.Text = h;
            _mainWindow.Minutes.Text = m;
            _mainWindow.Seconds.Text = s;
        }
        #endregion

        private void InitializeTextBoxesNumericCheck()
        {
            _mainWindow.Hours.PreviewTextInput += Value_PreviewTextInput;
            _mainWindow.Minutes.PreviewTextInput += Value_PreviewTextInput;
            _mainWindow.Seconds.PreviewTextInput += Value_PreviewTextInput;
        }

        /// <summary>
        /// TextBox to only accept numeric input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Value_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            _kitchenTimer.Stop();
            _mainWindow.ButtonStartPause.Content = "Start";

            if (!_controlsAreEnable)
            {
                _controlsAreEnable = _controls.SwitchControls(true);
            }
        }

        private void ButtonStartPause_Click(object sender, RoutedEventArgs e)
        {
            if (!_kitchenTimer.IsEnable)
            {
                if (!_onPause)
                {
                    _kitchenTimer.SetTimeIntervalFrom(_mainWindow.Hours.Text, _mainWindow.Minutes.Text, _mainWindow.Seconds.Text);
                }

                _kitchenTimer.Start();
                _mainWindow.ButtonStartPause.Content = "Pause";
                _onPause = false;

                if (_controlsAreEnable)
                {
                    //st.Start();
                    _controlsAreEnable = _controls.SwitchControls(false);
                    //st.Stop();
                }
            }
            else
            {
                _kitchenTimer.Pause();
                _mainWindow.ButtonStartPause.Content = "Start";
                _onPause = true;
            }
        }


        private IEnumerable<FrameworkElement> GetAllControls()
        {
            var getAllControlsQuery = from control in _mainWindow.FindVisualChildren<FrameworkElement>()
                                          // Тип элементов, которые надо отключить
                                      where (control is TextBox || control is Button || control is TextBlock || control is ScrollBar) &&
                                            // Имена элементов, которые надо добавить в исключение
                                            control.Name != "ButtonStartPause" && control.Name != "ButtonStop" &&
                                            control.Name != "TbCountDown"
                                      select control;

            return getAllControlsQuery;
        }


        #region Event handlers
        private void _kitchenTimer_TimeTickEvent()
        {
            _mainWindow.TbCountDown.Dispatcher.BeginInvoke(new Action(
                () => _mainWindow.TbCountDown.Text = _kitchenTimer.TimeInterval.ToString("c")));
        }

        private void _kitchenTimer_TimesOutEvent()
        {
            _mainWindow.ButtonStartPause.Dispatcher.BeginInvoke(new Action(
                () => _mainWindow.ButtonStartPause.Content = "Start"));
        }
        #endregion

        #region Serialize / Deserialize
        private void _mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Serialize();
        }

        private async void InitializeAfterDeserialization()
        {
            if (!await DeserializeAsync())
            {
                _kitchenTimer = new KitchenTimer();
            }
            else
            {
                InitializeTextBoxesFromTimeInterval();
            }

            _kitchenTimer.TimeTickEvent += _kitchenTimer_TimeTickEvent;
            _kitchenTimer.TimesOutEvent += _kitchenTimer_TimesOutEvent;
            InitializeTextBoxesNumericCheck();
        }

        private void InitializeTextBoxesFromTimeInterval()
        {
            _mainWindow.Hours.Text = _kitchenTimer.TimeInterval.Hours.ToString();
            _mainWindow.Minutes.Text = _kitchenTimer.TimeInterval.Minutes.ToString();
            _mainWindow.Seconds.Text = _kitchenTimer.TimeInterval.Seconds.ToString();
        }

        private Task<bool> DeserializeAsync()
        {
            return Task.Run(() =>
            {
                if (File.Exists(@"_kitchenTimer.bin"))
                {
                    IFormatter formatter = new BinaryFormatter();

                    using (FileStream fs = new FileStream(@"_kitchenTimer.bin", FileMode.Open))
                    {
                        _kitchenTimer = (KitchenTimer)formatter.Deserialize(fs);
                    }
                    return true;
                }
                return false;
            });
        }

        private void Serialize()
        {
            IFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream(@"_kitchenTimer.bin", FileMode.Create))
            {
                formatter.Serialize(fs, _kitchenTimer);
            }
        }
        #endregion
    }
}
