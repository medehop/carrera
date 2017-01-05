using Caliburn.Micro;
using System;
using System.IO.Ports;

namespace Carrera
{
    class CarreraViewModel : PropertyChangedBase
    {
        private SerialPort mySerialPort;

        int _lapCounter1 = 0;
        int _lapCounter2 = 0;
        DateTime _timeStamp1;
        DateTime _timeStamp2;

        DateTime? _lastLapTime1;
        DateTime? _lastLapTime2;

        string _lastLapTimeString1;
        string _lastLapTimeString2;

        public int LapCounter1
        {   get
            {
                return _lapCounter1;
            }
            private set
            {
                _lapCounter1 = value;
                NotifyOfPropertyChange(() => LapCounter1);
            }
        }

        public int LapCounter2
        {
            get
            {
                return _lapCounter2;
            }
            private set
            {
                _lapCounter2 = value;
                NotifyOfPropertyChange(() => LapCounter2);
            }
        }

        public string LastLapTimeString1
        {
            get
            {
                return _lastLapTimeString1;
            }
            private set
            {
                _lastLapTimeString1 = value;
                NotifyOfPropertyChange(() => LastLapTimeString1);
            }
        }

        public string LastLapTimeString2
        {
            get
            {
                return _lastLapTimeString2;
            }
            private set
            {
                _lastLapTimeString2 = value;
                NotifyOfPropertyChange(() => LastLapTimeString2);
            }
        }

        public void Start()
        {
            LapCounter1 = 0;
            LapCounter2 = 0;
            _timeStamp1 = DateTime.Now;
            _timeStamp2 = DateTime.Now;

            mySerialPort = new SerialPort("COM4");

            mySerialPort.BaudRate = 9600;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;
            mySerialPort.RtsEnable = false;

            mySerialPort.PinChanged += new SerialPinChangedEventHandler(PinChangedReveiveHandler);

            mySerialPort.Open();

        }

        public void Stop()
        {
            if (mySerialPort != null)
                mySerialPort.Close();
        }

        private void PinChangedReveiveHandler(
                    object sender,
                    SerialPinChangedEventArgs e)
        {
            switch (e.EventType)
            {
                case SerialPinChange.CDChanged:
                    if (_timeStamp1.AddSeconds(1) < DateTime.Now)
                    {
                        _timeStamp1 = DateTime.Now;
                        if (!_lastLapTime1.HasValue)
                        {
                            _lastLapTime1 = DateTime.Now;
                        }
                        else
                        {
                            LapCounter1++;
                            LastLapTimeString1 = DateTime.Now.Subtract(_lastLapTime1.Value).ToString("mm\\:ss\\.ff");
                            _lastLapTime1 = DateTime.Now;
                        }
                    }
                    break;
                case SerialPinChange.DsrChanged:
                    if (_timeStamp2.AddSeconds(1) < DateTime.Now)
                    {
                        _timeStamp2 = DateTime.Now;
                        if (!_lastLapTime2.HasValue)
                        {
                            _lastLapTime2 = DateTime.Now;
                        }
                        else
                        {
                            LapCounter2++;
                            LastLapTimeString2 = DateTime.Now.Subtract(_lastLapTime2.Value).ToString("mm\\:ss\\.ff");
                            _lastLapTime2 = DateTime.Now;
                        }
                    }
                    break;
            }
        }
    }
}
