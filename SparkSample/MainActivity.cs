using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Robinhood.Spark;
using Object = Java.Lang.Object;
using static SparkSample.Resource;

namespace SparkSample
{
    [Activity(Label = "Spark Sample", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Layout.activity_main);

            var sparkView = FindViewById<SparkView>(Id.sparkview);
            _adapter = new RandomizedAdapter();

            sparkView.Adapter = _adapter;
            sparkView.Scrub += SparkView_Scrub;

            var button = FindViewById<Button>(Id.random_button);
            button.Click += delegate {
                _adapter.Randomize();
            };

            _scrubInfoTextView = FindViewById<TextView>(Id.scrub_info_textview);
        }

        void SparkView_Scrub(object sender, SparkView.ScrubEventArgs e)
        {
            object value = e.Value;
            _scrubInfoTextView.Text = (value != null) ? $"Scrubbing value: {value}" : "Tap and hold the graph to scrub";
        }

        RandomizedAdapter _adapter;
        TextView _scrubInfoTextView;
    }

    public class RandomizedAdapter : SparkAdapter
    {
        public RandomizedAdapter()
        {
            _yData = new float[50];
            _random = new Random();
            Randomize();
        }

        public void Randomize()
        {
            for (int i = 0, count = _yData.Length; i < count; i++)
            {
                _yData[i] = (float)_random.NextDouble() * 100;
            }

            NotifyDataSetChanged();
        }
        public override int Count => _yData.Length;
        public override Object GetItem(int index) => _yData[index];
        public override float GetY(int index) => _yData[index];

        readonly float[] _yData;
        readonly Random _random;
    }
}

