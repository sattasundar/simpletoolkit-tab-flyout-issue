namespace TechAid.Views.Controls;

public partial class TabBarIconView : ContentView
{
    public PageType Page { get; set; }

    public ImageSource Source
    {
        get => (ImageSource)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }
    public Color TintColor
    {
        get => (Color)GetValue(TintColorProperty);
        set => SetValue(TintColorProperty, value);
    }
    public static readonly BindableProperty SourceProperty =
        BindableProperty.Create(nameof(Source), typeof(ImageSource), typeof(TabBarIconView), default(ImageSource), BindingMode.OneWay);

    public static readonly BindableProperty TintColorProperty =
       BindableProperty.Create(nameof(TintColor), typeof(Color), typeof(TabBarIconView), default(Color), BindingMode.OneWay);
    public TabBarIconView()
    {
        InitializeComponent();
        TintColor = Colors.Gray; //setting default icon color
        BindingContext = this;
    }
}