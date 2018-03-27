﻿using System.Linq;
using System.Threading.Tasks;
using Bridge.Navigation;
using Bridge.Spaf;
using realworld.spaf.Classes;
using realworld.spaf.Models.Request;
using realworld.spaf.Services;
using static Retyped.knockout;

namespace realworld.spaf.ViewModels
{
    class SettingsViewModel : LoadableViewModel
    {
        private readonly IUserService _userService;
        private readonly INavigator _navigator;

        protected override string ElementId() => SpafApp.SettingsId;

        public KnockoutObservable<string> ImageUri { get; set; }
        public KnockoutObservable<string> Username { get; set; }
        public KnockoutObservable<string> Biography { get; set; }
        public KnockoutObservable<string> Email { get; set; }
        public KnockoutObservable<string> NewPassword { get; set; }
        public KnockoutObservableArray<string> Errors { get; set; }


        public SettingsViewModel(IUserService userService, INavigator navigator)
        {
            this._userService = userService;
            this._navigator = navigator;

            this.ImageUri = ko.observable.Self<string>();
            this.Username = ko.observable.Self<string>();
            this.Biography = ko.observable.Self<string>();
            this.Email = ko.observable.Self<string>();
            this.NewPassword = ko.observable.Self<string>();
            this.Errors = ko.observableArray.Self<string>();

            this.PopulateEntries();
        }

        private void PopulateEntries()
        {
            var user = this._userService.LoggedUser;
            this.Username.Self(user.Username);
            this.Email.Self(user.Email);
            this.ImageUri.Self(user.Image);
            this.Biography.Self(user.Bio);
        }

        private async Task UpdateSettings()
        {
            try
            {
                await this._userService.UpdateSettings(this.Username.Self(), this.NewPassword.Self(), this.Biography.Self(), this.Email.Self(), this.ImageUri.Self());
                this._navigator.Navigate(SpafApp.ProfileId);

            }
            catch (PromiseException e)
            {
                var errors = e.GetErrorList();
                this.Errors.push(errors.ToArray());
            }
        }
    }
}
