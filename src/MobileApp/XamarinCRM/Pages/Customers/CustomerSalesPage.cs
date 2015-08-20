﻿using Xamarin;
using Xamarin.Forms;
using XamarinCRM.Layouts;
using XamarinCRM.Pages.Base;
using XamarinCRM.Statics;
using XamarinCRM.Views.Base;
using XamarinCRM.Views.Customers;
using XamarinCRM.Views.Custom;

namespace XamarinCRM.Pages.Customers
{
    public class CustomerSalesPage : ModelBoundContentPage<CustomerSalesViewModel>
    {
        public CustomerSalesPage(CustomerSalesViewModel viewModel)
        {
            BackgroundColor = Color.Transparent;

            BindingContext = viewModel;

            #region header
            StackLayout headerStackLayout = new UnspacedStackLayout();

            Label companyTitleLabel = new Label()
            {
                Text = TextResources.Customers_Orders_EditOrder_CompanyTitle,
                TextColor = Palette._007,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                XAlign = TextAlignment.Start,
                YAlign = TextAlignment.End,
                LineBreakMode = LineBreakMode.TailTruncation
            };

            Label companyNameLabel = new Label()
            {
                TextColor = Device.OnPlatform(Palette._006, Color.White, Color.White),
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                XAlign = TextAlignment.Start,
                YAlign = TextAlignment.Start,
                LineBreakMode = LineBreakMode.TailTruncation
            };
            companyNameLabel.SetBinding(Label.TextProperty, "Account.Company");

            RelativeLayout headerLabelsRelativeLayout = new RelativeLayout() { HeightRequest = Sizes.LargeRowHeight };

            headerLabelsRelativeLayout.Children.Add(
                view: companyTitleLabel,
                widthConstraint: Constraint.RelativeToParent(parent => parent.Width),
                heightConstraint: Constraint.RelativeToParent(parent => parent.Height / 2));

            headerLabelsRelativeLayout.Children.Add(
                view: companyNameLabel,
                yConstraint: Constraint.RelativeToParent(parent => parent.Height / 2),
                widthConstraint: Constraint.RelativeToParent(parent => parent.Width),
                heightConstraint: Constraint.RelativeToParent(parent => parent.Height / 2));

            ContentView headerLabelsView = new ContentView() { Padding = new Thickness(20, 0), Content = headerLabelsRelativeLayout };

            headerStackLayout.Children.Add(new ContentViewWithBottomBorder() { Content = headerLabelsView });
            #endregion

            #region weekly sales chart
            CustomerWeeklySalesChartView customerWeeklySalesChartView = new CustomerWeeklySalesChartView() { BindingContext = ViewModel };
            #endregion

            #region category sales chart
            CustomerCategorySalesChartView customerCategorySalesChartView = new CustomerCategorySalesChartView() { BindingContext = ViewModel };
            #endregion

            #region compose view hierarchy
            StackLayout stackLayout = new UnspacedStackLayout();
            stackLayout.Children.Add(headerStackLayout);
            stackLayout.Children.Add(customerWeeklySalesChartView);
            stackLayout.Children.Add(customerCategorySalesChartView);
            #endregion

            #region platform adjustments
            Device.OnPlatform(
                Android: () =>
                {
                    BackgroundColor = Palette._009;
                }
            );
            #endregion

            Content = new ScrollView() { Content = stackLayout };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!ViewModel.IsInitialized)
            {
                await ViewModel.ExecuteLoadSeedDataCommand(ViewModel.Account);
                ViewModel.IsInitialized = true;
            }

            Insights.Track("Customer Sales Page");
        }
    }
}

