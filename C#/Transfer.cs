using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;

namespace WindowsFormsApplication2
{
    //[Serializable]
    public class transfer
    {
        private string _data;
        private string _id;
        private int _dl;

        public transfer()
        {
            _id = "0x000";
            _dl = 8;
            _data = "00 00 00 00 00 00 00 00";

        }

        public string id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int dl
        {
            get { return _dl; }
            set { _dl = value; }
        }

        public string data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        public string[] dataArray()
        {
            //string[] dataarray = new string[8];
            string[] dataarray = _data.Split(' ');
            return dataarray;
        }

    }
}
