using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.DivideintoClasses.Excels
{
    public class ExportToExcelHelper
    {
        /// <summary>
        /// 生成模板
        /// </summary>
        /// <param name="keyRequest"></param>
        public static async Task ExportToExcelWithHeaderStyle()
        {
            XSSFWorkbook workbook = new XSSFWorkbook();
            XSSFSheet sheet = (XSSFSheet)workbook.CreateSheet("Sheet1");

            // 创建表头行
            var headerRowCategory = sheet.CreateRow(0);
            var headerRowClassify = sheet.CreateRow(1);
            //创建示例行
            var rowExample = sheet.CreateRow(2);

            // 设置类别表头样式
            XSSFCellStyle headerCategoryStyle = (XSSFCellStyle)workbook.CreateCellStyle();
            headerCategoryStyle.FillForegroundColor = IndexedColors.LightBlue.Index;
            headerCategoryStyle.FillPattern = FillPattern.SolidForeground;
            headerCategoryStyle.Alignment = HorizontalAlignment.Center;
            headerCategoryStyle.VerticalAlignment = VerticalAlignment.Center;
            XSSFFont fontCategory = (XSSFFont)workbook.CreateFont();
            fontCategory.FontHeightInPoints = 12;
            fontCategory.IsBold = true;
            headerCategoryStyle.SetFont(fontCategory);

            // 设置分类表头样式
            XSSFCellStyle headerClassifyStyle = (XSSFCellStyle)workbook.CreateCellStyle();
            headerClassifyStyle.FillForegroundColor = IndexedColors.LightBlue.Index;
            headerClassifyStyle.FillPattern = FillPattern.SolidForeground;
            headerClassifyStyle.Alignment = HorizontalAlignment.Center;
            headerClassifyStyle.VerticalAlignment = VerticalAlignment.Center;
            XSSFFont fontClassify = (XSSFFont)workbook.CreateFont();
            fontClassify.FontHeightInPoints = 12;
            fontClassify.IsBold = true;
            headerClassifyStyle.SetFont(fontClassify);

            //设置内容样式
            XSSFCellStyle headerContentStyle = (XSSFCellStyle)workbook.CreateCellStyle();
            headerContentStyle.FillForegroundColor = IndexedColors.Black.Index;
            headerContentStyle.FillPattern = FillPattern.SolidForeground;
            headerContentStyle.Alignment = HorizontalAlignment.Center;
            headerContentStyle.VerticalAlignment = VerticalAlignment.Center;
            XSSFFont fontContent = (XSSFFont)workbook.CreateFont();
            fontContent.FontHeightInPoints = 10;
            fontContent.IsBold = false;
            headerContentStyle.SetFont(fontContent);

            // 所属年级
            var cellGrade = headerRowCategory.CreateCell(0, CellType.String);
            cellGrade.SetCellValue("所属年级");
            cellGrade.CellStyle = headerCategoryStyle;
            //填写示例值
            var cellExample = rowExample.CreateCell(0, CellType.String);
            cellExample.SetCellValue("高中一年级");

            // 班级
            var cellClass = headerRowCategory.CreateCell(1, CellType.String);
            cellClass.SetCellValue("班级");
            cellClass.CellStyle = headerCategoryStyle;
            //填写示例值
            cellExample = rowExample.CreateCell(1, CellType.String);
            cellExample.SetCellValue("高2024级1班");

            // 合并单元格
            headerRowClassify.CreateCell(0, CellType.String).SetCellValue("");
            headerRowClassify.CreateCell(1, CellType.String).SetCellValue("");
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 0, 0)); // 合并年级列头
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 1, 1)); // 合并班级列头

            double columnWidth = 15 * 256;
            sheet.SetColumnWidth(0, columnWidth);
            sheet.SetColumnWidth(1, columnWidth);
            //查询所有学科
            var classHourListGroupBy = new List<string>() { "语文", "数学", "外语", "物理" };
            int index = 2;
            foreach (var item in classHourListGroupBy)
            {
                var cellSubject = headerRowCategory.CreateCell(index, CellType.String);
                cellSubject.SetCellValue(item);
                cellSubject.CellStyle = headerClassifyStyle;
                var cellC1 = headerRowClassify.CreateCell(index, CellType.String);
                cellC1.SetCellValue("课程周正课时");
                cellC1.CellStyle = headerClassifyStyle;
                rowExample.CreateCell(index, CellType.Numeric).SetCellValue(3);
                sheet.SetColumnWidth(index, columnWidth);

                index += 1;
                headerRowCategory.CreateCell(index).SetCellValue("");
                var cellC2 = headerRowClassify.CreateCell(index, CellType.String);
                cellC2.SetCellValue("早自习课时");
                cellC2.CellStyle = headerClassifyStyle;
                rowExample.CreateCell(index, CellType.Numeric).SetCellValue(0);
                sheet.SetColumnWidth(index, columnWidth);

                index += 1;
                headerRowCategory.CreateCell(index).SetCellValue("");
                var cellC3 = headerRowClassify.CreateCell(index, CellType.String);
                cellC3.SetCellValue("晚自习课时");
                cellC3.CellStyle = headerClassifyStyle;
                rowExample.CreateCell(index, CellType.Numeric).SetCellValue(0);
                sheet.SetColumnWidth(index, columnWidth);

                index += 1;
                //headerRowCategory.CreateCell(index).SetCellValue("");
                //var cellC4 = headerRowClassify.CreateCell(index, CellType.String);
                //cellC4.SetCellValue("任教老师");
                //cellC4.CellStyle = headerClassifyStyle;
                //rowExample.CreateCell(index, CellType.String).SetCellValue("张三");
                //sheet.SetColumnWidth(index, columnWidth);

                //index += 1;
                //headerRowCategory.CreateCell(index).SetCellValue("");
                //var cellC5 = headerRowClassify.CreateCell(index, CellType.String);
                //cellC5.SetCellValue("副教师");
                //cellC5.CellStyle = headerClassifyStyle;
                //rowExample.CreateCell(index, CellType.String).SetCellValue("李四");
                //sheet.SetColumnWidth(index, columnWidth);
                //index += 1;
                //合并科目类别
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, index - 3, index - 1)); // 合并年级列头

            }
            // 写入到文件
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Download\\output.xlsx");
            FileInfo info = new FileInfo(filePath);
            if (info.Directory != null && !Directory.Exists(info.Directory.FullName))
            {
                Directory.CreateDirectory(info.Directory.FullName);
            }
            using (FileStream file = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(file);
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// 导入Excel到模型
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static async Task<List<ImportExcelModel>> ImportExcelToModel(string filePath)
        {
            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Download\\output.xlsx");
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }
            string? ext = Path.GetExtension(filePath)?.ToLower();
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook;
                switch (ext)
                {
                    case ".xls":
                        workbook = new HSSFWorkbook(fileStream);
                        break;
                    case ".xlsx":
                        workbook = new XSSFWorkbook(fileStream);
                        break;
                    default:
                        throw new InvalidOperationException("导入文件格式不正确");
                }
                List<ImportExcelModel> importExcelModels = await ImportExcelSheetToModel(workbook);
                return importExcelModels;
            }
        }

        /// <summary>
        /// 解析导入数据
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        private static async  Task<List<ImportExcelModel>> ImportExcelSheetToModel(IWorkbook workbook)
        {
            List<ImportExcelModel> models = new List<ImportExcelModel>();
            XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(0);
            List<string> Courses = new List<string>();
            int rows = sheet.LastRowNum;
            for (int i = 0; i <= rows; i++)
            {
                IRow theRow = sheet.GetRow(i);
                switch (i)
                {
                    case 0: //为表头类别行，记录了年级 班级  课程1  课程2
                        for (int j = 2; j < theRow.Cells.Count; j++)
                        { 
                            string? theCourse = theRow.GetCell(j).StringCellValue?.Trim();
                            if (!string.IsNullOrWhiteSpace(theCourse) && !Courses.Contains(theCourse))
                            {
                                Courses.Add(theCourse);
                            }
                        }
                        break;
                    case 1: //为表头分类行，记录了年级 班级  课程(周正课时 早自习课时 晚自习课时)
                        break;
                    default: //正文
                        ImportExcelModel? model = await ImportExcelSheetRowToModel(Courses, theRow);
                        if (model != null)
                        {
                            models.Add(model);
                        }
                        break;
                }
            }
            return models;
        }

        /// <summary>
        /// 将行数据解析到模型
        /// </summary>
        /// <param name="theRow"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static async Task<ImportExcelModel?> ImportExcelSheetRowToModel(List<string> courses, IRow theRow)
        {
            string? gradeName = theRow.GetCell(0).StringCellValue?.Trim();
            string? className = theRow.GetCell(1).StringCellValue?.Trim();
            if (string.IsNullOrWhiteSpace(gradeName) || string.IsNullOrWhiteSpace(className))
            {
                return null;
            }
            ImportExcelModel model = new ImportExcelModel();
            model.GradeName = gradeName;
            model.ClassName = className;
            model.Courses = new List<CourseClassHoursInfo>();

            int c = 2;
            foreach (var course in courses)
            {
                model.CourseName = course;
                CourseClassHoursInfo info = new CourseClassHoursInfo();
                for (int i = 1; i <= 3; i++)
                {
                    ICell theCell = theRow.GetCell(c++);
                    switch (theCell.CellType)
                    {
                        case CellType.Numeric:
                            switch (i)
                            {
                                case 1:
                                    info.Regular = theCell.NumericCellValue;
                                    break;
                                case 2:
                                    info.Morning = theCell.NumericCellValue;
                                    break;
                                case 3:
                                    info.Evening = theCell.NumericCellValue;
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case CellType.String:
                            string? svalue = theCell.StringCellValue?.Trim();
                            if (!string.IsNullOrWhiteSpace(svalue) && float.TryParse(svalue, out float fvalue))
                            {
                                switch (i)
                                {
                                    case 1:
                                        info.Regular = fvalue;
                                        break;
                                    case 2:
                                        info.Regular = fvalue;
                                        break;
                                    case 3:
                                        info.Regular = fvalue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                    }
                }
                model.Courses.Add(info);
            }
            await Task.CompletedTask;
            return model;
        }
    }
}
