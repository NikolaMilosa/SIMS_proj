using Model;
using Repository.PeriodPersistance;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ZdravoHospital.GUI.Secretary.DTOs;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using ZdravoHospital.GUI.Secretary.ViewModels;

namespace ZdravoHospital.GUI.Secretary.Service
{
    public class WeeklyReportService
    {
        private IPeriodRepository _periodRepository;
        private static string path = @"..\..\..\EXPORTPDF\Secretary\";
        public WeeklyReportService()
        {
            _periodRepository = new PeriodRepository();
        }

        public List<WeeklyReportDTO> GetDesiredPeriods(DateTime selectedDate)
        {
            List<Period> allPeriods = _periodRepository.GetValues();
            DateTime endDate = selectedDate.AddDays(7);
            List<WeeklyReportDTO> dtos = new List<WeeklyReportDTO>();
            foreach(var p in allPeriods)
            {
                if(p.StartTime.Date >= selectedDate.Date && p.StartTime.Date <= endDate.Date)
                {
                    dtos.Add(getWeeklyReportDTOFromPeriod(p));
                }
            }
            return dtos;
        }

        private WeeklyReportDTO getWeeklyReportDTOFromPeriod(Period period)
        {
            WeeklyReportDTO dto = new WeeklyReportDTO(period.StartTime, period.Duration, period.PatientUsername, period.DoctorUsername, period.PeriodType, period.RoomId);
            return dto;
        }

        public void GeneratePDF(DateTime start, List<WeeklyReportDTO> weeklyReport)
        {
            Document document = new Document(PageSize.A4);

            var currentPath = path + "weekly-report" + "-"
                              + Guid.NewGuid() + ".pdf";

            FileStream outputStream = new FileStream(currentPath, FileMode.Create);

            using (outputStream)
            {
                PdfWriter.GetInstance(document, outputStream);
                Paragraph lineSeparator = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));


                document.Open();

                Paragraph title = new Paragraph("REPORT\n\n");
                title.Font.Size = 18;
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                Paragraph header = new Paragraph("Week from: " + start.ToShortDateString() + " to " + start.AddDays(7).ToShortDateString() + "\n");
                header.Font.Size = 16;
                document.Add(header);
                document.Add(lineSeparator);

                Paragraph periodsHeader = new Paragraph("Scheduled periods in selected time\n");
                periodsHeader.Font.Size = 16;
                document.Add(periodsHeader);
                document.Add(lineSeparator);

                Paragraph reportContent = new Paragraph();
                reportContent.Font.Size = 14;
                int count = 0;
                foreach (var report in weeklyReport)
                {
                    reportContent.Add(++count + "." + report.Format() + "\n\n");
                }

                document.Add(reportContent);

                document.Close();
                SecretaryWindowVM.CustomMessageBox = new CustomMessageBox("Success", "Exported successfully!");
                SecretaryWindowVM.CustomMessageBox.Owner = SecretaryWindowVM.SecretaryWindow;
                SecretaryWindowVM.CustomMessageBox.Show();
            }
        }
    }
}
