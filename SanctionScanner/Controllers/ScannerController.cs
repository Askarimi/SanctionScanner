﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SanctionScanner.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using ExcelDataReader;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Threading;

namespace SanctionScanner.Controllers
{
    public class ScannerController : Controller
    {
        private readonly SanctionScannerDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        public ScannerController(SanctionScannerDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index()
        {

            return View(await _context.SourceSanctions.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        //450314@Mojgan
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SourceSanctionModel model, IFormFile formFile)
        {
            if (ModelState.IsValid)
            {

                SourceSanction sourceSanction = new SourceSanction();
                sourceSanction.FormatFile = model.FormatFile;
                sourceSanction.HasFile = model.HasFile;
                model.NameFile = model.NameFile;
                sourceSanction.SourceName = model.SourceName;

                //if (model.FormFile != null)
                //{
                //    string importFolder = Path.Combine(_hostingEnvironment.WebRootPath, "ImportFiles");
                //    string filePath = Path.Combine(importFolder, model.FormFile.FileName);
                //    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                //    var excelDataTable = GetDataTableFromExcel(filePath);

                //    List<Sanction> sanctions = new List<Sanction>();
                //    sanctions = excelDataTable.AsEnumerable().Select(c => new Sanction
                //    {
                //        AdditionalInformation = Convert.ToString(c["AdditionalInformation"]),
                //        Address = Convert.ToString(c["Address"]),
                //        Citizenship = Convert.ToString(c["Citizenship"]),
                //        Committees = Convert.ToString(c["Committees"]),
                //        ControlDate = Convert.ToString(c["ControlDate"]),
                //        CountryRelated = Convert.ToString(c["CountryRelated"]),
                //        DateofBirth = Convert.ToString(c["DateofBirth"]),
                //        EntityType = Convert.ToString(c["Type"]),
                //        InsertDate = Convert.ToString(DateTime.Now.Date),
                //        IsActive = Convert.ToByte(1),
                //        LegalName = Convert.ToString(c["NameofIndividualorEntity"]),
                //        ListingInformation = Convert.ToString(c["ListingInformation"]),
                //        NameType = Convert.ToString(c["NameType"]),
                //        PlaceofBirth = Convert.ToString(c["PlaceofBirth"]),
                //        SactionUID = new Guid(),
                //    }).ToList();
                //}          
                _context.SourceSanctions.Add(sourceSanction);
                await _context.SaveChangesAsync();
                //return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var sourceSanction = _context.SourceSanctions.FindAsync(id).Result;
            if (sourceSanction == null)
            {
                return NotFound();
            }
            SourceSanction model = new SourceSanction();
            model.Id = sourceSanction.Id;
            model.HasFile = sourceSanction.HasFile;
            model.NameFile = sourceSanction.NameFile;
            model.SourceName = sourceSanction.SourceName;
            model.SourceCode = sourceSanction.SourceCode;
            return View(model);
        }
        [HttpPost()]
        public async Task<IActionResult> Edit(int id, SourceSanction model, IFormFile file)
        {
            if (model == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                    {
                        ViewBag.ErrorMessage = "Not Support file extension";
                        return View("Index");
                    }
                    var sanctions = new List<Sanction>();
                    SourceSanction sourceSanction = new SourceSanction();
                    sourceSanction.Id = id;
                    sourceSanction.FormatFile = model.FormatFile;
                    sourceSanction.HasFile = model.HasFile;
                    sourceSanction.NameFile = model.NameFile;
                    sourceSanction.SourceCode = model.SourceCode;
                    sourceSanction.SourceName = model.SourceName;
                    using (var stream = new MemoryStream())
                    {
                        string importFolder = Path.Combine(_hostingEnvironment.WebRootPath, "ImportFiles");
                        string filePath = Path.Combine(importFolder, file.FileName);
                        await file.CopyToAsync(stream);
                        using (var package = new ExcelPackage(stream))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                            var rowCount = worksheet.Dimension.Rows;
                            var counterRecord = 0;
                            for (int row = 2; row <= rowCount; row++)
                            {
                                if (!CheckHasRecord(id, worksheet.Cells[row, 1].Value.ToString().Trim()))
                                {
                                    model.Sanctions.Add(new Sanction
                                    {
                                        RefrenceId = worksheet.Cells[row, 1].Value == null ? "" : worksheet.Cells[row, 1].Value.ToString().Trim(),
                                        LegalName = worksheet.Cells[row, 2].Value == null ? "" : worksheet.Cells[row, 2].Value.ToString().Trim(),
                                        EntityType = worksheet.Cells[row, 3].Value == null ? "" : worksheet.Cells[row, 3].Value.ToString().Trim(),
                                        NameType = worksheet.Cells[row, 4].Value == null ? "" : worksheet.Cells[row, 4].Value.ToString().Trim(),
                                        DateofBirth = worksheet.Cells[row, 5].Value == null ? "" :  worksheet.Cells[row, 5].Value.ToString().Trim(),
                                        PlaceofBirth = worksheet.Cells[row, 6].Value == null ? "" :  worksheet.Cells[row, 6].Value.ToString().Trim(),
                                        Citizenship = worksheet.Cells[row, 7].Value == null ? "" : worksheet.Cells[row, 7].Value.ToString().Trim(),
                                        Address = worksheet.Cells[row, 8].Value == null ? "" : worksheet.Cells[row, 8].Value.ToString().Trim(),
                                        AdditionalInformation = worksheet.Cells[row, 9].Value == null ? "" : worksheet.Cells[row, 9].Value.ToString().Trim(),
                                        ListingInformation = worksheet.Cells[row, 10].Value == null ? "" : worksheet.Cells[row, 10].Value.ToString().Trim(),
                                        Committees = worksheet.Cells[row, 11].Value == null ? "" : worksheet.Cells[row, 11].Value.ToString().Trim(),
                                        ControlDate = worksheet.Cells[row, 12].Value == null ? "" : worksheet.Cells[row, 12].Value.ToString().Trim(),
                                        InsertDate = DateTime.UtcNow.Date.ToString(),
                                        IsActive = Convert.ToByte(1),
                                        SactionUID = Guid.NewGuid()
                                    });
                                }
                                if (counterRecord == 10)
                                {
                                    sourceSanction.Sanctions = model.Sanctions;
                                    _context.Update(sourceSanction);
                                    await _context.SaveChangesAsync();
                                    counterRecord = 0;
                                }
                                else
                                    counterRecord++;
                            }
                        }
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!SourceSanctionExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<ActionResult> Import(IFormFile formFile, CancellationToken cancellationToken)
        {


            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.ErrorMessage = "Not Support file extension";
                return View("Index");
            }

            var list = new List<Sanction>();

            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream, cancellationToken);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        list.Add(new Sanction
                        {
                            InsertDate = worksheet.Cells[row, 1].Value.ToString().Trim(),
                            AdditionalInformation = worksheet.Cells[row, 2].Value.ToString().Trim()
                        });
                    }
                }
            }

            // add list to db ..  
            // here just read and return  

            return RedirectToAction("Index");
        }

        private DataTable GetDataTableFromExcel(String Path)
        {
            try
            {
                XSSFWorkbook wb;
                XSSFSheet sh;
                String Sheet_name;
                using (var fs = new FileStream(Path, FileMode.Open, FileAccess.Read))
                {
                    wb = new XSSFWorkbook(fs);
                    Sheet_name = wb.GetSheetAt(0).SheetName;  //get first sheet name
                }
                DataTable DT = new DataTable();
                DT.Rows.Clear();
                DT.Columns.Clear();
                // get sheet
                sh = (XSSFSheet)wb.GetSheet(Sheet_name);
                int i = 0;
                while (sh.GetRow(i) != null)
                {
                    // add neccessary columns
                    if (DT.Columns.Count < sh.GetRow(i).Cells.Count)
                    {
                        for (int j = 0; j < sh.GetRow(i).Cells.Count; j++)
                        {
                            DT.Columns.Add(sh.GetRow(i).Cells[j].ToString(), typeof(string));
                        }
                    }
                    // add row
                    DT.Rows.Add();
                    var t = 1;
                    // write row value
                    for (int j = 0; j < sh.GetRow(i).Cells.Count; j++)
                    {
                        try
                        {
                            if (i > (i + 1))
                            {
                                var cell = sh.GetRow(i + 1).GetCell(j) != null ? sh.GetRow(i + 1).GetCell(j).ToString() : " ";
                                DT.Rows[i][j] = cell; //sh.GetRow(i).GetCell(j);
                            }
                        }
                        catch (Exception ex2)
                        {

                            throw;
                        }

                        t++;
                    }
                    i++;
                }
                return DT;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private bool SourceSanctionExists(int id)
        {
            return _context.SourceSanctions.Any(e => e.Id == id);
        }
        private bool CheckHasRecord(int id, string refrenceId)
        {
            //var findSource = _context.Sanctions.FirstOrDefault(c => c.SourceSaction_Id == id && c.RefrenceId.Equals(refrenceId));
            //if (findSource == null)
            //    return false;
            return _context.Sanctions.Any(c => c.SourceSaction_Id == id && c.RefrenceId.Equals(refrenceId));
        }
    }
}