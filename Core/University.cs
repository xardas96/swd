using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWD_projekt.Core
{
    public class University
    {
        private int id;
        public int ID
        {
            get
            {
                return id;
            }
        }
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
        }
        private double distance;
        public double Distance
        {
            get
            {
                return distance;
            }
            set
            {
                distance = value;
            }
        }

        public University(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }
}