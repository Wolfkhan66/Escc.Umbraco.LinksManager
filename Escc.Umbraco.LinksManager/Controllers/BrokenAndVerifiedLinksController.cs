using Escc.Umbraco.LinksManager.Models.BrokenAndVerifiedLinks;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Escc.Umbraco.LinksManager.Controllers
{
    public class BrokenAndVerifiedLinksController : Controller
    {
        #region Global Variables
        public string _BrokenFilePath = ConfigurationManager.AppSettings["InspyderBrokenLinks"];
        public string _VerifiedFilePath = ConfigurationManager.AppSettings["InspyderVerifiedLinks"];
        #endregion

        #region ActionResults
        [Route("Broken", Name = "Broken")]
        public ActionResult Broken(int ShowResultsFrom = 0)
        {
            var model = new BrokenLinksViewModel();
            model.ShowResultsFrom = ShowResultsFrom;
            try
            {
                model.BrokenLinksTable.Table = PrepareDataTable("Link Errors", _BrokenFilePath, ShowResultsFrom);
            }
            catch (Exception error)
            {
                model.ErrorMessage = error.Message;
            }
            model.TotalBroken = CountCSVRows("Link Errors", _BrokenFilePath);
            return View(model);
        }

        [Route("Verified", Name = "Verified")]
        public ActionResult Verified(int ShowResultsFrom = 0)
        {
            var model = new VerifiedLinksViewModel();
            model.ShowResultsFrom = ShowResultsFrom;
            try
            {
                model.VerifiedLinksTable.Table = PrepareDataTable("Verified Links", _VerifiedFilePath, ShowResultsFrom);
            }
            catch (Exception error)
            {
                model.ErrorMessage = error.Message;
            }
            model.TotalVerified = CountCSVRows("Verified Links", _VerifiedFilePath);
            return View(model);
        }

        public ActionResult SearchAllBroken(string Query)
        {
            var model = new BrokenLinksViewModel();
            model.Search = true;
            model.Query = Query;
            try
            {
                model.BrokenLinksTable.Table = PrepareSearchDataTable("Link Errors", _BrokenFilePath, Query);
            }
            catch (Exception error)
            {
                model.ErrorMessage = error.Message;
            }
            model.TotalBroken = CountCSVRows("Link Errors", _BrokenFilePath);
            return View("Broken", model);
        }

        public ActionResult SearchAllVerified(string Query)
        {
            var model = new VerifiedLinksViewModel();
            model.Search = true;
            model.Query = Query;
            try
            {
                model.VerifiedLinksTable.Table = PrepareSearchDataTable("Verified Links", _VerifiedFilePath, Query);
            }
            catch (Exception error)
            {
                model.ErrorMessage = error.Message;
            }
            model.TotalVerified = CountCSVRows("Verified Links", _VerifiedFilePath);
            return View("Verified", model);
        }
        #endregion

        #region Helpers
        public void CheckFilePath(string _filePath)
        {
            // Check a file path was supplied
            if (string.IsNullOrEmpty(_filePath))
            {
                var err = new Exception("No File Path Supplied!");
                throw err;
            }

            // Check file is present & accessible
            if (!System.IO.File.Exists(_filePath))
            {
                var err = new Exception("No CSV file was found or it is not accessible!");
                throw err;
            }
        }

        public DataTable PrepareSearchDataTable(string TableType, string _filePath, string Query)
        {
            var Results = ProcessCSVFile(TableType, _filePath);
            var Table = new DataTable();
            switch (TableType)
            {
                case "Link Errors":
                    Table.Columns.Add("Referrer", typeof(string));
                    Table.Columns.Add("Broken Link", typeof(string));
                    Table.Columns.Add("Error Type", typeof(string));
                    foreach (var field in Results)
                    {
                        if (field[1].Contains(Query))
                        {
                            Table.Rows.Add(field[0], field[1], field[2]);
                        }
                    }
                    break;
                case "Verified Links":
                    Table.Columns.Add("Link", typeof(string));
                    Table.Columns.Add("Type", typeof(string));
                    Table.Columns.Add("Referrer", typeof(string));
                    Table.Columns.Add(" Content Type", typeof(string));
                    foreach (var field in Results)
                    {
                        if (field[0].Contains(Query) || field[2].Contains(Query))
                        {
                            Table.Rows.Add(field[0], field[1], field[2], field[3]);
                        }
                    }
                    break;
            }   
            return Table;
        }


        public DataTable PrepareDataTable(string TableType, string _filePath, int ShowResultsFrom = 0)
        {
            var Results = ProcessCSVFile(TableType, _filePath);
            var Table = new DataTable();
            Results.RemoveRange(0, ShowResultsFrom);
            var ResultsToShow = Results.Take(2000);
            switch (TableType)
            {
                case "Link Errors":
                    Table.Columns.Add("Referrer", typeof(string));
                    Table.Columns.Add("Broken Link", typeof(string));
                    Table.Columns.Add("Error Type", typeof(string));
                    foreach (var field in Results)
                    {
                        Table.Rows.Add(field[0], field[1], field[2]);
                    }
                    break;

                case "Verified Links":
                    Table.Columns.Add("Link", typeof(string));
                    Table.Columns.Add("Type", typeof(string));
                    Table.Columns.Add("Referrer", typeof(string));
                    Table.Columns.Add(" Content Type", typeof(string));
                    foreach (var field in ResultsToShow)
                    {
                        Table.Rows.Add(field[0], field[1], field[2], field[3]);
                    }
                    break;
            }
            return Table;
        }

        public List<string[]> ProcessCSVFile(string fileType, string _filePath)
        {
            CheckFilePath(_filePath);
            var Results = new List<string[]>();
            using (var csvParser = new TextFieldParser(_filePath))
            {
                csvParser.CommentTokens = new[] { "#" };
                csvParser.SetDelimiters(",");
                csvParser.HasFieldsEnclosedInQuotes = true;

                string d;
                // Read until we find the Report Title line (Link Errors)
                do
                {
                    d = csvParser.ReadLine();
                    if (d == null)
                    {
                        // Do Nothing, Keep going until we find the title line
                    };
                } while (!d.StartsWith(fileType));

                // Read past the row with the column names
                csvParser.ReadLine();
                while (!csvParser.EndOfData)
                {
                    // Read cells on the current line, move the pointer to the next line.
                    var fields = csvParser.ReadFields();

                    // empty row? Move to the next one
                    if (fields == null) continue;
                    // Add the fields from the row to the results
                    Results.Add(fields);
                }
            }
            return Results;
        }

        public int CountCSVRows(string fileType, string _filePath)
        {
            CheckFilePath(_filePath);
            int lineNumber = 0;
            using (var csvParser = new TextFieldParser(_filePath))
            {
                csvParser.CommentTokens = new[] { "#" };
                csvParser.SetDelimiters(",");
                csvParser.HasFieldsEnclosedInQuotes = true;
                string d;
                // Read until we find the Report Title line (Broken Links)
                do
                {
                    d = csvParser.ReadLine();
                    if (d == null)
                    {
                        // Throw Exception
                    };
                } while (!d.StartsWith(fileType));

                // Read past the row with the column names
                csvParser.ReadLine();
                while (!csvParser.EndOfData)
                {
                    lineNumber++;
                    csvParser.ReadLine();
                }
            }
            return lineNumber;
        }
        #endregion
    }
}