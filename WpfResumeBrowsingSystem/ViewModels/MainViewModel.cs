using System;
using System.Collections.Generic;
using System.Linq;
using WpfResumeBrowsingSystem.Globe;
using WpfResumeBrowsingSystem.DBL;

namespace WpfResumeBrowsingSystem.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            this.DataList = (new SqlHelper()).GetTable<Staffs>();
        }

        private List<Staffs> dataList;

        public List<Staffs> DataList
        {
            get { return dataList; }
            set { dataList = value; this.RaisePropertyChanged("DataList"); }
        }

        private Staffs currentStaff;

        public Staffs CurrentStaff
        {
            get { return currentStaff; }
            set { currentStaff = value; this.RaisePropertyChanged("CurrentStaff"); }
        }

        private List<Experiences> currentExperienceList;

        public List<Experiences> CurrentExperienceList
        {
            get { return currentExperienceList; }
            set { currentExperienceList = value;  this.RaisePropertyChanged("CurrentExperienceList"); }
        }


    }
}
