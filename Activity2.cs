/*using Android.OS;
using Android.App;
using Android.Widget;
using Android.Hardware;

//이동거리:이중적분 사용
//      n+(n+1)/2*델타t->속력

namespace PhysicsLab
{
    public class Activity2 : Activity
    {
        static readonly object _syncLock = new object();
        TextView _sensorTextView;

        public TextView SensorTextView { get => _sensorTextView; set => _sensorTextView = value; }

        public float Integral(float start,float diff0,float diff1,float time)
        {
            float result = 0;
            result = start + diff0 + ((diff1 - diff0) / 2);
            return result;
        }

        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
           //어떻게할지 모르겠음
        }
        public void OnSensorResume_1()
        {
            base.OnResume();
            SensorManager.RegisterListener(SensorManager.GetDefaultSensor(SensorType.Accelerometer),SensorDelay.Ui);
        }
        public void onSensorPause_1()
        {
            base.OnPause();
            SensorManager.UnregisterListener(this);
        }
    }
}*/