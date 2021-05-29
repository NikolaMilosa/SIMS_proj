using Model;
using Repository.DoctorPersistance;
using Repository.PatientPersistance;
using Repository.PeriodPersistance;
using Repository.RoomPersistance;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ZdravoHospital.GUI.Secretary.DTOs;

namespace ZdravoHospital.GUI.Secretary.Service
{
    public class PeriodsService
    {
        private IDoctorRepository _doctorRepository;
        private IPatientRepository _patientRepository;
        private IPeriodRepository _periodRepository;
        private IRoomRepository _roomRepository;

        public PeriodsService()
        {
            _doctorRepository = new DoctorRepository();
            _patientRepository = new PatientRepository();
            _periodRepository = new PeriodRepository();
            _roomRepository = new RoomRepository();
        }

        public List<Patient> GetPatients()
        {
            return _patientRepository.GetValues();
        }
        public List<Doctor> GetDoctors()
        {
            return _doctorRepository.GetValues();
        }
        public List<Period> GetPeriods()
        {
            return _periodRepository.GetValues();
        }
        public List<Room> GetRooms()
        {
            return _roomRepository.GetValues();
        }

        public void ProcessPeriodDeletion(int periodId)
        {
            _periodRepository.DeleteById(periodId);
        }

        public bool ProcessPeriodCreation(PeriodDTO periodDTO)
        {
            Period period = createPeriodFromDto(periodDTO);
            PeriodAvailabilityDTO periodAvailableDTO = new PeriodAvailabilityDTO(period.PeriodId, periodDTO.PeriodAvailable);
            checkPeriodAvailability(period, periodAvailableDTO);

            if (isPeriodAvailable(periodAvailableDTO))
            {
                _periodRepository.Create(period);
                return true;
            }
            else
            {
                giveAvailabilityFeedbackMessage(periodAvailableDTO);
                return false;
            }
        }

        private void giveAvailabilityFeedbackMessage(PeriodAvailabilityDTO periodAvailableDTO)
        {
            if (periodAvailableDTO.PeriodAvailable == PeriodAvailability.DOCTOR_UNAVAILABLE)
                MessageBox.Show("Selected doctor is unavailable in selected period.", "Doctor unavailable");
            else if (periodAvailableDTO.PeriodAvailable == PeriodAvailability.PATIENT_UNAVAILABLE)
                MessageBox.Show("Selected patient is unavailable in selected period.", "Patient unavailable");
            else if (periodAvailableDTO.PeriodAvailable == PeriodAvailability.ROOM_UNAVAILABLE)
                MessageBox.Show("Selected room is unavailable in selected period.", "Room unavailable");
            else
                MessageBox.Show("Selected time is not acceptable.", "Time unacceptable");
        }


        private Period createPeriodFromDto(PeriodDTO periodDTO)
        {
            string[] hoursAndMinutes = periodDTO.Time.Split(":");
            DateTime periodStartTime = new DateTime(periodDTO.Date.Year, periodDTO.Date.Month, periodDTO.Date.Day, Int32.Parse(hoursAndMinutes[0]), Int32.Parse(hoursAndMinutes[1]), 0);
            Period newPeriod = new Period(periodStartTime, Int32.Parse(periodDTO.Duration), (PeriodType)periodDTO.PeriodTypeIndex, periodDTO.Patient.Username, periodDTO.Doctor.Username, periodDTO.Room.Id);
            return newPeriod;
        }

        private void checkPeriodAvailability(Period period, PeriodAvailabilityDTO periodAvailableDTO)
        {
            setInitialPeriodAvailability(periodAvailableDTO);
            checkTimeAvailabilityForPeriod(period, periodAvailableDTO);
            checkDoctorAvailabilityForPeriod(period, periodAvailableDTO);
            checkPatientAvailabilityForPeriod(period, periodAvailableDTO);
            checkRoomAvailabilityForPeriod(period, periodAvailableDTO);
        }

        private void setInitialPeriodAvailability(PeriodAvailabilityDTO periodAvailableDTO)
        {
            periodAvailableDTO.PeriodAvailable = PeriodAvailability.AVAILABLE;
        }

        private void checkTimeAvailabilityForPeriod(Period period, PeriodAvailabilityDTO periodAvailableDTO)
        {
            if (period.StartTime < DateTime.Now.AddMinutes(PeriodDTO.MIN_MINUTES_DIFFERENCE))
            {
                periodAvailableDTO.PeriodAvailable = PeriodAvailability.TIME_UNACCEPTABLE;
            }
        }

        private void checkDoctorAvailabilityForPeriod(Period period, PeriodAvailabilityDTO periodAvailableDTO)
        {
            List<Period> periods = GetPeriods();
            foreach (Period existingPeriod in periods)
            {
                if (periodsHaveSameDoctors(period, existingPeriod) && periodsOverlap(period, existingPeriod))
                {
                    periodAvailableDTO.PeriodAvailable = PeriodAvailability.DOCTOR_UNAVAILABLE;
                }
            }
        }
        private void checkPatientAvailabilityForPeriod(Period period, PeriodAvailabilityDTO periodAvailableDTO)
        {
            List<Period> periods = GetPeriods();
            foreach (Period existingPeriod in periods)
            {
                if (periodsHaveSamePatients(period, existingPeriod) && periodsOverlap(period, existingPeriod))
                {
                    periodAvailableDTO.PeriodAvailable = PeriodAvailability.PATIENT_UNAVAILABLE;
                }
            }
        }

        private void checkRoomAvailabilityForPeriod(Period period, PeriodAvailabilityDTO periodAvailableDTO)
        {
            List<Period> periods = GetPeriods();
            foreach (Period existingPeriod in periods)
            {
                if (periodsHaveSameRooms(period, existingPeriod) && periodsOverlap(period, existingPeriod))
                {
                    periodAvailableDTO.PeriodAvailable = PeriodAvailability.ROOM_UNAVAILABLE;
                }
            }
        }

        private bool periodsOverlap(Period newPeriod, Period existingPeriod)
        {
            DateTime existingPeriodEndTime = existingPeriod.StartTime.AddMinutes(existingPeriod.Duration);
            DateTime newPeriodEndtime = newPeriod.StartTime.AddMinutes(newPeriod.Duration);
            if (newPeriod.StartTime < existingPeriodEndTime && newPeriodEndtime > existingPeriod.StartTime)
            {
                return true;
            }
            return false;
        }

        private bool periodsHaveSameDoctors(Period newPeriod, Period existingPeriod)
        {
            if (newPeriod.DoctorUsername == existingPeriod.DoctorUsername)
            {
                return true;
            }
            return false;
        }

        private bool periodsHaveSamePatients(Period newPeriod, Period existingPeriod)
        {
            if (newPeriod.PatientUsername == existingPeriod.PatientUsername)
            {
                return true;
            }
            return false;
        }

        private bool periodsHaveSameRooms(Period newPeriod, Period existingPeriod)
        {
            if (newPeriod.RoomId == existingPeriod.RoomId)
            {
                return true;
            }
            return false;
        }
        private bool isPeriodAvailable(PeriodAvailabilityDTO periodAvailableDTO)
        {
            if (periodAvailableDTO.PeriodAvailable == PeriodAvailability.AVAILABLE)
            {
                return true;
            }
            return false;
        }
    }
}
