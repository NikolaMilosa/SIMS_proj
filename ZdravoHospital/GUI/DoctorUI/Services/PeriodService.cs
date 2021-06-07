using iTextSharp.text;
using iTextSharp.text.pdf;
using Model;
using Repository.DoctorPersistance;
using Repository.PatientPersistance;
using Repository.PeriodPersistance;
using Repository.ReferralPersistance;
using System.Collections.Generic;
using System.IO;
using ZdravoHospital.GUI.DoctorUI.DTOs;
using ZdravoHospital.GUI.DoctorUI.Validations;

namespace ZdravoHospital.GUI.DoctorUI.Services
{
    public class PeriodService
    {
        private PeriodRepository _periodRepository;
        private ReferralRepository _referralRepository;
        private PeriodValidation _periodValidation;
        private DoctorRepository _doctorRepository;
        private PatientRepository _patientRepository;

        public PeriodService()
        {
            _periodRepository = new PeriodRepository();
            _referralRepository = new ReferralRepository();
            _periodValidation = new PeriodValidation();
            _doctorRepository = new DoctorRepository();
            _patientRepository = new PatientRepository();
        }

        public List<Period> GetPeriods()
        {
            return _periodRepository.GetValues();
        }

        public void CreateNewPeriod(Period period, Referral referral)
        {
            _periodValidation.ValidatePeriod(period);
            _periodRepository.Create(period);

            if (referral != null)
            {
                referral.PeriodId = period.PeriodId;
                referral.IsUsed = true;
                _referralRepository.Update(referral);

                period.ParentReferralId = referral.ReferralId;
                _periodRepository.Update(period);
            }
        }

        public void CancelPeriod(int periodId)
        {
            int referralId = _periodRepository.GetById(periodId).ParentReferralId;
            _periodRepository.DeleteById(periodId);

            if (referralId != -1)
            {
                Referral referral =_referralRepository.GetById(referralId);
                referral.PeriodId = -1;
                referral.IsUsed = false;
                _referralRepository.Update(referral);
            }
        }

        public void UpdatePeriod(Period period)
        {
            _periodValidation.ValidatePeriod(period, true);
            _periodRepository.Update(period);
        }

        public void UpdatePeriodWithoutValidation(Period period)
        {
            _periodRepository.Update(period);
        }

        public Period GetPeriod(int periodId)
        {
            return _periodRepository.GetById(periodId);
        }

        public List<PatientInfoPeriodDisplayDTO> GetPatientInfoPeriodDisplayDTOs(string patientUsername)
        {
            var dtos = new List<PatientInfoPeriodDisplayDTO>();

            foreach (Period period in _periodRepository.GetValues())
                if (period.PatientUsername.Equals(patientUsername))
                    dtos.Add(new PatientInfoPeriodDisplayDTO(period,
                        _doctorRepository.GetById(period.DoctorUsername)));

            return dtos;
        }

        public void GeneratePeriodReport(Period period)
        {
            string path = GeneratePeriodReportFilename(period);

            Document document = new Document(PageSize.A4);

            using (var outputStream = new FileStream(path, FileMode.Create))
            {
                PdfWriter.GetInstance(document, outputStream);
                Paragraph lineSeparator = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

                document.Open();

                Patient patient = _patientRepository.GetById(period.PatientUsername);
                Doctor doctor = _doctorRepository.GetById(period.DoctorUsername);

                string text = period.PeriodType.ToString() + " - " + period.StartTime.ToString("dd.MM.yyyy.") + " " +
                    period.StartTime.ToString("HH:mm") + " - Room " + period.RoomId + "\n" +
                    "Patient: " + patient.Name + " " + patient.Surname + "\n" +
                    "Doctor: " + doctor.Name + " " + doctor.Surname + " (" + doctor.SpecialistType.SpecializationName + ")";
                Paragraph title = new Paragraph(text);
                title.Font.Size = 18;
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);
                document.Add(lineSeparator);

                document.Add(new Paragraph("\n\n"));

                if (period.PeriodType == PeriodType.APPOINTMENT)
                    text = "Anamnesis:\n";
                else
                    text = "Operation details:\n";
                Paragraph anamnesisHeader = new Paragraph(text);
                anamnesisHeader.Font.Size = 16;
                document.Add(anamnesisHeader);

                if (period.Details == null || period.Details.Equals(""))
                {
                    if (period.PeriodType == PeriodType.APPOINTMENT)
                        text = "No anamnesis was entered during this appointment.\n";
                    else
                        text = "No operation details were entered during this operation.\n";
                }
                else
                    text = period.Details;
                Paragraph anamnesisContent = new Paragraph(text);
                anamnesisContent.Font.Size = 14;
                anamnesisContent.IndentationLeft = 15;
                document.Add(anamnesisContent);

                document.Add(new Paragraph("\n\n"));

                text = "Prescription:\n";
                Paragraph prescriptionHeader = new Paragraph(text);
                prescriptionHeader.Font.Size = 16;
                document.Add(prescriptionHeader);

                if (period.Prescription == null || period.Prescription.TherapyList.Count == 0)
                {
                    text = "No therapies prescribed.";
                    Paragraph prescriptionContent = new Paragraph(text);
                    prescriptionContent.Font.Size = 14;
                    prescriptionContent.IndentationLeft = 15;
                    document.Add(prescriptionContent);
                }
                else
                {
                    for (int i = 0; i < period.Prescription.TherapyList.Count; i++)
                    {
                        Therapy therapy = period.Prescription.TherapyList[i];
                        text = (i + 1).ToString() + ") Medicine: " + therapy.Medicine.MedicineName + "\n";

                        Paragraph medicineNameContent = new Paragraph(text);
                        medicineNameContent.Font.Size = 14;
                        medicineNameContent.IndentationLeft = 15;
                        document.Add(medicineNameContent);

                        text = "- Start time: " + therapy.StartHours.ToString("HH:mm") + "\n" +
                            "- Times per day: " + therapy.TimesPerDay + "\n" +
                            "- Pause in days: " + therapy.PauseInDays + "\n" +
                            "- End date: " + therapy.EndDate.ToString("dd.MM.yyyy.") + "\n" +
                            "- Instructions: " + therapy.Instructions;
                        Paragraph therapyContent = new Paragraph(text);
                        therapyContent.Font.Size = 14;
                        therapyContent.IndentationLeft = 50;
                        document.Add(therapyContent);
                    }
                }

                document.Add(new Paragraph("\n\n"));

                text = "Hospital treatment:\n";
                Paragraph treatmentHeader = new Paragraph(text);
                treatmentHeader.Font.Size = 16;
                document.Add(treatmentHeader);

                if (period.Treatment == null)
                    text = "Patient wasn't sent to hospital treatment.";
                else
                {
                    text = "- Start date: " + period.Treatment.StartDate.ToString("dd.MM.yyyy.") + "\n" +
                        "- Duration: " + period.Treatment.Duration + " day(s)\n" +
                        "- Room: " + period.RoomId;
                }

                Paragraph treatmentContent = new Paragraph(text);
                treatmentContent.Font.Size = 14;
                treatmentContent.IndentationLeft = 15;
                document.Add(treatmentContent);

                document.Close();
            }
        }

        public string GeneratePeriodReportFilename(Period period)
        {
            return period.DoctorUsername + "$" + period.PatientUsername + "$" +
                period.StartTime.ToString("dd_MM_yyyy") + "$" + period.StartTime.ToString("HH_mm") + ".pdf";
        }
    }
}
