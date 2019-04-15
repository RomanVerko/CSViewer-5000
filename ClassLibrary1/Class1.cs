using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Station: IComparable<Station>
    {
        public string Name;
        public int ID;
        public string RegistrationDate;
        public string Location;
        public Owner Owner;
        public int Flow;
        public int MainTenanceTime;
        public string Road;
        public int otherOwners;
        public Station(string name,int iD, string registration, string location, Owner own, int flow, int maint, string road)
        {
            own.Name = CorrectWord(own.Name);
            name = CorrectWord(name);
            road = CorrectWord(road);

            Name = name;
            ID = iD;
            RegistrationDate = registration;
            Location = location;
            Owner = own;
            Flow = flow;
            MainTenanceTime = maint;
            Road = road;

        }
        public Station()
        {
            Owner = new Owner("Owner's name");
        }
        string CorrectWord(string old)
        {
            old = old.Replace('"', ' ');
            old = old.Replace("  ", " ");
            old = old.Trim();
            return old;
        }

        public int CompareTo(Station stat)
        {
            if (stat.otherOwners > otherOwners) return 1;
            else if (stat.otherOwners == otherOwners) return 0;
            else return -1;
        }
    }

    public class Owner
    {
        public string Name;
        public Owner(string a)
        {
            Name = a;
        }
    }
}
