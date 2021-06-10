using iTextSharp.text;
using iTextSharp.text.pdf;
using Model;
using Repository.DoctorPersistance;
using Repository.PatientPersistance;
using System.IO;

namespace ZdravoHospital.GUI.DoctorUI.Services
{
    public class PeriodReportService
    {
        private PatientRepository _patientRepository;
        private DoctorRepository _doctorRepository;
        private const float _headerFontSize = 18;
        private const float _sectionHeaderFontSize = 16;
        private const float _sectionParagraphFontSize = 14;
        private Document _document;
        private Period _period;

        public PeriodReportService()
        {
            _patientRepository = new PatientRepository();
            _doctorRepository = new DoctorRepository();
        }

        public void GeneratePeriodReport(Period period)
        {
            string path = GenerateReportFilename(period);
            _document = new Document(PageSize.A4);
            _period = period;

            using (var outputStream = new FileStream(path, FileMode.Create))
            {
                PdfWriter.GetInstance(_document, outputStream);
                
                _document.Open();
                WriteReport();
                _document.Close();
            }
        }

        public string GenerateReportFilename(Period period)
        {
            return period.DoctorUsername + "$" + period.PatientUsername + "$" +
                period.StartTime.ToString("dd_MM_yyyy") + "$" + period.StartTime.ToString("HH_mm") + ".pdf";
        }

        private void WriteReport()
        {
            WriteReportHeader();
            _document.Add(new Paragraph("\n\n"));
            WritePeriodDetailsSection();
            _document.Add(new Paragraph("\n\n"));
            WritePrescriptionSection();
            _document.Add(new Paragraph("\n\n"));
            WriteTreatmentSection();
        }

        private void WriteReportHeader()
        {
            string text = GenerateReportHeaderText(_period);
            Paragraph header = new Paragraph(text);
            header.Font.Size = _headerFontSize;
            header.Alignment = Element.ALIGN_CENTER;
            _document.Add(header);

            Paragraph lineSeparator =
                new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            _document.Add(lineSeparator);
        }

        private string GenerateReportHeaderText(Period _period)
        {
            Patient patient = _patientRepository.GetById(_period.PatientUsername);
            Doctor doctor = _doctorRepository.GetById(_period.DoctorUsername);

            return _period.PeriodType.ToString() + " - " + _period.StartTime.ToString("dd.MM.yyyy.") + " " +
                _period.StartTime.ToString("HH:mm") + " - Room " + _period.RoomId + "\n" +
                "Patient: " + patient.Name + " " + patient.Surname + "\n" +
                "Doctor: " + doctor.Name + " " + doctor.Surname + " (" + doctor.SpecialistType.SpecializationName + ")";
        }

        private void WritePeriodDetailsSection()
        {
            WritePeriodDetailsSectionHeader();
            WritePeriodDetailsSectionParagraph();
        }

        private void WritePeriodDetailsSectionHeader()
        {
            string text;

            if (_period.PeriodType == PeriodType.APPOINTMENT)
                text = "Anamnesis:\n";
            else
                text = "Operation details:\n";

            Paragraph _periodDetailsHeader = new Paragraph(text);
            _periodDetailsHeader.Font.Size = _sectionHeaderFontSize;
            _document.Add(_periodDetailsHeader);
        }

        private void WritePeriodDetailsSectionParagraph()
        {
            string text;

            if (_period.Details == null || _period.Details.Equals(""))
            {
                if (_period.PeriodType == PeriodType.APPOINTMENT)
                    text = "No anamnesis was entered during this appointment.\n";
                else
                    text = "No operation details were entered during this operation.\n";
            }
            else
                text = _period.Details;

            Paragraph _periodDetailsContent = new Paragraph(text);
            _periodDetailsContent.Font.Size = _sectionParagraphFontSize;
            _periodDetailsContent.IndentationLeft = 15;
            _document.Add(_periodDetailsContent);
        }

        private void WritePrescriptionSection()
        {
            WritePrescriptionSectionHeader();
            WritePrescriptionSectionParagraph();
        }

        private void WritePrescriptionSectionHeader()
        {
            string text = "Prescription:\n";
            Paragraph prescriptionHeader = new Paragraph(text);
            prescriptionHeader.Font.Size = _sectionHeaderFontSize;
            _document.Add(prescriptionHeader);
        }

        private void WritePrescriptionSectionParagraph()
        {
            string text;

            if (_period.Prescription == null || _period.Prescription.TherapyList.Count == 0)
            {
                text = "No therapies prescribed.";
                Paragraph prescriptionContent = new Paragraph(text);
                prescriptionContent.Font.Size = _sectionParagraphFontSize;
                prescriptionContent.IndentationLeft = 15;
                _document.Add(prescriptionContent);
            }
            else
            {
                for (int i = 0; i < _period.Prescription.TherapyList.Count; i++)
                {
                    Therapy therapy = _period.Prescription.TherapyList[i];
                    text = (i + 1).ToString() + ") Medicine: " + therapy.Medicine.MedicineName + "\n";

                    Paragraph medicineNameContent = new Paragraph(text);
                    medicineNameContent.Font.Size = _sectionParagraphFontSize;
                    medicineNameContent.IndentationLeft = 15;
                    _document.Add(medicineNameContent);

                    text = "- Start time: " + therapy.StartHours.ToString("HH:mm") + "\n" +
                        "- Times per day: " + therapy.TimesPerDay + "\n" +
                        "- Pause in days: " + therapy.PauseInDays + "\n" +
                        "- End date: " + therapy.EndDate.ToString("dd.MM.yyyy.") + "\n" +
                        "- Instructions: " + therapy.Instructions;
                    Paragraph therapyContent = new Paragraph(text);
                    therapyContent.Font.Size = _sectionParagraphFontSize;
                    therapyContent.IndentationLeft = 50;
                    _document.Add(therapyContent);
                }
            }
        }

        private void WriteTreatmentSection()
        {
            WriteTreatmentSectionHeader();
            WriteTreatmentSectionParagraph();
        }

        private void WriteTreatmentSectionHeader()
        {
            string text = "Hospital treatment:\n";
            Paragraph treatmentHeader = new Paragraph(text);
            treatmentHeader.Font.Size = _sectionHeaderFontSize;
            _document.Add(treatmentHeader);
        }

        private void WriteTreatmentSectionParagraph()
        {
            string text;

            if (_period.Treatment == null)
                text = "Patient wasn't sent to hospital treatment.";
            else
            {
                text = "- Start date: " + _period.Treatment.StartDate.ToString("dd.MM.yyyy.") + "\n" +
                    "- Duration: " + _period.Treatment.Duration + " day(s)\n" +
                    "- Room: " + _period.RoomId;
            }

            Paragraph treatmentParagraph = new Paragraph(text);
            treatmentParagraph.Font.Size = _sectionParagraphFontSize;
            treatmentParagraph.IndentationLeft = 15;
            _document.Add(treatmentParagraph);
        }
    }
}
