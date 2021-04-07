using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using CsvHelper.Configuration.Attributes;

namespace BLL.Data
{
    public class InputRecord
    {
        public DateTime Date { get; set; }
        [BooleanTrueValues("Reader entry")]
        [BooleanFalseValues("Reader exit")]
        public bool Event { get; set; }
        public string Gate { get; set; }
    }
}
