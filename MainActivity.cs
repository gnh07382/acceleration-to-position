using Android.App;
using Android.Widget;
using Android.OS;
using System;


namespace PhysicsLab
{
    [Activity(Label = "PhysicsLab", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        TextView _sensorTextView;

        bool hasUpdated = false;
        DateTime lastUpdate;
        float last_x = 0.0f;
        float last_y = 0.0f;
        float last_z = 0.0f;   //전역변수

        float deltaT = 0;

        float[] accel_x = new float[2];
           /* accel_x[0] = 0;
            accel_x[1] = 0;*/

        float[] speed_x = new float[2];
            //speed_x[0] = 0;
            //speed_x[1] = 0;
        float[] posit_x = new float[2];

        public TextView SensorTextView { get => _sensorTextView; set => _sensorTextView = value; }

        //posit_x[0] = 0;
        //posit_x[1] = 0;

        public float Integral(float start, float diff0, float diff1, float time)
        {
            float result = 0;
            result = start + diff0 + ((diff1 - diff0) / 2)*(time/1000);
            return result;
        }//적분 알고리즘
        public void OnSensorChanged(Android.Hardware.SensorEvent e)
        {
            accel_x[0] = 0;
            accel_x[1] = 0;

            speed_x[0] = 0;
            speed_x[1] = 0;

            posit_x[0] = 0;
            posit_x[1] = 0;

            const double movementEndCheckVariable = 0.11;

            if (e.Sensor.Type == Android.Hardware.SensorType.Accelerometer)
            {
                float x = e.Values[0];
                float y = e.Values[1];
                float z = e.Values[2];


                DateTime curtime = DateTime.Now;
                if (hasUpdated == false)
                {
                    hasUpdated = true;
                    lastUpdate = curtime;
                    last_x = x;
                    last_y = y;
                }
                else
                {
                    lastUpdate = curtime;
                    curtime = DateTime.Now;
                    deltaT = (float)(curtime - lastUpdate).TotalMilliseconds;
                    float time = deltaT / 1000;
                    speed_x[1] = Integral(speed_x[0], accel_x[0], accel_x[1], time);
                    posit_x[1] = Integral(posit_x[0], speed_x[0], speed_x[1], time);
                    accel_x[0] = accel_x[1];
                    speed_x[0] = speed_x[1];
                    posit_x[0] = posit_x[1];

                    int countx = 0;

                    if (accel_x[1] < movementEndCheckVariable &&accel_x[1] > -movementEndCheckVariable)
                    { countx++; }
                    else
                    { countx = 0; }
                    if (countx >= 5)
                    {
                        speed_x[1] = 0;
                        speed_x[0] = 0;
                    }//움직임 확인
                }
            }
        }//센서 관련
        
        public void OnAccuracyChanged(Android.Hardware.Sensor sensor, Android.Hardware.SensorStatus accuracy)
        {

        }//정확도 관련 건들지X
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            object _syncLock = new object();
            //TextView _sensorTextView;

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            var sensorManager = GetSystemService(SensorService) as Android.Hardware.SensorManager;
            var sensor = sensorManager.GetDefaultSensor(Android.Hardware.SensorType.Accelerometer);
            //sensorManager.RegisterListener(this, sensor, Android.Hardware.SensorDelay.Game);

                Button startbtn = FindViewById<Button>(Resource.Id.startbtn);
                startbtn.Click += delegate
                {
                    //OnSensorChanged(e);

                    lock (_syncLock)
                    {
                        SensorTextView.Text = string.Format("accel: x={0:f}",accel_x[1]);
                        SensorTextView = FindViewById<TextView>(Resource.Id.accelerometer_text);
                    }

                    lock (_syncLock)
                    {
                        SensorTextView.Text = string.Format("speed: x={0:f}", speed_x[1]);
                        SensorTextView = FindViewById<TextView>(Resource.Id.speed);
                    }
                    lock (_syncLock)
                    {
                        SensorTextView.Text = string.Format("distance: x={0:f}", posit_x[1]);
                        SensorTextView = FindViewById<TextView>(Resource.Id.dist1);
                    }


                    base.OnResume();
                };//스타트 버튼 클릭
                Button stopbtn = FindViewById<Button>(Resource.Id.stopbtn);
                stopbtn.Click += delegate
                { 
                    base.OnPause();
                };//스톱 버튼 클릭
            
        }
    }
}

