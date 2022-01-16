global using static System.Math;
global using KiDev.AndroidAutoFinder;
global using static GraphicsApp.Resource;
global using Color = Android.Graphics.Color;

using Android.Content;
using Android.Hardware;
using GraphicsApp.Drawers;
using GraphicsApp.Filters;
using GraphicsApp.Graphics;
using Filter = GraphicsApp.Filters.Filter;

namespace GraphicsApp;

// Основной Activity
[SetView(Layout.activity_main)]
[Activity(Label = "@string/app_name", MainLauncher = true)]
public partial class MainActivity : Activity, ISensorEventListener
{
    [FindById(Id.exit_button)] Button exitButton;
    [FindById(Id.bipyramid_ex)] Button bipyrButton;
    [FindById(Id.cube_example)] Button cubeButton;

    void AfterOnCreate()
    {
        var manager = (SensorManager)GetSystemService(SensorService);
        manager.RegisterListener(this, manager.GetDefaultSensor(SensorType.Gravity), SensorDelay.Game);
        exitButton.Click += (_, _) => Finish();
        cubeButton.Click += (_, _) => LaunchDrawer(new CubeDrawer(() => gravityPoint));
        bipyrButton.Click += (_, _) => LaunchDrawer(new BipyramidDrawer(8));
    }
    
    // Запускает SurfaceActivity с заданным отрисовщиком
    void LaunchDrawer(IDrawer drawer)
    {
        SurfaceActivity.Drawer = drawer;
        Intent intent = new(this, typeof(SurfaceActivity));
        StartActivity(intent);
    }

    #region Реализация ISensorEventListener
    Point3D gravityPoint; // Точка, описывающая гравитацию

    Filter filterX = new MedianFilter(17).Chain(new ExponentialFilter(0.85)),
           filterY = new MedianFilter(17).Chain(new ExponentialFilter(0.85)),
           filterZ = new MedianFilter(17).Chain(new ExponentialFilter(0.85)); // Фильты по координатам

    public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy) { }

    public void OnSensorChanged(SensorEvent e)
    {
        if (e.Sensor.Type != SensorType.Gravity) return;
        gravityPoint = new(
            filterX.Put(-e.Values[0]),
            filterY.Put(e.Values[1]),
            filterZ.Put(e.Values[2])
        );
    }
    #endregion
}
