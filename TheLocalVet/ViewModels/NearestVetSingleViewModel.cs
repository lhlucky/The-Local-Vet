﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TheLocalVet.Interfaces;
using TheLocalVet.Languages;
using TheLocalVet.Models;
using Xamarin.Forms;

namespace TheLocalVet.ViewModels
{
    public class NearestVetSingleViewModel : BaseViewModel
    {
        private VetModel _vetModel;
        private IDialer _phoneDialer;

        public RelayCommand CallVetCommand { get; private set; }
        public RelayCommand EmailVetCommand { get; private set; }
        public RelayCommand VisitWebCommand { get; private set; }

        public string Name
        {
            get { return _vetModel.Name; }
            set { _vetModel.Name = value; OnPropertyChanged("Name"); }
        }
        
        public string Address
        {
            get { return _vetModel.Address.ToString(); }
            set { OnPropertyChanged("Address"); }
        }

        public string HomeVisit
        {
            get
            {
                if (_vetModel.HomeVisit)
                    return AppResources.Yes;
                else
                    return AppResources.No;
            }
            set { OnPropertyChanged("HomeVisit"); }
        }

        public string CompetencyString
        {
            get
            {
                if (_vetModel.MainCompetency == Competency.bigvet)
                    return AppResources.bigvet;
                else
                    return AppResources.smallvet;
            }
            set { OnPropertyChanged("CompetencyString"); }
        }

        public string Email
        {
            get { return _vetModel.Email; }
            set { OnPropertyChanged("Email"); }
        }

        public string Website
        {
            get { return _vetModel.WebSite; }
            set { OnPropertyChanged("Website"); }
        }

        public string Phone
        {
            get { return ParsePhone(_vetModel.Phone); }
            set { OnPropertyChanged("Phone"); }
        }

        public string OpeningHours
        {
            get { return ParseOpeningHours(_vetModel.OpeningHours); }
            set { OnPropertyChanged("OpeningHours"); }
        }

        public NearestVetSingleViewModel(VetModel vetModel)
        {
            _vetModel = vetModel;

            CallVetCommand = new RelayCommand(CallVet);
            EmailVetCommand = new RelayCommand(EmailVet);
            VisitWebCommand = new RelayCommand(VisitWeb);

            _phoneDialer = DependencyService.Get<IDialer>();
        }

        private void VisitWeb()
        {
            Debug.WriteLine("{0} - {1}", Website, new Uri(Website));
            Device.OpenUri(new Uri(Website));
        }

        private void EmailVet()
        {
            Debug.WriteLine(Email);
            Device.OpenUri(new Uri(string.Format("mailto:{0}", Email)));
        }

        private void CallVet()
        {
            Debug.WriteLine("calling {0}", Phone);

            if (_phoneDialer != null)
                _phoneDialer.Dial(Phone, Name);
        }

        private string ParseOpeningHours(List<string> openingHours)
        {
            string returnData = string.Empty;

            foreach(var openingHour in openingHours)
            {
                returnData += string.Format("{0} {1}", openingHour, Environment.NewLine);
            }

            return returnData;
        }

        private string ParsePhone(List<string> phones)
        {
            string returnData = string.Empty;

            if(phones.Count == 0)
            {
                returnData = string.Empty;
            }
            //else if(phones.Count == 1)
            //{
            //    returnData = phones.First();
            //}
            else
            {
                returnData = phones.First();
                //foreach(var phone in phones)
                //{
                //    returnData += string.Format("{0}, ", phone);
                //}
            }

            return returnData;
        }
    }
}