using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VotingSystem.Models;

namespace VotingSystem.Controllers
{
    public class HomeController : Controller
    {
        SystemDb1 db = new SystemDb1();

        public ActionResult Index()
        {
            return View();
        }

        public FileResult DownloadExcel()
        {
            // string path = Server.MapPath("~/Doc/Voters.xlsx");
            String path = "/Doc/Members.xlsx";
            return File(path, "application/vnd.ms-excel", "Members.xlsx");
        }

        public ActionResult RegisterInstitution()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterInstitution(Institution details)
        {
            try
            {
                var query = (from r in db.Institutions
                             where r.InstitutionRegNo == details.InstitutionRegNo || r.InstitutionEmail == details.InstitutionEmail
                             select r).SingleOrDefault();

                if (query == null)
                {
                    if (ModelState.IsValid)
                    {
                        TempData["Institution"] = details;
                        return RedirectToAction("UploadMemberList");
                    }

                    else
                    {
                        ModelState.Clear();
                        ViewBag.Message = "Registration Unsuccessful!";
                        return View();
                    }      
                }
                
                else
                {
                    ModelState.Clear();
                    ViewBag.Message = "Institution already Registered!";
                    return View();
                }
                   
            }

            catch(Exception ex)
            {
                string error = ex.Message;
                ViewBag.error = error;
                return View("Error"); ;
            }
           
        }

        public ActionResult UploadMemberList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadMemberList(Member details, HttpPostedFileBase FileUpload)
        {

            Institution model = (Institution)TempData["Institution"];
            List<string> data = new List<string>();

            if (FileUpload != null)
            {
                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {


                    string filename = FileUpload.FileName;
                    string targetpath = Server.MapPath("~/Doc/Upload");
                    FileUpload.SaveAs(targetpath + filename);
                    string pathToExcelFile = targetpath + filename;
                    var connectionString = "";
                    if (filename.EndsWith(".xls"))
                    {
                        connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                    }
                    else if (filename.EndsWith(".xlsx"))
                    {
                        connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                    }

                    var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
                    var ds = new DataSet();

                    adapter.Fill(ds, "ExcelTable");

                    DataTable dtable = ds.Tables["ExcelTable"];

                    string sheetName = "Sheet1";

                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    var query = (from a in excelFile.Worksheet<Member>(sheetName) select a).ToList();

                    var Number = query.Count();

                    try
                    {
                        foreach (var a in query)
                        {
                            if (a.MemberFirstName != "" && a.MemberLastName != "" && a.MemberInstitutionNo != "" && a.MemberTelephone != "" && a.MemberGender != "" && a.MemberEmail != "")
                            {
                                Member TU = new Member();

                                TU.MemberFirstName = a.MemberFirstName;
                                TU.MemberLastName = a.MemberLastName;
                                TU.MemberInstitutionNo = a.MemberInstitutionNo;
                                TU.MemberInstitution = model.InstitutionName;
                                TU.MemberTelephone = a.MemberTelephone;
                                TU.MemberGender = a.MemberGender;
                                TU.MemberEmail = a.MemberEmail;
                                TU.MemberStatus = "Pending";

                                db.Members.Add(TU);

                                Number--;
                            }
                        }

                            if (Number == 0)
                            {
                                model.InstitutionPassword = "default"; //To be replaced by email message to institution email block
                                db.Institutions.Add(model);
                                db.SaveChanges();
                                
                                //Delete File
                                System.IO.File.Delete(pathToExcelFile);
                                return RedirectToAction("Index", "InstitutionAccount");
                            }
 
                            else
                            {
                                System.IO.File.Delete(pathToExcelFile);
                                ViewBag.Message = "Registration Unscuccessful! Check Excel File Data";
                                return RedirectToAction("Index");

                            }

                    }

                    catch (DbEntityValidationException ex)
                    {
                        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                        {

                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                           {

                                Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);

                            }

                       }
                    }
                  }
          
        }

                ViewBag.Message = "Registration Unsuccessful! Upload Member File";
                return RedirectToAction("Index");

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (db != null)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}