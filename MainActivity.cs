using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Media;
using AndroidX.AppCompat.App;

namespace morseButton
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private ToneGenerator toneGen;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            // from Resources/layout/activity_main.axml
            SetContentView(Resource.Layout.activity_main);

            var button = FindViewById<Button>(Resource.Id.button1);

            // Initialize tone generator
            toneGen = new ToneGenerator(Stream.Music, 100);

            // Attach touch listener to button
            button.SetOnTouchListener(new ButtonTouchListener(toneGen));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            toneGen?.Release();
            toneGen = null;
        }
    }

    // Button hold touch listener
    public class ButtonTouchListener : Java.Lang.Object, View.IOnTouchListener
    {
        private readonly ToneGenerator _toneGen;

        public ButtonTouchListener(ToneGenerator toneGen)
        {
            _toneGen = toneGen;
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    _toneGen.StartTone(Tone.Dtmf0); 
                    return true;
                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
                    _toneGen.StopTone();
                    return true;
            }
            return false;
        }
    }
}