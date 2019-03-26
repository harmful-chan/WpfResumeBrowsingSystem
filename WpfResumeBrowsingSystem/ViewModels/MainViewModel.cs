using System;
using System.Collections.Generic;
using System.Linq;
using WpfResumeBrowsingSystem.Globe;
using WpfResumeBrowsingSystem.DBL;
using System.Windows.Input;
using WpfResumeBrowsingSystem.Commands;
using System.Windows;
using System.Windows.Controls;

namespace WpfResumeBrowsingSystem.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            //获取Staff总表
            this.StaffTable = SqlHelper.GetTable<Staffs>();

            //获取Experience总表
            this.ExperienceTable = SqlHelper.GetTable<Experiences>();

            this.Selected = new SelectedCommand(p =>
            {
                this.CurrentStaff = p as Staffs;
                this.CurrentExperience = new List<Experiences>(
                    this.ExperienceTable.Where(
                        x => x.Staff_Sid == this.CurrentStaff.Sid
                        ));
            });
        }
        
        //选择改变命令
        public ICommand Selected { get; set; }

        //Staff总表
        private List<Staffs> _staffTable;

        public List<Staffs> StaffTable
        {
            get { return _staffTable; }
            set { _staffTable = value; this.RaisePropertyChanged("StaffTable"); }
        }

        //Experience总表
        private List<Experiences> _experienceTable;

        public List<Experiences> ExperienceTable
        {
            get { return _experienceTable; }
            set { _experienceTable = value; }
        }


        //当前选择Staff
        private Staffs _currentStaff;

        public Staffs CurrentStaff
        {
            get { return _currentStaff; }
            set { _currentStaff = value; this.RaisePropertyChanged("CurrentStaff"); }
        }

        //当前Staff对应的Experience
        private List<Experiences> _currentExperience;

        public List<Experiences> CurrentExperience
        {
            get { return _currentExperience; }
            set { _currentExperience = value;  this.RaisePropertyChanged("CurrentExperience"); }
        }


    }
}
