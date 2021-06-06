using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout.Element;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using Model;
using Repository.PatientPersistance;
using Repository.PeriodPersistance;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using Paragraph = iTextSharp.text.Paragraph;

namespace ZdravoHospital.Services.Manager
{
    public class DoctorReportService
    {
        #region Fields

        private IPeriodRepository _periodRepository;
        private IPatientRepository _patientRepository;

        private static string path = @"..\..\..\EXPORTPDF\Manager\";

        #endregion

        public DoctorReportService(InjectorDTO injector)
        {
            _periodRepository = injector.PeriodRepository;
            _patientRepository = injector.PatientRepository;
        }

        public ObservableCollection<DoctorReportDTO> CreateReport(Doctor doctor, DateTime start, DateTime end)
        {
            var report = new List<DoctorReportDTO>();

            var periods = _periodRepository.GetValues();

            foreach (var period in periods)
            {
                if (period.DoctorUsername.Equals(doctor.Username) &&
                    ((period.StartTime < start && period.StartTime.AddMinutes(period.Duration) > start) ||
                     (period.StartTime > start && period.StartTime.AddMinutes(period.Duration) < end) ||
                     (period.StartTime < end && period.StartTime.AddMinutes(period.Duration) > end)))
                {
                    report.Add(new DoctorReportDTO()
                    {
                        Date = period.StartTime,
                        PatientName = _patientRepository.GetById(period.PatientUsername).Name,
                        PatientUsername = period.PatientUsername,
                        RoomNumber = period.RoomId,
                        Type = (period.PeriodType == PeriodType.APPOINTMENT) ? "APPOINTMENT" : "OPERATION"
                    });
                }
            }
            
            return new ObservableCollection<DoctorReportDTO>(report);
        }

        public void GeneratePDF(Doctor doctor, DateTime start, DateTime end, List<DoctorReportDTO> doctorReport)
        {
            Document document = new Document(PageSize.A4);

            var currentPath = path + doctor.Name + "-" 
                              + start.Day + "_" + start.Month + "_" + start.Year + "-" 
                              + end.Day + "_" + end.Month + "_" + end.Year + "-" 
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

                Paragraph header = new Paragraph("Header\n");
                header.Font.Size = 16;
                document.Add(header);
                document.Add(lineSeparator);

                Paragraph headerContent = new Paragraph();
                headerContent.Font.Size = 14;
                headerContent.Add(doctor.Format() + "\n\n\n");
                document.Add(headerContent);

                Paragraph activitiesHeader = new Paragraph("Activities\n");
                activitiesHeader.Font.Size = 16;
                document.Add(activitiesHeader);
                document.Add(lineSeparator);

                Paragraph reportContent = new Paragraph();
                reportContent.Font.Size = 14;
                int count = 0;
                foreach (var docrep in doctorReport)
                {
                    reportContent.Add(++count + "." + docrep.Format() + "\n\n");
                }

                document.Add(reportContent);

                document.Close();
            }
        }
    }
}
