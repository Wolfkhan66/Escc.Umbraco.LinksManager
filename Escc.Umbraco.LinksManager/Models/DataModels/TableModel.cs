﻿using System.Data;

namespace Escc.Umbraco.LinksManager.Models.DataModels
{
    public class TableModel
    {
        public DataTable Table { get; set; }
        public string ID { get; set; }

        public TableModel(string id)
        {
            ID = id;
        }
    }
}