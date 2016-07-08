using System;
using System.IO;
using System.Media;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace WpfApplication1.Model
{
    [Serializable]
    class KitchenTimer : ISerializable
    {
        public bool IsEnable
        {
            get { return _dispatcherTimer.Enabled; }
            set { _dispatcherTimer.Enabled = value; }
        }

        public TimeSpan TimeInterval
        {
            get { return _tSpan; }
            set
            {
                _tSpan = value;
                OnTimeTickEvent();
            }
        }

        public event Action TimeTickEvent;
        public event Action TimesOutEvent;
        private TimeSpan _tSpan;
        private TimeSpan _lastSetTimeSpan;
        private Timer _dispatcherTimer;
        private readonly SoundPlayer _player;
        private bool _playerEnable;
        private readonly TimeSpan _interval = new TimeSpan(0, 0, 1);

        public KitchenTimer()
        {
            InitializeTimer();

            _player = new SoundPlayer(@"zvuki-zvonok_budilnika.wav");
        }

        protected KitchenTimer(SerializationInfo info, StreamingContext context) : this()
        {
            Type t = typeof(TimeSpan);
            _tSpan = _lastSetTimeSpan = (TimeSpan)info.GetValue("_lastSetTimeSpan", t);
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //info.AddValue("_tSpan", _tSpan);
            info.AddValue("_lastSetTimeSpan", _lastSetTimeSpan);
        }

        private void InitializeTimer()
        {
            _dispatcherTimer = new Timer(_interval.TotalMilliseconds);
            _dispatcherTimer.Elapsed += _dispatcherTimer_Tick;
        }

        private void _dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (!_tSpan.TotalSeconds.Equals(1.0))
            {
                TimeInterval = TimeInterval.Subtract(_interval);
            }
            else
            {
                TimeInterval = TimeInterval.Subtract(_interval);
                _dispatcherTimer.Stop();
                PlayerPlay();
                OnTimesOutEvent();
            }
        }

        public void Start()
        {
            _dispatcherTimer.Start();

            if (_playerEnable)
            {
                PlayerStop();
            }
        }

        public void Pause()
        {
            _dispatcherTimer.Stop();
        }

        /// <summary>
        /// Останавливает таймер и звуковой сигнал, а также сбрасывает значение интервала вреемени в установленное сперва методом SetTimeIntervalFrom().
        /// </summary>
        public void Stop()
        {
            if (IsEnable || _playerEnable) // Зашита от повторного нажатия на кнопку
            {
                _dispatcherTimer.Stop();
                PlayerStop();
                TimeInterval = _lastSetTimeSpan;
            }
        }

        private void PlayerPlay()
        {
            try
            {
                _player.LoadAsync();
                _player.PlayLooping();
            }
            catch (FileNotFoundException e)
            {
                ExceptionHandler(e);
            }
            catch (InvalidOperationException e)
            {
                ExceptionHandler(e);
            }
            _playerEnable = true;
        }

        private void PlayerStop()
        {
            _player.Stop();
            _playerEnable = false;
        }

        private void ExceptionHandler(Exception e)
        {
            MessageBox.Show(e.Message, e.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public async void SetTimeIntervalFrom(string h, string m, string s)
        {
            //if (string.IsNullOrEmpty(h) || string.IsNullOrEmpty(m) || string.IsNullOrEmpty(s))
            //    return;
            //_lastSetTimeSpan = TimeInterval = new TimeSpan(int.Parse(h), int.Parse(m), int.Parse(s));

            int hh, mm, ss;
            int.TryParse(h, out hh);
            int.TryParse(m, out mm);
            int.TryParse(s, out ss);

            _lastSetTimeSpan = TimeInterval = new TimeSpan(hh, mm, ss);

            //int[] arr = await Task.WhenAll(TryParseAsync(h), TryParseAsync(m), TryParseAsync(s));
            //_lastSetTimeSpan = TimeInterval = new TimeSpan(arr[0], arr[1], arr[2]);
        }

        private Task<int> TryParseAsync(string s)
        {
            int i;
            return Task.Run(() =>
            {
                int.TryParse(s, out i);
                return i;
            });
        }

        protected virtual void OnTimeTickEvent()
        {
            TimeTickEvent?.Invoke();
        }

        protected virtual void OnTimesOutEvent()
        {
            TimesOutEvent?.Invoke();
        }
    }
}