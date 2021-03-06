﻿using Android.Widget;
using Plugin.InputKit.Platforms.Droid;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Plugin.InputKit.Shared.Utils.PopupMenu;

[assembly: ResolutionGroupName("Plugin.InputKit.Platforms")]
[assembly: ExportEffect(typeof(MenuEffect), "MenuEffect")]
namespace Plugin.InputKit.Platforms.Droid
{
    public class MenuEffect : PlatformEffect
    {
        PopupMenu ToggleMenu;
        InternalPopupEffect Effect;
        protected override void OnAttached()
        {
            Effect = (InternalPopupEffect)Element.Effects.FirstOrDefault(e => e is InternalPopupEffect);

            if (Effect != null)
                Effect.Parent.OnPopupRequest += OnPopupRequest;

            if (Control != null)
            {
                ToggleMenu = new PopupMenu(Plugin.CurrentActivity.CrossCurrentActivity.Current.AppContext, Control);
                ToggleMenu.Gravity = Android.Views.GravityFlags.Right;
                ToggleMenu.MenuItemClick += MenuItemClick;
            }

            else if (Container != null)
            {
                ToggleMenu = new PopupMenu(Plugin.CurrentActivity.CrossCurrentActivity.Current.AppContext, Container);
                ToggleMenu.Gravity = Android.Views.GravityFlags.Right;
                ToggleMenu.MenuItemClick += MenuItemClick;
            }
        }

        void OnPopupRequest(View view)
        {
            if (Effect.Parent.ItemsSource == null)
                return;

            ToggleMenu.Menu.Clear();

            for (int i = 0; i < Effect.Parent.ItemsSource.Count; i++)
            {
                ToggleMenu.Menu.Add(0, i, 0, new Java.Lang.String(Effect.Parent.ItemsSource[i] != null ? Effect.Parent.ItemsSource[i].ToString() : ""));
            }
            ToggleMenu.Show();
        }

        protected override void OnDetached()
        {
            if (ToggleMenu != null)
                ToggleMenu.MenuItemClick -= MenuItemClick;

            if (Effect != null)
                Effect.Parent.OnPopupRequest -= OnPopupRequest;
        }

        void MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e)
            => Effect?.Parent.InvokeItemSelected(e.Item.ToString(), e.Item.ItemId);
    }
}
