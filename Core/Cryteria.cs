using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWD_projekt.Core
{
    public class Cryteria
    {
        public static readonly int CITY_DISTANCE_CRYTERIA = 9;
        public static readonly int FREE_UNIVERSITY_CRYTERIA = 10;

        private int id;
        public int ID {
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

        public Cryteria(int id, string name)
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