namespace TechAid;

public partial class AppShell : SimpleToolkit.SimpleShell.SimpleShell
{
    private const string FlyoutAnimationKey = "FlyoutAnimation";
    private const float FlyoutBackdropOpacity = 0.2f;

    private double flyoutWidth => 285 + safeArea.Left;
    private Thickness safeArea;
    public AppShell()
    {
        InitializeComponent();

        AddTab(typeof(WorkOrdersPage), PageType.WorkOrdersPage);
        AddTab(typeof(AssetsPage), PageType.AssetsPage);
        AddTab(typeof(InspectionPage), PageType.InspectionPage);
        AddTab(typeof(SchedulePage), PageType.SchedulePage);
        AddTab(typeof(InventoryPage), PageType.InventoryPage);

        Loaded += AppShellLoaded;

        HideFlyout();
    }
    private static void AppShellLoaded(object sender, EventArgs e)
    {
        var shell = sender as AppShell;

        shell.Window.SubscribeToSafeAreaChanges(safeArea =>
        {
            shell.safeArea = safeArea;

            shell.pageContainer.Margin = safeArea;
            shell.tabBarView.Margin = safeArea;
            //shell.bottomBackgroundRectangle.IsVisible = safeArea.Bottom > 0;
            //shell.bottomBackgroundRectangle.HeightRequest = safeArea.Bottom;
            shell.rootPageContainer.Padding = new Thickness(0, safeArea.Top, 0, 0);
            shell.navBar.Padding = new Thickness(safeArea.Left, 0, safeArea.Right, 0);
            shell.flyoutContent.Padding = new Thickness(safeArea.Left, safeArea.Top, 0, safeArea.Bottom);
            shell.UpdateFlyoutWidth();
        });
    }

    private void AddTab(Type page, PageType pageEnum)
    {
        Tab tab = new Tab { Route = pageEnum.ToString(), Title = pageEnum.ToString() };
        tab.Items.Add(new ShellContent { ContentTemplate = new DataTemplate(page) });

        tabBar.Items.Add(tab);
    }

    private void TabBarViewCurrentPageChanged(object sender, TabBarEventArgs e)
    {
        Shell.Current.GoToAsync("///" + e.CurrentPage.ToString());
    }
    private void MenuClicked(object sender, EventArgs e)
    {
        ShowFlyout(true);
    }
    private void ShowFlyout(bool animated = false)
    {
        flyoutBackdrop.InputTransparent = false;

        if (!animated)
        {
            flyout.TranslationX = 0;
            // TODO: I cannot use Opacity because it is broken on Android
            flyoutBackdrop.Fill = Colors.Black.WithAlpha(FlyoutBackdropOpacity);
            return;
        }

        flyout.AbortAnimation(FlyoutAnimationKey);

        var animation = new Animation(v =>
        {
            flyout.TranslationX = -flyoutWidth * v;
            flyoutBackdrop.Fill = Colors.Black.WithAlpha((float)((1 - v) * FlyoutBackdropOpacity));
        }, 1, 0);

        animation.Commit(flyout, FlyoutAnimationKey, easing: Easing.CubicOut, finished: (v, b) =>
        {
            flyout.TranslationX = 0;
            flyoutBackdrop.Fill = Colors.Black.WithAlpha(FlyoutBackdropOpacity);
        });
    }

    private void UpdateFlyoutWidth()
    {
        flyout.WidthRequest = flyoutWidth;

        if (flyout.TranslationX < 0)
            HideFlyout();
        else
            ShowFlyout();
    }
    private void HideFlyout(bool animated = false)
    {
        flyoutBackdrop.InputTransparent = true;

        if (!animated)
        {
            flyout.TranslationX = -flyoutWidth;
            flyoutBackdrop.Fill = Colors.Black.WithAlpha(0);
            return;
        }

        flyout.AbortAnimation(FlyoutAnimationKey);

        var animation = new Animation(v =>
        {
            flyout.TranslationX = -flyoutWidth * v;
            flyoutBackdrop.Fill = Colors.Black.WithAlpha((float)((1 - v) * FlyoutBackdropOpacity));
        });

        animation.Commit(flyout, FlyoutAnimationKey, easing: Easing.CubicIn, finished: (v, b) =>
        {
            flyout.TranslationX = -flyoutWidth;
            flyoutBackdrop.Fill = Colors.Black.WithAlpha(0);
        });
    }
    private void FlyoutBackdropTapped(object sender, TappedEventArgs e)
    {
        HideFlyout(true);
    }
}
public enum PageType
{
    WorkOrdersPage, AssetsPage, InspectionPage, SchedulePage,  InventoryPage
}
